using System.Collections.Generic;

namespace BAMCIS.PrestoClient.Model.Jmx
{
    public class JmxMbeanOperation
    {
        #region Public Properties

        public string Name { get; set; }

        public int Impact { get; set; }

        public string ReturnType { get; set; }

        /// <summary>
        /// TODO: Need to find out what type of object is in parameters
        /// </summary>
        public IEnumerable<dynamic> Parameters { get; set; }

        #endregion
    }
}