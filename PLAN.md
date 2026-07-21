# Lelleplanner — Plan

Living document. Update this at the start/end of each iteration — it should
always reflect "what are we building right now" and "what's next," not the
full end-state (that's [VISION.md](VISION.md)).

**Working cadence:** ~30 min/day, ~60 min on Fridays.
**Iteration budget:** MVP ≤ 4 hours. Each iteration after that: 4–16 hours.

<hr>

## Status: Iteration 2 (v0.3) — Monthly Quests + Achievements — complete, ready to tag v0.3

**Progress:** v0.1 and v0.2 both shipped and tagged. Iteration 2 kicked off session 13
as design-only (see [Iteration 2](#iteration-2-v03-monthly-quests--achievements) below).
Session 1 added `GameClock.GetGameMonthStart` with cutover-hour and year-boundary tests.
Session 2 wired `MonthStartDate`/`MonthlyQuests` (progress-counter based, via a new
`MonthlyQuest` type) and `MonthlyRolloverIfNeeded()` into `GameState`, plus a new
`Persistence` call site. Session 3 added the `QuestCompleted` domain event (raised from
both `GameEngine.CompleteQuest` and `CompleteMetaQuest`, guarded against re-firing on an
already-completed quest) and its first subscriber, `MonthlyProgressTracker`, which
increments monthly-quest progress and completes it at threshold — subscribed once at the
composition root in `Program.cs`, with `GameEngine` staying unaware monthly quests exist.
Session 4 added the `Achievement` type, `GameEngine.RaiseQuestCompleted` (so non-`GameEngine`
publishers can raise `QuestCompleted` too), `AchievementTracker` (a second independent
subscriber, tracking lifetime counts and awarding `MarkovFragments`), and its test coverage.
Session 5 wired monthly quests + achievements into the console UI as read-only status
displays (`ConsoleRenderer.RenderMonthlyList`/`RenderAchievements`), plus a `MarkovFragments`
line on the banner. Session 6 manually verified the month-boundary rollover with a
controlled fixture (nonzero monthly progress, stale `MonthStartDate` only): progress
correctly reset to 0 and `MonthStartDate` advanced, with daily/weekly state untouched —
every Definition of Done item for this iteration is now checked off.

<hr>

## Iteration 1 (v0.2) recap — Testing + Weekly Quests — complete, tagged v0.2

MVP (v0.1) shipped and tagged after session 6. Session 7 scaffolded
`Lelleplanner.Tests` (xUnit) with `GameClock.GetGameDate` boundary tests. Session 8
added `GameEngine.CompleteDailyQuest` tests (full-clear + coin award, double-award
guard, partial-completion) and, as a design cleanup surfaced along the way, converted
`GameEngine` to a static class (it held no instance state) — call sites in
`Program.cs` updated to match. Session 9 (iteration session 3) added
`GameClock.GetGameWeekStart`, `GameState.WeeklyQuests`/`WeeklyCoins`/
`WeekStartDate`/`WeeklyRolloverIfNeeded`, and `GameEngine.CompleteWeeklyQuest` —
deliberately mirroring the daily equivalents rather than sharing code yet. Session 4
extracted the shared daily/weekly rollover abstraction: `GameState` gained a private
`ResetQuestsIfNeeded` (returns the resolved date rather than mutating a parameter,
since `DateOnly` is a struct) backing `DailyRolloverIfNeeded`/`WeeklyRolloverIfNeeded`,
and `GameEngine` gained a private `CompleteMetaQuest` (takes an `Action onCleared`)
backing `CompleteDailyQuest`/`CompleteWeeklyQuest`. Session 5 wired weekly quests into
`ConsoleRenderer`/`Program.cs`: a single combined, `[Daily]`/`[Weekly]`-tagged quest
list (continuous numbering across both) replaces the daily-only list, `RenderBanner`
shows both coin/progress lines, and a `WeeklyCelebration` art asset backs
`RenderWeeklyClearCelebration`. Along the way, fixed two real bugs surfaced by manual
smoke-testing: the daily/weekly clear checks in `Program.cs` were reading
pre-action quest lists instead of post-action ones, and `GameEngine.CompleteQuest`
only ever searched `DailyQuests`, silently no-op'ing on any weekly quest key (fixed
with `?? gameState.WeeklyQuests.FirstOrDefault(...)`). Session 6 manually verified
both rollover directions against the real console app (stale-week fixture: weekly
quests reset, daily untouched; stale-day fixture: daily quests reset, weekly
untouched) with the real save file backed up and restored around each run — every
Definition of Done item for this iteration is now checked off. Tagged `v0.2`.

<hr>

## MVP (v0.1) scope

### In scope
- The 6 daily quests from the vision doc, hardcoded (no add/edit/remove UI yet)
- The auto-completing meta-quest, "Excellent work, daily cleared!", which
  triggers when all 6 are done
- Daily Coins: +1 awarded on daily clear, balance persists (nothing to spend
  it on yet — that's the Shop, later)
- A day-rollover rule: quests reset to unchecked once per game-day; coin
  balance is untouched
- Console UI with an ASCII title banner and a small celebration when the day
  is fully cleared
- Persistence to a local JSON file

### Explicitly out of scope (deferred to later iterations)
- Weekly and Monthly quests, Achievements
- Weekly Coins, Markov Fragments
- The Shop (all 4 items)
- The Deckbox (48-card collection)
- Automated tests (see [Open questions](#open-questions--parking-lot) — this
  is a deliberate choice, not an oversight)
- Anything GUI — console only for now

### Definition of done
- [x] `dotnet run` from a clean checkout shows an ASCII title banner and
      today's 6 quests with their checked/unchecked state
- [x] Entering a quest's number marks it complete (one-directional — quests
      only reset via the next game-day rollover, not by re-entering the
      number)
- [x] Completing all 6 triggers the clear celebration and awards exactly
      1 Daily Coin (not repeatable within the same game-day)
- [x] Daily Coin balance persists across runs
- [x] Quest state persists across runs within the same game-day
- [x] On a new game-day, quests auto-reset to unchecked; coins are untouched
- [x] State lives in the user's AppData folder as JSON, not inside the repo

<hr>

## Tech spec (MVP)

### Stack
- C#, .NET 8 (LTS)
- Console app, no external packages beyond the BCL (`System.Text.Json` for
  persistence)

### Why two projects from day one
Normally an MVP this small would live in one project. Here we're
deliberately splitting **logic** from **console I/O** immediately, because:
- it costs about 5 extra minutes now (one more `dotnet new`, one project
  reference)
- it means adding a test project later (iteration 2) is a pure addition —
  zero refactoring, since `Core` never touches `Console.ReadLine`/`WriteLine`
- it's the same seam that later becomes the port for swapping the console UI
  for a GUI (iteration 8) and JSON for a real database (iteration 7)

This is the one piece of structure we're paying for upfront. Everything else
(DDD tactical patterns, repository interfaces, value objects) is intentionally
**not** here yet — those get introduced deliberately in later iterations,
partly so there's something real to refactor rather than guessing at
abstractions from a blank file.

### Solution layout
```
lelleplanner/
├── VISION.md
├── PLAN.md
├── .gitignore
├── Lelleplanner.sln
└── src/
    ├── Lelleplanner.Core/
    │   ├── Quest.cs           # Key, Title, Goal, Completed
    │   ├── GameState.cs       # GameDate, DailyCoins, List<Quest>
    │   ├── GameClock.cs       # "what game-day is it right now"
    │   └── GameEngine.cs      # complete quest, detect full clear, rollover
    └── Lelleplanner.ConsoleApp/
        ├── Program.cs         # composition root + main loop
        ├── ConsoleRenderer.cs # draws banner, quest list, prompts
        └── AsciiArt.cs        # banner + celebration art
```

### Day rollover
Quests shouldn't reset at midnight if you're still awake grinding — so the
"game-day" rolls over at a configurable cutover hour, default **04:00**:

```csharp
// GameClock
DateOnly GetGameDate(DateTime now, int cutoverHour = 4) =>
    now.Hour < cutoverHour
        ? DateOnly.FromDateTime(now.Date.AddDays(-1))
        : DateOnly.FromDateTime(now.Date);
```

On startup: compute the current game-day, compare to `GameState.GameDate`.
If different, reset all `Quest.Completed` to `false`, update `GameDate`, and
save immediately — before rendering anything.

Cutover hour is confirmed at **04:00**, hardcoded as a constant for the MVP.
A settings menu to change this (and other config) at runtime is a nice
later-iteration idea — see the roadmap.

### Persistence
Plain JSON, stored outside the repo at
`%LOCALAPPDATA%\Lelleplanner\gamestate.json`, so it survives `dotnet run` from
any working directory and never gets accidentally committed.

Quests are keyed by a stable string (`"food-for-thought"`, not array index),
so reordering the hardcoded quest list later can't silently corrupt saved
state:

```json
{
  "gameDate": "2026-07-03",
  "dailyCoins": 3,
  "quests": [
    { "key": "food-for-thought", "completed": false },
    { "key": "shiny-pearly-whites", "completed": true }
  ]
}
```

<hr>

## Suggested session breakdown (fits the 30/60-min cadence)
Not mandatory — just a way to see the 4-hour budget mapped onto real days.

| Session | Time | Focus |
|---|---|---|
| 1 | 30 min | `dotnet new sln` + both projects, project reference, git init, `.gitignore`, confirm it builds and runs |
| 2 | 45-60 min | `Quest`, `GameState`, `GameClock` + rollover logic + JSON load/save (persistence) |
| 3 | 30 min | `GameEngine` (complete quest, full-clear detection, coin award) |
| 4 | 30 min | `AsciiArt` + `ConsoleRenderer` |
| 5 (Fri) | 60 min | Wire up `Program.cs` main loop end-to-end; manually test a full clear and a rollover |
| 6 | 30 min | Bug fixes, walk the Definition of Done checklist, tag/commit `v0.1` |

Total: ~3.75-4 hours, at or just under the 4-hour cap.

<hr>

## Iteration 1 (v0.2): Testing + Weekly Quests

**Budget:** 4-8 hours.

### Scope
- New `Lelleplanner.Tests` project (xUnit), referencing `Core`
- Tests for `GameClock.GetGameDate` (cutover-hour boundary, including the
  month-rollover case that was buggy in session 5) and
  `GameEngine.CompleteDailyQuest` (full-clear detection + the double-award
  guard)
- Weekly quests from VISION.md: `Shiny Sparkly!` (dishes), `Tidy Room, Tidy
  Mind` (clean a room), and the `Week Survived` meta-quest
- Weekly Coins: +1 awarded on `Week Survived`
- A week-boundary rollover, parallel to the day rollover but on a weekly
  cadence

### Where it'll get interesting
Daily and weekly rollover will look suspiciously similar (`GameClock` needs
a "what game-week is it," `GameState` needs a second `RolloverIfNeeded`-
shaped method, `GameEngine` needs a second full-clear check). That
duplication is the intended motivation to extract a shared abstraction —
worth pausing on once the second copy exists, rather than guessing the
abstraction upfront.

### Suggested session breakdown
| Session | Focus |
|---|---|
| 1 | Scaffold `Lelleplanner.Tests`, wire xUnit + project reference, first `GameClock` tests |
| 2 | Tests for `GameEngine.CompleteDailyQuest` (full-clear + double-award guard) |
| 3 | Add Weekly quests + `Week Survived` + `WeeklyCoins` to `GameState`; week rollover logic |
| 4 | Extract the shared daily/weekly rollover abstraction (once duplication is visible) |
| 5 | Wire weekly quests into `ConsoleRenderer`/`Program.cs` |
| 6 (Fri) | Manual test both rollovers, new Definition of Done checklist, tag `v0.2` |

### Definition of done
- [x] `dotnet test` runs and passes, covering `GameClock` boundary cases and
      `GameEngine.CompleteDailyQuest`'s full-clear + double-award guard
- [x] Weekly quest list renders alongside daily quests, with its own
      active/completed display
- [x] Completing both weekly quests triggers `Week Survived` and awards
      exactly 1 Weekly Coin (not repeatable within the same game-week)
- [x] Weekly Coin balance persists across runs
- [x] On a new game-week, weekly quests auto-reset; daily quests and coins
      are unaffected by the weekly rollover (and vice versa)
- [x] Any duplication between daily/weekly rollover has been resolved via a
      deliberate shared abstraction, not left copy-pasted

<hr>

## Iteration 2 (v0.3): Monthly Quests + Achievements — complete, ready to tag v0.3

**Budget:** 4-8 hours.

### Scope
- `GameClock.GetGameMonthStart`, mirroring `GetGameWeekStart`/`GetGameDate` (respects
  the 04:00 cutover hour; game-month boundary, not calendar `DateTime.Month`)
- Monthly quests from VISION.md, as *progress counters* rather than single checkboxes:
  `Habit Handled!` (`daily-quest-clear` cleared 25x this game-month), `Plates For
  Days!` (`shiny-sparkly` cleared 4x), `Tidy Home, Tidy Life` (`tidy-room-tidy-mind`
  cleared 4x). No monthly meta-quest and no Monthly Coin — the vision doc only
  defines two coin currencies (Daily, Weekly); monthly quests feed achievements
  instead
- `GameState.MonthStartDate` + `MonthlyRolloverIfNeeded()`: each new game-month,
  monthly quest progress counters reset to 0 (mirrors the daily/weekly rollover
  pattern, doesn't share `ResetQuestsIfNeeded` directly since it's counting, not
  toggling `Completed`)
- A `QuestCompleted` domain event: a small event (e.g. a `QuestCompleted(string
  QuestKey)` record + a C# `event Action<QuestCompleted>?` on `GameEngine`), raised
  whenever any quest — daily, weekly, *or monthly* — completes. This decouples "a
  quest cleared" from "what reacts to it": monthly-progress tracking and
  achievement-checking both subscribe as independent handlers instead of
  `GameEngine` calling them directly
- Achievements from VISION.md — `Habits Maintained!`, `Mythical Kitchen`, `Always
  Ready` — each tracking a *lifetime* counter (never resets) of how many times its
  corresponding monthly quest has completed. Complete exactly once, ever, at 4
  clears; award exactly 1 Markov Fragment on completion
- `GameState.MarkovFragments` currency

### Where it'll get interesting
This is the first tier of *cascading* completions: a daily quest clearing can cause
`daily-quest-clear` to clear, which (via the same `QuestCompleted` event) can cause
`habit-handled` to clear, which can cause the `habits-maintained` achievement to
complete. Getting the event subscription wired so each tier reacts only to the
events relevant to it — without `GameEngine` needing to know about achievements at
all — is the actual design exercise here, not just plumbing three more quests
through the existing pattern.

### Suggested session breakdown
| Session | Focus |
|---|---|
| 1 | `GameClock.GetGameMonthStart` + boundary tests (incl. year rollover, Dec→Jan) |
| 2 | `GameState` monthly quests: `MonthStartDate`, `MonthlyQuests` w/ progress counters, `MonthlyRolloverIfNeeded()` |
| 3 | Define & raise the `QuestCompleted` event from `GameEngine`; a subscribed handler increments monthly progress and completes monthly quests |
| 4 | Achievements + `MarkovFragments`: a subscribed handler reacts to monthly-quest completions, tracks lifetime counts, completes achievements, awards fragments |
| 5 | Wire monthly quests + achievements into `ConsoleRenderer`/`Program.cs` |
| 6 (Fri) | Manual test full cascade + month rollover, Definition of Done walkthrough, tag `v0.3` |

### Definition of done
- [x] `GameClock.GetGameMonthStart` tested, including the year-boundary case
- [x] Monthly quest progress increments when its underlying daily/weekly quest
      clears, and each monthly quest completes exactly once per game-month (no
      double-counting a single clear, no re-completing after already complete this
      month)
- [x] `QuestCompleted` event exists; monthly-progress tracking and
      achievement-checking are both independent subscribers to it, not direct calls
      from `GameEngine`
- [x] Achievements track lifetime counts of monthly-quest clears, complete exactly
      once ever, and award exactly 1 Markov Fragment each
- [x] Monthly quest list and achievement status render in the console UI
- [x] On a new game-month, monthly quest progress resets to 0; daily quests, weekly
      quests, and achievements are unaffected (and vice versa — daily/weekly
      rollover doesn't touch monthly progress)
- [x] `dotnet test` passes, covering the new `GameClock`, monthly-progress, and
      achievement logic

<hr>

## Roadmap (post-MVP iterations)
Rough, one-liner per iteration — each gets its own detailed planning pass
when we actually get there, not now.

1. **Testing + Weekly quests** (4–8h) — add `Lelleplanner.Tests` (xUnit)
   referencing `Core`; test `GameClock` and full-clear detection; add Weekly
   quests, `Week Survived`, Weekly Coins, week-boundary rollover. Likely
   surfaces duplication between daily/weekly rollover — a natural, motivated
   moment to extract a shared abstraction (not a guessed one).
2. **Monthly quests + Achievements** (4–8h) — needs historical counters
   (e.g. "times daily fully cleared"), not just today's checklist. Good
   moment to introduce a lightweight domain event
   (`QuestCompleted`) so "what happened" and "what reacts to it" (currency,
   achievements) decouple.
3. **Currency + Shop** (4–8h) — all 4 shop items; stock counts; `Shortcut!`
   needs to reach back into the quest board to auto-complete two quests —
   a good excuse to keep Shop decoupled from the quest board via a small
   interface rather than a direct dependency.
4. **Deckbox** (4–8h) — 48-card pool, random draw excluding Edgar Markov,
   dedicated Daddy Markov path, a collection view. Draws are without
   replacement and no duplicates occur — 47 purchases of "Missing Eddie
   Card" gets every non-Markov card exactly once, matching the "47 in
   stock" figure.
5. **DDD tactical pass** (8–16h) — the deliberate "rehearse DDD" iteration:
   turn `GameState` into a real aggregate with invariants (can't double-award
   a daily coin, currency can't go negative), introduce value objects for
   currencies, introduce an `IGameStateRepository` with the JSON store as one
   implementation. Doing this as a refactor of working code, rather than
   DDD-from-a-blank-file, is the point.
6. **Persistence upgrade** (8–16h) — swap JSON for SQLite (EF Core or
   Dapper — worth trying one, then maybe comparing against the other later).
   This is where the repository abstraction from iteration 5 pays for itself.
7. **GUI** (8–16h) — swap the console UI for a real UI (WPF / MAUI /
   Avalonia — pick one when we get here) on top of an unchanged `Core`.
8. **Open-ended** — streak visuals, reminders/notifications, a settings menu
   (cutover hour and other config, changeable at runtime instead of a
   hardcoded constant), polish. Not committed to yet.

<hr>

## Open questions / parking lot
- **GUI framework** (iteration 7): WPF vs .NET MAUI vs Avalonia — decide
  closer to the time.
- **Confirmation prompt on quest completion** (console UI, iteration TBD):
  since completing a quest is one-directional until the next rollover, a
  lightweight y/n confirmation could guard against misclicks. Weigh against
  added friction for a daily-use tool.
