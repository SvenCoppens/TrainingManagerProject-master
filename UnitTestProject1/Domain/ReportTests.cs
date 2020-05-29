using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using DataLayer;
using UnitTestProject1;
using Microsoft.VisualStudio.TestPlatform.Common;

namespace DomainLibrary.Domain.Tests
{
    [TestClass()]
    public class ReportTests
    {
        [TestMethod()]
        public void TestMonthlyRunningReport()
        {
            //Arrange
            TrainingManager addingManager = new TrainingManager(new UnitOfWork(new TrainingContextTest()));
            DateTime fastestRunTime = new DateTime(2010, 4, 12);
            DateTime ignoredSessionTime = new DateTime(2000, 2, 5);
            DateTime longestRunTime = new DateTime(2010, 4, 10);


            RunningSession fastestRun = new RunningSession(fastestRunTime, 20000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession secFastRun = new RunningSession(ignoredSessionTime, 20000, new TimeSpan(5, 0, 0), null, TrainingType.Endurance, null);
            RunningSession longestRun = new RunningSession(longestRunTime, 25000, new TimeSpan(10, 0, 0), null, TrainingType.Endurance, null);

            addingManager.AddRunningTraining(fastestRun.When,fastestRun.Distance,fastestRun.Time,fastestRun.AverageSpeed,fastestRun.TrainingType,fastestRun.Comments);
            addingManager.AddRunningTraining(secFastRun.When, secFastRun.Distance, secFastRun.Time, secFastRun.AverageSpeed, secFastRun.TrainingType, secFastRun.Comments);
            addingManager.AddRunningTraining(longestRun.When, longestRun.Distance, longestRun.Time, longestRun.AverageSpeed, longestRun.TrainingType, longestRun.Comments);

            DateTime secondRunTime = new DateTime(2010, 5, 10);
            DateTime firstRunTime = new DateTime(2010, 5, 1);
            DateTime thirdRunTime = new DateTime(2010, 5, 31);
            DateTime laterRunTime = new DateTime(2010, 6, 1);
            DateTime earlyRunTime = new DateTime(2010, 4, 30);

            RunningSession secondRun = new RunningSession(secondRunTime,5000,new TimeSpan(1,0,0),null,TrainingType.Endurance,null);
            RunningSession firstRun = new RunningSession(firstRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession thirdRun = new RunningSession(thirdRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession laterRun = new RunningSession(laterRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession earlyRun = new RunningSession(earlyRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);

            addingManager.AddRunningTraining(secondRun.When, secondRun.Distance, secondRun.Time, secondRun.AverageSpeed, secondRun.TrainingType, secondRun.Comments);
            addingManager.AddRunningTraining(firstRun.When, firstRun.Distance, firstRun.Time, firstRun.AverageSpeed, firstRun.TrainingType, firstRun.Comments);
            addingManager.AddRunningTraining(thirdRun.When, thirdRun.Distance, thirdRun.Time, thirdRun.AverageSpeed, thirdRun.TrainingType, thirdRun.Comments);
            addingManager.AddRunningTraining(laterRun.When, laterRun.Distance, laterRun.Time, laterRun.AverageSpeed, laterRun.TrainingType, laterRun.Comments);
            addingManager.AddRunningTraining(earlyRun.When, earlyRun.Distance, earlyRun.Time, earlyRun.AverageSpeed, earlyRun.TrainingType, earlyRun.Comments);

            //Act
            TrainingManager reportManager = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            int reportYear = 2010;
            int reportMonth = 5;
            Report report = reportManager.GenerateMonthlyRunningReport(reportYear, reportMonth);

            //Assert
            Assert.AreEqual(report.StartDate, new DateTime(reportYear, reportMonth, 1), "StartDate dit not match up");
            Assert.AreEqual(report.EndDate, new DateTime(reportYear, reportMonth, DateTime.DaysInMonth(reportYear, reportMonth)), "EndDate did not match up");

            Assert.IsNull(report.Rides, "Rides was not null");
            Assert.AreEqual(report.RunningSessions, 3,"Number of RunningSession was incorrect");
            Assert.AreEqual(report.TotalSessions, 3,"Number of TotalSessions was incorrect");
            Assert.AreEqual(report.Runs[0].When, firstRunTime,"The first element in Runs dit not match,probably not sorted by date properly");
            Assert.AreEqual(report.Runs[1].When, secondRunTime, "The second element in Runs dit not match");
            Assert.AreEqual(report.Runs[2].When, thirdRunTime, "The third element in Runs dit not match");

            Assert.AreEqual(report.MaxDistanceSessionRunning.When, longestRunTime,"Max Distance Run did not match up");
            Assert.AreEqual(report.MaxSpeedSessionRunning.When, fastestRunTime, "Max Speed Run did not match up");
            Assert.AreEqual(report.TotalRunningDistance, secondRun.Distance + firstRun.Distance + thirdRun.Distance);
            Assert.AreEqual(report.TotalRunningTrainingTime, secondRun.Time + firstRun.Time + thirdRun.Time);

            Assert.AreEqual(report.TimeLine[0].Item2, report.Runs[0], "Runs and Timeline did not match up");
            Assert.AreEqual(report.TimeLine[1].Item2, report.Runs[1], "Runs and Timeline did not match up");
            Assert.AreEqual(report.TimeLine[2].Item2, report.Runs[2], "Runs and Timeline did not match up");
        }

        [TestMethod()]
        public void TestMonthlyTotalReport()
        {
            //Arrange
            TrainingManager addingManager = new TrainingManager(new UnitOfWork(new TrainingContextTest()));
            DateTime fastestRideTime = new DateTime(2010, 4, 12);
            DateTime ignoredRideTime = new DateTime(2000, 2, 5);
            DateTime longestRideTime = new DateTime(2010, 4, 10);
            DateTime wattestRideTime = new DateTime(2005, 8, 6);

            DateTime fastestRunTime = new DateTime(2010, 4, 11);
            DateTime ignoredSessionTime = new DateTime(2000, 2, 4);
            DateTime longestRunTime = new DateTime(2010, 4, 15);

            CyclingSession fastestRide = new CyclingSession(fastestRideTime, 20, new TimeSpan(1, 0, 0), null,30, TrainingType.Endurance, null,BikeType.CityBike);
            CyclingSession secFastRide = new CyclingSession(ignoredRideTime, 20, new TimeSpan(5, 0, 0), null, 35, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession longestRide = new CyclingSession(longestRideTime, 25, new TimeSpan(10, 0, 0), null, 25, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession wattestRide = new CyclingSession(wattestRideTime, 10, new TimeSpan(1, 0, 0), null, 200, TrainingType.Endurance, null, BikeType.CityBike);

            RunningSession fastestRun = new RunningSession(fastestRunTime, 20000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession secFastRun = new RunningSession(ignoredSessionTime, 20000, new TimeSpan(5, 0, 0), null, TrainingType.Endurance, null);
            RunningSession longestRun = new RunningSession(longestRunTime, 25000, new TimeSpan(10, 0, 0), null, TrainingType.Endurance, null);

            addingManager.AddCyclingTraining(fastestRide.When, fastestRide.Distance, fastestRide.Time, fastestRide.AverageSpeed, fastestRide.AverageWatt, fastestRide.TrainingType, fastestRide.Comments,fastestRide.BikeType);
            addingManager.AddCyclingTraining(secFastRide.When, secFastRide.Distance, secFastRide.Time, secFastRide.AverageSpeed, secFastRide.AverageWatt, secFastRide.TrainingType, secFastRide.Comments, secFastRide.BikeType);
            addingManager.AddCyclingTraining(longestRide.When, longestRide.Distance, longestRide.Time, longestRide.AverageSpeed, longestRide.AverageWatt, longestRide.TrainingType, longestRide.Comments, longestRide.BikeType);
            addingManager.AddCyclingTraining(wattestRide.When, wattestRide.Distance, wattestRide.Time, wattestRide.AverageSpeed, wattestRide.AverageWatt, wattestRide.TrainingType, wattestRide.Comments, wattestRide.BikeType);

            addingManager.AddRunningTraining(fastestRun.When, fastestRun.Distance, fastestRun.Time, fastestRun.AverageSpeed, fastestRun.TrainingType, fastestRun.Comments);
            addingManager.AddRunningTraining(secFastRun.When, secFastRun.Distance, secFastRun.Time, secFastRun.AverageSpeed, secFastRun.TrainingType, secFastRun.Comments);
            addingManager.AddRunningTraining(longestRun.When, longestRun.Distance, longestRun.Time, longestRun.AverageSpeed, longestRun.TrainingType, longestRun.Comments);

            DateTime secondRideTime = new DateTime(2010, 5, 10);
            DateTime firstRideTime = new DateTime(2010, 5, 1);
            DateTime thirdRideTime = new DateTime(2010, 5, 31);
            DateTime laterRideTime = new DateTime(2010, 6, 1);
            DateTime earlyRideTime = new DateTime(2010, 4, 30);

            DateTime secondRunTime = new DateTime(2010, 5, 11);
            DateTime firstRunTime = new DateTime(2010, 5, 2);
            DateTime thirdRunTime = new DateTime(2010, 5, 30);
            DateTime laterRunTime = new DateTime(2010, 6, 2);
            DateTime earlyRunTime = new DateTime(2010, 4, 29);


            CyclingSession secondRide = new CyclingSession(secondRideTime, 5, new TimeSpan(1, 0, 0), null, 10, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession firstRide = new CyclingSession(firstRideTime, 5, new TimeSpan(1, 0, 0), null, 50, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession thirdRide = new CyclingSession(thirdRideTime, 5, new TimeSpan(1, 0, 0), null, 30, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession laterRide = new CyclingSession(laterRideTime, 5, new TimeSpan(1, 0, 0), null, 20, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession earlyRide = new CyclingSession(earlyRideTime, 5, new TimeSpan(1, 0, 0), null, 15, TrainingType.Endurance, null, BikeType.CityBike);

            RunningSession secondRun = new RunningSession(secondRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession firstRun = new RunningSession(firstRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession thirdRun = new RunningSession(thirdRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession laterRun = new RunningSession(laterRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);
            RunningSession earlyRun = new RunningSession(earlyRunTime, 5000, new TimeSpan(1, 0, 0), null, TrainingType.Endurance, null);


            addingManager.AddCyclingTraining(secondRide.When, secondRide.Distance, secondRide.Time, secondRide.AverageSpeed,secondRide.AverageWatt, secondRide.TrainingType, secondRide.Comments,secondRide.BikeType);
            addingManager.AddCyclingTraining(firstRide.When, firstRide.Distance, firstRide.Time, firstRide.AverageSpeed,firstRide.AverageWatt, firstRide.TrainingType, firstRide.Comments,firstRide.BikeType);
            addingManager.AddCyclingTraining(thirdRide.When, thirdRide.Distance, thirdRide.Time, thirdRide.AverageSpeed,thirdRide.AverageWatt, thirdRide.TrainingType, thirdRide.Comments,thirdRide.BikeType);
            addingManager.AddCyclingTraining(laterRide.When, laterRide.Distance, laterRide.Time, laterRide.AverageSpeed,laterRide.AverageWatt, laterRide.TrainingType, laterRide.Comments,laterRide.BikeType);
            addingManager.AddCyclingTraining(earlyRide.When, earlyRide.Distance, earlyRide.Time, earlyRide.AverageSpeed,earlyRide.AverageWatt, earlyRide.TrainingType, earlyRide.Comments,earlyRide.BikeType);

            addingManager.AddRunningTraining(secondRun.When, secondRun.Distance, secondRun.Time, secondRun.AverageSpeed, secondRun.TrainingType, secondRun.Comments);
            addingManager.AddRunningTraining(firstRun.When, firstRun.Distance, firstRun.Time, firstRun.AverageSpeed, firstRun.TrainingType, firstRun.Comments);
            addingManager.AddRunningTraining(thirdRun.When, thirdRun.Distance, thirdRun.Time, thirdRun.AverageSpeed, thirdRun.TrainingType, thirdRun.Comments);
            addingManager.AddRunningTraining(laterRun.When, laterRun.Distance, laterRun.Time, laterRun.AverageSpeed, laterRun.TrainingType, laterRun.Comments);
            addingManager.AddRunningTraining(earlyRun.When, earlyRun.Distance, earlyRun.Time, earlyRun.AverageSpeed, earlyRun.TrainingType, earlyRun.Comments);


            //Act
            TrainingManager reportManager = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            int reportYear = 2010;
            int reportMonth = 5;
            Report report = reportManager.GenerateMonthlyTrainingsReport(reportYear, reportMonth);

            //Assert
            Assert.AreEqual(report.StartDate, new DateTime(reportYear, reportMonth, 1),"StartDate dit not match up");
            Assert.AreEqual(report.EndDate, new DateTime(reportYear, reportMonth, DateTime.DaysInMonth(reportYear, reportMonth)),"EndDate did not match up");

            Assert.AreEqual(report.RunningSessions,3, "Number of RunningSessions was incorrect");
            Assert.AreEqual(report.CyclingSessions, 3, "Number of CyclingSessions was incorrect");
            Assert.AreEqual(report.TotalSessions, 6, "Number of TotalSessions was incorrect");
            Assert.AreEqual(report.Rides[0].When, firstRideTime, "The first element in Rides did not match, probably not sorted by date properly");
            Assert.AreEqual(report.Rides[1].When, secondRideTime, "The second element in Rides did not match");
            Assert.AreEqual(report.Rides[2].When, thirdRideTime, "The third element in Rides did not match");
            Assert.AreEqual(report.Runs[0].When, firstRunTime, "The first element in Runs did not match,probably not sorted by date properly");
            Assert.AreEqual(report.Runs[1].When, secondRunTime, "The second element in Runs did not match");
            Assert.AreEqual(report.Runs[2].When, thirdRunTime, "The third element in Runs did not match");

            Assert.AreEqual(report.MaxDistanceSessionCycling.When, longestRideTime, "Max Distance Ride did not match up");
            Assert.AreEqual(report.MaxSpeedSessionCycling.When, fastestRideTime, "Max Speed Ride did not match up");
            Assert.AreEqual(report.MaxWattSessionCycling.When, wattestRideTime, "Max Watt Ride did not match up");
            Assert.AreEqual(report.TotalCyclingDistance, secondRide.Distance + firstRide.Distance + thirdRide.Distance);
            Assert.AreEqual(report.TotalCyclingTrainingTime, secondRide.Time + firstRide.Time + thirdRide.Time);

            Assert.AreEqual(report.MaxDistanceSessionRunning.When, longestRunTime, "Max Distance Run did not match up");
            Assert.AreEqual(report.MaxSpeedSessionRunning.When, fastestRunTime, "Max Speed Run did not match up");
            Assert.AreEqual(report.TotalRunningDistance, secondRun.Distance + firstRun.Distance + thirdRun.Distance);
            Assert.AreEqual(report.TotalRunningTrainingTime, secondRun.Time + firstRun.Time + thirdRun.Time);

            Assert.AreEqual(report.TotalTrainingTime, report.TotalRunningTrainingTime + report.TotalCyclingTrainingTime, "TotalTrainingTime did not add up");

            Assert.AreEqual(report.TimeLine[0].Item2, report.Rides[0], "Timeline did not match up");
            Assert.AreEqual(report.TimeLine[0].Item1, SessionType.Cycling, "Type of first Item did not match up");
            Assert.AreEqual(report.TimeLine[1].Item2, report.Runs[0], "Timeline did not match up");
            Assert.AreEqual(report.TimeLine[1].Item1, SessionType.Running, "Type of second Item did not match up");
            Assert.AreEqual(report.TimeLine[2].Item2, report.Rides[1], "Timeline did not match up");
            Assert.AreEqual(report.TimeLine[2].Item1, SessionType.Cycling, "Type of third Item did not match up");
            Assert.AreEqual(report.TimeLine[3].Item2, report.Runs[1], "Timeline did not match up");
            Assert.AreEqual(report.TimeLine[3].Item1, SessionType.Running, "Type of fourth Item did not match up");
            Assert.AreEqual(report.TimeLine[4].Item2, report.Runs[2], "Timeline did not match up");
            Assert.AreEqual(report.TimeLine[4].Item1, SessionType.Running, "Type of fifth Item did not match up");
            Assert.AreEqual(report.TimeLine[5].Item2, report.Rides[2], "Timeline did not match up");
            Assert.AreEqual(report.TimeLine[5].Item1, SessionType.Cycling, "Type of sixth Item did not match up");
        }
        [TestMethod()]
        public void TestMonthlyCyclingReport()
        {
            //Arrange
            TrainingManager addingManager = new TrainingManager(new UnitOfWork(new TrainingContextTest()));
            DateTime fastestRideTime = new DateTime(2010, 4, 12);
            DateTime ignoredRideTime = new DateTime(2000, 2, 5);
            DateTime longestRideTime = new DateTime(2010, 4, 10);
            DateTime wattestRideTime = new DateTime(2005, 8, 6);

            CyclingSession fastestRide = new CyclingSession(fastestRideTime, 20, new TimeSpan(1, 0, 0), null, 30, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession secFastRide = new CyclingSession(ignoredRideTime, 20, new TimeSpan(5, 0, 0), null, 35, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession longestRide = new CyclingSession(longestRideTime, 25, new TimeSpan(10, 0, 0), null, 25, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession wattestRide = new CyclingSession(wattestRideTime, 10, new TimeSpan(1, 0, 0), null, 200, TrainingType.Endurance, null, BikeType.CityBike);

            addingManager.AddCyclingTraining(fastestRide.When, fastestRide.Distance, fastestRide.Time, fastestRide.AverageSpeed, fastestRide.AverageWatt, fastestRide.TrainingType, fastestRide.Comments, fastestRide.BikeType);
            addingManager.AddCyclingTraining(secFastRide.When, secFastRide.Distance, secFastRide.Time, secFastRide.AverageSpeed, secFastRide.AverageWatt, secFastRide.TrainingType, secFastRide.Comments, secFastRide.BikeType);
            addingManager.AddCyclingTraining(longestRide.When, longestRide.Distance, longestRide.Time, longestRide.AverageSpeed, longestRide.AverageWatt, longestRide.TrainingType, longestRide.Comments, longestRide.BikeType);
            addingManager.AddCyclingTraining(wattestRide.When, wattestRide.Distance, wattestRide.Time, wattestRide.AverageSpeed, wattestRide.AverageWatt, wattestRide.TrainingType, wattestRide.Comments, wattestRide.BikeType);

            DateTime secondRideTime = new DateTime(2010, 5, 10);
            DateTime firstRideTime = new DateTime(2010, 5, 1);
            DateTime thirdRideTime = new DateTime(2010, 5, 31);
            DateTime laterRideTime = new DateTime(2010, 6, 1);
            DateTime earlyRideTime = new DateTime(2010, 4, 30);

            CyclingSession secondRide = new CyclingSession(secondRideTime, 5, new TimeSpan(1, 0, 0), null, 10, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession firstRide = new CyclingSession(firstRideTime, 5, new TimeSpan(1, 0, 0), null, 50, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession thirdRide = new CyclingSession(thirdRideTime, 5, new TimeSpan(1, 0, 0), null, 30, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession laterRide = new CyclingSession(laterRideTime, 5, new TimeSpan(1, 0, 0), null, 20, TrainingType.Endurance, null, BikeType.CityBike);
            CyclingSession earlyRide = new CyclingSession(earlyRideTime, 5, new TimeSpan(1, 0, 0), null, 15, TrainingType.Endurance, null, BikeType.CityBike);

            addingManager.AddCyclingTraining(secondRide.When, secondRide.Distance, secondRide.Time, secondRide.AverageSpeed, secondRide.AverageWatt, secondRide.TrainingType, secondRide.Comments, secondRide.BikeType);
            addingManager.AddCyclingTraining(firstRide.When, firstRide.Distance, firstRide.Time, firstRide.AverageSpeed, firstRide.AverageWatt, firstRide.TrainingType, firstRide.Comments, firstRide.BikeType);
            addingManager.AddCyclingTraining(thirdRide.When, thirdRide.Distance, thirdRide.Time, thirdRide.AverageSpeed, thirdRide.AverageWatt, thirdRide.TrainingType, thirdRide.Comments, thirdRide.BikeType);
            addingManager.AddCyclingTraining(laterRide.When, laterRide.Distance, laterRide.Time, laterRide.AverageSpeed, laterRide.AverageWatt, laterRide.TrainingType, laterRide.Comments, laterRide.BikeType);
            addingManager.AddCyclingTraining(earlyRide.When, earlyRide.Distance, earlyRide.Time, earlyRide.AverageSpeed, earlyRide.AverageWatt, earlyRide.TrainingType, earlyRide.Comments, earlyRide.BikeType);

            //Act
            TrainingManager reportManager = new TrainingManager(new UnitOfWork(new TrainingContextTest(true)));
            int reportYear = 2010;
            int reportMonth = 5;
            Report report = reportManager.GenerateMonthlyCyclingReport(reportYear, reportMonth);

            //Assert
            Assert.AreEqual(report.StartDate, new DateTime(reportYear, reportMonth, 1), "StartDate dit not match up");
            Assert.AreEqual(report.EndDate, new DateTime(reportYear, reportMonth, DateTime.DaysInMonth(reportYear, reportMonth)), "EndDate did not match up");

            Assert.IsNull(report.Runs, "Runs was not null");
            Assert.AreEqual(report.CyclingSessions, 3, "Number of CyclingSessions was incorrect");
            Assert.AreEqual(report.TotalSessions, 3, "Number of TotalSessions was incorrect");
            Assert.AreEqual(report.Rides[0].When, firstRideTime, "The first element in Ridesdit not match,probably not sorted by date properly");
            Assert.AreEqual(report.Rides[1].When, secondRideTime, "The second element in Rides dit not match");
            Assert.AreEqual(report.Rides[2].When, thirdRideTime, "The third element in Rides dit not match");

            Assert.AreEqual(report.MaxDistanceSessionCycling.When, longestRideTime, "Max Distance Ride did not match up");
            Assert.AreEqual(report.MaxSpeedSessionCycling.When, fastestRideTime, "Max Speed Ride did not match up");
            Assert.AreEqual(report.MaxWattSessionCycling.When, wattestRideTime, "Max Watt Ride did not match up");
            Assert.AreEqual(report.TotalCyclingDistance, secondRide.Distance + firstRide.Distance + thirdRide.Distance);
            Assert.AreEqual(report.TotalCyclingTrainingTime, secondRide.Time + firstRide.Time + thirdRide.Time);

            Assert.AreEqual(report.TimeLine[0].Item2, report.Rides[0], "Rides and Timeline did not match up");
            Assert.AreEqual(report.TimeLine[1].Item2, report.Rides[1], "Rides and Timeline did not match up");
            Assert.AreEqual(report.TimeLine[2].Item2, report.Rides[2], "Rides and Timeline did not match up");
        }
    }
}