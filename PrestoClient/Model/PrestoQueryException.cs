using BAMCIS.PrestoClient.Model.Client;
using System;
using System.Net;

namespace BAMCIS.PrestoClient.Model
{
    public class PrestoQueryException : PrestoWebException
    {
        #region Public Properties

        public string SqlState { get; }

        public int ErrorCode { get; }

        public string ErrorName { get; }

        public string ErrorType { get; }

        public ErrorLocation ErrorLocation { get; }

        public FailureInfo FailureInfo {get;}

        #endregion

        #region Constructors

        public PrestoQueryException(QueryError error) : base(error.Message, HttpStatusCode.OK)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            this.ErrorCode = error.ErrorCode;
            this.ErrorLocation = error.ErrorLocation;
            this.ErrorName = error.ErrorName;
            this.ErrorType = error.ErrorType;
            this.FailureInfo = error.FailureInfo;
        }

        public PrestoQueryException(QueryError error, HttpStatusCode code) : base(error.Message, code)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            this.ErrorCode = error.ErrorCode;
            this.ErrorLocation = error.ErrorLocation;
            this.ErrorName = error.ErrorName;
            this.ErrorType = error.ErrorType;
            this.FailureInfo = error.FailureInfo;
        }

        #endregion
    }
}
