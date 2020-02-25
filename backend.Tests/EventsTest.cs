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
    }
}
