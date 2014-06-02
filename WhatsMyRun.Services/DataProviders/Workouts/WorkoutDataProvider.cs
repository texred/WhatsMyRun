using System;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WhatsMyRun.DataModel.Workouts;
using WhatsMyRun.Services.DataRequestor;
using System.Linq;

namespace WhatsMyRun.Services.DataProviders.Workouts
{
    public class WorkoutDataProvider : IWorkoutDataProvider
    {
        private const string ServiceUri = "https://oauth2-api.mapmyapi.com/v7.0/workout/?user={0}";
        private readonly IDataRequestor _requestor;

        public WorkoutDataProvider(IDataRequestor requestor)
        {
            _requestor = requestor;
        }

        public async Task<IEnumerable<WorkoutDataModel>> GetWorkoutsForUserAsync(int userId)
        {
            var uri = new Uri(string.Format(ServiceUri, userId));
            //changed to string because of compile issues
            var workoutDataString = await _requestor.GetDataAsync(uri);
            var workoutData = JObject.Parse(workoutDataString);

            var workouts = new List<WorkoutDataModel>();

            var wCount = workoutData["_embedded"]["workouts"].Children().Count();
            foreach (JObject workoutObj in workoutData["_embedded"]["workouts"])
            {
                var workout = ParseWorkout(workoutObj);
                workouts.Add(workout);
            }
            return workouts;
        }

        private static WorkoutDataModel ParseWorkout(JObject workoutObj)
        {
            var workout = new WorkoutDataModel();

            workout.ActiveTimeInSeconds = workoutObj["aggregates"]["active_time_total"].ValueWithDefault<double>();
            workout.AverageSpeedInX = workoutObj["aggregates"]["speed_avg"].ValueWithDefault<double>();
            workout.DistanceInMeters = workoutObj["aggregates"]["distance_total"].ValueWithDefault<double>();
            workout.ElapsedTimeInSeconds = TimeSpan.FromSeconds(workoutObj["aggregates"]["elapsed_time_total"].ValueWithDefault<double>());
            workout.MetabolicEnergeyTotal = workoutObj["aggregates"]["metabolic_energy_total"].ValueWithDefault<double>();
            workout.Notes = workoutObj["notes"].ValueWithDefault<string>();
            workout.StartTime = workoutObj["start_datetime"].ValueWithDefault<DateTime>();
            workout.TotalSteps = workoutObj["aggregates"]["steps_total"].ValueWithDefault<int>();

            return workout;
        }
    }
}
