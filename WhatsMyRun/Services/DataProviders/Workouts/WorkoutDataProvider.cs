using System;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WhatsMyRun.DataModel.Workouts;
using WhatsMyRun.Services.DataRequestor;

namespace WhatsMyRun.Services.DataProviders.Workouts
{
    public class WorkoutDataProvider : IWorkoutDataProvider
    {
        private const string ServiceUri = "https://oauth2-api.mapmyapi.com/v7.0/workout/?user={0}";

        public async Task<IEnumerable<WorkoutDataModel>> GetWorkoutsForUserAsync(int userId)
        {
            var requestor = ServiceLocator.Current.GetInstance<IDataRequestor>();
            var uri = new Uri(string.Format(ServiceUri, userId));
            var workoutData = await requestor.GetDataAsync(uri);

            var workouts = new List<WorkoutDataModel>();
            
            foreach (JObject workoutObj in workoutData["_embedded"]["workouts"]) //workoutData.GetValue("_embedded$workouts"))
            {
                var workout = new WorkoutDataModel();
                workout.ActiveTimeInSeconds = workoutObj["aggregates"]["active_time_total"].Value<double>();
                workouts.Add(workout);
            }
            return workouts;
        }
    }
}
