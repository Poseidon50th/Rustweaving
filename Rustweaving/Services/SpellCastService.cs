using Rustweaving.Spells;
using Vintagestory.API.Common;

namespace Rustweaving.Services;

public sealed class SpellCastService
{
    public SpellCastService(ICoreAPI api, SpellRegistry spellRegistry)
    {
        Api = api;
        SpellRegistry = spellRegistry;
    }

    public ICoreAPI Api { get; }

    public SpellRegistry SpellRegistry { get; }
}
