namespace YoApp.Clients.Core
{
    public static class MathHelper
    {
        /// <summary>
        /// Interpolate between two values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="amount"></param>
        /// <returns>Interpolated result.</returns>
        public static double Lerp(double x, double y, double amount)
        {
            return x + (y - x) * amount;
        }
    }
}
