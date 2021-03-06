﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WhatsMyRun.Services.DataProviders.Workouts;
using WhatsMyRun.Services.DataRequestor;

namespace WhatsMyRun.Tests
{
    [TestClass]
    public class WorkoutDataProviderTest
    {
        private static readonly string dataLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                                      "\\SampleWorkoutsData.json";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

        }

        [TestMethod]
        public async Task GetDataAsync_Default_WorkoutDataParsed()
        {
            //ARRANGE
            var mockRequestor = new Moq.Mock<IDataRequestor>();
            var mockedData = GetAllWorkoutsDataString();


            var provider = new WorkoutDataProvider(mockRequestor.Object);
            mockRequestor.Setup(r => r.GetDataAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult(mockedData));

            //ACT
            var workouts = await provider.GetWorkoutsForUserAsync(1);

            //For runs over _ (6mi for now), show the min/mile times over time.
            //so convert the distance to miles, then check it.
            var lastWorkout = workouts.Last();
            var speed = lastWorkout.AverageSpeedInMilesPerHour;
            TimeSpan mileTime = lastWorkout.AverageTimePerMile;
            var miles = lastWorkout.DistanceInMiles;

            Trace.WriteLine("Workout count: " + workouts.Count());
            //started to parse the data for ALL workouts, but not all workouts have this data
            //(ie workout #20 doesn't have AverageSpeedInX
            foreach (var workout in workouts)
            {
                //Assert.IsTrue(workout.ActiveTimeInSeconds > 0, "ActiveTimeInSeconds not set");
                //Assert.IsTrue(workout.AverageSpeedInX > 0, "AverageSpeedInX not set");
                Assert.IsTrue(workout.DistanceInMeters > 0, "DistanceInMeters not set");
                //Assert.IsTrue(workout.ElapsedTimeInSeconds.Seconds > 0, "ElapsedTimeInSeconds not set");
                //Assert.IsTrue(workout.MetabolicEnergeyTotal > 0, "MetabolicEnergeyTotal not set");
                Assert.IsTrue(workout.StartTime > DateTime.Parse("1/1/2010"));
            }

            var firstWorkout = workouts.First();
            //Certain property are only set on the first of the test data, just assert that:
            Assert.AreEqual(3600, firstWorkout.ActiveTimeInSeconds);
            Assert.AreEqual(2.3290784, firstWorkout.AverageSpeedInMetersPerSecond);
            Assert.AreEqual(8384.68224, firstWorkout.DistanceInMeters);
            Assert.AreEqual(3600, firstWorkout.ElapsedTimeInSeconds.TotalSeconds);
            Assert.AreEqual(2953904.0, firstWorkout.MetabolicEnergeyTotal);
            Assert.AreEqual("test notes", firstWorkout.Notes);
            Assert.AreEqual(DateTime.Parse("2014-01-26T18:00:00+00:00"), firstWorkout.StartTime);
            Assert.AreEqual(200, firstWorkout.TotalSteps);

            //Wanted this in a sep test, but with async, I couldn't figure out how to have the work done in ClassInitialize
            Assert.AreEqual(20, workouts.Count(), "workout count differs");
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

            ////To see the test fail, bring back this line:
            //mockRequestor.Setup(r => r.GetDataAsync(new Uri("http://test.com")))
            //    .Returns(Task.FromResult(mockedData));

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

        private string GetAllWorkoutsDataString()
        {
            var mockedData = File.ReadAllText(dataLocation);
            var workoutData = JObject.Parse(mockedData);

            return workoutData.ToString();
        }

        private string GetSingleWorkoutDataString()
        {
            var workoutData = JObject.Parse(GetAllWorkoutsDataString());
            
            RemoveAllButOneWorkout(workoutData);

            //also set workouts to 1 in case code is using that
            workoutData["total_count"] = "1";

            return workoutData.ToString();
        }

        private static void RemoveAllButOneWorkout(JObject workoutData)
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
