using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace BAMCIS.PrestoClient.Model.Statement
{
    /// <summary>
    /// An enumerable of response batches in the response of a <see cref="PrestodbClient.ExecuteQueryV1Batched"/> call.
    /// </summary>
    public class ExecuteQueryV1BatchEnumerable : IAsyncEnumerable<QueryResultsV1>, IAsyncDisposable, IDisposable
    {
        #region Private Properties

        private readonly PrestodbClient _client;
        private readonly PostStatementV1Response _initialStatementResponse;

        // There can only be a single iterator (cannot reset iteration).
        // If iteration needs to happen again, results should be saved into an in-memory data structure explicitly by the caller.
        private ExecuteQueryV1BatchIterator _theOnlyIterator;

        #endregion

        #region Constructor

        public ExecuteQueryV1BatchEnumerable(PostStatementV1Response initialStatementResponse, PrestodbClient client)
        {
            _initialStatementResponse = initialStatementResponse;
            _client = client;
        }

        #endregion

        #region Finalizers

        ~ExecuteQueryV1BatchEnumerable() => Dispose(false);

        #endregion

        #region Dispose / DisposeAsync

        public void Dispose()
        {
            // Dispose of unmanaged + managed resources.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Implementing standard DisposeAsync Pattern: https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync
        public async ValueTask DisposeAsync()
        {
            // Perform async cleanup of managed resources.
            await DisposeAsyncCore().ConfigureAwait(false);

            // Dispose of unmanaged resources.
            Dispose(false);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Dispose of managed resources
            if (disposing)
            {
                _theOnlyIterator?.Dispose();
                _theOnlyIterator = null;
            }

            // Dispose of unmanaged resources
            // -- NONE in this class
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_theOnlyIterator is not null)
            {
                await _theOnlyIterator.DisposeAsync().ConfigureAwait(false);
                _theOnlyIterator = null;
            }
        }

        #endregion

        #region IAsyncEnumerable

        public IAsyncEnumerator<QueryResultsV1> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            if (_theOnlyIterator == null)
            {
                _theOnlyIterator = new ExecuteQueryV1BatchIterator(this, _initialStatementResponse, cancellationToken);
            }
            return _theOnlyIterator;
        }

        #endregion

        public class ExecuteQueryV1BatchIterator : IAsyncEnumerator<QueryResultsV1>, IAsyncDisposable, IDisposable
        {
            #region Private Properties

            private readonly ExecuteQueryV1BatchEnumerable _enumerable;
            private readonly PostStatementV1Response _initialStatementResponse;

            private CancellationToken _cancellationToken;

            private Uri _lastNonNullUri;
            private int _numberOfBatchesIterated;
            private bool _isClosed;

            private bool _disposed = false;

            #endregion

            #region Public Properties

            public QueryResultsV1 Current { get; private set; }

            #endregion

            #region Constructors

            public ExecuteQueryV1BatchIterator(ExecuteQueryV1BatchEnumerable enumerable, PostStatementV1Response initialStatementResponse, CancellationToken cancellationToken)
            {
                _enumerable = enumerable;
                _initialStatementResponse = initialStatementResponse;

                _cancellationToken = cancellationToken;
                _numberOfBatchesIterated = 0;
                _isClosed = false;
            }

            #endregion

            #region Finalizers

            ~ExecuteQueryV1BatchIterator() => Dispose(false);

            #endregion

            #region Dispose / DisposeAsync

            public void Dispose()
            {
                // Dispose of unmanaged + managed resources.
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            // Implementing standard DisposeAsync Pattern: https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync
            public async ValueTask DisposeAsync()
            {
                // Perform async cleanup of managed resources.
                await DisposeAsyncCore().ConfigureAwait(false);

                // Dispose of unmanaged resources.
                Dispose(false);

                // Suppress finalization.
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    // Dispose of managed resources
                    if (disposing)
                    {
                        CloseQuery();
                    }

                    // Dispose of unmanaged resources
                    // -- NONE in this class

                    _disposed = true;
                }
            }

            protected virtual async ValueTask DisposeAsyncCore()
            {
                await CloseQueryAsync();
            }

            #endregion

            #region IAsyncEnumerator

            public async ValueTask<bool> MoveNextAsync()
            {
                // Already dead check
                if (_isClosed)
                {
                    return false;
                }

                // Check if cancellation was requested, if so, stop the iteration early
                if (_cancellationToken.IsCancellationRequested)
                {
                    await CloseQueryAsync();
                    return false;
                }

                // Data case check

                // Base case: Return initial response data directly, since request is done already
                if (_numberOfBatchesIterated == 0)
                {
                    Current = _initialStatementResponse.QueryResults;
                    ++_numberOfBatchesIterated;
                    return true;
                }
                // Nth case: Make async request to get next response data
                else
                {
                    // Programming consistency checks
                    if (Current == null)
                    {
                        throw new IndexOutOfRangeException("No current value despite having already started iteration.");
                    }

                    // Keep track of the last, non-null uri so we can
                    // send a delete request to it at the end
                    if (Current.NextUri == null)
                    {
                        await CloseQueryAsync();
                        return false;
                    }

                    // Put a pause in between each call to reduce CPU usage
                    await Task.Delay(_enumerable._client.Configuration.CheckInterval, _cancellationToken).ConfigureAwait(false);

                    // Send request and receive response
                    var request = new GetNextUriV1Request(Current);
                    var response = await _enumerable._client.GetNextUriV1(request, _cancellationToken).ConfigureAwait(false);

                    // Keep track of the last, non-null uri so we can
                    // send a delete request to it at the end
                    if (Current.NextUri != null)
                    {
                        _lastNonNullUri = Current.NextUri;
                    }

                    Current = response.QueryResults;

                    ++_numberOfBatchesIterated;
                    return true;
                }
            }

            #endregion

            #region Private Methods

            private async Task CloseQueryAsync()
            {
                if (!_isClosed)
                {
                    if (_lastNonNullUri != null)
                    {
                        var request = new DeleteLastUriV1Request(_lastNonNullUri);
                        await _enumerable._client.DeleteLastUriV1(request, CancellationToken.None).ConfigureAwait(false);
                    }
                    _isClosed = true;
                }
            }

            private async void CloseQuery()
            {
                if (!_isClosed)
                {
                    if (_lastNonNullUri != null)
                    {
                        var request = new DeleteLastUriV1Request(_lastNonNullUri);
                        _enumerable._client.DeleteLastUriV1(request, CancellationToken.None).GetAwaiter().GetResult();
                    }
                    _isClosed = true;
                }
            }

            #endregion

        }
    }
}
