using BAMCIS.PrestoClient.Model.SPI.Type;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BAMCIS.PrestoClient.Model.Client
{
    /// <summary>
    /// From com.facebook.presto.client.ClientTypeSignature.java
    /// </summary>
    public class ClientTypeSignature
    {
        #region Private Fields

        private static readonly Regex PATTERN = new Regex(".*(?:[<>,].*)?");

        #endregion

        #region Public Properties

        public string RawType { get; }

        [Obsolete("This property is deprecated and clients should switch to Arguments}")]
        public IEnumerable<ClientTypeSignature> TypeArguments {
            get
            {
                List<ClientTypeSignature> Results = new List<ClientTypeSignature>();

                foreach (ClientTypeSignatureParameter Arg in this.Arguments)
                {
                    switch (Arg.Kind)
                    {
                        case ParameterKind.TYPE:
                            {
                                Results.Add(Arg.GetTypeSignature());
                                break;
                            }
                        case ParameterKind.NAMED_TYPE:
                            {
                                Results.Add(new ClientTypeSignature(Arg.GetNamedTypeSignature().TypeSignature));
                                break;
                            }
                        default:
                            {
                                return new List<ClientTypeSignature>();
                            }

                    }
                }

                return Results;
            }
        }

        [Obsolete("This property is deprecated and clients should switch to Arguments}")]
        public IEnumerable<object> LiteralArguments {
            get
            {
                List<object> Results = new List<object>();

                foreach (ClientTypeSignatureParameter Arg in this.Arguments)
                {
                    switch (Arg.Kind)
                    {
                        case ParameterKind.NAMED_TYPE:
                            {
                                Results.Add(Arg.GetNamedTypeSignature().Name);
                                break;
                            }
                        default:
                            {
                                return new List<object>();
                            }
                        
                    }
                }

                return Results;
            }
        }

        public IEnumerable<ClientTypeSignatureParameter> Arguments { get; }

        #endregion

        #region Constructors

        public ClientTypeSignature(TypeSignature typeSignature) : this(typeSignature.Base, typeSignature.Parameters.Select(x => new ClientTypeSignatureParameter(x)))
        {
        }

        public ClientTypeSignature(string rawType, IEnumerable<ClientTypeSignatureParameter> arguments) : this(rawType, new ClientTypeSignature[0], new object[0], arguments)
        {        
        }

        [JsonConstructor]
        public ClientTypeSignature(string rawType, IEnumerable<ClientTypeSignature> typeArguments, IEnumerable<object> literalArguments, IEnumerable<ClientTypeSignatureParameter> arguments)
        {
            if (String.IsNullOrEmpty(rawType))
            {
                throw new ArgumentNullException("rawType");
            }

            Match RegexMatch = PATTERN.Match(rawType);

            if (!RegexMatch.Success)
            {
                throw new FormatException($"Bad characters in rawType: {rawType}.");
            }

            this.RawType = rawType;

            if (arguments != null)
            {
                this.Arguments = arguments;
            }
            else
            {
                if (typeArguments == null)
                {
                    throw new ArgumentNullException("typeArguments");
                }

                if (literalArguments == null)
                {
                    throw new ArgumentNullException("literalArguments");
                }

                List<ClientTypeSignatureParameter> ConvertedArguments = new List<ClientTypeSignatureParameter>();

                // Talking to a legacy server (< 0.133)
                if (rawType.Equals(StandardTypes.ROW))
                {
                    int TypeArgSize = typeArguments.Count();
                    int LiteralArgSize = literalArguments.Count();

                    if (TypeArgSize != LiteralArgSize)
                    {
                        throw new ArgumentException($"The size of type arguments and literal arguments did not match: {TypeArgSize} && {LiteralArgSize}");
                    }

                    int Counter = 0;

                    foreach (object Item in literalArguments)
                    {
                        if (!(Item is string))
                        {
                            throw new ArgumentException($"Expected literal argument {Item}, {Counter} in literalArguments to be a string.");
                        }

                        ConvertedArguments.Add(new ClientTypeSignatureParameter(new TypeSignatureParameter(new NamedTypeSignature(Item.ToString(), ToTypeSignature(typeArguments.ElementAt(Counter))))));

                        Counter++;
                    }
                }
                else
                {
                    if (LiteralArguments.Any())
                    {
                        throw new ArgumentException("Unexpected literal arguments from legacy server.");
                    }

                    foreach (ClientTypeSignature TypeArgument in TypeArguments)
                    {
                        ConvertedArguments.Add(new ClientTypeSignatureParameter(ParameterKind.TYPE, TypeArgument));
                    }
                }

                this.Arguments = ConvertedArguments;
            }
        }

        #endregion

        #region Private Methods

        private static TypeSignature ToTypeSignature(ClientTypeSignature signature)
        {
            IEnumerable<TypeSignatureParameter> Parameters = signature.Arguments.Select(x => LegacyClientTypeSignatureParameterToTypeSignatureParameter(x));
            return new TypeSignature(signature.RawType, Parameters);
        }

        private static TypeSignatureParameter LegacyClientTypeSignatureParameterToTypeSignatureParameter(ClientTypeSignatureParameter parameter)
        {
            switch (parameter.Kind)
            {
                case ParameterKind.LONG:
                    throw new ArgumentException("Unexpected long type literal returned by legacy server");
                case ParameterKind.TYPE:
                    return new TypeSignatureParameter(ToTypeSignature(parameter.GetTypeSignature()));
                case ParameterKind.NAMED_TYPE:
                    return new TypeSignatureParameter(parameter.GetNamedTypeSignature());
                default:
                    throw new ArgumentException($"Unknown parameter kind {parameter.Kind}.");
            }
        }

        #endregion
    }
}
