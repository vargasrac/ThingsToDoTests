using Moq;
using Microsoft.AspNetCore.Mvc;
using ThingsToDo.BLService;
using ThingsToDo.Controllers;
using ThingsToDo.Models;

public class ToDoTasksControllerTests
{
    private readonly Mock<IBLService> _mockBLService;
    private readonly ToDoTasksController _controller;

    public ToDoTasksControllerTests()
    {
        _mockBLService = new Mock<IBLService>();
        _controller = new ToDoTasksController(_mockBLService.Object);
    }

    [Fact]
    public async Task GetToDoTask_ReturnsAllTasks()
    {
        // Arrange
        var tasks = new List<ToDoTask>
        {
            new ToDoTask { Id = 1, Description = "Task 1" },
            new ToDoTask { Id = 2, Description = "Task 2" }
        };

        _mockBLService.Setup(service => service.GetToDoTaskAll()).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetToDoTask();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<ToDoTask>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnTasks = Assert.IsType<List<ToDoTask>>(okResult.Value);
        Assert.Equal(2, returnTasks.Count);
    }

    [Fact]
    public async Task GetToDoTaskById_ReturnsNotFound()
    {
        // Arrange
        _mockBLService.Setup(service => service.GetToDoTaskById(1)).ReturnsAsync((ToDoTask)null);

        // Act
        var result = await _controller.GetToDoTask(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetToDoTasksByPage_ReturnsTasks()
    {
        // Arrange
        var tasks = new List<ToDoTask>
        {
            new ToDoTask { Id = 1, Description = "Task 1" },
            new ToDoTask { Id = 2, Description = "Task 2" }
        };

        _mockBLService.Setup(service => service.GetToDoTaskByPage(1, 2)).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetToDoTasksByPage(1, 2);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<ToDoTask>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnTasks = Assert.IsType<List<ToDoTask>>(okResult.Value);
        Assert.Equal(2, returnTasks.Count);
    }

    [Fact]
    public async Task GetToDoTaskByTimestamps_ReturnsFilteredTasks()
    {
        // Arrange
        var tasks = new List<ToDoTask>
        {
            new ToDoTask { Id = 1, Description = "Task 1", TimeStamp = new DateTime(2024, 12, 10) },
            new ToDoTask { Id = 2, Description = "Task 2", TimeStamp = new DateTime(2024, 12, 11) }
        };

        _mockBLService.Setup(service => service.GetToDoTaskByTimestamps(
            It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetToDoTaskByTimestamps(new DateTime(2024, 12, 10), new DateTime(2024, 12, 11));

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<ToDoTask>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnTasks = Assert.IsType<List<ToDoTask>>(okResult.Value);
        Assert.Equal(2, returnTasks.Count);
    }

    // Add more test methods for other endpoints similarly...
}
