using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Rustweaving.Networking;

public sealed class MagicNetwork
{
    public MagicNetwork(ICoreAPI api)
    {
        Api = api;
    }

    public ICoreAPI Api { get; }

    public void InitializeClient(ICoreClientAPI api)
    {
    }

    public void InitializeServer(ICoreServerAPI api)
    {
    }
}
