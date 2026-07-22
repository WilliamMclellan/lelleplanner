# Session Summaries

## Session 1 — 2026-07-03
Summary: Scaffolding and repo hygiene for the Lelleplanner MVP.

Actions performed:
- Created solution and projects:
  - Lelleplanner.sln
  - src\Lelleplanner.Core (classlib, net8.0)
  - src\Lelleplanner.ConsoleApp (console, net8.0)
- Added ProjectReference: ConsoleApp -> Core
- Added global.json pinning SDK 8.0.422
- Initialized git and added .gitignore
- Added .gitattributes (normalized first line to "* text=auto") and ran normalization
- Updated PLAN.md: marked "Session 1 complete" and noted Session 2 next
- Verified dotnet sdks (8.0.422 and 10.0.301 installed) and successful build

Files created/modified:
- Lelleplanner.sln
- src\Lelleplanner.Core\Lelleplanner.Core.csproj
- src\Lelleplanner.ConsoleApp\Lelleplanner.ConsoleApp.csproj
- src\Lelleplanner.ConsoleApp\Program.cs (template)
- global.json
- .gitignore
- .gitattributes
- PLAN.md
- SESSIONSUMMARIES.md (this file)

## Session 2 — 2026-07-03
Summary: Core types and persistence for the Lelleplanner MVP.

Actions performed:
- Created Quest.cs: key, title, goal, completed properties with null-check constructor
- Created GameClock.cs: static utility with GetGameDate() logic (4:00 AM cutover)
- Created GameState.cs: game date, daily coins, quest list; RolloverIfNeeded() method
- Created Persistence.cs: static JSON load/save to %APPDATA%\Lelleplanner\gamestate.json
- Updated VISION.md to standardize quest names ("Buff Papaya", "Pretty Boy Papaya")
- Confirmed build succeeds

Files created:
- src\Lelleplanner.Core\Quest.cs
- src\Lelleplanner.Core\GameClock.cs
- src\Lelleplanner.Core\GameState.cs
- src\Lelleplanner.Core\Persistence.cs

Next steps (Session 3):
- Implement GameEngine.cs: toggle quest, detect full clear, award coin
- Wire these methods into Program.cs main loop (Session 5)

## Session 3 — 2026-07-03
Summary: GameEngine for the Lelleplanner MVP, plus a design change and two bug fixes surfaced along the way.

Actions performed:
- Created GameEngine.cs: `CompleteQuest` (marks a quest complete by key) and
  `CompleteDailyQuest` (detects all 6 daily quests complete, marks the
  "Excellent work, daily cleared!" meta-quest complete, and awards exactly
  one Daily Coin per game-day)
- Design change: quest completion is one-directional (cleared only by the
  next game-day rollover), not a toggle — updated PLAN.md's Definition of
  Done and solution layout comment to match; parked a "confirmation prompt
  on quest completion" idea for a future console-UI iteration
- Added the meta-quest as a 7th `Quest` in `GameState.InitializeDefaultQuests()`
- Fixed a bug in `GameEngine.CompleteDailyQuest`'s full-clear check (was
  counting non-meta quests instead of checking their `Completed` state) and
  added a guard against re-awarding the coin on repeated calls
- Enabled nullable annotations on `FirstOrDefault` results (`Quest?`) to
  match the project's nullable reference types setting
- Fixed a bug in `Persistence.Save` where an unclosed `File.Create` stream
  locked the file and made the following `File.WriteAllText` throw; then
  simplified by removing the redundant `File.Create` call entirely, since
  `WriteAllText` creates the file itself
- Fixed persistence path: corrected `%APPDATA%\Local\...` to
  `%LOCALAPPDATA%\...`, and updated PLAN.md/CONTEXT.md to match
- Manually smoke-tested end-to-end: completing all 6 quests, full-clear +
  coin award, double-award guard, and the Persistence save/load round-trip
- Confirmed build succeeds with no warnings

Files created/modified:
- src\Lelleplanner.Core\GameEngine.cs
- src\Lelleplanner.Core\GameState.cs
- src\Lelleplanner.Core\Persistence.cs
- PLAN.md
- CONTEXT.md

Next steps (Session 4):
- Implement AsciiArt.cs (banner + celebration art) and ConsoleRenderer.cs
  (draws banner, quest list, prompts)

## Session 4 — 2026-07-03
Summary: AsciiArt and ConsoleRenderer for the Lelleplanner MVP, with a project-boundary fix and a couple of real bugs caught along the way.

Actions performed:
- Created ConsoleRenderer.cs (in Lelleplanner.ConsoleApp) with four static,
  stateless rendering methods: RenderBanner, RenderQuestList (numbers active
  quests, tags completed ones as [CLEAR]), RenderDailyClearCelebration, and
  PromptForQuestNumber
- Caught and fixed: ConsoleRenderer was initially created inside
  Lelleplanner.Core, which would have reintroduced console I/O into the
  console-free Core project; moved it to Lelleplanner.ConsoleApp with the
  correct namespace
- Caught and fixed an infinite loop in PromptForQuestNumber (input was only
  read once, before the retry loop, instead of on every iteration)
- Removed a dead/no-op "Stop" input check that looked like an unfinished
  quit feature
- Replaced a hardcoded quest-count literal in RenderBanner with
  gameState.Quests.Count()
