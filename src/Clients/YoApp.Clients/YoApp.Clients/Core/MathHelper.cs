namespace YoApp.Clients.Core
{
    public static class MathHelper
    {
        /// <summary>
        /// Interpolate between two values.
        /// </summary>
        /// <param name="fromValue"></param>
        /// <param name="toValue"></param>
        /// <param name="amount"></param>
        /// <returns>Interpolated result.</returns>
        public static double Lerp(double fromValue, double toValue, double amount)
        {
            return fromValue + (toValue - fromValue) * amount;
        }
    }
}
