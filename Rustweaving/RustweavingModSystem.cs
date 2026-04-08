using Rustweaving.BlockEntities;
using Rustweaving.Client;
using Rustweaving.Entities;
using Rustweaving.Items;
using Rustweaving.Networking;
using Rustweaving.Services;
using Rustweaving.Spells;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Rustweaving;

public sealed class RustweavingModSystem : ModSystem
{
    private const string LogPrefix = "[Rustweaving]";

    private SpellRegistry? spellRegistry;
    private SpellCastService? spellCastService;
    private MagicNetwork? magicNetwork;
    private ManaStateService? manaStateService;
    private HudManaBar? hudManaBar;

    public override void Start(ICoreAPI api)
    {
        api.Logger.Notification($"{LogPrefix} Registering vertical slice skeleton on {api.Side}.");

        api.RegisterItemClass(ItemSpellbook.ClassName, typeof(ItemSpellbook));
        api.RegisterItemClass(ItemSpellFocus.ClassName, typeof(ItemSpellFocus));
        api.RegisterEntity(EntitySpellProjectile.ClassName, typeof(EntitySpellProjectile));
        api.RegisterEntity(EntitySummonedWisp.ClassName, typeof(EntitySummonedWisp));
        api.RegisterBlockEntityClass(BlockEntityRitualCircle.ClassName, typeof(BlockEntityRitualCircle));

        spellRegistry = new SpellRegistry(api);
        magicNetwork = new MagicNetwork(api);
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        spellRegistry ??= new SpellRegistry(api);
        spellCastService = new SpellCastService(api, spellRegistry);
        magicNetwork ??= new MagicNetwork(api);
        magicNetwork.InitializeServer(api);
        manaStateService = new ManaStateService(api, magicNetwork);
        api.Event.RegisterGameTickListener(manaStateService.EnsureOnlinePlayersInitialized, 1000);

        api.Logger.Notification($"{LogPrefix} Server-side vertical slice skeleton initialized.");
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        magicNetwork ??= new MagicNetwork(api);
        magicNetwork.InitializeClient(api);
        hudManaBar = new HudManaBar(api, magicNetwork);

        api.Logger.Notification($"{LogPrefix} Client-side vertical slice skeleton initialized.");
    }
}