- Created AsciiArt.cs with Banner and Celebration ASCII art (raw string
  literals, plain 7-bit ASCII only for Windows Console compatibility)
- Confirmed build succeeds with no warnings

Files created:
- src\Lelleplanner.ConsoleApp\ConsoleRenderer.cs
- src\Lelleplanner.ConsoleApp\AsciiArt.cs

Next steps (Session 5):
- Wire GameEngine + ConsoleRenderer + Persistence into Program.cs's main
  loop end-to-end; manually test a full clear and a rollover

## Session 5 — 2026-07-04
Summary: Wired up Program.cs's main loop end-to-end, closing out the MVP's
core gameplay flow. Also a working-agreement change: the user now wants to
write Lelleplanner's application code themselves, with Claude guiding via
diagnosis and design discussion rather than writing it directly (added to
CONTEXT.md's working agreements).

Actions performed:
- Wired the main loop in Program.cs: load state, render banner/quest list
  each iteration, prompt for a quest number, validate it, complete the
  chosen quest, detect and celebrate a fresh daily clear, and save
- Added `GameEngine.HasRemainingQuests` and `GameEngine.ValidQuestNumber` to
  support the loop's exit and input-validation guard clauses
- Added `ConsoleRenderer.RenderQuestCompleted` and `AsciiArt.QuestCompleted`
  for per-quest completion feedback
- Caught and fixed several bugs surfaced during the build-out:
  - `GameClock.GetGameDate` constructed the previous day via
    `new DateOnly(year, month, day - 1)`, which throws on the 1st of any
    month; replaced with `DateOnly.FromDateTime(now.Date.AddDays(-1))`
  - `GameState.RolloverIfNeeded` reset quest completion on a new game-day
    but never updated `GameDate` itself, leaving it permanently stale;
    fixed to update it once, after the reset, using a cached date value
  - An early `ValidQuestNumber` draft only accepted input exactly equal to
    the active-quest count (`> activeQuests || < activeQuests` instead of
    `< 1`), rejecting every other valid number
  - The daily-clear path initially called `GameEngine.CompleteQuest` on the
    meta-quest directly instead of `GameEngine.CompleteDailyQuest`, which
    would have shown the celebration without ever awarding the Daily Coin
  - Consolidated duplicated "Saving... / File Saved!" console output into a
    single `Save` local function in Program.cs (not GameEngine, to keep
    Console I/O out of Lelleplanner.Core), called after every quest
    completion and at both exit paths
- Manually tested end-to-end: a full daily clear (banner, quest list,
  numbered completion, celebration, coin award) and a day rollover (quests
  reset, coins untouched) both confirmed working
- Walked the MVP Definition of Done checklist in PLAN.md — all items now
  checked off

Files created/modified:
- src\Lelleplanner.ConsoleApp\Program.cs
- src\Lelleplanner.Core\GameEngine.cs
- src\Lelleplanner.Core\GameClock.cs
- src\Lelleplanner.Core\GameState.cs
- src\Lelleplanner.ConsoleApp\ConsoleRenderer.cs
- src\Lelleplanner.ConsoleApp\AsciiArt.cs
- CONTEXT.md (added the "Claude doesn't write app code" working agreement)
- PLAN.md

Next steps (Session 6):
- Bug fixes, walk the Definition of Done checklist, tag/commit v0.1 (DoD
  checklist is already fully checked off as of this session — Session 6 may
  end up being light: a final read-through plus the v0.1 tag/commit)

## Session 6 — 2026-07-04
Summary: Final read-through and Definition of Done walkthrough for the MVP.
Confirmed session 5's work holds up end-to-end; no new bugs found. Light
session, as anticipated.

Actions performed:
- Confirmed a clean `dotnet build` with 0 warnings/errors
- Read through every Core and ConsoleApp source file against the DoD
  checklist in PLAN.md; all items check out against the actual code
- Ran a smoke test (`dotnet run` piping `0`) to confirm the banner, quest
  list, and clean exit/save path all still work
- Updated PLAN.md's status line to "MVP (v0.1) — complete" and pointed
  "what's next" at iteration 1 (Testing + Weekly quests)

Files created/modified:
- PLAN.md
- SESSIONSUMMARIES.md

Next steps: tag/commit `v0.1` (user to do via git), then start iteration 1
(Testing + Weekly quests) whenever ready.

When ready, run: dotnet build && dotnet run --project src\Lelleplanner.ConsoleApp

## Session 7 — 2026-07-04
Summary: Scaffolded the test project to kick off Iteration 1.

Actions performed:
- Created `Lelleplanner.Tests` (xUnit), referencing `Lelleplanner.Core`
- Added the project to `Lelleplanner.sln`
- Wrote `GameClockTests.cs`: a `[Theory]`-driven test covering `GameClock.GetGameDate`'s
  cutover-hour boundary, including the month/year-rollover edge cases (e.g. Jan 1st
  before cutover correctly resolving to Dec 31st of the prior year)
- Updated PLAN.md and CONTEXT.md to reflect Iteration 1 starting

Files created/modified:
- src\Lelleplanner.Tests\Lelleplanner.Tests.csproj
- src\Lelleplanner.Tests\GameClockTests.cs
- Lelleplanner.sln
- PLAN.md
- CONTEXT.md

Next steps (Session 8): tests for `GameEngine.CompleteDailyQuest` (full-clear
detection + double-award guard).

## Session 8 — 2026-07-05
Summary: Tests for `GameEngine.CompleteDailyQuest`, plus a design cleanup surfaced
along the way — `GameEngine` held no instance state, so it was converted to a
static class to match `GameClock`'s pattern.

Actions performed:
- Converted `GameEngine` from an instance class to `static`, along with all four
  of its methods (`CompleteQuest`, `CompleteDailyQuest`, `HasRemainingQuests`,
  `ValidQuestNumber`)
- Updated `Program.cs`: removed `var gameEngine = new GameEngine();` and switched
  every call site to the static form (`GameEngine.MethodName(...)`)
- Wrote `GameEngineTests.cs` with three cases for `CompleteDailyQuest`:
  - `GiveDailyCoin` — completing all 6 non-meta quests awards exactly 1 Daily
    Coin and flips the `daily-quest-clear` meta-quest to `Completed`
  - `NoDoubleReward` — calling `CompleteDailyQuest` twice in a row after a full
    clear leaves `DailyCoins` at 1, not 2 (the double-award guard)
  - `NotDoneYet` — leaving one non-meta quest incomplete keeps `DailyCoins` at 0
    and the meta-quest `Completed` at `false`
- Confirmed `dotnet build` (full solution) and `dotnet test` both pass clean —
  8 tests total (5 `GameClock` + 3 `GameEngine`)
- Updated PLAN.md: checked off the `dotnet test` Definition-of-Done item for
  Iteration 1 and moved "what's next" to session 3

Files created/modified:
- src\Lelleplanner.Core\GameEngine.cs
- src\Lelleplanner.ConsoleApp\Program.cs
- src\Lelleplanner.Tests\GameEngineTests.cs
- PLAN.md
- SESSIONSUMMARIES.md

Next steps (Session 3 of Iteration 1): add the Weekly quests (`Shiny Sparkly!`,
`Tidy Room, Tidy Mind`) and the `Week Survived` meta-quest, `WeeklyCoins` on
`GameState`, and week-boundary rollover logic.

## Session 9 — 2026-07-05
Summary: Iteration 1 session 3 — Weekly quests, `WeeklyCoins`, and week rollover,
built as a deliberate mirror of the existing daily machinery rather than sharing
code yet (the duplication is intentional; extracting it is session 4).

Actions performed:
- Added `GameClock.GetGameWeekStart`, derived from `GetGameDate` (not raw
  `DateTime`) so it inherits cutover-hour handling for free; added three
  `[Fact]` tests in `GameClockTests.cs` covering before-cutover (previous
  week), at-cutover-on-a-Monday (new week), and a mid-week (Wednesday) case
  that exercises walking back to the correct Monday
- Added to `GameState`: `WeekStartDate`, `WeeklyCoins`, `WeeklyQuests`,
  `InitializeWeeklyQuests()` (`shiny-sparkly`, `tidy-room-tidy-mind`, and a
  `weekly-quest-clear` meta-quest), and `WeeklyRolloverIfNeeded()` — a parallel
  structure to the existing daily members, not a shared abstraction
- Added `GameEngine.CompleteWeeklyQuest`, a direct structural copy of
  `CompleteDailyQuest` operating on `WeeklyQuests`/`WeeklyCoins`
- Wired `WeeklyRolloverIfNeeded()` into both `Persistence.LoadOrCreate` call
  sites, alongside the existing `RolloverIfNeeded()` calls
- Caught and fixed a copy-paste bug: the weekly meta-quest's title initially
  read "Excellent work, daily cleared!" (copied from the daily meta-quest)
  instead of a weekly-specific message
- Confirmed `dotnet build` (0 warnings/errors) and `dotnet test` (11 passed)
  after each round of changes

Files created/modified:
- src\Lelleplanner.Core\GameClock.cs
- src\Lelleplanner.Core\GameState.cs
- src\Lelleplanner.Core\GameEngine.cs
- src\Lelleplanner.Core\Persistence.cs
- src\Lelleplanner.Tests\GameClockTests.cs
- PLAN.md
- CONTEXT.md

Not yet done (by design — later sessions): weekly quests aren't wired into
`ConsoleRenderer`/`Program.cs` (session 5), and no automated tests exist yet
for `GameEngine.CompleteWeeklyQuest` or `GameState.WeeklyRolloverIfNeeded`.

Next steps (Session 4 of Iteration 1): extract the shared daily/weekly
rollover abstraction now that both copies exist side by side.

## Session 10 — 2026-07-06
Summary: Iteration 1 session 4 — extracted the shared daily/weekly rollover
abstraction, closing the last open Definition-of-Done item for this
iteration's testing scope.

Actions performed:
- `GameState`: replaced the duplicated `RolloverIfNeeded`/`WeeklyRolloverIfNeeded`
  bodies with a shared private `ResetQuestsIfNeeded(storedDate, currentDate, quests)`
  that returns the resolved date rather than mutating a parameter (since `DateOnly`
  is a struct, mutating a by-value parameter wouldn't propagate back to the
  caller — this was a real bug in an early draft, caught before landing); both
  rollover methods now assign its return value back to their own property.
  Renamed `RolloverIfNeeded` to `DailyRolloverIfNeeded` for symmetry with
  `WeeklyRolloverIfNeeded`, and marked the helper `private` (internal detail,
  not part of the public API)
- `GameEngine`: replaced the duplicated `CompleteDailyQuest`/`CompleteWeeklyQuest`
  bodies with a shared private `CompleteMetaQuest(quests, metaQuestKey, Action onCleared)`
  — since the two methods differ not just in which list/key they use but in
  *what happens on clear* (incrementing `DailyCoins` vs `WeeklyCoins`), the
  varying behavior is passed in as an `Action` lambda (e.g.
  `() => gameState.DailyCoins++`) rather than as data
- Renamed `GameState.Quests` to `DailyQuests` for clarity now that
  `WeeklyQuests` exists alongside it; updated all call sites (`GameEngine`,
  `Program.cs`, `GameEngineTests`)
- Confirmed `dotnet build` (0 warnings/errors) and `dotnet test` (11 passed)
  after each round of changes
- Checked off PLAN.md's "duplication resolved via a deliberate shared
  abstraction" Definition-of-Done item for Iteration 1

Files created/modified:
- src\Lelleplanner.Core\GameState.cs
- src\Lelleplanner.Core\GameEngine.cs
- src\Lelleplanner.Core\Persistence.cs
- src\Lelleplanner.ConsoleApp\Program.cs
- src\Lelleplanner.Tests\GameEngineTests.cs
- PLAN.md
- CONTEXT.md

Next steps (Session 5 of Iteration 1): wire weekly quests into
`ConsoleRenderer`/`Program.cs`.

## Session 11 — 2026-07-06
Summary: Iteration 1 session 5 — wired weekly quests into the console UI
alongside daily quests, and caught two real bugs via manual smoke-testing
(with save-file backup/restore around each test run, to avoid corrupting
real progress).

Actions performed:
- `Program.cs`: built `activeDailyQuestList`/`activeWeeklyQuestList` (and
  their completed counterparts) each loop iteration, then combined the two
  active lists (and the two completed lists) into single `[Daily]`/`[Weekly]`-
  tagged lists (`List<(Quest Quest, string Label)>`) via `Select` + `Concat` —
  one continuously-numbered list drives both the on-screen rendering and the
  numeric quest-selection lookup, so display order and lookup order can't
  drift apart
- `ConsoleRenderer.RenderQuestList`: updated to accept the tagged tuple lists
  and print each quest's label next to its title
- `ConsoleRenderer.RenderBanner`: added weekly coin/progress parameters and a
  second output line alongside the existing daily one
- Added `AsciiArt.WeeklyCelebration` (a trophy, mirroring the daily bell art)
  and `ConsoleRenderer.RenderWeeklyClearCelebration`, wired into `Program.cs`
  alongside the existing daily clear-celebration call
- Caught and fixed a bug where the retry-loop input-validation check used
  `activeDailyQuestList.Count()` instead of the combined `activeQuestList
  .Count()`, which would have rejected valid quest numbers pointing at
  weekly quests
- Caught and fixed a bug where the daily/weekly clear-detection checks in the
  main loop read `activeDailyQuestList`/`activeWeeklyQuestList` from *before*
  the current action's `GameEngine.CompleteQuest` call, instead of
  recomputing them afterward — meaning the "did this just clear everything"
  check was always one action behind
- Caught and fixed a deeper bug in `GameEngine.CompleteQuest`: it only ever
  searched `gameState.DailyQuests` for the given key, so completing any
  weekly quest silently no-op'd. Fixed with a null-coalescing fallback:
  `gameState.DailyQuests.FirstOrDefault(...) ?? gameState.WeeklyQuests
  .FirstOrDefault(...)`
- Manually smoke-tested the full weekly-clear path end-to-end (with the real
  save file backed up and restored around each run): completing both weekly
  quests correctly triggered `Week Survived`, awarded exactly 1 Weekly Coin,
  and showed the weekly celebration only once, at the right moment
- Confirmed `dotnet build` (0 warnings/errors) and `dotnet test` (11 passed)
  throughout
- Checked off PLAN.md's three remaining Weekly-Quests Definition-of-Done
  items (list renders alongside daily, `Week Survived` awards exactly 1
  Weekly Coin, balance persists)

Note for next real play session: the real save file still has one
pre-existing inconsistency predating this session's fixes (all 6 daily
quests marked complete, but `daily-quest-clear` never flagged, from before
the recompute-timing fix existed) — the next quest completed in real
gameplay will trigger one unexpected "ALL DAILY QUESTS CLEARED!" catch-up and
Daily Coin. Expected and self-healing, not a new bug.

