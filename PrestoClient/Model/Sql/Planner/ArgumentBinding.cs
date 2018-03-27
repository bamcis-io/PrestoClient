using System;

namespace BAMCIS.PrestoClient.Model.Sql.Planner
{
    /// <summary>
    /// From com.facebook.presto.sql.planner.Partitioning.java (internal class ArgumentBinding)
    /// </summary> 
    public sealed class ArgumentBinding
    {
        #region Public Properties

        public Symbol Column { get; }

        public object Constant { get; }

        #endregion

        #region Constructors

        public ArgumentBinding(Symbol column, object constant)
        {
            if ((column != null) == (constant != null))
            {
                throw new ArgumentException("Either column or constant must be set, not both, and both cannot be null.");
            }

            this.Column = column;
            this.Constant = constant;
        }

        #endregion

        #region Public Methods

        public bool IsConstant()
        {
            return this.Constant != null;
        }

        public bool IsVariable()
        {
            return this.Column != null;
        }

        public static ArgumentBinding ColumnBinding(Symbol column)
        {
            return new ArgumentBinding(column, null);
        }

        public static ArgumentBinding ConstantBinding(object constant)
        {
            return new ArgumentBinding(null, constant);
        }

        public ArgumentBinding Translate(Func<Symbol, Symbol> translator)
        {
            if (this.IsConstant())
            {
                return this;
            }
            else
            {
                return ColumnBinding(translator.Invoke(this.Column));
            }
        }

        #endregion
    }
}
