using BAMCIS.PrestoClient.Serialization;
using Newtonsoft.Json;
using System;

namespace BAMCIS.PrestoClient.Model
{
    [JsonConverter(typeof(DataSizeConverter))]
    public class DataSize : IComparable<DataSize>
    {
        #region Public Properties

        public double Size { get; }

        public DataSizeUnit Unit { get; }

        #endregion

        #region Constructors

        [JsonConstructor]
        public DataSize(double size, DataSizeUnit unit)
        {
            if (Double.IsInfinity(size))
            {
                throw new ArgumentOutOfRangeException("size", "The size is infinity.");
            }

            if (Double.IsNaN(size))
            {
                throw new ArgumentOutOfRangeException("size", "The size is NaN.");
            }

            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", "The size is less than 0.");
            }

            this.Size = size;
            this.Unit = unit;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{this.Size.ToString("0.##")}{this.Unit.GetUnitString()}";
        }

        public double GetValue(DataSizeUnit unit)
        {
            return this.Size * (this.Unit.GetFactor() * (1.0 / unit.GetFactor()));
        }

        public int CompareTo(DataSize other)
        {
            return this.GetValue(DataSizeUnit.BYTE).CompareTo(other.GetValue(DataSizeUnit.BYTE));
        }

        #endregion

    }
}
