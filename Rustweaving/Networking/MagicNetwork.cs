using Rustweaving.Magic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Rustweaving.Networking;

public sealed class MagicNetwork
{
    private const string LogPrefix = "[Rustweaving]";

    public MagicNetwork(ICoreAPI api)
    {
        Api = api;
    }

    public ICoreAPI Api { get; }

    public void InitializeClient(ICoreClientAPI api)
    {
        api.Logger.Notification($"{LogPrefix} Client mana sync bridge initialized.");
    }

    public void InitializeServer(ICoreServerAPI api)
    {
        api.Logger.Notification($"{LogPrefix} Server mana sync bridge initialized.");
    }

    public MagicState? ReadManaState(IPlayer player)
    {
        byte[]? data = player.WorldData.GetModdata(MagicState.ModDataKey);
        return SerializerUtil.Deserialize(data, default(MagicState));
    }

    public void SyncManaState(IServerPlayer player, MagicState state, string reason)
    {
        byte[] serializedState = SerializerUtil.Serialize(state);
        player.WorldData.SetModdata(MagicState.ModDataKey, serializedState);
        player.BroadcastPlayerData(false);

        Api.Logger.Notification(
            $"{LogPrefix} mana sync player={player.PlayerUID} reason={reason} current={state.CurrentMana} max={state.MaxMana}");
    }
}
