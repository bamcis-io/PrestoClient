using BAMCIS.PrestoClient.Model.Client;
using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.IO;
using static BAMCIS.PrestoClient.Model.Statement.ExecuteQueryV1BatchEnumerable;

namespace BAMCIS.PrestoClient.Model.Statement
{
    /// <summary>
    /// An encapsulation of the batched responses received from a <see cref="PrestodbClient.ExecuteQueryV1Batched"/> request.
    /// </summary>
    public class ExecuteQueryV1BatchedResponse : IAsyncDisposable, IDisposable
    {
        #region Private Properties

        private readonly IAsyncEnumerable<QueryResultsV1> _responses;
        private readonly CancellationToken _cancellationToken;

        private IAsyncEnumerator<QueryResultsV1> _responseIterator;
        private SemaphoreSlim _iterationCacheLock;
        private QueryResultsV1 _materializedResultsFirstBatch;
        private QueryResultsV1 _materializedResultsLatestBatch;
        private QueryResultsV1 _materializedResultsFirstColumnsBatch;
        private Queue<QueryResultsV1> _materializedResultsBatchesWithDataCache;
        private bool _beganIteration;

        private bool _disposed = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates whether the query was successfully closed by the client.
        /// </summary>
        public bool QueryClosed { get; private set; }

        #endregion

        #region Constructor

        public ExecuteQueryV1BatchedResponse(IAsyncEnumerable<QueryResultsV1> responses, CancellationToken cancellationToken)
        {
            _responses = responses;
            _cancellationToken = cancellationToken;
            _beganIteration = false;
            _materializedResultsBatchesWithDataCache = new Queue<QueryResultsV1>();
            _iterationCacheLock = new SemaphoreSlim(1, 1);
        }

        #endregion

        #region Finalizer

        ~ExecuteQueryV1BatchedResponse() => Dispose(false);

        #endregion

        #region Public Methods

        /// <summary>
        /// Iterates through the batches of responses, and returns only the batches with data.
        /// <br/><br/>
        /// NOTE: Data can only be iterated through once. To reiterate, the caller is responsible for caching results in-memory.
        /// </summary>
        /// <returns>Returns one batch at a time.</returns>
        public async IAsyncEnumerable<QueryResultsV1> GetBatchesAsync()
        {
            await _iterationCacheLock.WaitAsync();
            try
            {
                // If query is closed, do not support getting data, as we do not keep all the batches.
                // In order to iterate through batches more than once, the caller must explicitly store the results.
                if (QueryClosed)
                {
                    throw new PrestoException("Cannot get batches because the query is already closed.");
                }

                if (_responseIterator == null)
                {
                    _responseIterator = _responses.GetAsyncEnumerator(_cancellationToken);
                }

                if (!await MoveToOrBeyondFirstDataBatchAsync().ConfigureAwait(false))
                {
                    await EndIterationAsync();
                    yield break;
                }

                // Return & remove any cached results before iterating over new ones
                while (_materializedResultsBatchesWithDataCache.Any())
                {
                    var batchToReturn = _materializedResultsBatchesWithDataCache.Dequeue();
                    _iterationCacheLock.Release();
                    yield return batchToReturn;
                    await _iterationCacheLock.WaitAsync();
                }

                // Iterate over remaining responses

                // Fetch and return, don't keep data batches going forward
                while (await _responseIterator.MoveNextAsync().ConfigureAwait(false))
                {
                    var found = EnsureCacheHasBatchWithData(_responseIterator);
                    EnsureCacheHasFirstBatch(_responseIterator);
                    EnsureCacheHasColumns(_responseIterator);
                    EnsureCacheHasLatestBatch(_responseIterator);

                    if (found)
                    {
                        var batchToReturn = _materializedResultsBatchesWithDataCache.Dequeue();
                        _iterationCacheLock.Release();
                        yield return batchToReturn;
                        await _iterationCacheLock.WaitAsync();
                    }
                }

                await EndIterationAsync();
                yield break;
            }
            finally
            {
                _iterationCacheLock.Release();
            }
        }

        /// <summary>
        /// Iterates through the batches of responses, and returns data one row at a time from the batches.
        /// <br/><br/>
        /// NOTE: Data can only be iterated through once. To reiterate, the caller is responsible for caching results in-memory.
        /// </summary>
        /// <returns>Returns one data row at a time.</returns>
        public async IAsyncEnumerable<List<dynamic>> GetDataAsync()
        {
            await foreach (var dataBatch in GetBatchesAsync())
            {
                foreach (var dataRow in dataBatch.GetData())
                {
                    yield return dataRow;
                }
            }
        }

