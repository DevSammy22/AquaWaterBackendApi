using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AquaWater.Data.Services.Utilities
{
    public class Validation
    {
        public static bool EmailValidator(string check)
        {
            string emailPattern = "^[a-zA-Z][A-Za-z0-9._]*@[a-z]+\\.[a-z]{2,3}$";
            if (Regex.IsMatch(check, emailPattern) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool PasswordValidator(string check)
        {
            string passwordPattern = "^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&!^.])[A-Za-z\\d@$!%*#?&.]{6,}$";
            if (Regex.IsMatch(check, passwordPattern) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool NameValidator(string check)
        {
            string namePattern = "^[A-Z]{1}[a-z]{2,}$";
            if (Regex.IsMatch(check, namePattern) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static double Latitiude(double lat)
        {
            if (lat < -90 || lat > 90)
            {
                throw new ArgumentException("Latitude must be between -90 and 90 degrees inclusive.");
            }
            return lat;
        }

        public static double Longitude(double lon)
        {
            if (lon < -180 || lon > 180)
            {
                throw new ArgumentException("Latitude must be between -90 and 90 degrees inclusive.");
            }
            return lon;
        }
    }
}
