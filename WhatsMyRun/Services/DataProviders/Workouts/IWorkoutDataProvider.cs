using System.Collections.Generic;
using System.Threading.Tasks;
using WhatsMyRun.DataModel.Workouts;

namespace WhatsMyRun.Services.DataProviders.Workouts
{
    public interface IWorkoutDataProvider
    {
        Task<IEnumerable<WorkoutDataModel>> GetWorksForUser(int userId);
    }
}
