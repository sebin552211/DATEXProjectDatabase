using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Service;
using DATEX_ProjectDatabase.Models;
using NUnit.Framework.Legacy;

namespace Datex_UnitTest.UnitTest.Services
{
    [TestFixture]
    public class ProjectManagerServiceTest
    {
        private Mock<IProjectManagerRepository> _mockProjectManagerRepository;
        private Mock<IProjectRepository> _mockProjectRepository;
        private ProjectManagerService _service;

        [SetUp]
        public void Setup()
        {
            _mockProjectManagerRepository = new Mock<IProjectManagerRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _service = new ProjectManagerService(_mockProjectManagerRepository.Object, _mockProjectRepository.Object);
        }

        // Test cases go here

        [Test]
        public async Task UpsertProjectManagerAsync_ProjectNotFound_ThrowsArgumentException()
        {
            // Arrange
            var projectId = 1;
            _mockProjectRepository.Setup(repo => repo.GetProjectByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _service.UpsertProjectManagerAsync(new ProjectManagers(), projectId));
            Assert.That(ex.Message, Is.EqualTo("Project not found"));
        }

        [Test]
        public async Task UpsertProjectManagerAsync_NoExistingManager_AddsNewManager()
        {
            // Arrange
            var projectId = 1;
            var project = new Project { ProjectId = projectId, ProjectManager = "John Doe" };
            var newManager = new ProjectManagers { Email = "john.doe@example.com" };

            _mockProjectRepository.Setup(repo => repo.GetProjectByIdAsync(projectId)).ReturnsAsync(project);
            _mockProjectManagerRepository.Setup(repo => repo.GetProjectManagerByProjectIdAsync(projectId)).ReturnsAsync((ProjectManagers)null);

            // Act
            var result = await _service.UpsertProjectManagerAsync(newManager, projectId);

            // Assert
            _mockProjectManagerRepository.Verify(repo => repo.AddProjectManagerAsync(It.Is<ProjectManagers>(pm =>
                pm.ProjectId == projectId &&
                pm.Name == "John Doe" &&
                pm.Email == "john.doe@example.com"
            )), Times.Once);

            ClassicAssert.AreEqual("John Doe", result.Name);
            ClassicAssert.AreEqual("john.doe@example.com", result.Email);
        }



    }
}
