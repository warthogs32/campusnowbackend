using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using backend.Models;
using backend.Repositories;

namespace backend.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BookmarkTest
    {
        private BookmarkRepository _bookmarkRepo;
        private UserRepository _userRepo;
        private EventRepository _eventRepo;
        private LoginRepository _loginRepo;
        private List<EventRecord> events;
        private List<UserRecord> users;

        [TestInitialize]
        public void Initialize()
        {
            _eventRepo = new EventRepository(true);
            _userRepo = new UserRepository(true);
            _bookmarkRepo = new BookmarkRepository(true);
            _loginRepo = new LoginRepository(true);

            events = new List<EventRecord>();
            users = new List<UserRecord>();
            users.Add(new UserRecord()
            {
                UserName = "TestUser",
                Password = "TestPass",
                FirstName = "John",
                LastName = "Smith",
                JoinDate = DateTime.Now,
                IsAdmin = false,
                UserId = 1
            });
            users.Add(new UserRecord()
            {
                UserName = "User2",
                Password = "Pass2",
                FirstName = "Barrack",
                LastName = "Obama",
                JoinDate = DateTime.Now,
                IsAdmin = true,
                UserId = 2
            });

            events.Add(new EventRecord()
            {
                ListingId = 1,
                Title = "TestEvent",
                Description = "TestDescription",
                StartTime = new DateTime(2020, 4, 1, 12, 0, 0),
                EndTime = new DateTime(2020, 4, 1, 14, 20, 0),
                LocX = 35.3,
                LocY = -120.7,
                UserId = 1
            });
            events.Add(new EventRecord()
            {
                ListingId = 2,
                Title = "SecondEvent",
                Description = "This event starts at 4am lol",
                StartTime = new DateTime(2020, 3, 27, 4, 0, 0),
                EndTime = new DateTime(2020, 3, 27, 7, 30, 0),
                LocX = 35.7,
                LocY = -121,
                UserId = 1
            });
            _userRepo.ResetAutoIncrement();
            _eventRepo.ResetAutoIncrement();
            _bookmarkRepo.ResetAutoIncrement();

            foreach (UserRecord record in users)
            {
                _userRepo.PostNewUser(record);
            }
            foreach (EventRecord record in events)
            {
                _loginRepo.IsUserLoginValid("TestUser", "TestPass");
                _eventRepo.PostNewEvent(record);
                _bookmarkRepo.AddNewBookmark(record);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            events.Clear();
            users.Clear();

            _bookmarkRepo.ResetBookmarks();
            _eventRepo.ResetEvents();
            _userRepo.ResetUsers();
        }

        [TestMethod]
        public void TestAddBookmark()
        {
            // Arrange
            BookmarkRepository repo = _bookmarkRepo;

            // Act
            bool result = repo.AddNewBookmark(events[0]);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestGetBookmarkedEvents()
        {
            // Arrange
            BookmarkRepository repo = _bookmarkRepo;
            List<EventRecord> expected1 = events;
            List<EventRecord> expected2 = new List<EventRecord>();

            // Act
            _loginRepo.IsUserLoginValid("TestUser", "TestPass");
            List<EventRecord> result1 = repo.GetAllBookmarkedEvents();
            _loginRepo.IsUserLoginValid("User2", "Pass2");
            List<EventRecord> result2 = repo.GetAllBookmarkedEvents();

            // Assert
            Assert.AreEqual(expected1.Count, result1.Count);
            Assert.AreEqual(expected2.Count, result2.Count);
            for (int i = 0; i < expected1.Count; i++)
            {
                Assert.AreEqual(expected1[i].ListingId, result1[i].ListingId);
                Assert.AreEqual(expected1[i].Title, result1[i].Title);
                Assert.AreEqual(expected1[i].Description, result1[i].Description);
            }
        }
    }
}