Files created/modified:
- src\Lelleplanner.ConsoleApp\Program.cs
- src\Lelleplanner.ConsoleApp\ConsoleRenderer.cs
- src\Lelleplanner.ConsoleApp\AsciiArt.cs
- src\Lelleplanner.Core\GameEngine.cs
- PLAN.md
- CONTEXT.md

Next steps (Session 6 of Iteration 1): manually test both rollovers (day and
week boundary) in the console app, walk the Definition of Done checklist,
tag `v0.2`.

## Session 12 — 2026-07-06
Summary: Iteration 1 session 6 — manually verified both rollover directions
against the real console app, closing out the last open Definition of Done
item for Iteration 1. No code changes; this was a verification-only session.

Actions performed:
- Backed up the real save file (`%LOCALAPPDATA%\Lelleplanner\gamestate.json`)
- Wrote a "stale week, current day" fixture (WeekStartDate a week in the
  past, GameDate current, all weekly quests marked complete, daily quests
  mid-progress) and ran the app: confirmed `WeekStartDate` advanced to the
  current game-week, all weekly quests reset to incomplete, `WeeklyCoins`
  untouched — and daily quests/coins were completely unaffected
- Wrote a "stale day, current week" fixture (GameDate a day in the past,
  WeekStartDate current, all daily quests + the meta-quest marked complete,
  weekly quests mid-progress) and ran the app: confirmed `GameDate` advanced
  to today, all daily quests (including the meta-quest) reset to incomplete,
  `DailyCoins` untouched — and weekly quests/coins were completely unaffected
