using LadeBrik.Database;
using LadeBrik.Models;

namespace LadeBrik.Services;

public class LadeBrikService : ILadeBrikService
{
    private readonly LadeBrikDbContext _LadeBrikDbContext;
    private readonly ILogger<LadeBrikService> _logger;
    public LadeBrikService(LadeBrikDbContext LadeBrikDbContext, ILogger<LadeBrikService> logger)
    {
        _LadeBrikDbContext = LadeBrikDbContext;
        _logger = logger;
    }
    public bool BlockLadeBrik(string id)
    {
        try
        {
            var chip = _LadeBrikDbContext.LadeBriks.Find(id);
            if (chip == null)
            {
                _logger.LogWarning("LadeBrik with ID {Id} not found.", id);
                return false;
            }

            chip.Active = false;
            _LadeBrikDbContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error blocking LadeBrik with ID {Id}.", id);
            return false;
        }
    }

    public LadeBrikModel CreateLadeBrik(string id)
    {
        try
        {
            // Check if a LadeBrik with the same ID already exists
            var existingChip = _LadeBrikDbContext.LadeBriks.Find(id);
            if (existingChip != null)
            {
                return existingChip;
            }

            var newChip = new LadeBrikModel
            {
                Id = id,
                Active = true
            };

            _LadeBrikDbContext.LadeBriks.Add(newChip);
            _LadeBrikDbContext.SaveChanges();

            return newChip;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating LadeBrik with ID {Id}.", id);
            throw;
        }
    }

    public bool VerifyLadeBrik(string id)
    {
        try
        {
            var chip = _LadeBrikDbContext.LadeBriks
                .FirstOrDefault(c => c.Id == id && c.Active);
            return chip != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying LadeBrik with ID {Id}.", id);
            return false;
        }
    }
}
