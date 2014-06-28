using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMyRun.DataModel
{
    public class UnitConverter
    {
        private const double MILES_IN_A_METER = 0.000621371;
        public static double ConvertMetersToMiles(double meters)
        {
            return meters * MILES_IN_A_METER;
        }
    }
}