- Restored the real save file after each test run
- Re-ran `dotnet test` (11 passed) as a final sanity check
- Checked off the last remaining Iteration 1 Definition of Done item in
  PLAN.md ("on a new game-week, weekly quests auto-reset; daily quests and
  coins are unaffected... and vice versa") — every item for this iteration
  is now checked
- Updated PLAN.md's status line to "complete, ready to tag v0.2" and
  CONTEXT.md's "where things stand"/"next step" to match

Files created/modified:
- PLAN.md
- CONTEXT.md

Next steps: tag `v0.2` (user to do via git), then start Iteration 2
(Monthly quests + Achievements) whenever ready.

## Session 13 — 2026-07-07
Summary: Kicked off Iteration 2 (Monthly Quests + Achievements). Design-only
session — no code changes — to resolve two open questions before scaffolding
anything.

Actions performed:
- Decided monthly quest cadence: monthly quest progress counters reset every
  game-month (mirroring the existing daily/weekly rollover pattern via a new
  `GameClock.GetGameMonthStart`), while achievements track a separate
  *lifetime* counter of monthly-quest clears and complete exactly once, never
  resetting — matching VISION.md's "these are only completed once"
- Decided event design: introduce a real C# `QuestCompleted` domain event
  (raised from `GameEngine` whenever any quest completes) rather than
  extending the existing `Action`-callback pattern from `CompleteMetaQuest`.
  Monthly-progress tracking and achievement-checking will each be independent
  subscribers, so a daily quest clearing can cascade into a monthly quest
  clearing can cascade into an achievement completing, without `GameEngine`
  knowing about achievements at all
- Wrote the full Iteration 2 section into PLAN.md: scope, session breakdown,
  and Definition of Done, mirroring Iteration 1's structure
- Updated PLAN.md's status line and CONTEXT.md's "where things stand"/"next
  step" to reflect Iteration 2 starting

Files created/modified:
- PLAN.md
- CONTEXT.md
- SESSIONSUMMARIES.md

Next steps (Session 1 of Iteration 2): `GameClock.GetGameMonthStart` +
boundary tests (including the year-rollover case, Dec→Jan), mirroring the
existing `GetGameWeekStart` tests.

## Session 14 — 2026-07-07
Summary: Iteration 2 session 1 — `GameClock.GetGameMonthStart`, mirroring
`GetGameWeekStart`.

Actions performed:
- Added `GameClock.GetGameMonthStart(DateTime now)` + parameterless overload:
  resolves the game-date via the existing `GetGameDate` first (inheriting
  cutover-hour handling for free), then builds the month start directly from
  its `Year`/`Month` — no manual date arithmetic, avoiding the "day - 1"
  class of bug that hit `GetGameDate` in session 5
- Caught mid-session: an early draft made `GetGameWeekStart(DateTime now)`
  private to shrink public surface area, which broke the build —
  `GameClockTests.cs` calls that overload directly across the
  `Lelleplanner.Tests`/`Lelleplanner.Core` assembly boundary, since the
  `DateTime now` parameter exists specifically as a testability seam (inject
  a fixed instant instead of mocking `DateTime.Now`), not just as a
  convenience overload. Reverted to public, and kept `GetGameMonthStart`'s
  overload public for the same reason
- Added four tests to `GameClockTests.cs`: start-of-month at/before cutover,
  mid-month, and the year-boundary case (Jan 1st before cutover resolves to
  December of the *previous* year)
- Confirmed `dotnet build` (0 warnings/errors) and `dotnet test` (15 passed)

Files created/modified:
- src\Lelleplanner.Core\GameClock.cs
- src\Lelleplanner.Tests\GameClockTests.cs

Next steps (Session 2 of Iteration 2): wire `MonthStartDate` and
`MonthlyQuests` (with progress counters, not plain booleans) into
`GameState`, plus `MonthlyRolloverIfNeeded()`.

## Session 15 — 2026-07-07
Summary: Iteration 2 session 2 — wired monthly quests (as progress counters,
not plain booleans) into `GameState`.

Actions performed:
- Added `MonthlyQuest.cs`: a type parallel to `Quest`, not sharing it —
  monthly quests are driven by a `Progress`/`Threshold` count rather than a
  single `Completed` toggle, a genuinely different shape rather than
  duplication of the same shape
- Added `GameState.MonthStartDate` (set from `GameClock.GetGameMonthStart()`
  in the constructor) and `MonthlyQuests` (via a new
  `InitializeMonthlyQuests()`): `habit-handled` (threshold 25),
  `plates-for-days` (threshold 4), `tidy-home-tidy-life` (threshold 4). No
  monthly meta-quest and no Monthly Coin — VISION.md only defines two coin
  currencies; monthly quests feed achievements instead (session 4)
- Added `GameState.MonthlyRolloverIfNeeded()`, backed by a new private
  `ResetProgressIfNeeded` — deliberately separate from the existing
  `ResetQuestsIfNeeded` (used by daily/weekly), since a new game-month needs
  to zero out `Progress` as well as `Completed`, not just flip a bool
- Wired `MonthlyRolloverIfNeeded()` into both `Persistence.LoadOrCreate` call
  sites, alongside the existing daily/weekly rollover calls
- Caught and fixed a real bug: `InitializeMonthlyQuests()` initially
  constructed all three `MonthlyQuest`s without passing `threshold` (relying
  on its default of `0`), since none of the three positional-looking calls
  filled in the 4th/5th constructor parameters — fixed with named arguments
  (`threshold: 25`, etc.)
- Caught and fixed two smaller inconsistencies: `MonthlyQuest`'s constructor
  threw `NullReferenceException` instead of `ArgumentNullException` for null
  args (mismatched `Quest`'s convention), and had unused template `using`s
- Confirmed `dotnet build` (0 warnings/errors) and `dotnet test` (15 passed)

Files created/modified:
- src\Lelleplanner.Core\MonthlyQuest.cs
- src\Lelleplanner.Core\GameState.cs
- src\Lelleplanner.Core\Persistence.cs

Next steps (Session 3 of Iteration 2): define and raise a `QuestCompleted`
domain event from `GameEngine`; give it a first subscriber that increments
monthly-quest progress.

## Session 16 — 2026-07-07
Summary: Iteration 2 session 3 — the `QuestCompleted` domain event, and its
first subscriber, `MonthlyProgressTracker`.

Actions performed:
- Added `QuestCompleted.cs`: a small `record QuestCompleted(string
  QuestKey)`
- Added `GameEngine.OnQuestCompleted`, a static `event Action<QuestCompleted>?`,
  raised from both `CompleteQuest` and `CompleteMetaQuest` right after a
  quest's `Completed` flips to `true`
- Caught and fixed a latent double-fire risk in `CompleteQuest`: it had no
  guard against being called on an already-completed quest (harmless before,
  since its only real side effect was setting a bool that was already
  `true`) — now that completion drives an event with real downstream
  effects, folded the guard into the lookup itself
  (`FirstOrDefault(q => q.Key == questKey && !q.Completed)`) so an
  already-completed quest is never found, matching `CompleteMetaQuest`'s
  existing guard
- Added `MonthlyProgressTracker.cs`: a static class deliberately outside
  `GameEngine`, so `GameEngine` never needs to know monthly quests exist.
  `HandleQuestCompleted(QuestCompleted, GameState)` maps a source quest key
  (`daily-quest-clear`, `shiny-sparkly`, `tidy-room-tidy-mind`) to its
  monthly quest via a `Dictionary<string, string>` + `TryGetValue`,
  increments `Progress`, and completes the monthly quest at `Threshold` —
  guarded (`Progress < Threshold`) against incrementing past it on repeated
  triggers
- Wired the subscription once, at the composition root in `Program.cs`,
  right after `Persistence.LoadOrCreate()`:
  `GameEngine.OnQuestCompleted += evt => MonthlyProgressTracker.HandleQuestCompleted(evt, gameState);`
- Added `MonthlyProgressTrackerTests.cs` with four cases: progress
  increments on a matching key, threshold reached completes the quest, no
  double-counting past threshold on repeated triggers, and an unmapped key
  is a no-op (asserted by calling the method directly with no exception
  wrapping needed — xUnit fails a test automatically on any unhandled
  exception, no `Assert.DoesNotThrow` equivalent needed)
- Trimmed unused template `using`s from `GameEngineTests.cs` along the way
- Confirmed `dotnet build` (0 warnings/errors) and `dotnet test` (19 passed)

Files created/modified:
- src\Lelleplanner.Core\QuestCompleted.cs
- src\Lelleplanner.Core\GameEngine.cs
- src\Lelleplanner.Core\MonthlyProgressTracker.cs
- src\Lelleplanner.ConsoleApp\Program.cs
- src\Lelleplanner.Tests\MonthlyProgressTrackerTests.cs
- src\Lelleplanner.Tests\GameEngineTests.cs

Next steps (Session 4 of Iteration 2): achievements + `MarkovFragments` —
subscribe a second, independent handler to `QuestCompleted` that reacts to
monthly-quest completions, tracks lifetime counts, completes achievements,
and awards fragments.

## Session 17 — 2026-07-20
Summary: Iteration 2 session 4 — Achievements + `MarkovFragments`, subscribing
to the same `QuestCompleted` event as `MonthlyProgressTracker`.

Actions performed:
- Resolved a design gap surfaced before writing any code: C# events can only
  be invoked from their declaring class, so `MonthlyProgressTracker` (outside
  `GameEngine`) had no way to raise `OnQuestCompleted` when a monthly quest
  hit its threshold — meaning monthly-quest completions were silent and
  nothing could react to them, despite PLAN.md describing `QuestCompleted` as
  covering daily, weekly, *and* monthly clears. Considered three options
  (a `GameEngine.RaiseQuestCompleted` passthrough method; extracting a
  dedicated event-publisher class; skipping the event and chaining directly
  in `Program.cs`) and went with the passthrough method — smallest change,
  keeps one event pipeline, and matches the project's stated preference for
  deferring bigger abstractions (like a real event bus) to Iteration 5's
  DDD pass, once a second real motivating case exists
- Added `GameEngine.RaiseQuestCompleted(string questKey)`, a public static
  method that just invokes `OnQuestCompleted`; `MonthlyProgressTracker` calls
  it right after completing a monthly quest
- Added `Achievement.cs`: `Key`, `Title`, `Goal`, `LifetimeCount`,
  `Threshold` (4, per VISION.md, kept as a field rather than hardcoded for
  consistency with `MonthlyQuest`'s shape), `Completed` — mirrors
  `MonthlyQuest` but lifetime-only, never reset by rollover
- Added `AchievementTracker.cs`: a second independent static subscriber to
  `QuestCompleted`, structurally parallel to `MonthlyProgressTracker` — maps
  a monthly-quest key to its achievement key via a `Dictionary`, increments
  `LifetimeCount`, completes the achievement and awards exactly 1
  `MarkovFragment` at threshold, guarded against double-completion/-award
- Added `GameState.Achievements` (via `InitializeAchievements()`) and
  `GameState.MarkovFragments`; deliberately left both out of every rollover
  method, since achievements and their fragment reward are lifetime, not
  per-month
- Wired the second subscription in `Program.cs`, alongside the existing one:
  `GameEngine.OnQuestCompleted += evt => AchievementTracker.HandleQuestCompleted(evt, gameState);`
- Traced the full cascade to confirm it terminates correctly: a daily quest
  clearing fires `QuestCompleted`, which `MonthlyProgressTracker` handles and
  (at threshold) re-raises `QuestCompleted` for the monthly quest — a
  synchronous, nested (not concurrent) re-entry into the same event, which
  `AchievementTracker` then handles in turn. Monthly-quest keys never appear
  as source keys in `MonthlyQuestSources`, so the nested call can't cascade
  further and the recursion terminates by construction
- Added `AchievementTrackerTests.cs` with five cases, mirroring
  `MonthlyProgressTrackerTests.cs`'s four (lifetime count increments on a
  matching key, threshold reached completes the achievement, no
  double-completion past threshold, an unmapped key is a no-op) plus one new
  case specific to `AchievementTracker`: awards exactly 1 `MarkovFragment`,
  not one per triggering call
- Confirmed `dotnet build` (0 warnings/errors) and `dotnet test` (24 passed)

Files created/modified:
- src\Lelleplanner.Core\GameEngine.cs
- src\Lelleplanner.Core\MonthlyProgressTracker.cs
- src\Lelleplanner.Core\Achievement.cs
- src\Lelleplanner.Core\AchievementTracker.cs
- src\Lelleplanner.Core\GameState.cs
- src\Lelleplanner.ConsoleApp\Program.cs
- src\Lelleplanner.Tests\AchievementTrackerTests.cs
- PLAN.md
- CONTEXT.md

Next steps (Session 5 of Iteration 2): wire monthly quests + achievements
into `ConsoleRenderer`/`Program.cs`'s display.

## Session 18 — 2026-07-21
Summary: Iteration 2 session 5 — wired monthly quests + achievements into the
console UI as read-only status displays, closing the last code-facing
Definition of Done item for this iteration.

Actions performed:
- `ConsoleRenderer.RenderBanner`: added a `markovFragments` parameter and a
  third banner line, mirroring the existing Daily/Weekly Coins lines
- Added `ConsoleRenderer.RenderMonthlyList(List<MonthlyQuest>)` and
  `RenderAchievements(List<Achievement>)`: since monthly quests and
  achievements complete automatically as a side effect of
  `MonthlyProgressTracker`/`AchievementTracker` reacting to `QuestCompleted`
  (not by the player picking a number), they're deliberately kept out of the
  existing numbered active/completed quest list and rendered as their own
  read-only `[progress/threshold]`/`[COMPLETED]` sections
- Wired both new render calls into `Program.cs`'s main loop, alongside the
  existing `RenderQuestList` call, and passed `gameState.MarkovFragments`
  into `RenderBanner`
- Caught and fixed a real bug during review: the Markov Fragments banner
  line initially read `{markovFragments}/4`, a cap that didn't match any
  defined vision mechanic — 3 achievements each award exactly 1 fragment, so
  the true lifetime max is 3, not 4 (the `4` was the achievement threshold —
  monthly-quest clears needed *per achievement* — bleeding into the wrong
  number). Confirmed no Shop item exists that sells fragments before fixing
  the display to `/3`
- Confirmed `dotnet build` (0 warnings/errors) and `dotnet test` (24 passed)
- Manually smoke-tested via `dotnet run`: confirmed the Monthly Quests and
  Achievements sections render correctly against real save-file state, and
  the Markov Fragments line shows `/3`
- Checked off PLAN.md's "Monthly quest list and achievement status render in
  the console UI" Definition of Done item

Files created/modified:
- src\Lelleplanner.ConsoleApp\ConsoleRenderer.cs
- src\Lelleplanner.ConsoleApp\Program.cs
- PLAN.md
- CONTEXT.md

Next steps (Session 6 of Iteration 2, Friday session): manually verify the
month-boundary rollover against the real console app (mirroring session 12's
daily/weekly verification), walk the full Definition of Done checklist, tag
`v0.3`.

## Session 19 — 2026-07-21
Summary: Iteration 2 session 6 — manually verified the month-boundary
rollover against the real console app, closing out the last open Definition
of Done item for Iteration 2. No code changes; verification-only session,
plus a real bug scare that turned out to be a testing artifact rather than
an actual defect.

Actions performed:
- Backed up the real save file (`%LOCALAPPDATA%\Lelleplanner\gamestate.json`)
- Investigated a discrepancy the user noticed while doing their own manual
  rollover testing: staling only `MonthStartDate` appeared to reset nothing,
  while staling `GameDate`, `WeekStartDate`, *and* `MonthStartDate` together
  appeared to reset everything
- Wrote a controlled "only `MonthStartDate` stale" fixture with nonzero
  monthly quest progress (5/25, 2/4, 3/4) and current `GameDate`/
  `WeekStartDate`, then ran the app: confirmed `MonthStartDate` advanced to
  the current game-month and all three monthly quest progress counters
  correctly reset to 0 — daily quests, weekly quests, and their coins were
  completely unaffected
- Root-caused the user's original observation: the save file on disk before
  this session already had all three `MonthlyQuests` sitting at 0 progress,
  so a 0→0 reset was indistinguishable from no reset happening at all: not a
  bug, just an artifact of testing with a fixture that had nothing to reset
- Restored the real save file exactly as it was found (untouched beyond this
  session's temporary fixture)
- Confirmed via user's own `dotnet test` run: 24 passed on a clean build
- Checked off both remaining Iteration 2 Definition of Done items in
  PLAN.md (month-rollover correctness, `dotnet test` passing) — every item
  for this iteration is now checked
- Updated PLAN.md's status line and Iteration 2 section header to "complete,
  ready to tag v0.3", and CONTEXT.md's "where things stand"/"next step" to
  match, pointing at tagging `v0.3` and then a design session for Iteration 3
  (Currency + Shop)

Files created/modified:
- PLAN.md
- CONTEXT.md

Next steps: tag `v0.3` (user to do via git), then start Iteration 3
(Currency + Shop) with a design-only session whenever ready.

## Session 20 — 2026-07-22
Summary: Closed out Iteration 2's docs (which had gone stale relative to the
actual merged state), added its LEARNINGS.md section, and kicked off
Iteration 3 (Currency + Shop) as a design-only session.

Actions performed:
- Confirmed `v0.3` is tagged and merged into `master` (PR #19, merging
  `sessions/session18/iteration-2-check-dod`) — session had opened questioning
  this since an earlier branch's copy of the docs still described Iteration 2
  as in-progress
- Fixed stale wording in PLAN.md ("ready to tag v0.3" -> "tagged v0.3") and
  CONTEXT.md (stale "as of" date, stale "next step" pointing at tagging
  instead of Iteration 3, stale "master up to date through v0.1" -> v0.3)
- Added the Iteration 2 section to LEARNINGS.md: domain events for decoupling
  publisher from subscriber, the "events can only be raised from their
  declaring class" gotcha behind `GameEngine.RaiseQuestCompleted`, progress
  counters vs. toggles as a genuinely different shape (not shared code),
  lifetime state deliberately excluded from every rollover, and
  guard-by-construction vs. guard-by-check
- Resolved three open design questions for Iteration 3 before writing its
  PLAN.md section: (1) `Missing Eddie Card`/`Daddy Markov [Serialized]` are
  deferred to Iteration 4, since both depend on the not-yet-built Deckbox and
  would otherwise ship half-working; (2) Shop will call
  `GameEngine.CompleteQuest` directly rather than through a new interface,
  since `GameEngine` is Core's one real implementation today; (3) a real gap
  found while tracing `Shortcut!`'s flow — full-clear detection currently
  lives only in `Program.cs`'s main loop (`Program.cs:83-87`), so a Shop
  purchase completing the last two daily quests wouldn't trigger
  `daily-quest-clear`/the Daily Coin — resolved by folding that detection
  into `GameEngine.CompleteQuest` itself
- Wrote the full Iteration 3 section into PLAN.md (scope, "where it'll get
  interesting" — retiring `Program.cs`'s bool-tracking celebration check in
  favor of subscribing to `QuestCompleted`, session breakdown, Definition of
  Done) and updated the roadmap's item 3/4 one-liners to match the deferred-
  items decision
- Updated CONTEXT.md's "where things stand" and "next step" to point at
  Iteration 3 session 2

Files created/modified:
- CONTEXT.md
- PLAN.md
- LEARNINGS.md
- SESSIONSUMMARIES.md

Next steps (Session 2 of Iteration 3): `ShopItem` type + `GameState.ShopItems`,
seeded with `Shortcut!` and `Cheat Day!` (stock counts included).

(Will append a short summary at the end of each completed session.)