        /// <summary>
        /// Gets the list of columns in the query response.
        /// </summary>
        /// <returns>List of columns in the query response.</returns>
        /// <exception cref="PrestoException"></exception>
        public async Task<IReadOnlyList<Column>> GetColumnsAsync()
        {
            await _iterationCacheLock.WaitAsync();
            try
            {
                // If query is closed, still support getting columns from the cached response
                if (QueryClosed)
                {
                    if (_materializedResultsFirstColumnsBatch == null)
                    {
                        throw new PrestoException("Cannot get columns because first response batch with columns is missing.");
                    }

                    return _materializedResultsFirstColumnsBatch.Columns.ToList();
                }

                if (_responseIterator == null)
                {
                    _responseIterator = _responses.GetAsyncEnumerator(_cancellationToken);
                }

                if (!await MoveToOrBeyondFirstColumnsBatchAsync().ConfigureAwait(false))
                {
                    await EndIterationAsync();
                    throw new PrestoException("Cannot get columns because first response batch with columns is missing.");
                }

                if (_materializedResultsFirstColumnsBatch == null)
                {
                    await EndIterationAsync();
                    throw new PrestoException("Cannot get columns because first response batch with columns is missing.");
                }

                // Return columns
                return _materializedResultsFirstColumnsBatch.Columns.ToList();
            }
            finally
            {
                _iterationCacheLock.Release();
            }
        }

        /// <summary>
        /// Gets the query statement stats.
        /// </summary>
        /// <returns>Query statement stats</returns>
        /// <exception cref="PrestoException"></exception>
        public async Task<StatementStats> GetStatsAsync()
        {
            await _iterationCacheLock.WaitAsync();
            try
            {
                // If query is closed, still support getting stats from the cached response
                if (QueryClosed)
                {
                    if (_materializedResultsLatestBatch == null)
                    {
                        throw new PrestoException("Cannot get stats because latest response batch is missing.");
                    }

                    return _materializedResultsLatestBatch.GetStats();
                }

                if (_responseIterator == null)
                {
                    _responseIterator = _responses.GetAsyncEnumerator(_cancellationToken);
                }

                if (!await MoveToOrBeyondFirstBatchAsync().ConfigureAwait(false))
                {
                    await EndIterationAsync();
                    throw new PrestoException("Cannot get stats because latest response batch is missing.");
                }

                if (_materializedResultsLatestBatch == null)
                {
                    await EndIterationAsync();
                    throw new PrestoException("Cannot get stats because latest response batch is missing.");
                }

                return _materializedResultsLatestBatch.GetStats();
            }
            finally
            {
                _iterationCacheLock.Release();
            }
        }

        /// <summary>
        /// Cancels the query. Can be used to stop and destroy the batch response iterator.
        /// </summary>
        /// <returns></returns>
        public async Task CancelAsync()
        {
            await DisposeAsync().ConfigureAwait(false);
        }

        #endregion

        #region Private Methods

        private async Task EndIterationAsync()
        {
            if (_responseIterator != null)
            {
                await _responseIterator.DisposeAsync().ConfigureAwait(false);
                _responseIterator = null;
            }

            QueryClosed = true;
        }

        private void EndIteration()
        {
            (_responseIterator as IDisposable)?.Dispose();
            _responseIterator = null;
            QueryClosed = true;
        }

        /// <summary>
        /// Move the iterator to a position at which the first overall batch is captured and cached, or no-op if it already is.
        /// </summary>
        /// <returns>True if the iterator was successfully moved to a position or no-op'd such that the first overall batch is available in the cache, otherwise false.</returns>
        private async ValueTask<bool> MoveToOrBeyondFirstBatchAsync()
        {
            // If already materialized, then we're already beyond the target,
            // so just indicate so and do not advance the iterator any further.
            if (_materializedResultsFirstBatch != null)
            {
                return true;
            }

            // Otherwise, iterate until we get the target,
            // saving anything else of interest along the way
            if (await _responseIterator.MoveNextAsync().ConfigureAwait(false))
            {
                var found = EnsureCacheHasFirstBatch(_responseIterator);
                EnsureCacheHasBatchWithData(_responseIterator);
                EnsureCacheHasColumns(_responseIterator);
                EnsureCacheHasLatestBatch(_responseIterator);

                if (found)
                {
                    return true;
                }
            }

            // If we spun through all iterations without reaching the target,
            // then stop (i.e. return false).
            return false;
        }

