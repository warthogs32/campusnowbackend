using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace backend.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetEventById()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            int eventId = 1;
            String expectedEventTitle = "ExampleEvent";
            String expectedEventDescription = "New EventDescription";

            // Act
            EventRecord record = repo.GetEventById(eventId);

            // Assert
            Assert.True(expectedEventTitle == record.Title);
            Assert.True(expectedEventDescription == record.Description);
        }
        [TestMethod]
        public void TestGetAllEvents()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            int expectedLength = 2;
            String expectedEventTitle1 = "ExampleEvent";
            String expectedEventTitle2 = "ExampleTitle2";
            String expectedEventDescription1 = "New EventDescription";
            String expectedEventDescription2 = "ExampleDesc2";

            // Act
            List<EventRecord> records = repo.GetAllEvents();

            // Assert
            Assert.True(expectedLength == records.size());
            Assert.True(expectedEventTitle1 == records.get(0).Title);
            Assert.True(expectedEventDescription1 == records.get(0).Description);
            Assert.True(expectedEventTitle2 == records.get(1).Title);
            Assert.True(expectedEventDescription2 == records.get(1).Description);
        }

        [TestMethod]
        public void TestPostNewEvent()
        {
            // Arrange
            EventRepository repo = new EventRepository();
            bool expectedResult = true;
            EventRecord record = new EventRecord()
            {
                ListingId = -1,
                Title = "Test Event",
                Description = "Test Description",
                StartTime = new DateTime (2020, 3, 15, 1, 0, 0),
                EndTime = new DateTime(2020, 3, 15, 3, 0, 0),
                LocX = 30.5,
                LocY = 120.7,
                UserId = 1
            };

            // Act
            boolean result = repo.PostNewEvent(record);

            // Assert
            Assert.True(expectedResult == result);
        }
    }
}
