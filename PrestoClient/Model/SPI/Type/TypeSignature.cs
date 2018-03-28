using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAMCIS.PrestoClient.Model.SPI.Type
{
    public class TypeSignature
    {
        #region Private Fields

        private static Dictionary<string, string> BASE_NAME_ALIAS_TO_CANONICAL = new Dictionary<string, string>();

        static TypeSignature()
        {
            BASE_NAME_ALIAS_TO_CANONICAL.Add("int", StandardTypes.INTEGER);
        }

        #endregion

        #region Public Properties

        public string Base { get; set; }

        public IEnumerable<TypeSignatureParameter> Parameters { get; set; }

        public bool Calculated { get; set; }

        #endregion

        #region Constructors

        public TypeSignature(string @base, params TypeSignatureParameter[] parameters) : this(@base, parameters.ToList())
        {
        }

        public TypeSignature(string @base, IEnumerable<TypeSignatureParameter> parameters)
        {
            if (String.IsNullOrEmpty(@base))
            {
                throw new ArgumentNullException("base");
            }

            this.Base = @base;
            this.Parameters = parameters ?? throw new ArgumentNullException("parameters");
            this.Calculated = parameters.Any(x => x.IsCalculated());
        }

        #endregion


        #region Public Methods

        public override string ToString()
        {
            if (this.Base.Equals(StandardTypes.ROW))
            {
                return "";
            }
            else if (this.Base.Equals(StandardTypes.VARCHAR) && 
                this.Parameters.Count() == 1 && 
                this.Parameters.ElementAt(0).IsLongLiteral() &&
                this.Parameters.ElementAt(0).GetLongLiteral() == VarcharType.UNBOUNDED_LENGTH
                )
            {
                return this.Base;
            }
            else
            {
                StringBuilder TypeName = new StringBuilder(this.Base);

                if (this.Parameters.Any())
                {
                    TypeName.Append($"({String.Join(",", this.Parameters.Select(x => x.ToString()))})");
                }

                return TypeName.ToString();
            }
        }

        public IEnumerable<TypeSignature> GetTypeParametersAsTypeSignatures()
        {
            foreach (TypeSignatureParameter Parameter in this.Parameters)
            {
                if (Parameter.Kind != ParameterKind.TYPE)
                {
                    throw new FormatException($"Expected all parameters to be TypeSignatures but {Parameter.ToString()} was found");
                }

                yield return Parameter.GetTypeSignature();
            }
        }

        #endregion
    }
}