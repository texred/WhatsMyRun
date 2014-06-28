using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatsMyRun.DataModel.Workouts;

namespace WhatsMyRun.Tests
{
    /// <summary>
    /// Summary description for WorkoutDataModelTest
    /// </summary>
    [TestClass]
    public class WorkoutDataModelTest
    {
        public WorkoutDataModelTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void DistanceInMeters_60_DistanceInMilesAccurate()
        {

            var workout = new WorkoutDataModel();
            workout.DistanceInMeters= 60.05;

            Assert.AreEqual(0.03731332855, workout.DistanceInMiles);
        }

        [TestMethod]
        public void DistanceInMeters_0_DistanceInMiles0()
        {

            var workout = new WorkoutDataModel();
            workout.DistanceInMeters = 0;

            Assert.AreEqual(0, workout.DistanceInMiles);
        }
        [TestMethod]
        public void WorkoutDataModel_9Min14Mile_SpeedPropertiesAccurate()
        {
            var workout = new WorkoutDataModel();
            workout.AverageSpeedInMetersPerSecond = 2.881930031;

            Assert.AreEqual(6.446691883053, workout.AverageSpeedInMilesPerHour, .000000001);
            
            Assert.AreEqual(new TimeSpan(0, 9, 18), workout.AverageTimePerMile);
        }

        [TestMethod]
        public void WorkoutDataModel_0Speed_SpeedPropertiesAccurate()
        {
            var workout = new WorkoutDataModel();
            workout.AverageSpeedInMetersPerSecond = 0;

            Assert.AreEqual(0, workout.AverageSpeedInMilesPerHour);
            Assert.AreEqual(new TimeSpan(0), workout.AverageTimePerMile);
        }
    }
}
