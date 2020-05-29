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
    public class CyclingSessionTests
    {
        [TestMethod]
        public void TestAverageSpeedCalculation()
        {
            //Arrange
            DateTime date = DateTime.Now;
            float distance = 25;
            TimeSpan time = new TimeSpan(2, 0, 0);

            //act
            CyclingSession cs = new CyclingSession(date, distance, time, null, null, TrainingType.Endurance, "", BikeType.CityBike);
            float expectation = 12.5f;

            //Assert
            Assert.IsNotNull(cs.AverageSpeed, "No Attempt at a calculation was made");
            Assert.AreEqual(cs.AverageSpeed, expectation, "AverageSpeed was not calculated correctly when not given");
        }
        [TestMethod]
        public void TestCorrectBindings()
        {
            //Arrange
            DateTime now = DateTime.Now;
            float distance = 5.4f;
            TimeSpan time = new TimeSpan(2, 3, 4);
            float speed = 4.8f;
            int watt = 12;
            TrainingType training = TrainingType.Endurance;
            string comment = "test comment";
            BikeType bike = BikeType.CityBike;

            //Act
            CyclingSession cyclingSession = new CyclingSession(now, distance, time, speed, watt, training, comment, bike);

            //Assert
            Assert.AreEqual(cyclingSession.When, now, "StartTime was not bound properly");
            Assert.AreEqual(cyclingSession.Distance, distance, "Distance was not bound properly");
            Assert.AreEqual(cyclingSession.Time, time, "Duration was not bound properly");
            Assert.AreEqual(cyclingSession.AverageSpeed, speed, "Average Speed was not bound properly");
            Assert.AreEqual(cyclingSession.AverageWatt, watt, "AverageWatt was not bound properly");
            Assert.AreEqual(cyclingSession.TrainingType, training, "TrainingType was not bound properly");
            Assert.AreEqual(cyclingSession.Comments, comment, "Comments was not bound properly");
            Assert.AreEqual(cyclingSession.BikeType, bike, "BikeType was not bound properly");
        }
        [TestMethod]
        public void TestNullAcceptance()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(2, 3, 4);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;

            //Act
            CyclingSession cyclingSession = new CyclingSession(now, null, time, null, null, training, null, bike);

            //Assert
            Assert.IsNull(cyclingSession.AverageSpeed, "AverageSpeed did not accept null");
            Assert.IsNull(cyclingSession.Distance, "Distance did not accept null");
            Assert.IsNull(cyclingSession.AverageWatt, "Wattage did not accept null");
            Assert.IsNull(cyclingSession.Comments, "comments did not accept null");
            Assert.IsNotNull(cyclingSession.When, "When was also made null");
            Assert.IsNotNull(cyclingSession.Time, "Time was also made null");
            Assert.IsNotNull(cyclingSession.TrainingType, "TrainingType was also made null");
            Assert.IsNotNull(cyclingSession.BikeType, "BikeType was also made null");
        }
        [TestMethod]
        public void DateTimeInFutureDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now.AddMinutes(10);
            TimeSpan time = new TimeSpan(2, 3, 4);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, null, time, null, null, training, null, bike));
        }
        [TestMethod]
        public void DistanceShouldNotBeNegative_ShouldThrowDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(2, 3, 4);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;
            float distance = -1;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, distance, time, null, null, training, null, bike));
        }
        [TestMethod]
        public void DistanceShouldNotBeOver500Km_ShouldThrowDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(2, 3, 4);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;
            float distance = 501;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, distance, time, null, null, training, null, bike));
        }
        [TestMethod]
        public void DurationShouldNotBeLessThan0_ShouldThrowDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(-1);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;
            float? distance = null;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, distance, time, null, null, training, null, bike));
        }
        [TestMethod]
        public void DurationShouldNotBeMoreThan20Hours_ShouldThrowDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(20, 0, 1);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;
            float? distance = null;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, distance, time, null, null, training, null, bike));
        }
        [TestMethod]
        public void AverageSpeedShouldNotBeLessThan0_ShouldThrowDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(2, 0, 0);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;
            float? distance = null;
            float? averageSpeed = -1;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, distance, time, averageSpeed, null, training, null, bike));
        }
        [TestMethod]
        public void AverageSpeedShouldNotBeMoreThan60_ShouldThrowDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(2, 0, 0);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;
            float? distance = null;
            float? averageSpeed = 61;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, distance, time, averageSpeed, null, training, null, bike));
        }
        [TestMethod]
        public void AverageWattShouldNotBeLessThan0_ShouldThrowDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(2, 0, 0);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;
            float? distance = null;
            float? averageSpeed = 30;
            int watt = -1;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, distance, time, averageSpeed, watt, training, null, bike));
        }
        [TestMethod]
        public void AverageWattShouldNotBeMoreThan800_ShouldThrowDomainException()
        {
            //Arrange
            DateTime now = DateTime.Now;
            TimeSpan time = new TimeSpan(2, 0, 0);
            TrainingType training = TrainingType.Endurance;
            BikeType bike = BikeType.CityBike;
            float? distance = null;
            float? averageSpeed = 30;
            int watt = 801;

            //Act
            TrainingManager m = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));

            //Assert
            Assert.ThrowsException<DomainException>(() => m.AddCyclingTraining(now, distance, time, averageSpeed, watt, training, null, bike));
        }
    }
}
