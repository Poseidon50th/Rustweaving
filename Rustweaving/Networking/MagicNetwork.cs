using Rustweaving.Magic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
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

    public MagicState? ReadPersistedManaState(IPlayer player)
    {
        byte[]? data = player.WorldData.GetModdata(MagicState.ModDataKey);
        return SerializerUtil.Deserialize(data, default(MagicState));
    }

    public MagicState? ReadSyncedManaState(IPlayer player)
    {
        Entity? entity = player.Entity;
        if (entity?.WatchedAttributes is null)
        {
            return null;
        }

        return new MagicState
        {
            CurrentMana = entity.WatchedAttributes.GetInt(MagicState.CurrentManaWatchedAttributeKey, 0),
            MaxMana = entity.WatchedAttributes.GetInt(MagicState.MaxManaWatchedAttributeKey, 0)
        };
    }

    public void SyncManaState(IServerPlayer player, MagicState state, string reason)
    {
        byte[] serializedState = SerializerUtil.Serialize(state);
        player.WorldData.SetModdata(MagicState.ModDataKey, serializedState);
        WriteWatchedManaState(player.Entity, state);
        player.BroadcastPlayerData(false);

        Api.Logger.Notification(
            $"{LogPrefix} mana sync player={player.PlayerUID} reason={reason} current={state.CurrentMana} max={state.MaxMana}");
    }

    public bool SyncWatchedManaState(IServerPlayer player, MagicState state, string reason)
    {
        bool synced = WriteWatchedManaState(player.Entity, state);
        if (synced)
        {
            Api.Logger.Notification(
                $"{LogPrefix} mana sync player={player.PlayerUID} reason={reason} current={state.CurrentMana} max={state.MaxMana}");
        }

        return synced;
    }

    private static bool WriteWatchedManaState(Entity? entity, MagicState state)
    {
        if (entity?.WatchedAttributes is null)
        {
            return false;
        }

        entity.WatchedAttributes.SetInt(MagicState.CurrentManaWatchedAttributeKey, state.CurrentMana);
        entity.WatchedAttributes.SetInt(MagicState.MaxManaWatchedAttributeKey, state.MaxMana);
        entity.WatchedAttributes.MarkPathDirty(MagicState.CurrentManaWatchedAttributeKey);
        entity.WatchedAttributes.MarkPathDirty(MagicState.MaxManaWatchedAttributeKey);
        return true;
    }
}
