using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace WhatsMyRun.Services.Tests
{
    [TestClass]
    public class WorkoutDataProviderTest
    {
        [TestMethod]
        public void GetWorkoutsForUserAsync_Default_CallsDataProvider()
        {
            Assert.Fail("unwritten");
            //Moq 4.2 by default doesn't work with win store apps
            //will see about making the other projects be PCLs, but that means that any win classes can't go in the service.
        }
    }
}
