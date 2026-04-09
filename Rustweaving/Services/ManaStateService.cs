using System;
using Rustweaving.Magic;
using Rustweaving.Networking;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Rustweaving.Services;

public sealed class ManaStateService
{
    private const string LogPrefix = "[Rustweaving]";

    private readonly ICoreServerAPI api;
    private readonly MagicNetwork magicNetwork;

    public ManaStateService(ICoreServerAPI api, MagicNetwork magicNetwork)
    {
        this.api = api;
        this.magicNetwork = magicNetwork;
    }

    public void EnsureOnlinePlayersInitialized(float deltaTime)
    {
        foreach (IPlayer onlinePlayer in api.World.AllOnlinePlayers)
        {
            if (onlinePlayer is IServerPlayer serverPlayer)
            {
                EnsureState(serverPlayer);
            }
        }
    }

    public MagicState EnsureState(IServerPlayer player)
    {
        MagicState? existingState = magicNetwork.ReadPersistedManaState(player);
        bool created = existingState is null;

        MagicState state = existingState?.Clone() ?? MagicState.CreateDefault();
        int originalCurrentMana = state.CurrentMana;
        int originalMaxMana = state.MaxMana;

        state.Clamp();

        if (created)
        {
            api.Logger.Notification($"{LogPrefix} mana init player={player.PlayerUID} current={state.CurrentMana} max={state.MaxMana}");
            magicNetwork.SyncManaState(player, state, "init");
            return state;
        }

        if (originalCurrentMana != state.CurrentMana || originalMaxMana != state.MaxMana)
        {
            api.Logger.Notification($"{LogPrefix} mana init clamp player={player.PlayerUID} current={state.CurrentMana} max={state.MaxMana}");
            magicNetwork.SyncManaState(player, state, "clamp");
            return state;
        }

        magicNetwork.SyncWatchedManaState(player, state, "entitySync");
        return state;
    }

    public MagicState GetState(IServerPlayer player)
    {
        return EnsureState(player);
    }

    public void SetState(IServerPlayer player, MagicState state, string reason)
    {
        state.Clamp();
        magicNetwork.SyncManaState(player, state, reason);
    }

    public void ApplySleepRestore(IServerPlayer player, double sleepFraction, bool completedNaturally)
    {
        MagicState state = EnsureState(player);
        int previousMana = state.CurrentMana;
        double clampedFraction = Math.Clamp(sleepFraction, 0d, 1d);

        if (completedNaturally || clampedFraction >= 1d)
        {
            state.CurrentMana = state.MaxMana;
        }
        else
        {
            int restoredMana = (int)Math.Floor(state.MaxMana * clampedFraction);
            state.CurrentMana = Math.Min(state.MaxMana, state.CurrentMana + restoredMana);
        }

        int gainedMana = state.CurrentMana - previousMana;

        api.Logger.Notification(
            $"{LogPrefix} sleep restore player={player.PlayerUID} fraction={clampedFraction:0.###} restored={gainedMana} current={state.CurrentMana} max={state.MaxMana}");

        if (gainedMana != 0)
        {
            magicNetwork.SyncManaState(player, state, "sleepRestore");
        }
    }

    public void ApplyMeditationRestore(IServerPlayer player, int requestedMana)
    {
        MagicState state = EnsureState(player);
        int meditationCap = (int)Math.Floor(state.MaxMana * 0.5f);
        int restoredMana = Math.Clamp(requestedMana, 0, Math.Max(0, meditationCap - state.CurrentMana));

        if (restoredMana <= 0)
        {
            api.Logger.Notification(
                $"{LogPrefix} meditation restore player={player.PlayerUID} restored=0 current={state.CurrentMana} cap={meditationCap}");
            return;
        }

        state.CurrentMana += restoredMana;

        api.Logger.Notification(
            $"{LogPrefix} meditation restore player={player.PlayerUID} restored={restoredMana} current={state.CurrentMana} cap={meditationCap} max={state.MaxMana}");

        magicNetwork.SyncManaState(player, state, "meditationRestore");
    }
}
