using Microsoft.EntityFrameworkCore;
using Moq;
using TimeTracker.Database;
using Task = System.Threading.Tasks.Task;
using TimeTrackerTask = TimeTracker.Database.Task;

namespace TimeTracker.Tests;

public interface ITimeTrackerRepository
{
    Task StartTrackingAsync(Guid userId);
    Task StopTrackingAsync(Guid userId);
    Task<TimeSpan> GetTrackedTimeAsync(Guid userId);
}

public class TimeTrackerService
{
    private readonly ITimeTrackerRepository _repository;

    public TimeTrackerService(ITimeTrackerRepository repository)
    {
        _repository = repository;
    }

    public async Task StartTracking(Guid userId)
    {
        await _repository.StartTrackingAsync(userId);
    }

    public async Task StopTracking(Guid userId)
    {
        await _repository.StopTrackingAsync(userId);
    }

    public async Task<TimeSpan> GetTrackedTime(Guid userId)
    {
        return await _repository.GetTrackedTimeAsync(userId);
    }
}

public class TimeTrackerServiceTests
{
    private readonly Mock<ITimeTrackerRepository> _mockRepo;
    private readonly TimeTrackerService _service;

    public TimeTrackerServiceTests()
    {
        _mockRepo = new Mock<ITimeTrackerRepository>();
        _service = new TimeTrackerService(_mockRepo.Object);
    }

    [Fact]
    public async Task StartTracking_ShouldCallRepositoryMethod()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        await _service.StartTracking(userId);

        // Assert
        _mockRepo.Verify(r => r.StartTrackingAsync(userId), Times.Once);
    }

    [Fact]
    public async Task StopTracking_ShouldCallRepositoryMethod()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        await _service.StopTracking(userId);

        // Assert
        _mockRepo.Verify(r => r.StopTrackingAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetTrackedTime_ShouldReturnExpectedTime()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedTime = TimeSpan.FromHours(2);

        _mockRepo.Setup(r => r.GetTrackedTimeAsync(userId)).ReturnsAsync(expectedTime);

        // Act
        var result = await _service.GetTrackedTime(userId);

        // Assert
        Assert.Equal(expectedTime, result);
    }
}