using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.PrestoClient.Model.Sql.Tree
{
    /// <summary>
    /// From com.facebook.presto.sql.tree.QualifiedName.java
    /// </summary>
    public class QualifiedName
    {
        #region Public Properties

        public IEnumerable<string> Parts { get; }

        public IEnumerable<string> OriginalParts { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public QualifiedName(IEnumerable<string> originalParts, IEnumerable<string> parts)
        {
            this.OriginalParts = originalParts;
            this.Parts = parts;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return String.Join(".", this.Parts);
        }

        public QualifiedName GetPrefix()
        {
            if (this.Parts.Count() == 1)
            {
                return null;
            }
            else
            {
                IEnumerable<string> SubList = this.Parts.Take(this.Parts.Count() - 1);
                return new QualifiedName(SubList, SubList);
            }
        }

        #endregion
    }
}
