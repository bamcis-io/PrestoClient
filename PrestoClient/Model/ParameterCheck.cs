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

        public static void NonNull<T>(T value, string parameterName, string message = "") where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }
    }
}
