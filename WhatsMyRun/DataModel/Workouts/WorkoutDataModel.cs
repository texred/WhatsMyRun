using System;

namespace WhatsMyRun.DataModel.Workouts
{
    public class WorkoutDataModel
    {
        public DateTime StartTime { get; set; }
        public string Notes { get; set; }
        public TimeSpan ElapsedTimeInSeconds { get; set; }
        public double ActiveTimeInSeconds { get; set; }
        public double DistanceInMeters { get; set; }
        public int TotalSteps { get; set; }
        public double AverageSpeedInX { get; set; }
        public double MetabolicEnergeyTotal { get; set; }
    }
}
