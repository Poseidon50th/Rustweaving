using System;
using Vintagestory.API.Client;
using Rustweaving.Magic;
using Rustweaving.Networking;
using Vintagestory.API.Common;

namespace Rustweaving.Client;

public sealed class HudManaBar : GuiDialogGeneric
{
    private const string LogPrefix = "[Rustweaving]";
    private const string ComposerName = "rustweaving-manahud";
    private const string TextKey = "manaText";
    private const string BarKey = "manaBar";
    private static readonly double HudWidth = 180;
    private static readonly double HudHeight = 44;
    private static readonly double TextWidth = 180;
    private static readonly double TextHeight = 16;
    private static readonly double BarWidth = 180;
    private static readonly double BarHeight = 12;

    private readonly ICoreClientAPI api;
    private readonly MagicNetwork magicNetwork;
    private readonly long updateListenerId;

    private bool composedSuccessfully;
    private int lastCurrentMana = -1;
    private int lastMaxMana = -1;

    public HudManaBar(ICoreClientAPI api, MagicNetwork magicNetwork) : base("Rustweaving Mana", api)
    {
        Api = api;
        this.api = api;
        this.magicNetwork = magicNetwork;

        ComposeHud();
        updateListenerId = api.Event.RegisterGameTickListener(OnHudTick, 250);
        TryOpen();
    }

    public ICoreClientAPI Api { get; }

    public override double DrawOrder => 0.2;

    public override bool CaptureAllInputs()
    {
        return false;
    }

    public override bool ShouldReceiveKeyboardEvents()
    {
        return false;
    }

    public override bool ShouldReceiveMouseEvents()
    {
        return false;
    }

    public override void OnGuiOpened()
    {
        base.OnGuiOpened();
        UpdateHud(forceLog: false);
    }

    public override void Dispose()
    {
        api.Event.UnregisterGameTickListener(updateListenerId);
        base.Dispose();
    }

    private void ComposeHud()
    {
        if (HudWidth <= 0 || HudHeight <= 0 || TextWidth <= 0 || TextHeight <= 0 || BarWidth <= 0 || BarHeight <= 0)
        {
            api.Logger.Notification($"{LogPrefix} skipping mana HUD compose due to invalid bounds width={HudWidth} height={HudHeight}");
            composedSuccessfully = false;
            return;
        }

        ElementBounds dialogBounds = ElementBounds.Fixed(EnumDialogArea.LeftTop, 12, 12, HudWidth, HudHeight);
        ElementBounds textBounds = ElementBounds.Fixed(0, 0, TextWidth, TextHeight);
        ElementBounds statbarBounds = ElementBounds.Fixed(0, 24, BarWidth, BarHeight);

        api.Logger.Notification($"{LogPrefix} composing mana HUD width={HudWidth} height={HudHeight}");

        SingleComposer = api.Gui
            .CreateCompo(ComposerName, dialogBounds)
            .AddDynamicText("Mana 0 / 0", CairoFont.WhiteSmallishText(), textBounds, TextKey)
            .AddStatbar(statbarBounds, new double[] { 0.12, 0.47, 0.86, 0.95 }, false, BarKey)
            .Compose(false);

        composedSuccessfully = true;
    }

    private void OnHudTick(float deltaTime)
    {
        if (!composedSuccessfully)
        {
            return;
        }

        if (!IsOpened())
        {
            TryOpen();
        }

        UpdateHud(forceLog: false);
    }

    private void UpdateHud(bool forceLog)
    {
        if (!composedSuccessfully || SingleComposer is null)
        {
            return;
        }

        IPlayer? player = api.World.Player;
        MagicState? state = player is null ? null : magicNetwork.ReadSyncedManaState(player);

        if (player?.Entity is null || state is null || state.MaxMana <= 0)
        {
            return;
        }

        int currentMana = state.CurrentMana;
        int maxMana = state.MaxMana;

        SingleComposer.GetDynamicText(TextKey).SetNewText($"Mana {currentMana} / {maxMana}");
        SingleComposer.GetStatbar(BarKey).SetValues(currentMana, 0, Math.Max(1, maxMana));

        if (forceLog || currentMana != lastCurrentMana || maxMana != lastMaxMana)
        {
            api.Logger.Notification($"{LogPrefix} mana sync client current={currentMana} max={maxMana}");
            lastCurrentMana = currentMana;
            lastMaxMana = maxMana;
        }
    }
}
