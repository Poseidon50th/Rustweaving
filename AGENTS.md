# Rustweaving repository rules

Read these first before making changes:
- docs/vertical-slice.md
- docs/spell-schema.md
- docs/manual-test-checklist.md

Project target:
- Vintage Story 1.21.x code mod in C#
- Windows-first local development
- Keep multiplayer behavior server-authoritative

Hard rules:
- Do not redesign the project structure unless explicitly asked.
- Do not add unrelated refactors.
- Do not add classes, leveling, spell slots, staffs, or wands in v0.
- v0 uses one spellbook item only.
- The spellbook opens a spell-selection UI, and the selected spell is then cast from the spellbook.
- Mana does not passively regenerate.
- Mana restore sources in v0 are ONLY sleep and meditation.
- Sleep restores mana proportionally to sleep completed; full natural sleep restores all mana.
- Meditation restores mana only while seated and never above 50% of max mana.
- Keep gameplay authority on the server. The client may request actions and render UI/FX, but the server validates and resolves them.
- Prefer minimal working code over speculative abstraction.
- Do not use Harmony unless explicitly required.
- Use placeholder assets when art is missing; do not block implementation on final art.
- Gameplay is server-authoritative.
- Do not invent Vintage Story APIs; inspect the repo and docs first.
- Keep features small and complete.
- Do not refactor unrelated systems.
- Prefer one file/class per responsibility.
- Add registrations for new item/entity/block entity classes.
- Update docs/manual-test-checklist.md for every gameplay change.
- Build after each task.
- If a task would require a broad framework, implement only the minimum needed for the requested feature.

Implementation rules:
- Reuse the existing registry/service/network pattern if present.
- Add logging for mana init, sleep start/end, meditation start/end, spell select, cast request accept/reject, summon spawn/despawn, and ritual start/cancel/complete.
- Build after each task.
- Update docs/manual-test-checklist.md whenever behavior changes.

At the end of each task:
- Report files changed
- Report build result
- Report known risks or follow-up items