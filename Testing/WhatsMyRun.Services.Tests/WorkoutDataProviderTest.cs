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
using Newtonsoft.Json;

namespace WhatsMyRun.Tests
{
    [TestClass]
    public class WorkoutDataProviderTest
    {
        static Moq.Mock<IDataRequestor> mockRequestor = new Moq.Mock<IDataRequestor>();
        static IEnumerable<WorkoutDataModel> workouts;
        private static readonly string dataLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SampleWorkoutsData.json";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //ARRANGE
            var mockedData = File.ReadAllText(dataLocation);
            var provider = new WorkoutDataProvider(mockRequestor.Object);
            mockRequestor.Setup(r => r.GetDataAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(mockedData));

            //To see the test fail, bring back this line:
            mockRequestor.Setup(r => r.GetDataAsync(new Uri("http://test.com")))
                .Returns(Task.FromResult(mockedData));

            //ACT
            workouts = provider.GetWorkoutsForUserAsync(1).Result;
        }


        [TestMethod]
        public void GetDataAsync_MultipleWorkouts_20Results()
        {
            Assert.AreEqual(20, workouts.Count());
        }

        [TestMethod]
        public void GetDataAsync_Default_WorkoutDataParsed()
        {
            foreach (var workout in workouts)
            {
                Assert.IsTrue(workout.ActiveTimeInSeconds > 0, "ActiveTimeInSeconds not set");
                Assert.IsTrue(workout.AverageSpeedInX > 0, "AverageSpeedInX not set");
                Assert.IsTrue(workout.DistanceInMeters > 0, "DistanceInMeters not set");
                Assert.IsTrue(workout.ElapsedTimeInSeconds.Seconds > 0, "ElapsedTimeInSeconds not set");
                Assert.IsTrue(workout.MetabolicEnergeyTotal > 0, "MetabolicEnergeyTotal not set");
                Assert.IsTrue(workout.StartTime > DateTime.Parse("1/1/2010"));
                Assert.IsTrue(workout.TotalSteps > 0);
            }

            //notes was only set on the first of the test data, just assert that:
            Assert.IsFalse(String.IsNullOrWhiteSpace(workouts.First().Notes), "Notes not set");
        }

        [TestMethod]
        public async Task GetDataAsync_Malformed_ThrowsException()
        {
            //ARRANGE
            var mockRequestor = new Moq.Mock<IDataRequestor>();
            var mockedMalformedData = "<x></x>";
            var provider = new WorkoutDataProvider(mockRequestor.Object);
            mockRequestor.Setup(r => r.GetDataAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(mockedMalformedData));

            //ACT
            try
            {
                await provider.GetWorkoutsForUserAsync(1);
                Assert.Fail("no ex thrown");
            }
            catch (JsonReaderException ex)
            {
                var exMessage = ex.Message;
                //malformed data exception
            }

        }


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
