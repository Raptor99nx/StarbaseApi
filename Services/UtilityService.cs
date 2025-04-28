namespace StargateAPI.Services
{
    public class UtilityService
    {
        /// <summary>
        /// Drill down into the inner exceptions to get the full exception message(s).
        /// </summary>
        /// <param name="exception">The Exception being thrown to parse.</param>
        /// <returns>A string with all error messages, including any inner exceptions.</returns>
        public static string GetFullExceptionMessage(Exception exception)
        { 
            if (exception == null)
                return string.Empty;

            return exception.Message + " " + GetFullExceptionMessage(exception.InnerException);
        }
    }
}
