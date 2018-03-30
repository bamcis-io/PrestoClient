using BAMCIS.PrestoClient.Model.SPI.Type;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Metadata
{
    /// <summary>
    /// From com.facebook.presto.metadata.Signature.java
    /// </summary>
    public class Signature
    {
        #region Public Properties

        public string Name { get; }

        public FunctionKind Kind { get; }

        public IEnumerable<TypeVariableConstraint> TypeVariableConstraints { get; }

        public IEnumerable<LongVariableConstraint> LongVariableConstraints { get; }

        public TypeSignature ReturnType { get; }

        public IEnumerable<TypeSignature> ArgumentTypes { get; }

        public bool VariableArity { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Signature(
            string name, 
            FunctionKind kind, 
            IEnumerable<TypeVariableConstraint> typeVariableConstraints,
            IEnumerable<LongVariableConstraint> longVariableConstraints,
            TypeSignature returnType,
            IEnumerable<TypeSignature> argumentTypes,
            bool variableArity)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            this.Name = name;
            this.Kind = kind;
            this.TypeVariableConstraints = typeVariableConstraints;
            this.LongVariableConstraints = longVariableConstraints;
            this.ReturnType = returnType;
            this.ArgumentTypes = argumentTypes;
            this.VariableArity = variableArity;
        }

        #endregion
    }
}
