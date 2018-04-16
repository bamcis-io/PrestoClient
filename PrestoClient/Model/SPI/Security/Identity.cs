using System;
using System.Text;

namespace BAMCIS.PrestoClient.Model.SPI.Security
{
    /// <summary>
    /// From com.facebook.presto.spi.security.Identity.java
    /// </summary>
    public class Identity
    {
        #region Public Properties

        public string User { get; }

        /// <summary>
        /// TODO: This is a java.security.Principal object
        /// </summary>
        public dynamic Principal { get; }

        #endregion

        #region Constructors

        public Identity(string user, dynamic principal)
        {
            ParameterCheck.NotNullOrEmpty(user, "user");

            this.User = user;
            this.Principal = principal;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            Identity Id = (Identity)obj;

            return Object.Equals(this.User, Id.User);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(this.User);
        }

        public override string ToString()
        {
            StringBuilder SB = new StringBuilder("Identity{");
            SB.Append("user='").Append(this.User).Append("\'");

            if (this.Principal != null)
            {
                SB.Append(", prinicipal="); //.Append(this.Principal.Get());
            }

            SB.Append("}");

            return SB.ToString();
        }

        #endregion
    }
}
