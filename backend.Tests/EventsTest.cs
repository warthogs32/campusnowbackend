using System;
using System.Collections.Generic;
using backend.Models;
using System.Diagnostics.CodeAnalysis;
using backend.Repositories;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace backend.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventsTest
    {
        private List<EventRecord> TestEventList;

        [TestInitialize]
        public void Initialize()
        {
            EventRepository event_repo = new EventRepository();
            UserRepository user_repo = new UserRepository();

            TestEventList = new List<EventRecord>();

            //TODO: build json file from existing SQL database
            String user_json = "[]";
            String events_json = "[]";

            try
            {
                using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                {
                    conn.Open();
                    String AddUserQuery = @"select * into cn.Users from OPENJSON (@json)";
                    String AddEventsQuery = @"select * into cn.Events from OPENJSON (@json)";
                    using (SqlCommand cmd = new SqlCommand(AddUserQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@json", user_json);

                        cmd.ExecuteNonQuery();
                    }
                    using (SqlCommand cmd = new SqlCommand(AddEventsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@json", events_json);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error encountered while setting up sql test");
            }


        }

        [TestCleanup]
        public void Cleanup()
        {
            TestEventList.Clear();
            try
            {
                using (SqlConnection conn = new SqlConnection(backend.Properties.Resources.sqlconnection))
                {
                    conn.Open();
                    String DeleteUserQuery = @"delete from cn.Users";
                    String DeleteEventsQuery = @"delete from cn.Events";
                    using (SqlCommand cmd = new SqlCommand(DeleteUserQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    using (SqlCommand cmd = new SqlCommand(DeleteEventsQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error encountered while tearing down sql test");
            }
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
            loginRepo.IsUserLoginValid("TestUser", "TestPassword");
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
            loginRepo.IsUserLoginValid("TestUser", "TestPassword");
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
            Assert.AreEqual(expectedSize, results.Count);
            for (int i = 0; i < expectedSize; i++)
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
            Console.WriteLine("HEllo?");

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
