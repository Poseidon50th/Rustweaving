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

    private readonly ICoreClientAPI api;
    private readonly MagicNetwork magicNetwork;
    private readonly long updateListenerId;

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
        ElementBounds dialogBounds = ElementBounds.Fixed(EnumDialogArea.RightTop, -260, 40, 220, 64);
        ElementBounds backgroundBounds = dialogBounds.ForkBoundingParent(0, 0, 0, 0);
        ElementBounds textBounds = ElementBounds.Fixed(12, 10, 196, 20);
        ElementBounds statbarBounds = ElementBounds.Fixed(12, 34, 196, 14);

        SingleComposer = api.Gui
            .CreateCompo(ComposerName, dialogBounds)
            .AddShadedDialogBG(backgroundBounds)
            .AddDynamicText("Mana 0 / 0", CairoFont.WhiteSmallishText(), textBounds, TextKey)
            .AddStatbar(statbarBounds, new double[] { 0.12, 0.47, 0.86, 0.95 }, false, BarKey)
            .Compose(false);
    }

    private void OnHudTick(float deltaTime)
    {
        if (!IsOpened())
        {
            TryOpen();
        }

        UpdateHud(forceLog: false);
    }

    private void UpdateHud(bool forceLog)
    {
        IPlayer? player = api.World.Player;
        MagicState? state = player is null ? null : magicNetwork.ReadManaState(player);

        int currentMana = state?.CurrentMana ?? 0;
        int maxMana = state?.MaxMana ?? 0;

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
