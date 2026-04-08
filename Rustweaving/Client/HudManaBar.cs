using Vintagestory.API.Client;

namespace Rustweaving.Client;

public sealed class HudManaBar
{
    public HudManaBar(ICoreClientAPI api)
    {
        Api = api;
    }

    public ICoreClientAPI Api { get; }
}
