using System;

namespace BAMCIS.PrestoClient.Model
{
    public sealed class ExponentialDecay
    {
        #region Constructors

        private ExponentialDecay()
        {
        }

        #endregion

        #region Public Methods

        public static double OneMinute()
        {
            // alpha for a target weight of 1/E at 1 minute
            return 1.0 / 60;
        }

        public static double FiveMinutes()
        {
            // alpha for a target weight of 1/E at 5 minutes
            return 1.0 / 5 * 60;
        }

        public static double FifteenMinutes()
        {
            // alpha for a target weight of 1/E at 15 minutes
            return 1.0 / 15 * 60;
        }

        public static double Seconds(int seconds)
        {
            if (seconds <= 0)
            {
                throw new ArgumentOutOfRangeException("seconds", "Seconds must be greater than zero.");
            }

            // alpha for a target weight of 1/E at the specified number of seconds
            return 1.0 / seconds;
        }

        /**
         * Compute the alpha decay factor such that the weight of an entry with age 'targetAgeInSeconds' is targetWeight'
         */
        public static double ComputeAlpha(double targetWeight, long targetAgeInSeconds)
        {
            ParameterCheck.Check(targetAgeInSeconds > 0, "targetAgeInSeconds must be > 0");
            ParameterCheck.Check(targetWeight > 0 && targetWeight < 1, "targetWeight must be in range (0, 1)");

            return -Math.Log(targetWeight) / targetAgeInSeconds;
        }

        #endregion
    }
}
