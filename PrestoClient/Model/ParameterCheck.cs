using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.PrestoClient.Model
{
    public static class ParameterCheck
    {
        public static void OutOfRange(bool expression, string parameterName, string message = "")
        {
            if (expression == false)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        public static void Check(bool expression, string message)
        {
            if (expression == false)
            {
                throw new ArgumentException(message);
            }
        }

        public static void NonNull<T>(T value, string parameterName, string message = "") where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }

        public static void NotNullOrEmpty(string value, string parameterName, string message = "")
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }
    }
}
