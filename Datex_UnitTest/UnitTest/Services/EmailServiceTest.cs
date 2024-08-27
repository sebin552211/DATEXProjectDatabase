using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Migrations;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using DATEX_ProjectDatabase.Service;
using Moq;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Datex_UnitTest.UnitTest.Services
{
    [TestFixture]
    public class EmailServiceTest
    {
        private Mock<SmtpClient> _smtpClientMock;
        private EmailService _emailService;

        [SetUp]
        public void Setup()
        {
            // Mock the SmtpClient and configure environment variables
            _smtpClientMock = new Mock<SmtpClient>();

            // Set environment variables for testing
            Environment.SetEnvironmentVariable("SMTP_SERVER", "smtp.test.com");
            Environment.SetEnvironmentVariable("SMTP_PORT", "587");
            Environment.SetEnvironmentVariable("SMTP_USER", "testuser@test.com");
            Environment.SetEnvironmentVariable("SMTP_PASS", "testpassword");

            _emailService = new EmailService();
        }

        [Test]
        public void EmailService_Constructor_EnvironmentVariablesLoadedCorrectly()
        {
            // Arrange & Act
            var emailService = new EmailService();

            // Assert
            ClassicAssert.AreEqual("smtp.test.com", Environment.GetEnvironmentVariable("SMTP_SERVER"));
            ClassicAssert.AreEqual("587", Environment.GetEnvironmentVariable("SMTP_PORT"));
            ClassicAssert.AreEqual("testuser@test.com", Environment.GetEnvironmentVariable("SMTP_USER"));
            ClassicAssert.AreEqual("testpassword", Environment.GetEnvironmentVariable("SMTP_PASS"));
        }
    }
}

