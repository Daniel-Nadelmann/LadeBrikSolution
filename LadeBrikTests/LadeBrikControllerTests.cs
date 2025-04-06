using LadeBrik.Controllers;
using LadeBrik.Models;
using LadeBrik.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LadeBrikTests;

public class LadeBrikControllerTests
{
    private Mock<ILadeBrikService> _serviceMock;
    private Mock<ILogger<LadeBrikController>> _loggerMock;
    private LadeBrikController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<ILadeBrikService>();
        _loggerMock = new Mock<ILogger<LadeBrikController>>();
        _controller = new LadeBrikController(_loggerMock.Object, _serviceMock.Object);
    }

    [Test]
    public void Create_ShouldReturnBadRequestIfTagIsInvalid()
    {
        var result = _controller.Create(123);

        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public void Create_ShouldReturnOkIfTagIsValid()
    {
        var tag = 1234567890;
        var formattedTag = $"dk-{tag}-clever";
        var chip = new LadeBrikModel { Id = formattedTag, Active = true };
        _serviceMock.Setup(s => s.CreateLadeBrik(formattedTag)).Returns(chip);

        var result = _controller.Create(tag);

        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.AreEqual(chip, okResult.Value);
    }

    [Test]
    public void Verify_ShouldReturnOkIfChipIsVerified()
    {
        var id = "test-id";
        _serviceMock.Setup(s => s.VerifyLadeBrik(id)).Returns(true);

        var result = _controller.Verify(id);

        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public void Verify_ShouldReturnNotFoundIfChipIsNotVerified()
    {
        var id = "test-id";
        _serviceMock.Setup(s => s.VerifyLadeBrik(id)).Returns(false);

        var result = _controller.Verify(id);

        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void Block_ShouldReturnOkIfChipIsBlocked()
    {
        var id = "test-id";
        _serviceMock.Setup(s => s.BlockLadeBrik(id)).Returns(true);

        var result = _controller.Block(id);

        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public void Block_ShouldReturnNotFoundIfChipIsNotBlocked()
    {
        var id = "test-id";
        _serviceMock.Setup(s => s.BlockLadeBrik(id)).Returns(false);

        var result = _controller.Block(id);

        Assert.IsInstanceOf<NotFoundResult>(result);
    }
}
