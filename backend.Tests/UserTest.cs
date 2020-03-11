using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using backend.Repositories;
using backend.Models;

namespace backend.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UserTest
    {
        private UserRepository _userRepo;
        private LoginRepository _loginRepo;
        private List<UserRecord> users;
        
        [TestInitialize]
        public void Init()
        {
            _userRepo = new UserRepository(true);
            _loginRepo = new LoginRepository(true);
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

            _userRepo.ResetAutoIncrement();

            foreach (UserRecord record in users)
            {
                _userRepo.PostNewUser(record);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            users.Clear();
            _userRepo.ResetUsers();
        }

        [TestMethod]
        public void TestPostNewUser()
        {
            // Arrange
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
            bool result = _userRepo.PostNewUser(user);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void testPostDuplicateUser()
        {
            // Arrange
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
            bool result = _userRepo.PostNewUser(user);

            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void TestIsUserLoginValid()
        {
            // Arrange
            String validUser1 = "TestUser";
            String validPass1 = "TestPass";
            String validUser2 = "User2";
            String validPass2 = "Pass2";
            String validUser3 = "TestUser";
            String invalidPass3 = "Wrong Password";
            String invalidUser4 = "UnknownUser";
            String invalidPass4 = "Pass2";

            // Act
            bool result1 = _loginRepo.IsUserLoginValid(validUser1, validPass1);
            bool result2 = _loginRepo.IsUserLoginValid(validUser2, validPass2);
            bool result3 = _loginRepo.IsUserLoginValid(validUser3, invalidPass3);
            bool result4 = _loginRepo.IsUserLoginValid(invalidUser4, invalidPass4);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }
    }
}
