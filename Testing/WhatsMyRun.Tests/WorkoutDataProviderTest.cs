using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatsMyRun.Services.DataRequestor;
using Moq;
using WhatsMyRun.Services.DataProviders.Workouts;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace WhatsMyRun.Tests
{
    [TestClass]
    public class WorkoutDataProviderTest
    {
        private string dataLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SampleWorkoutsData.json";
        [TestMethod]
        public void WorkoutDataProvider_Default_CallsRequestor()
        {
            //ARRANGE
            var mockRequestor = new Moq.Mock<IDataRequestor>();
            var mockedData = File.ReadAllText(dataLocation);
            var provider = new WorkoutDataProvider(mockRequestor.Object);
            mockRequestor.Setup(r => r.GetDataAsync<string>(It.IsAny<Uri>()))
                .Returns(Task.FromResult(mockedData));

            //To see the test fail, bring back this line:
            //mockRequestor.Setup(r => r.GetDataAsync<int>(It.IsAny<Uri>()))
            //    .Returns(Task.FromResult(1));

            //ACT
            provider.GetWorkoutsForUserAsync(1)
                .Wait();
            mockRequestor.VerifyAll();
        }
    }
}
