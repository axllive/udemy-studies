using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            int age = today.Year - dob.Year;
            
            if (dob > today.AddYears(-age)) age--;

            return age;
        }
    }
}