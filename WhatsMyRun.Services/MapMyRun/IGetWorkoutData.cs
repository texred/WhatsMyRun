using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsMyRun.Services.MapMyRun.Responses.WorkoutResponse;

namespace WhatsMyRun.Services.MapMyRun
{
    public interface IGetWorkoutData
    {
        WorkoutResponse GetWorkoutData(int workoutID);
    }
}
