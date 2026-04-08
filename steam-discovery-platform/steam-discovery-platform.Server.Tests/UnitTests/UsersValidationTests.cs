using Microsoft.AspNetCore.Mvc;
using steam_discovery_platform.Server.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace steam_discovery_platform.Server.Tests.UnitTests
{
    public class UsersValidationTests
    {
        readonly DataValidation dataValidation = new DataValidation();

        [Fact]
        public void ValidatePassword_ReturnsError_WhenTooShort()
        {
            var errors = dataValidation.ValidatePassword("abc");
            Assert.Contains("Password must be at least 6 characters long.", errors);
        }

        [Fact]
        public void ValidatePassword_ReturnsEmpty_WhenValid()
        {
            var errors = dataValidation.ValidatePassword("Test1234!");
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateUserName_ReturnsError_WhenTooShort()
        {
            var errors = dataValidation.ValidateName("a");
            Assert.Contains("Username must be at least 3 characters.", errors);
        }

        [Fact]
        public void ValidateUserName_ReturnsEmpty_WhenValid()
        {
            var errors = dataValidation.ValidateName("abcd");
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateEmail_ReturnsError_WhenDontContainsCharacter()
        {
            var errors = dataValidation.ValidateEmail("abc");
            Assert.Contains("email must contain @", errors);
        }

        [Fact]
        public void ValidateEmail_ReturnsError_WhenIsEmpty()
        {
            var errors = dataValidation.ValidateEmail("");
            Assert.Contains("email is required.", errors);
        }

        [Fact]
        public void ValidateEmail_ReturnsEmpty_WhenValid()
        {
            var errors = dataValidation.ValidateEmail("asd@asd");
            Assert.Empty(errors);
        }
    }
}
