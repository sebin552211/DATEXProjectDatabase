﻿/*using DATEX_ProjectDatabase.Interfaces;

using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using DATEX_ProjectDatabase.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datex_UnitTest.UnitTest.Services
{

    [TestFixture]
    public class ProjectJobServiceTest
    {

        [Test]
        public async Task CheckAndNotifyVocEligibilityAsync_NoProjectsWithTodayDate_NoEmailSent()
        {
            // Arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var projectManagerRepositoryMock = new Mock<IProjectManagerRepository>();
            var emailServiceMock = new Mock<IEmailService>();

            projectRepositoryMock.Setup(repo => repo.GetAllProjectsAsync())
                .ReturnsAsync(new List<Project>());

            var projectJobService = new ProjectJobService(
                projectRepositoryMock.Object,
                projectManagerRepositoryMock.Object,
                emailServiceMock.Object
            );

            // Act
            await projectJobService.CheckAndNotifyVocEligibilityAsync();

            // Assert
            projectManagerRepositoryMock.Verify(repo => repo.GetProjectManagerByProjectIdAsync(It.IsAny<int>()), Times.Never);
            emailServiceMock.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }



        [Test]
        public async Task CheckAndNotifyVocEligibilityAsync_ProjectManagerNotFound_NoEmailSent()
        {
            // Arrange
            var today = DateTime.Today;
            var project = new Project { ProjectId = 1, VOCEligibilityDate = today, ProjectName = "Test Project" };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            var projectManagerRepositoryMock = new Mock<IProjectManagerRepository>();
            var emailServiceMock = new Mock<IEmailService>();

            projectRepositoryMock.Setup(repo => repo.GetAllProjectsAsync())
                .ReturnsAsync(new List<Project> { project });
            projectManagerRepositoryMock.Setup(repo => repo.GetProjectManagerByProjectIdAsync(project.ProjectId))
                .ReturnsAsync((ProjectManagers)null);

            var projectJobService = new ProjectJobService(
                projectRepositoryMock.Object,
                projectManagerRepositoryMock.Object,
                emailServiceMock.Object
            );

            // Act
            await projectJobService.CheckAndNotifyVocEligibilityAsync();

            // Assert
            emailServiceMock.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            projectRepositoryMock.Verify(repo => repo.UpdateProjectAsync(It.IsAny<Project>()), Times.Never);
        }




        [Test]
        public async Task CheckAndNotifyVocEligibilityAsync_EmailSendingFails_NoProjectUpdate()
        {
            // Arrange
            var today = DateTime.Today;
            var project = new Project { ProjectId = 1, VOCEligibilityDate = today, ProjectName = "Test Project" };
            var projectManager = new ProjectManagers { Name = "Manager", Email = "manager@example.com" };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            var projectManagerRepositoryMock = new Mock<IProjectManagerRepository>();
            var emailServiceMock = new Mock<IEmailService>();

            projectRepositoryMock.Setup(repo => repo.GetAllProjectsAsync())
                .ReturnsAsync(new List<Project> { project });
            projectManagerRepositoryMock.Setup(repo => repo.GetProjectManagerByProjectIdAsync(project.ProjectId))
                .ReturnsAsync(projectManager);
            emailServiceMock.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var projectJobService = new ProjectJobService(
                projectRepositoryMock.Object,
                projectManagerRepositoryMock.Object,
                emailServiceMock.Object
            );

            // Act
            await projectJobService.CheckAndNotifyVocEligibilityAsync();

            // Assert
            projectRepositoryMock.Verify(repo => repo.UpdateProjectAsync(It.IsAny<Project>()), Times.Never);
        }




    }
}
*/