        /// <summary>
        /// Move the iterator to a position at which the first data batch is captured and cached, or no-op if it already is.
        /// </summary>
        /// <returns>True if the iterator was successfully moved to a position or no-op'd such that the first data batch is available in the cache, otherwise false.</returns>
        private async ValueTask<bool> MoveToOrBeyondFirstDataBatchAsync()
        {
            // If already materialized, then we're already beyond the target,
            // so just indicate so and do not advance the iterator any further.
            if (_materializedResultsBatchesWithDataCache.Any())
            {
                return true;
            }

            // Otherwise, iterate until we get the target,
            // saving anything else of interest along the way
            while (await _responseIterator.MoveNextAsync().ConfigureAwait(false))
            {
                EnsureCacheHasFirstBatch(_responseIterator);
                var found = EnsureCacheHasBatchWithData(_responseIterator);
                EnsureCacheHasColumns(_responseIterator);
                EnsureCacheHasLatestBatch(_responseIterator);

                if (found)
                {
                    return true;
                }
            }

            // If we spun through all iterations without reaching the target,
            // then stop (i.e. return false).
            return false;
        }

        /// <summary>
        /// Move the iterator to a position at which columns info is captured and cached, or no-op if it already is.
        /// </summary>
        /// <returns>True if the iterator was successfully moved to a position or no-op'd such that columns info is available in the cache, otherwise false.</returns>
        private async ValueTask<bool> MoveToOrBeyondFirstColumnsBatchAsync()
        {
            // If already materialized, then we're already beyond the target,
            // so just indicate so and do not advance the iterator any further.
            if (_materializedResultsFirstColumnsBatch != null)
            {
                return true;
            }

            // Otherwise, iterate until we get the target,
            // saving anything else of interest along the way
            while (await _responseIterator.MoveNextAsync().ConfigureAwait(false))
            {
                EnsureCacheHasFirstBatch(_responseIterator);
                EnsureCacheHasBatchWithData(_responseIterator);
                var found = EnsureCacheHasColumns(_responseIterator);
                EnsureCacheHasLatestBatch(_responseIterator);

                if (found)
                {
                    return true;
                }
            }

            // If we spun through all iterations without reaching the target,
            // then stop (i.e. return false).
            return false;
        }

        /// <summary>
        /// Check the iterator's current position for a batch with data, and cache it if available.
        /// </summary>
        /// <param name="responseIterator">The iterator to check</param>
        /// <returns>True if the cache contains a batch with data, otherwise false.</returns>
        private bool EnsureCacheHasBatchWithData(IAsyncEnumerator<QueryResultsV1> responseIterator)
        {
            if (responseIterator.Current?.Data != null)
            {
                _materializedResultsBatchesWithDataCache.Enqueue(responseIterator.Current);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check the iterator's current position for columns info, and cache it if available.
        /// </summary>
        /// <param name="responseIterator">The iterator to check</param>
        /// <returns>True if the cache contains columns info, whether from the iterator's current position or from earlier, otherwise false.</returns>
        private bool EnsureCacheHasColumns(IAsyncEnumerator<QueryResultsV1> responseIterator)
        {
            if (_materializedResultsFirstColumnsBatch != null)
            {
                return true;
            }

            if (responseIterator.Current?.Columns != null)
            {
                _materializedResultsFirstColumnsBatch = responseIterator.Current;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check the iterator's current position for the first batch, and cache it if available.
        /// </summary>
        /// <param name="responseIterator">The iterator to check</param>
        /// <returns>True if the cache contains the first batch, whether from the iterator's current position or from earlier, otherwise false.</returns>
        private bool EnsureCacheHasFirstBatch(IAsyncEnumerator<QueryResultsV1> responseIterator)
        {
            if (_beganIteration && _materializedResultsFirstBatch == null)
            {
                return false;
            }

            if (_materializedResultsFirstBatch != null)
            {
                return true;
            }

            _materializedResultsFirstBatch = responseIterator.Current;
            _beganIteration = true;
            return true;
        }

        /// <summary>
        /// Check the iterator's current position for the latest batch, and cache it if available.
        /// </summary>
        /// <param name="responseIterator">The iterator to check</param>
        /// <returns>True if the cache contains the latest batch, whether from the iterator's current position or from earlier, otherwise false.</returns>
        private bool EnsureCacheHasLatestBatch(IAsyncEnumerator<QueryResultsV1> responseIterator)
        {
            _materializedResultsLatestBatch = responseIterator.Current;
            return true;
        }

        #endregion

        #region Dispose / DisposeAsync

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Dispose of managed resources
                if (disposing)
                {
                    EndIteration();
                }

                _disposed = true;

                // Dispose of unmanaged resources
                // -- NONE in this class
            }
        }

        // Implementing standard DisposeAsync Pattern: https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync
        public async ValueTask DisposeAsync()
        {
            // Perform async cleanup.
            await DisposeAsyncCore().ConfigureAwait(false);

            // Dispose of unmanaged resources.
            Dispose(false);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            await EndIterationAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
