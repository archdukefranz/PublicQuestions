using System;
using System.Text;
using NUnit.Framework;
using NMock2;
using System.Data;

namespace PublicQuestions.UnitTests
{
    [TestFixture]
    public class BasicTest
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType);
        private Mockery _mockSQLDatabse;

        [SetUp]
        public void TestSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log.Info("Starting up for testing");
        }

        [TearDown]
        public void TestShutDown()
        {
            _log.Info("Shutting down test");
        }


        [Test(Description = "Test the service")]
        public void ServiceErrorTest()
        {
        }
    }
}