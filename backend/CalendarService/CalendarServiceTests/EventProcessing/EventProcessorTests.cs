using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalendarService.EventProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarService.EventProcessing.Tests
{
    [TestClass()]
    public class EventProcessorTests
    {
        [TestMethod()]
        public void BuildHtmlHeaderTest()
        {
            EventProcessor myEvent = new EventProcessor(null);
            string test = myEvent.BuildHtmlHeader(DateTime.Now);
            Assert.Fail();
        }
    }
}