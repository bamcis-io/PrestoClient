using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    public class TypeSignature
    {
        #region Private Fields

        private bool _Calculated;

        #endregion

        #region Public Properties

        public string RawType { get; set; }

        public IEnumerable<TypeSignature> TypeArguments { get; set; }

        public IEnumerable<dynamic> LiteralArguments { get; set; }

        public IEnumerable<TypeSignatureParameter> Arguments { get; set; }

        #endregion

        #region Constructors

        public TypeSignature()
        {
            this._Calculated = true;
        }

        #endregion


        #region Public Methods

        public bool IsCalculated()
        {
            return _Calculated;
        }

        #endregion
    }
}