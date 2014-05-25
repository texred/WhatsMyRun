using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatsMyRun.Services.DataRequestor;
using Moq;

namespace WhatsMyRun.Tests
{
    [TestClass]
    public class WorkoutDataProviderTest
    {
        [TestMethod]
        public void WorkoutDataProvider_Default_CallsRequestor()
        {
            var mockRequestor = new Moq.Mock<IDataRequestor>();
            //new WorkoutDataProvider(mockRequestor.Object);
            //mockRequestor.Verify(r => r.GetDataAsync(It.IsAny<Uri>()), Moq.Times.Once);

            Assert.Fail("unwritten");
            //WorkoutDataProvider
        }
    }
}
