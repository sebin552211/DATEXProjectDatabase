using DATEX_ProjectDatabase.Controllers;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Tests.Controllers
{
    [TestFixture]
    public class ProjectManagerControllerTests
    {
        private Mock<IProjectManagerService> _projectManagerServiceMock;
        private ProjectManagerController _controller;

        [SetUp]
        public void SetUp()
        {
            _projectManagerServiceMock = new Mock<IProjectManagerService>();
            _controller = new ProjectManagerController(_projectManagerServiceMock.Object);
        }

        [Test]
        public async Task PostProjectManager_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var projectManager = new ProjectManagers { ProjectId = 1, Name = "John Doe", Email = "john.doe@example.com" };
            _projectManagerServiceMock.Setup(s => s.UpsertProjectManagerAsync(projectManager, projectManager.ProjectId))
                .ReturnsAsync(projectManager);  // Return Task<ProjectManagers>

            // Act
            var result = await _controller.PostProjectManager(projectManager);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            ClassicAssert.IsNotNull(createdAtActionResult);
            ClassicAssert.AreEqual(201, createdAtActionResult.StatusCode);
            ClassicAssert.AreEqual(nameof(ProjectManagerController.GetProjectManager), createdAtActionResult.ActionName);
            ClassicAssert.AreEqual(projectManager.ProjectId, createdAtActionResult.RouteValues["id"]);
            ClassicAssert.AreEqual(projectManager, createdAtActionResult.Value);
        }


        [Test]
        public async Task PostProjectManager_ChecksNullData()
        {
            // Act
            var result = await _controller.PostProjectManager(null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual(400, badRequestResult.StatusCode);
            ClassicAssert.AreEqual("Project manager data is required.", badRequestResult.Value);
        }
        [Test]
        public async Task PutProjectManager_ValidData_ReturnsNoContent()
        {
            // Arrange
            var projectManager = new ProjectManagers { ProjectId = 1, Name = "John Doe", Email = "john.doe@example.com" };
            _projectManagerServiceMock.Setup(s => s.UpsertProjectManagerAsync(projectManager, projectManager.ProjectId))
                .ReturnsAsync(projectManager);  // Return Task<ProjectManagers>

            // Act
            var result = await _controller.PutProjectManager(projectManager.ProjectId, projectManager);

            // Assert
            var noContentResult = result as NoContentResult;
            ClassicAssert.IsNotNull(noContentResult);
            ClassicAssert.AreEqual(204, noContentResult.StatusCode);
        }


        [Test]
        public async Task PutProjectManager_ChecksMismatchedProjectId()
        {
            // Arrange
            var projectManager = new ProjectManagers { ProjectId = 1, Name = "John Doe", Email = "john.doe@example.com" };

            // Act
            var result = await _controller.PutProjectManager(2, projectManager);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual(400, badRequestResult.StatusCode);
            ClassicAssert.AreEqual("Project ID mismatch.", badRequestResult.Value);
        }

        [Test]
        public async Task PutProjectManager_ChecksNullData()
        {
            // Act
            var result = await _controller.PutProjectManager(1, null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            ClassicAssert.IsNotNull(badRequestResult);
            ClassicAssert.AreEqual(400, badRequestResult.StatusCode);
            ClassicAssert.AreEqual("Project manager data is required.", badRequestResult.Value);
        }
    }
}
