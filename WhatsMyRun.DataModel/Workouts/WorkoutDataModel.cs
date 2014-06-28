using System;

namespace WhatsMyRun.DataModel.Workouts
{
    //https://www.mapmyapi.com/docs/Units
    public class WorkoutDataModel
    {
        public DateTime StartTime { get; set; }
        public string Notes { get; set; }
        public TimeSpan ElapsedTimeInSeconds { get; set; }
        public double ActiveTimeInSeconds { get; set; }
        public double DistanceInMeters { get; set; }

        public double DistanceInMiles
        {
            get
            {
                return UnitConverter.ConvertMetersToMiles(DistanceInMeters);
            }
        }
        public int TotalSteps { get; set; }
        public double AverageSpeedInMetersPerSecond { get; set; }

        public double AverageSpeedInMilesPerHour
        {
            get 
            {
                return UnitConverter.ConvertMetersToMiles(AverageSpeedInMetersPerSecond * 3600);
            }
        }
        public double MetabolicEnergeyTotal { get; set; }

        public TimeSpan AverageTimePerMile
        {
            get
            {
                double minutesWithFractionedSecondsPerMile = 0;
                
                if (AverageSpeedInMilesPerHour > 0)
                    minutesWithFractionedSecondsPerMile = 60 / AverageSpeedInMilesPerHour;

                var minutesPerMile = (int)minutesWithFractionedSecondsPerMile;
                var secondsPerMile = (int)((minutesWithFractionedSecondsPerMile - minutesPerMile) * 60);// * by seconds in a minute

                //get decimal portion, and * 60
                return new TimeSpan(0, minutesPerMile, secondsPerMile);
            }
        }
    }
}
