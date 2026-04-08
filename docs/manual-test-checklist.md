# Manual test checklist

## Build / load

- [ ] Project builds successfully
- [ ] Mod loads without startup exceptions
- [ ] No missing registrations for items, entities, or block entities
- [ ] Vertical-slice skeleton item classes register without startup exceptions
- [ ] Vertical-slice skeleton entity classes register without startup exceptions
- [ ] Vertical-slice skeleton block entity class registers without startup exceptions

## Mana

- [ ] New player starts at full mana
- [ ] Mana persists after relog
- [ ] Mana does not passively regenerate
- [ ] Server initializes mana state on first join/load
- [ ] Server syncs mana state changes to the client
- [ ] Mana HUD shows current and max mana
- [ ] Mana HUD updates after relog or state sync

## Sleep restore

- [ ] Sleep start is logged
- [ ] Early interrupted sleep restores only a proportional amount of mana
- [ ] Full natural sleep restores all mana
- [ ] Sleep end logs fraction and mana restored
- [ ] Multiplayer: one player sleeping does not break another player’s mana state

## Meditation

- [ ] Meditation only starts while seated
- [ ] Meditation stops on movement
- [ ] Meditation stops on damage
- [ ] Meditation never restores above 50% max mana
- [ ] Meditation logs start/end and restored amount

## Spellbook

- [ ] Sneak + right click opens spell-selection UI
- [ ] Spell selection persists
- [ ] Normal right click casts selected spell
- [ ] UI closes and reopens correctly
- [ ] No duplicate dialogs

## Casting

- [ ] Server rejects casts without enough mana
- [ ] Server rejects casts on cooldown
- [ ] Accepted casts spend mana once
- [ ] Rejected casts do not spend mana
- [ ] Multiplayer: remote player sees spell results correctly

## Spells

- [ ] sparkbolt deals damage
- [ ] minorMend heals valid targets
- [ ] light starts and expires correctly
- [ ] summonWisp spawns and despawns correctly
- [ ] ritualWisp only works through ritual casting

## Ritual

- [ ] Ritual starts only with valid requirements
- [ ] Ritual interruption is handled correctly
- [ ] Ritual completion consumes reagents and produces the effect
- [ ] Ritual logs start/cancel/complete

## Assets / UI

- [ ] No missing texture errors
- [ ] No missing shape errors
- [ ] No missing icon errors
- [ ] Spellbook item renders correctly
- [ ] Summoned entity renders correctly
