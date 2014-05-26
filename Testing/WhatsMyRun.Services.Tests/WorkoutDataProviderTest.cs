using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatsMyRun.Services.DataRequestor;
using Moq;
using WhatsMyRun.Services.DataProviders.Workouts;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WhatsMyRun.DataModel.Workouts;

namespace WhatsMyRun.Tests
{
    [TestClass]
    public class WorkoutDataProviderTest
    {
        private static const string dataLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SampleWorkoutsData.json";
        
        [TestMethod]
        public async Task GetDataAsync_Default_CallsRequestor()
        {
            //ARRANGE
            var mockRequestor = new Moq.Mock<IDataRequestor>();
            var mockedData = File.ReadAllText(dataLocation);
            var provider = new WorkoutDataProvider(mockRequestor.Object);
            mockRequestor.Setup(r => r.GetDataAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(mockedData));

            //To see the test fail, bring back this line:
            mockRequestor.Setup(r => r.GetDataAsync(new Uri("http://test.com")))
                .Returns(Task.FromResult(mockedData));

            //ACT
            await provider.GetWorkoutsForUserAsync(1);
            mockRequestor.VerifyAll();
        }

        [TestMethod]
        public async Task GetDataAsync_SingleWorkout_SingleResult()
        {
            //ARRANGE
            var mockRequestor = new Moq.Mock<IDataRequestor>();
            var mockedData = GetSingleWorkoutDataString();

            var provider = new WorkoutDataProvider(mockRequestor.Object);
            mockRequestor.Setup(r => r.GetDataAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(mockedData));

            //ACT
            var workouts = await provider.GetWorkoutsForUserAsync(1);
            mockRequestor.VerifyAll();
            Assert.AreEqual(1, workouts.Count());
        }

        private string GetSingleWorkoutDataString()
        {
            var mockedData = File.ReadAllText(dataLocation);
            var workoutData = JObject.Parse(mockedData);
            var firstWorkout = workoutData["_embedded"]["workouts"].First();

            RemoveAllOneWorkout(workoutData);

            //also set workouts to 1 in case code is using that
            workoutData["total_count"] = "1";

            mockedData = workoutData.ToString();
            return mockedData;
        }

        private static void RemoveAllOneWorkout(JObject workoutData)
        {
            var workoutsToRemove = new List<JToken>();

            foreach (var workout in workoutData["_embedded"]["workouts"].Skip(1))
            {
                workoutsToRemove.Add(workout);
            }

            foreach (var workout in workoutsToRemove)
            {
                workout.Remove();
            }
        }
    }
}
