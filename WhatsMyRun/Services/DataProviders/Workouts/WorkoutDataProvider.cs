using System;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WhatsMyRun.DataModel.Workouts;
using WhatsMyRun.Services.DataRequestor;

namespace WhatsMyRun.Services.DataProviders.Workouts
{
    public class WorkoutDataProvider
    {
        private string ServiceUri = "https://oauth2-api.mapmyapi.com/v7.0/workout/?user={0}";

        public async Task<IEnumerable<WorkoutDataModel>> GetWorksForUserAsync(int userId)
        {
            var requestor = ServiceLocator.Current.GetInstance<IDataRequestor>();
            var uri = new Uri(string.Format(ServiceUri, userId));
            var workoutData = await requestor.GetDataAsync(uri);

            var workouts = new List<WorkoutDataModel>();
            
            foreach (JObject workoutObj in workoutData.GetValue("workout"))
            {
                var workout = new WorkoutDataModel();
                workout.ActiveTimeInSeconds = workoutObj.GetValue("aggregates$active_time_total").Value<double>();
                workouts.Add(workout);
            }
            return workouts;
        }
    }
}
