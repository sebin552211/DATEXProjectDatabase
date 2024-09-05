using DATEX_ProjectDatabase.Controllers;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DATEX_ProjectDatabase.Service;
using NUnit.Framework.Legacy;

namespace DATEX_ProjectDatabase.Tests
{
    [TestFixture]
    public class ProjectControllerTests
    {
        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IExternalApiService> _externalApiServiceMock;
        private ProjectController _controller;

        [SetUp]
        public void Setup()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _externalApiServiceMock = new Mock<IExternalApiService>();
            _controller = new ProjectController(_projectRepositoryMock.Object, _externalApiServiceMock.Object);
        }
        [Test]
        public async Task GetProjectById_ChecksWhetherProjectExistsOrNot()
        {
            // Arrange
            var project = new Project { ProjectId = 1, ProjectName = "Project1" };

            _projectRepositoryMock.Setup(r => r.GetProjectByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(project);

            // Act
            var result = await _controller.GetProjectById(1);

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.AreEqual(project, okResult.Value);
        }

        [Test]
        public async Task AddProjectEditableFields_ChecksforValidProject()
        {
            // Arrange
            var project = new Project { ProjectId = 1, ProjectName = "Project1" };

            // Act
            var result = await _controller.AddProjectEditableFields(project);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            ClassicAssert.IsNotNull(createdAtActionResult);
            ClassicAssert.AreEqual(201, createdAtActionResult.StatusCode);
            ClassicAssert.AreEqual("GetProjectById", createdAtActionResult.ActionName);
            ClassicAssert.AreEqual(project.ProjectId, createdAtActionResult.RouteValues["id"]);
            ClassicAssert.AreEqual(project, createdAtActionResult.Value);
            _projectRepositoryMock.Verify(r => r.AddProjectEditableFields(project), Times.Once);
            _projectRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateProjectEditableFields_ChecksIfProjectExists()
        {
            // Arrange
            var project = new Project { ProjectId = 1, ProjectName = "UpdatedProject" };

            _projectRepositoryMock.Setup(r => r.GetProjectByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Project { ProjectId = 1, ProjectName = "OldProject" });

            // Act
            var result = await _controller.UpdateProjectEditableFields(1, project);

            // Assert
            var noContentResult = result as NoContentResult;
            ClassicAssert.IsNotNull(noContentResult);
            ClassicAssert.AreEqual(204, noContentResult.StatusCode);
            _projectRepositoryMock.Verify(r => r.UpdateProjectEditableFields(1, project), Times.Once);
            _projectRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateProjectEditableFields_ChecksIfProjectDoesNotExist()
        {
            // Arrange
            _projectRepositoryMock.Setup(r => r.GetProjectByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Project)null);  // Return null as Task<Project>

            var project = new Project { ProjectId = 1, ProjectName = "Project1" };

            // Act
            var result = await _controller.UpdateProjectEditableFields(56, project);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            ClassicAssert.IsNotNull(notFoundResult);  // Ensure NotFoundObjectResult is returned
            ClassicAssert.AreEqual(404, notFoundResult.StatusCode);  // Check that status code is 404
        }


        [Test]
        public async Task SearchProjects_ChecksProjectsExist()
        {
            // Arrange
            var projects = new List<Project>
    {
        new Project { ProjectId = 1, ProjectName = "Project1" },
        new Project { ProjectId = 2, ProjectName = "Project2" }
    };

            _projectRepositoryMock.Setup(r => r.SearchProjects(It.IsAny<string>()))
                .Returns(projects);

            // Act
            var result = await _controller.SearchProjects("Project");

            // Assert
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.AreEqual(projects, okResult.Value);
        }

        [Test]
        public async Task SearchProjects_NoProjectsFound()
        {
            // Arrange
            _projectRepositoryMock.Setup(r => r.SearchProjects(It.IsAny<string>()))
                .Returns(new List<Project>());

            // Act
            var result = await _controller.SearchProjects("NonExistentProject");

            // Assert
            var notFoundResult = result as NotFoundResult;
            ClassicAssert.IsNotNull(notFoundResult);
            ClassicAssert.AreEqual(404, notFoundResult.StatusCode);
        }

    }
}
