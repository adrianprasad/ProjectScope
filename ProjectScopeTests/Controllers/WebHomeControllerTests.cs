using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectScope.Controllers;
using ProjectScope.Data;
using ProjectScope.Entity;
using ProjectScope.Models;
using ProjectScope.ViewModels;
using System.Collections.Generic;
using System.Text;

namespace ProjectScope.Tests.Controllers
{
    [TestClass]
    public class WebHomeControllerTests
    {
        private Mock<ILogger<WebHomeController>> _mockLogger;
        private Mock<IStudent> _mockStudentService;
        private Mock<IContact> _mockContactService;
        private Mock<ICourse> _mockCourseService;
        private WebHomeController _controller;
        private DefaultHttpContext _httpContext;
        private Mock<ISession> _mockSession;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<WebHomeController>>();
            _mockStudentService = new Mock<IStudent>();
            _mockContactService = new Mock<IContact>();
            _mockCourseService = new Mock<ICourse>();
            _mockSession = new Mock<ISession>();

            _controller = new WebHomeController(
                _mockLogger.Object,
                _mockStudentService.Object,
                _mockContactService.Object,
                _mockCourseService.Object
            );

            _httpContext = new DefaultHttpContext();
            _httpContext.Session = _mockSession.Object; // Mocked session
            _controller.ControllerContext = new ControllerContext { HttpContext = _httpContext };
        }

        [TestMethod]
        public void StudentLogin_ValidCredentials_ShouldRedirectToDashboard()
        {
            // Arrange
            var model = new LoginMainViewModel
            {
                Email = "test@example.com",
                Password = "password123",
                KeepLoggedIn = true
            };

            var student = new Student { Id = 1, Email = model.Email, Password = model.Password };

            _mockStudentService.Setup(s => s.Get(model.Email)).Returns(student);
            _mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()));

            // Act
            var result = _controller.StudentLogin(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("StudentDashboard", result.ActionName);
            Assert.AreEqual("WebHome", result.ControllerName);
        }

        [TestMethod]
        public void StudentLogin_InvalidCredentials_ShouldReturnViewWithError()
        {
            // Arrange
            var model = new LoginMainViewModel
            {
                Email = "wrong@example.com",
                Password = "wrongpassword"
            };

            _mockStudentService.Setup(s => s.Get(model.Email)).Returns((Student)null);

            // Act
            var result = _controller.StudentLogin(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("StudentLogin", result.ViewName);
            Assert.IsTrue(_controller.ModelState.ContainsKey(""));
        }

        [TestMethod]
        public void StudentLogin_KeepLoggedIn_ShouldSetCookie()
        {
            // Arrange
            var model = new LoginMainViewModel
            {
                Email = "test@example.com",
                Password = "password123",
                KeepLoggedIn = true
            };

            var student = new Student { Id = 1, Email = model.Email, Password = model.Password };

            _mockStudentService.Setup(s => s.Get(model.Email)).Returns(student);
            _mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()));

            // Act
            _controller.StudentLogin(model);

            // Assert
            Assert.IsTrue(_httpContext.Response.Headers["Set-Cookie"].ToString().Contains("Authenticated=true"));
        }
    }
}
