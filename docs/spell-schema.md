# Spell schema v0

## Purpose

All spells must use the same minimal schema.

## Required fields

- `code`: unique string id
- `displayName`: player-facing name
- `castKind`: `projectile | direct | utility | summon | ritual`
- `manaCost`: integer
- `cooldownMs`: integer
- `castTimeMs`: integer
- `durationMs`: integer, 0 if not timed
- `range`: number
- `requiresTarget`: bool
- `allowSelfTarget`: bool
- `ritualOnly`: bool
- `iconPath`: optional string
- `logLabel`: short string for logs

## Optional effect fields

- `damage`
- `heal`
- `projectileCode`
- `summonEntityCode`
- `lightLevel`
- `reagentCodes`
- `reagentQuantities`

## v0 spell definitions

### rustbolt

- castKind: projectile
- manaCost: 15
- cooldownMs: 1000
- castTimeMs: 0
- durationMs: 0
- range: 20
- requiresTarget: false
- allowSelfTarget: false
- ritualOnly: false

### minorGearmend

- castKind: direct
- manaCost: 20
- cooldownMs: 2000
- castTimeMs: 0
- durationMs: 0
- range: 8
- requiresTarget: true
- allowSelfTarget: true
- ritualOnly: false

### light

- castKind: utility
- manaCost: 10
- cooldownMs: 1000
- castTimeMs: 0
- durationMs: 60000
- range: 0
- requiresTarget: false
- allowSelfTarget: true
- ritualOnly: false

### summonWisp

- castKind: summon
- manaCost: 35
- cooldownMs: 10000
- castTimeMs: 1000
- durationMs: 120000
- range: 6
- requiresTarget: false
- allowSelfTarget: false
- ritualOnly: false

### ritualWisp

- castKind: ritual
- manaCost: 25
- cooldownMs: 0
- castTimeMs: 8000
- durationMs: 180000
- range: 3
- requiresTarget: false
- allowSelfTarget: false
- ritualOnly: true
