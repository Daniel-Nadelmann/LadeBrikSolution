using LadeBrik.Database;
using LadeBrik.Models;
using LadeBrik.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace LadeBrikTests;

public class LadeBrikServiceTests
{
    private LadeBrikDbContext _context;
    private LadeBrikService _service;
    private Mock<ILogger<LadeBrikService>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<LadeBrikDbContext>()
            .UseInMemoryDatabase(databaseName: "LadeBrikTestDb")
            .Options;
        _context = new LadeBrikDbContext(options);
        _loggerMock = new Mock<ILogger<LadeBrikService>>();
        _service = new LadeBrikService(_context, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void CreateLadeBrik_ShouldCreateNewChip()
    {
        var id = "test-id";
        var chip = _service.CreateLadeBrik(id);

        Assert.IsNotNull(chip);
        Assert.AreEqual(id, chip.Id);
        Assert.IsTrue(chip.Active);
    }

    [Test]
    public void VerifyLadeBrik_ShouldReturnTrueIfChipExistsAndIsActive()
    {
        var id = "test-id";
        _context.LadeBriks.Add(new LadeBrikModel { Id = id, Active = true });
        _context.SaveChanges();

        var result = _service.VerifyLadeBrik(id);

        Assert.IsTrue(result);
    }

    [Test]
    public void BlockLadeBrik_ShouldSetActiveToFalse()
    {
        var id = "test-id";
        _context.LadeBriks.Add(new LadeBrikModel { Id = id, Active = true });
        _context.SaveChanges();

        var result = _service.BlockLadeBrik(id);

        Assert.IsTrue(result);
        var chip = _context.LadeBriks.Find(id);
        Assert.IsFalse(chip.Active);
    }
}
