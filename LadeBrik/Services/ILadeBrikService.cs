using LadeBrik.Models;

namespace LadeBrik.Services;

public interface ILadeBrikService
{
    LadeBrikModel CreateLadeBrik(string tag);
    bool VerifyLadeBrik(string id);
    bool BlockLadeBrik(string id);
}
