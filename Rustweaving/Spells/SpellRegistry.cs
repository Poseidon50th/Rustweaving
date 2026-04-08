using Vintagestory.API.Common;

namespace Rustweaving.Spells;

public sealed class SpellRegistry
{
    public SpellRegistry(ICoreAPI api)
    {
        Api = api;
    }

    public ICoreAPI Api { get; }
}
