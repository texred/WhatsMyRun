using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsMyRun.Services.MapMyRun.Responses.WorkoutResponse;

namespace WhatsMyRun.Services.MapMyRun
{
    public class MapMyFitnessDataFetcher : IGetWorkoutData
    {
        public Responses.WorkoutResponse.WorkoutResponse GetWorkoutData(int workoutID)
        {
            //TODO Also needs to take into account OAuth to get the data. With this being a "DAL", that may muck up the design having the popup to the user to get their approval for auth

            //Alternatively, in a testing project, use Moq to change this type of data and just pass in the IGetWorkoutData to your business layer
            return new Responses.WorkoutResponse.WorkoutResponse()
            {
                start_datetime = DateTime.UtcNow.Date,
                name = "my previous workout",
                updated_datetime = DateTime.UtcNow,
                created_datetime = DateTime.UtcNow,
                aggregates = new Responses.WorkoutResponse.Aggregates()
                {
                    active_time_total = 4320,
                    elapsed_time_total = 4320,
                    distance_total = 12956.31355392F,
                    speed_avg = 2.99146656F
                },
                _links = new Responses.WorkoutResponse._Links()
                {
                    route = new Route[] { new Route() { id = "418360940" } }
                }
            };
            //throw new NotImplementedException();
        }
    }
}
