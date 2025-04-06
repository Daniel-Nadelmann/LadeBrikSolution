using LadeBrik.Services;
using Microsoft.AspNetCore.Mvc;

namespace LadeBrik.Controllers;

[ApiController]
[Route("[controller]")]
public class LadeBrikController : ControllerBase
{

    private readonly ILogger<LadeBrikController> _logger;
    private readonly ILadeBrikService _LadeBrikService;

    public LadeBrikController(ILogger<LadeBrikController> logger, ILadeBrikService LadeBrikService)
    {
        _logger = logger;
        _LadeBrikService = LadeBrikService;
    }

    [HttpPost("Create")]
    public IActionResult Create(long tag)
    {
        var tagString = tag.ToString();
        if (string.IsNullOrEmpty(tagString) || tagString.Length != 10)
        {
            _logger.LogWarning("Invalid tag: {Tag}. Tag must be a string of exactly 10 characters.", tagString);
            return BadRequest("Tag must be a string of exactly 10 characters.");
        }

        var formattedTag = $"dk-{tagString}-clever";
        try
        {
            var LadeBrik = _LadeBrikService.CreateLadeBrik(formattedTag);
            return Ok(LadeBrik);
        }
        catch (Exception ex)
        {
            if (ex is BadHttpRequestException)
            {
                return BadRequest(ex.Message);
            }
            _logger.LogError(ex, "Error creating charging chip");
            return StatusCode(500);
        }
    }

    [HttpGet("Verify")]
    public IActionResult Verify(string id)
    {
        try
        {
            var verification = _LadeBrikService.VerifyLadeBrik(id);
            if (verification)
            {
                return Ok();
            }
            else
            {
                return StatusCode(404);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying charging chip");
            return StatusCode(500);
        }
    }

    [HttpPut("Block")]
    public IActionResult Block(string id)
    {
        try
        {
            var success = _LadeBrikService.BlockLadeBrik(id);
            if (!success)
            {
                return StatusCode(404);
            }
            else
            {
                return Ok();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error blocking charging chip");
            return StatusCode(500);
        }
    }
}
