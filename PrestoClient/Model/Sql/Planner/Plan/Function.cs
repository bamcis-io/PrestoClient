using BAMCIS.PrestoClient.Model.Metadata;
using BAMCIS.PrestoClient.Model.Sql.Tree;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner.Plan
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.plan.WindowNode.java (internal class Function)
    /// </summary>
    public class Function
    {
        #region Public Properties

        public FunctionCall FunctionCall { get; }

        public Signature Signature { get; }

        public Frame Frame { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public Function(FunctionCall functionCall, Signature signature, Frame frame)
        {
            this.FunctionCall = functionCall ?? throw new ArgumentNullException("functionCall");
            this.Signature = signature ?? throw new ArgumentNullException("signature");
            this.Frame = frame ?? throw new ArgumentNullException("frame");
        }

        #endregion

        #region Public Properties

        public override int GetHashCode()
        {
            return Hashing.Hash(this.FunctionCall, this.Signature, this.Frame);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || typeof(Function) != obj.GetType())
            {
                return false;
            }

            Function other = (Function)obj;

            return object.Equals(this.FunctionCall, other.FunctionCall) &&
                    object.Equals(this.Signature, other.Signature) &&
                    object.Equals(this.Frame, other.Frame);
        }

        #endregion
    }
}
