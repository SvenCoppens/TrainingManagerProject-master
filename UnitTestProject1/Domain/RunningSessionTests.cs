using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using DataLayer;
using UnitTestProject1;

namespace DomainLibrary.Domain.Tests
{
    [TestClass()]
    public class RunningSessionTests
    {
        [TestMethod]
        public void TestCorrectBindings()
        {
            DateTime now = DateTime.Now;
            int distance = 2000;
            TimeSpan time = new TimeSpan(2, 3, 4);
            float speed = 4.8f;
            TrainingType training = TrainingType.Endurance;
            string comment = "test comment";

            RunningSession runningSession = new RunningSession(now, distance, time, speed, training, comment);

            Assert.AreEqual(runningSession.When, now, "StartTime was not bound properly");
            Assert.AreEqual(runningSession.Distance, distance, "Distance was not bound properly");
            Assert.AreEqual(runningSession.Time, time, "Duration was not bound properly");
            Assert.AreEqual(runningSession.AverageSpeed, speed, "Average Speed was not bound properly");
            Assert.AreEqual(runningSession.TrainingType, training, "TrainingType was not bound properly");
            Assert.AreEqual(runningSession.Comments, comment, "Comments was not bound properly");
        }
       [TestMethod]
       public void TestNullAcceptance()
        {
            DateTime now = DateTime.Now;
            int distance = 2000;
            TimeSpan time = new TimeSpan(2, 3, 4);
            float? speed = 20;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            RunningSession runningSession = new RunningSession(now, distance, time, speed, training, comment);

            Assert.IsNull(runningSession.Comments, "Comments did not accept null");
            Assert.IsNotNull(runningSession.AverageSpeed,"AverageSpeed was made Null");
            Assert.IsNotNull(runningSession.When, "When was also made null");
            Assert.IsNotNull(runningSession.Distance, "Distance was also made null");
            Assert.IsNotNull(runningSession.Time, "Time was also made null");
            Assert.IsNotNull(runningSession.TrainingType, "TrainingType was also made null");
        }
        [TestMethod]
        public void TestNullAverageSpeed_ShouldBeCalculatedAtCreation()
        {
            DateTime now = DateTime.Now;
            int distance = 21000;
            TimeSpan time = new TimeSpan(2, 0, 0);
            float? speed = null;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            RunningSession runningSession = new RunningSession(now, distance, time, speed, training, comment);
            float expectation = 10.5f;

            Assert.IsNotNull(runningSession.AverageSpeed, "No attempt at a calculation was made");
            Assert.AreEqual(expectation, runningSession.AverageSpeed,"AverageSpeed was not calculated correctly");
        }
        [TestMethod]
        public void DateTimeShouldNotBeInFuture_ShouldThrowDomainException()
        {
            DateTime now = DateTime.Now.AddMinutes(10);
            int distance = 2000;
            TimeSpan time = new TimeSpan(2, 3, 4);
            float? speed = null;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            Assert.ThrowsException<DomainException>(() => m.AddRunningTraining(now,distance,time,speed,training,comment));
        }
        [TestMethod]
        public void DistanceShouldNotBeNegative_ShouldThrowDomainException()
        {
            DateTime now = DateTime.Now;
            int distance = -1;
            TimeSpan time = new TimeSpan(2, 3, 4);
            float? speed = null;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            Assert.ThrowsException<DomainException>(() => m.AddRunningTraining(now, distance, time, speed, training, comment));
        }
        [TestMethod]
        public void DistanceShouldNotBeMoreThan50ThousandMeters_ShouldThrowDomainException()
        {
            DateTime now = DateTime.Now;
            int distance = 50001;
            TimeSpan time = new TimeSpan(2, 3, 4);
            float? speed = null;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            Assert.ThrowsException<DomainException>(() => m.AddRunningTraining(now, distance, time, speed, training, comment));
        }
        [TestMethod]
        public void DurationShouldNotBeNegative_ShouldThrowDomainException()
        {
            DateTime now = DateTime.Now;
            int distance = 10000;
            TimeSpan time = new TimeSpan(-1);
            float? speed = null;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            Assert.ThrowsException<DomainException>(() => m.AddRunningTraining(now, distance, time, speed, training, comment));
        }
        [TestMethod]
        public void DurationShouldNotBeMoreThan20Hours_ShouldThrowDomainException()
        {
            DateTime now = DateTime.Now;
            int distance = 10000;
            TimeSpan time = new TimeSpan(20,0,1);
            float? speed = null;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            Assert.ThrowsException<DomainException>(() => m.AddRunningTraining(now, distance, time, speed, training, comment));
        }
        [TestMethod]
        public void AverageSpeedShouldNotBeNegative_ShouldThrowDomainException()
        {
            DateTime now = DateTime.Now;
            int distance = 10000;
            TimeSpan time = new TimeSpan(2, 0, 1);
            float? speed = -1;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            Assert.ThrowsException<DomainException>(() => m.AddRunningTraining(now, distance, time, speed, training, comment));
        }
        [TestMethod]
        public void AverageSpeedShouldNotBeMoreThan30_ShouldThrowDomainException()
        {
            DateTime now = DateTime.Now;
            int distance = 10000;
            TimeSpan time = new TimeSpan(2, 0, 1);
            float? speed = 31;
            TrainingType training = TrainingType.Endurance;
            string comment = null;

            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            Assert.ThrowsException<DomainException>(() => m.AddRunningTraining(now, distance, time, speed, training, comment));
        }

    }
}
