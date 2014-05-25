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
        private readonly IDataRequestor _requestor;

        public WorkoutDataProvider(IDataRequestor requestor)
        {
            _requestor = requestor;
        }

        public async Task<IEnumerable<WorkoutDataModel>> GetWorkoutsForUserAsync(int userId)
        {
            var uri = new Uri(string.Format(ServiceUri, userId));
            //changed to string because of compile issues
            var workoutDataString = await _requestor.GetDataAsync<string>(uri);
            var workoutData = JObject.Parse(workoutDataString);

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
