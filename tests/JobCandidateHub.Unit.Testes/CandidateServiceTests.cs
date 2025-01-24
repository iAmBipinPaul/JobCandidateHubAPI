using JobCandidateHubAPI;
using JobCandidateHubAPI.Dtos.Candidates;
using JobCandidateHubAPI.Entities;
using JobCandidateHubAPI.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace JobCandidateHub.Unit.Testes
{
    public class CandidateServiceTests
    {
        private readonly Mock<DbSet<Candidate>> _dbSetMock;
        private readonly Mock<JobCandidateDbContext> _dbContextMock;
        private readonly CandidateService _candidateService;

        public CandidateServiceTests()
        {
            _dbSetMock = new Mock<DbSet<Candidate>>();
            _dbContextMock = new Mock<JobCandidateDbContext>(new DbContextOptions<JobCandidateDbContext>());
            var loggerMock = new Mock<ILogger<CandidateService>>();
            _dbContextMock.Setup(db => db.Candidates).Returns(_dbSetMock.Object);
            _candidateService = new CandidateService(_dbContextMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task CreateOrUpdate_ShouldCreateCandidate_WhenCandidateDoesNotExist()
        {
            // Arrange
            var requestInput = new CreateOrUpdateCandidateRequestInput
            {
                Email = "contact@bipinpaul.com",
                FirstName = "Bipin",
                LastName = "Paul",
                PhoneNumber = "1234567890",
                IntervalStateTime = new TimeOnly(9, 0),
                IntervalEndTime = new TimeOnly(17, 0),
                LinkedInProfileUrl = "https://www.linkedin.com/in/iambipinpaul/",
                GitHubProfileUrl = "https://github.com/iAmBipinPaul",
                Comments = "This is create"
            };

            _dbSetMock.Setup(db => db.FindAsync(requestInput.Email)).ReturnsAsync((Candidate)null);

            // Act
            var result = await _candidateService.CreateOrUpdate(requestInput);

            // Assert
            _dbSetMock.Verify(db => db.Add(It.IsAny<Candidate>()), Times.Once);
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);
            Assert.False(result.IsUpdate);
        }

        [Fact]
        public async Task CreateOrUpdate_ShouldUpdateCandidate_WhenCandidateExists()
        {
            // Arrange
            var existingCandidate = new Candidate
            {
                Email = "contact@bipinpaul.com",
                FirstName = "Bipin",
                LastName = "Paul",
                PhoneNumber = "1234567890",
                IntervalStateTime = new TimeOnly(9, 0),
                IntervalEndTime = new TimeOnly(17, 0),
                LinkedInProfileUrl = "https://www.linkedin.com/in/iambipinpaul/",
                GitHubProfileUrl = "https://github.com/iAmBipinPaul",
                Comments = "This is create"
            };

            var requestInput = new CreateOrUpdateCandidateRequestInput
            {
                Email = "contact@bipinpaul.com",
                FirstName = "Bipin",
                LastName = "Paul",
                PhoneNumber = "+977987654321",
                IntervalStateTime = new TimeOnly(10, 0),
                IntervalEndTime = new TimeOnly(18, 0),
                LinkedInProfileUrl = "https://www.linkedin.com/in/iambipinpaul/",
                GitHubProfileUrl = "https://github.com/iambipinpaul",
                Comments = "Updated comments"
            };

            _dbSetMock.Setup(db => db.FindAsync(requestInput.Email)).ReturnsAsync(existingCandidate);

            // Act
            var result = await _candidateService.CreateOrUpdate(requestInput);

            // Assert
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);
            Assert.True(result.IsUpdate);
            Assert.Equal(requestInput.FirstName, existingCandidate.FirstName);
            Assert.Equal(requestInput.LastName, existingCandidate.LastName);
            Assert.Equal(requestInput.PhoneNumber, existingCandidate.PhoneNumber);
            Assert.Equal(requestInput.IntervalStateTime, existingCandidate.IntervalStateTime);
            Assert.Equal(requestInput.IntervalEndTime, existingCandidate.IntervalEndTime);
            Assert.Equal(requestInput.LinkedInProfileUrl, existingCandidate.LinkedInProfileUrl);
            Assert.Equal(requestInput.GitHubProfileUrl, existingCandidate.GitHubProfileUrl);
            Assert.Equal(requestInput.Comments, existingCandidate.Comments);
        }

        [Fact]
        public async Task CreateOrUpdate_ShouldCreateNewCandidate_WhenEmailIsDifferent()
        {
            // Arrange
            var existingCandidate = new Candidate
            {
                Email = "contact@bipinpaul.com",
                FirstName = "Bipin",
                LastName = "Paul",
                PhoneNumber = "1234567890",
                IntervalStateTime = new TimeOnly(9, 0),
                IntervalEndTime = new TimeOnly(17, 0),
                LinkedInProfileUrl = "https://www.linkedin.com/in/iambipinpaul/",
                GitHubProfileUrl = "https://github.com/iAmBipinPaul",
                Comments = "This is create"
            };

            var requestInput = new CreateOrUpdateCandidateRequestInput
            {
                Email = "newemail@bipinpaul.com",
                FirstName = "Bipin",
                LastName = "Paul",
                PhoneNumber = "+977987654321",
                IntervalStateTime = new TimeOnly(10, 0),
                IntervalEndTime = new TimeOnly(18, 0),
                LinkedInProfileUrl = "https://www.linkedin.com/in/iambipinpaul/",
                GitHubProfileUrl = "https://github.com/iambipinpaul",
                Comments = "Updated comments"
            };

            _dbSetMock.Setup(db => db.FindAsync(requestInput.Email)).ReturnsAsync((Candidate)null);
            _dbSetMock.Setup(db => db.FindAsync(existingCandidate.Email)).ReturnsAsync(existingCandidate);

            // Act
            var result = await _candidateService.CreateOrUpdate(requestInput);

            // Assert
            _dbSetMock.Verify(db => db.Add(It.IsAny<Candidate>()), Times.Once);
            _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);
            Assert.False(result.IsUpdate);
        }
    }
}
