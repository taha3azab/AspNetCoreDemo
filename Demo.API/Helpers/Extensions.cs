using Microsoft.AspNetCore.Http;
using System;

namespace Demo.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime dateTime)
        {
            if (dateTime == null)
                return 0;
            var age = DateTime.Today.Year - dateTime.Year;
            if (dateTime.AddYears(age).Year > DateTime.Today.Year)
                age--;
            return age;
        }
    }
}