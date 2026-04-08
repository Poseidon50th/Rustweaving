# Rustweaving vertical slice v0

## Goal

Build the first playable and expandable magic slice for Vintage Story.

## In scope

- One spellbook item
- Spell-selection UI opened from the spellbook
- Selected spell persistence per player
- Mana resource
- Server-authoritative spell casting
- Multiplayer-safe state sync
- Cooldowns
- Durations
- One direct damage spell
- One direct heal spell
- One utility spell
- One summon spell
- One ritual cast path

## Out of scope

- Classes
- Leveling
- Spell slots
- Prepared spell lists
- Wands
- Staffs
- Subclasses
- Full D&D action economy
- Counterspell/reactions
- Harmony patches unless required later

## Player mana rules

- Each player has `currentMana` and `maxMana`
- New players start at full mana
- There is NO passive mana regeneration
- Mana restores only from sleep and meditation

## Sleep mana restore

- Sleep restore is proportional to sleep completed
- Formula for v0:
  - `sleepFraction = actualSleepTime / requiredFullSleepTime`
  - `manaRestored = floor(maxMana * sleepFraction)`
- Clamp sleepFraction to 0.0 - 1.0
- If the player completes full sleep naturally, set `currentMana = maxMana`
- If sleep is interrupted early, restore only the proportional amount
- Sleep restore is resolved on the server when sleep ends

## Meditation mana restore

- Meditation is only allowed while seated
- v0 seated check = mounted on an allowed seat / mountable seat
- Meditation is a channeled action
- Meditation can restore mana up to 50% of max mana, never higher
- Formula for v0:
  - `meditationCap = floor(maxMana * 0.5)`
  - restore until `currentMana == meditationCap` or meditation is interrupted
- Meditation is canceled by movement, damage, unseating, or manual cancel

## Spellbook UX

- v0 uses only the spellbook item
- Sneak + right click opens the spell-selection UI
- Normal right click casts the currently selected spell
- The selected spell remains stored on the player until changed
- The UI should be minimal and reliable before it is pretty

## Multiplayer

- Client sends requests only
- Server validates mana, cooldown, spell availability, target/range, and ritual requirements
- Server applies effects
- Client displays UI and visual feedback

## First spells

- `rustbolt` - simple projectile damage spell
- `minorGearmend` - direct heal spell
- `light` - timed utility light
- `summonWisp` - timed summon
- `ritualWisp` - ritual-only summon path

## Ritual v0

- One ritual circle block or placed ritual object
- One reagent recipe only
- One ritual output only
- Ritual uses cast time, interruption, reagent consumption, and server validation

## Logging requirements

Prefix all logs with `[Rustweaving]`

Must log:

- player mana init
- sleep start
- sleep end
- sleep fraction
- mana restored from sleep
- meditation start
- meditation tick/stop
- mana restored from meditation
- spell selection changes
- cast request received
- cast accepted/rejected with reason
- summon spawn/despawn
- ritual start/cancel/complete

## Compile-only skeleton

Created classes for the initial vertical-slice wiring:

- `RustweavingModSystem`
- `SpellRegistry`
- `SpellCastService`
- `MagicNetwork`
- `ItemSpellbook`
- `ItemSpellFocus`
- `HudManaBar`
- `EntitySpellProjectile`
- `EntitySummonedWisp`
- `BlockEntityRitualCircle`
