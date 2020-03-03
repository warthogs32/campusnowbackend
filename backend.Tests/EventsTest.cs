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
        private List<EventRecord> TestEventList;

        [ClassInitialize]
        public void setup()
        {
            EventRepository event_repo = new EventRepository();
            UserRepository user_repo = new UserRepository();

            // Post a couple sample events and sample users
            UserRecord test_user = new UserRecord()
            {
                UserId = 1,
                UserName = "TestUser",
                Password = "TestPassword",
                FirstName = "John",
                LastName = "Smith",
                JoinDate = new DateTime(2020, 3, 1, 12, 0, 0),
                IsAdmin = false
            };
            user_repo.PostNewUser(test_user);

            EventRecord test_event_1 = new EventRecord()
            {
                ListingId = 1,
                Title = "Event1",
                Description = "No description",
                StartTime = new DateTime(2020, 3, 4, 12, 0, 0),
                EndTime = new DateTime(2020, 3, 4, 13, 30, 0),
                LocX = 50.7,
                LocY = 80.3,
                UserId = 1 // get User id
            };
            EventRecord test_event_2 = new EventRecord()
            {
                ListingId = 2,
                Title = "Event2",
                Description = "A very fun event",
                StartTime = new DateTime(2020, 3, 7, 2, 45, 0),
                EndTime = new DateTime(2020, 3, 7, 6, 30, 0),
                LocX = 50.9,
                LocY = 79.9,
                UserId = 1 // get User id
            };

            TestEventList = new List<EventRecord>();
            TestEventList.Add(test_event_1);
            TestEventList.Add(test_event_2);

            user_repo.PostNewUser(test_user);
            event_repo.PostNewEvent(test_event_1);
            event_repo.PostNewEvent(test_event_2);
        }

        [ClassCleanup]
        public void teardown()
        {
            EventRepository event_repo = new EventRepository();
            UserRepository user_repo = new UserRepository();

            event_repo.ClearEvents();
            user_repo.ClearUsers();
            TestEventList.Clear();
        }

        [TestMethod]
        public void TestGetEventById()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            int eventId = 1;

            // Act
            EventRecord record = repo.GetEventById(eventId);
            EventRecord target = TestEventList[0];

            // Assert
            Assert.AreEqual(target.Title, record.Title);
            Assert.AreEqual(target.Description, record.Description);
        }
        [TestMethod]
        public void TestGetAllEvents()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            List<EventRecord> expectedEvents = TestEventList;

            // Act
            List<EventRecord> records = repo.GetAllEvents();

            // Assert
            Assert.AreEqual(expectedEvents.Count, records.Count);
            for (int i = 0; i < records.Count; i++)
            {
                Assert.AreEqual(expectedEvents[i].ListingId, records[i].ListingId);
                Assert.AreEqual(expectedEvents[i].Title, records[i].Title);
                Assert.AreEqual(expectedEvents[i].Description, records[i].Description);
            }
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
                ListingId = 3,
                Title = "Test Event 3",
                Description = "Test Description 3",
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
            int id = 2;

            // Act
            bool result = repo.DeleteEvent(id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDeleteInvalidEvent()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            int id = -1;

            // Act
            bool result = repo.DeleteEvent(id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestGetEventsByTimeRange()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            DateTime start = new DateTime(2020, 3, 1, 1, 0, 0);
            DateTime finish = new DateTime(2020, 4, 1, 1, 0, 0);
            List<EventRecord> expectedRecords = TestEventList;
            int expectedSize = expectedRecords.Count;

            // Act
            List<EventRecord> results;
            results = repo.GetEventsByTimeRange(start, finish);
            
            // Assert
            Assert.AreEqual(results.Count, expectedSize);
            for (int i = 0; i < expectedSize; i++)
            {
                Assert.AreEqual(results[i].ListingId, expectedRecords[i].ListingId);
                Assert.AreEqual(results[i].Title, expectedRecords[i].Title);
                Assert.AreEqual(results[i].Description, expectedRecords[i].Description);
            }
        }

        [TestMethod]
        public void TestInvalidTimeRange()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            DateTime start = new DateTime(2020, 3, 1, 5, 0, 0);
            DateTime end = new DateTime(2020, 2, 25, 12, 0, 0);
            int expectedEventCount = 0;

            // Act
            List<EventRecord> records = repo.GetEventsByTimeRange(start, end);

            // Assert
            Assert.AreEqual(records.Count, expectedEventCount);
        }
        [TestMethod]
        public void TestGetEventByUserId()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            int user_id = 1;
            List<EventRecord> expectedEvents = TestEventList;

            // Act
            List<EventRecord> records = repo.GetEventsByUserId(user_id);

            // Assert
            Assert.AreEqual(expectedEvents.Count, records.Count);
            for (int i = 0; i < records.Count; i++)
            {
                Assert.AreEqual(expectedEvents[i].ListingId, records[i].ListingId);
                Assert.AreEqual(expectedEvents[i].Title, records[i].Title);
                Assert.AreEqual(expectedEvents[i].Description, records[i].Description);
            }
        }
    }
}
