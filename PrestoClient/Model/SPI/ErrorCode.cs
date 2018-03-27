using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model.SPI
{
    /// <summary>
    /// From com.facebook.presto.spi.ErrorType.java
    /// </summary>
    public class ErrorCode
    {
        #region Public Properties

        public int Code { get; }

        public string Name { get; }

        public ErrorType Type { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public ErrorCode(int code, string name, ErrorType type)
        {
            if (code < 0)
            {
                throw new ArgumentOutOfRangeException("code", "The code cannot be negative");
            }

            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "The name cannot be null or empty.");
            }

            this.Code = code;
            this.Name = name;
            this.Type = type;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.Name}:{this.Code}";
        }

        #endregion
    }
}