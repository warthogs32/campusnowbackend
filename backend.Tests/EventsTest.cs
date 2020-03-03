using System;
using System.Collections.Generic;
using backend.Models;
using System.Diagnostics.CodeAnalysis;
using backend.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace backend.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventsTest
    {
        [TestMethod]
        public void TestGetEventById()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            int eventId = 5;
            String expectedEventTitle = "ExampleEvent";
            String expectedEventDescription = "New EventDescription";

            // Act
            EventRecord record = repo.GetEventById(eventId);

            // Assert
            Assert.AreEqual(expectedEventTitle, record.Title);
            Assert.AreEqual(expectedEventDescription, record.Description);
        }
        [TestMethod]
        public void TestGetAllEvents()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            int expectedLength = 1;
            String expectedEventTitle1 = "ExampleEvent";
            String expectedEventDescription1 = "New EventDescription";

            // Act
            List<EventRecord> records = repo.GetAllEvents();

            // Assert
            Assert.AreEqual(expectedLength, records.Count);
            Assert.AreEqual(expectedEventTitle1, records[0].Title);
            Assert.AreEqual(expectedEventDescription1, records[0].Description);
        }

        [TestMethod]
        public void TestPostNewEvent()
        {
            // Arrange
            LoginRepository loginRepo = new LoginRepository();
            ///loginRepo.IsUserLoginValid(...);
            EventRepository repo = new EventRepository();
            EventRecord record = new EventRecord()
            {
           
                Title = "Test Event",
                Description = "Test Description",
                StartTime = new DateTime (2020, 3, 15, 1, 0, 0),
                EndTime = new DateTime(2020, 3, 15, 3, 0, 0),
                LocX = 30.5,
                LocY = 120.7,
                UserId = 1
            };

            // Act
            bool result = repo.PostNewEvent(record);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUpdateEvent()
        {
            // Arrange
            LoginRepository loginRepo = new LoginRepository();
            EventRepository repo = new EventRepository();
            EventRecord record = new EventRecord()
            {
                Title = "Test Update Event",
                Description = "Test Update Description",
                StartTime = new DateTime(2020, 3, 25, 1, 0, 0),
                EndTime = new DateTime(2020, 3, 25, 2, 15, 0),
                LocX = 30.35,
                LocY = 120.74,
                UserId = 1
            };

            // Act
            bool result = repo.UpdateEvent(record);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDeleteEvent()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            int id = 3;

            // Act
            bool result = repo.DeleteEvent(id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGetEventsByTimeRange()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            DateTime start = new DateTime(2020, 3, 1, 1, 0, 0);
            DateTime finish = new DateTime(2020, 4, 1, 1, 0, 0);
            List<EventRecord> expectedRecords = new List<EventRecord>();
            //TODO: build expected list of eventRecords
            int expectedSize = expectedRecords.Count;

            // Act
            List<EventRecord> results = repo.GetEventsByTimeRange(start, finish);

            // Assert
            Assert.AreEqual(results.Count, expectedSize);
            for (int i = 0; i < expectedSize; i++)
            {
                Assert.AreEqual(results[i].ListingId, expectedRecords[i].ListingId);
                Assert.AreEqual(results[i].Title, expectedRecords[i].Title);
                Assert.AreEqual(results[i].Description, expectedRecords[i].Description);
                Assert.AreEqual(results[i].StartTime, expectedRecords[i].StartTime);
                Assert.AreEqual(results[i].EndTime, expectedRecords[i].EndTime);
            }
        }
    }
}
