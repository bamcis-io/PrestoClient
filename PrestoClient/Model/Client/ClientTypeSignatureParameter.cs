using BAMCIS.PrestoClient.Model.SPI.Type;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.ClientTypeSignatureParameter.java
    /// </summary>
    public class ClientTypeSignatureParameter
    {
        #region Public Properties

        public ParameterKind Kind { get; }

        public object Value { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ClientTypeSignatureParameter(ParameterKind kind, object value)
        {
            this.Kind = kind;
            this.Value = value;
        }

        public ClientTypeSignatureParameter(TypeSignatureParameter typeParameterSignature)
        {
            this.Kind = typeParameterSignature.Kind;

            switch (this.Kind)
            {
                case ParameterKind.TYPE:
                    {
                        this.Value = new ClientTypeSignature(typeParameterSignature.GetTypeSignature());
                        break;
                    }
                case ParameterKind.LONG:
                    {
                        this.Value = typeParameterSignature.GetLongLiteral();
                        break;
                    }
                case ParameterKind.NAMED_TYPE:
                    {
                        this.Value = typeParameterSignature.GetNamedTypeSignature();
                        break;
                    }
                case ParameterKind.VARIABLE:
                    {
                        this.Value = typeParameterSignature.GetVariable();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException($"Unknown type signature kind {this.Kind}.");
                    }
            }
        }

        #endregion

        #region Public Methods

        public ClientTypeSignature GetTypeSignature()
        {
            return (ClientTypeSignature)GetValue(ParameterKind.TYPE, typeof(ClientTypeSignature));
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
