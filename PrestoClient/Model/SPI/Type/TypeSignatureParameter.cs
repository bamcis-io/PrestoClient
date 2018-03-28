using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    /// <summary>
    /// From com.facebook.presto.spi.type.TypeSignatureParameter.java
    /// </summary>
    public class TypeSignatureParameter
    {
        #region Public Properties

        public ParameterKind Kind { get; }

        public dynamic Value { get; }

        #endregion

        #region Constructors

        public TypeSignatureParameter(TypeSignature typeSignature)
        {
            this.Kind = ParameterKind.TYPE;
            this.Value = typeSignature ?? throw new ArgumentNullException("typeSignature");
        }

        public TypeSignatureParameter(long longLiteral)
        {
            this.Kind = ParameterKind.LONG;
            this.Value = longLiteral;
        }

        public TypeSignatureParameter(NamedTypeSignature namedTypeSignature)
        {
            this.Kind = ParameterKind.NAMED_TYPE;
            this.Value = namedTypeSignature ?? throw new ArgumentNullException("namedTypeSignature");
        }

        public TypeSignatureParameter(string variable)
        {
            this.Kind = ParameterKind.VARIABLE;
            this.Value = variable;
        }

        [JsonConstructor]
        private TypeSignatureParameter(ParameterKind kind, object value)
        {
            this.Kind = kind;
            this.Value = value ?? throw new ArgumentNullException("value", "The value cannot be null"); ;
        }

        #endregion

        #region Public Methods

        public bool IsTypeSignature()
        {
            return this.Kind == ParameterKind.TYPE;
        }

        public bool IsLongLiteral()
        {
            return this.Kind == ParameterKind.LONG;
        }

        public bool IsNamedTypeSignature()
        {
            return this.Kind == ParameterKind.NAMED_TYPE;
        }

        public bool IsVariable()
        {
            return this.Kind == ParameterKind.VARIABLE;
        }

        public TypeSignature GetTypeSignature()
        {
            return (TypeSignature)GetValue(ParameterKind.TYPE, typeof(TypeSignature));
        }

        public long GetLongLiteral()
        {
            return (long)GetValue(ParameterKind.LONG, typeof(long));
        }

        public NamedTypeSignature GetNamedTypeSignature()
        {
            return (NamedTypeSignature)GetValue(ParameterKind.NAMED_TYPE, typeof(NamedTypeSignature));
        }

        public string GetVariable()
        {
            return (string)GetValue(ParameterKind.VARIABLE, typeof(string));
        }

        public bool IsCalculated()
        {
            switch (this.Kind)
            {
                case ParameterKind.TYPE:
                    return this.GetTypeSignature().Calculated;
                case ParameterKind.NAMED_TYPE:
                    return this.GetNamedTypeSignature().TypeSignature.Calculated;
                case ParameterKind.LONG:
                    return false;
                case ParameterKind.VARIABLE:
                    return true;
                default:
                    throw new ArgumentException($"Unexpected parameter kind: {this.Kind}");
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion

        #region Private Methods

        private object GetValue(ParameterKind expectedParameterKind, System.Type target)
        {
            return Convert.ChangeType(this.Value, target);
        }

        #endregion
    }
}