using BAMCIS.PrestoClient.Model.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model.Statement
{
    public class DeleteLastUriV1Response
    {
        #region Private Properties
        private bool _closed;

        #endregion

        #region Public Properties

        public bool Closed => _closed;

        #endregion

        #region Constructors

        public DeleteLastUriV1Response(bool closed)
        {
            _closed = closed;
        }

        #endregion
    }
}
