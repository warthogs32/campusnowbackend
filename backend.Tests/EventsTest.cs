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
        private List<UserRecord> TestUserList;

        [TestInitialize]
        public void Initialize()
        {
            EventRepository event_repo = new EventRepository(true);
            UserRepository user_repo = new UserRepository(true);
            LoginRepository login_repo = new LoginRepository(true);
            EventRecord event_record;
            UserRecord user_record = new UserRecord()

            TestEventList = new List<EventRecord>();
            TestUserList = new List<UserRecord>();
            TestUserList.Add(new UserRecord()
            {
                UserName = "TestUser",
                Password = "TestPass",
                FirstName = "John",
                LastName = "Smith",
                JoinDate = DateTime.Now,
                IsAdmin = false
            };
            });
            TestUserList.Add(new UserRecord()
            {
                UserName = "User2",
                Password = "Pass2",
                FirstName = "Barrack",
                LastName = "Obama",
                JoinDate = DateTime.Now,
                IsAdmin = true
            });

            TestEventList = new List<EventRecord>();
            event_record = new EventRecord()
            TestEventList.Add(new EventRecord()
            {
                ListingId = 1,
                Title = "TestEvent",
                Description = "TestDescription",
                StartTime = new DateTime(2020, 4, 1, 12, 0, 0),
                EndTime = new DateTime(2020, 4, 1, 14, 20, 0),
                LocX = 35.3,
                LocY = -120.7,
                UserId = 1
            };
            TestEventList.Add(event_record);
            event_record = new EventRecord()
            });
            TestEventList.Add(new EventRecord()
            {
                ListingId = 2,
                Title = "SecondEvent",
                Description = "This event starts at 4am lol",
                StartTime = new DateTime(2020, 3, 27, 4, 0, 0),
                EndTime = new DateTime(2020, 3, 27, 7, 30, 0),
                LocX = 35.7,
                LocY = -121,
                UserId = 1
            };
            TestEventList.Add(event_record);
            });
            user_repo.ResetAutoIncrement();
            event_repo.ResetAutoIncrement();

            user_repo.PostNewUser(user_record);
            login_repo.IsUserLoginValid("TestUser", "TestPass");
            foreach (UserRecord record in TestUserList)
            {
                user_repo.PostNewUser(record);
            }
            foreach (EventRecord record in TestEventList)
            {
                login_repo.IsUserLoginValid("TestUser", "TestPass");
                event_repo.PostNewEvent(record);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestEventList.Clear();
            EventRepository event_repo = new EventRepository(true);
            UserRepository user_repo = new UserRepository(true);

            event_repo.ResetEvents();
            user_repo.ResetUsers();
        }

        [TestMethod]
        public void TestGetEventById()
        {
            // Arrange
            EventRepository repo = new EventRepository(true);
            int eventId = 1;
            String expectedEventTitle = "TestEvent";
            String expectedEventDescription = "TestDescription";

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
            EventRepository repo = new EventRepository(true);
            List<EventRecord> expectedRecords = TestEventList;

            // Act
            List<EventRecord> records = repo.GetAllEvents();

            // Assert
            Assert.AreEqual(expectedRecords.Count, records.Count);
            for (int i = 0; i < expectedRecords.Count; i++)
            {
                Assert.AreEqual(expectedRecords[i].Title, records[i].Title);
                Assert.AreEqual(expectedRecords[i].Description, records[i].Description);
            }
        }

        [TestMethod]
        public void TestPostNewEvent()
        {
            // Arrange
            LoginRepository loginRepo = new LoginRepository(true);
            loginRepo.IsUserLoginValid("TestUser", "TestPass");
            EventRepository repo = new EventRepository(true);
            EventRecord record = new EventRecord()
            {
                Title = "Test Event",
                Description = "Test Description",
                StartTime = new DateTime (2020, 3, 15, 1, 0, 0),
                EndTime = new DateTime(2020, 3, 15, 3, 0, 0),
                LocX = 30.5,
                LocY = 120.7,
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
            LoginRepository loginRepo = new LoginRepository(true);
            loginRepo.IsUserLoginValid("TestUser", "TestPass");
            EventRepository repo = new EventRepository(true);
            EventRecord record = new EventRecord()
            {
                Title = "Test Update Event",
                Description = "Test Update Description",
                StartTime = new DateTime(2020, 3, 25, 1, 0, 0),
                EndTime = new DateTime(2020, 3, 25, 2, 15, 0),
                LocX = 30.35,
                LocY = 120.74,
                UserId = 1,
                ListingId = 1
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
            LoginRepository loginRepo = new LoginRepository(true);
            loginRepo.IsUserLoginValid("TestUser", "TestPass");
            EventRepository repo = new EventRepository(true);
            int toDelete = TestEventList[1].ListingId;

            // Act
            bool result = repo.DeleteEvent(toDelete);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGetEventsByTimeRange()
        {
            // Arrange
            EventRepository repo = new EventRepository(true);
            DateTime start = new DateTime(2020, 3, 1, 1, 0, 0);
            DateTime finish = new DateTime(2020, 4, 1, 1, 0, 0);
            List<EventRecord> expectedRecords = new List<EventRecord>();
            expectedRecords.Add(TestEventList[1]);

            // Act
            List<EventRecord> results = repo.GetEventsByTimeRange(start, finish);

            // Assert
            Assert.AreEqual(expectedRecords.Count, results.Count);
            for (int i = 0; i < expectedRecords.Count; i++)
            {
                Assert.AreEqual(expectedRecords[i].ListingId, results[i].ListingId);
                Assert.AreEqual(expectedRecords[i].Title, results[i].Title);
                Assert.AreEqual(expectedRecords[i].Description, results[i].Description);
            }
        }

        [TestMethod]
        public void TestInvalidTimeRange()
        {
            // Arrange
            EventRepository repo = new EventRepository(true);
            DateTime start = new DateTime(2020, 3, 1, 5, 0, 0);
            DateTime end = new DateTime(2020, 2, 25, 12, 0, 0);
            int expectedEventCount = 0;

            // Act
            List<EventRecord> records = repo.GetEventsByTimeRange(start, end);

            // Assert
            Assert.AreEqual(expectedEventCount, records.Count);
        }
        [TestMethod]
        public void TestGetEventByUserId()
        {
            // Arrange
            EventRepository repo = new EventRepository(true);
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
                Assert.AreEqual(expectedEvents[i].ListingId, records[i].ListingId);
                Assert.AreEqual(expectedEvents[i].Title, records[i].Title);
                Assert.AreEqual(expectedEvents[i].Description, records[i].Description);
                Assert.AreEqual(expectedEvents[i].StartTime, records[i].StartTime);
                Assert.AreEqual(expectedEvents[i].EndTime, records[i].EndTime);
            }
        }

        [TestMethod]
        public void testIsUserLoginValid()
        {
            // Arrange
            LoginRepository loginRepo = new LoginRepository(true);
            loginRepo.IsUserLoginValid("TestUser", "TestPass");
            EventRepository repo = new EventRepository(true);
            EventRecord record = TestEventList[0];

            // Act
            bool resultValid = repo.DoesEventBelongToUser(record.ListingId);

            // Assert
            Assert.IsTrue(resultValid);
        }

        [TestMethod]
        public void testIsUserLoginInvalid()
        {
            // Arrange
            LoginRepository loginRepo = new LoginRepository(true);
            loginRepo.IsUserLoginValid("User2", "Pass2");
            EventRepository repo = new EventRepository(true);
            EventRecord record = TestEventList[0];

            // Act
            bool resultInvalid = repo.DoesEventBelongToUser(record.ListingId);

            // Assert
            Assert.IsFalse(resultInvalid);
        }

        [TestMethod]
        public void testPostNewUser()
        {
            // Arrange
            LoginRepository loginRepo = new LoginRepository(true);
            UserRepository userRepo = new UserRepository(true);
            UserRecord user = new UserRecord()
            {
                UserName = "CalPoly",
                Password = "Mustang",
                JoinDate = DateTime.Now,
                FirstName = "Joe",
                LastName = "Garcia",
                IsAdmin = false
            };

            // Act
            bool result = userRepo.PostNewUser(user);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void testPostDuplicateUser()
        {
            // Arrange
            LoginRepository loginRepo = new LoginRepository(true);
            UserRepository userRepo = new UserRepository(true);
            UserRecord user = new UserRecord()
            {
                UserName = "TestUser",
                Password = "OtherPass",
                JoinDate = DateTime.Now,
                FirstName = "Jason",
                LastName = "Borne",
                IsAdmin = false
            };

            // Act
            bool result = userRepo.PostNewUser(user);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
