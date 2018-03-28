using System;
using System.ComponentModel;
using System.Reflection;

namespace BAMCIS.PrestoClient.Model
{
    /// <summary>
    /// From io.airlift.units.DataSize.java (internal class DataSizeUnit)
    /// </summary>
    public static class DataSizeUnitExtensionMethods
    {
        public static long GetFactor(this DataSizeUnit unit)
        {
            switch (unit)
            {
                case DataSizeUnit.BYTE:
                    {
                        return 1;
                    }
                case DataSizeUnit.KILOBYTE:
                    {
                        return (1 << 10);
                    }
                case DataSizeUnit.MEGABYTE:
                    {
                        return (1 << 20);
                    }
                case DataSizeUnit.GIGABYTE:
                    {
                        return (1 << 30);
                    }
                case DataSizeUnit.TERABYTE:
                    {
                        return (1 << 40);
                    }
                case DataSizeUnit.PETABYTE:
                    {
                        return (1 << 50);
                    }
                case DataSizeUnit.EXABYTE:
                    {
                        return (1 << 60);
                    }
                default:
                    {
                        throw new ArgumentException($"The unit {unit} is unknown.");
                    }
            }
        }

        public static string GetUnitString(this DataSizeUnit unit)
        {
            return unit.GetType().GetCustomAttribute<DescriptionAttribute>().Description;
        }
    }
}
