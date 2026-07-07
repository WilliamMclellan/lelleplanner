# Lelleplanner — Context for resuming elsewhere

**Read order when picking this back up:** this file → [VISION.md](VISION.md) → [PLAN.md](PLAN.md)

## What this is
A personal C# side project: a gamified daily-habit tracker (quests, coins,
an MTG-flavored card collection as a long-term reward). It's deliberately
built as a rehearsal ground for going from a scrappy MVP to a well-structured
app over time — later iterations exist specifically to practice
Domain-Driven Design and unit testing, not just to add features.

- Full motivation and complete feature vision: [VISION.md](VISION.md)
- Current build plan, MVP scope, and iteration roadmap: [PLAN.md](PLAN.md)

<hr>

## Where things stand (as of 2026-07-07)
- **MVP (v0.1) shipped and tagged.** Daily quests, Daily Coins, day rollover,
  console UI, and JSON persistence are all working end-to-end.
- **Iteration 1 (v0.2): Testing + Weekly Quests is complete and tagged.**
  Weekly quests, Weekly Coins, week rollover, and a shared daily/weekly
  rollover abstraction are all in, with test coverage and a manual,
  real-console verification of both rollover directions.
- **Iteration 2 (v0.3): Monthly Quests + Achievements has started.** Session
  13 was design-only (no code): monthly quest progress counters reset every
  game-month; achievements track a separate lifetime counter of monthly-quest
  clears and complete exactly once; a `QuestCompleted` domain event decouples
  quest completion from its reactions (monthly progress, achievements). Full
  scope and session breakdown are in PLAN.md's Iteration 2 section.
- Repo folder name: `lelleplanner` (on this machine:
  `C:\Users\William\repos\lelleplanner`)
- Git repository with a GitHub remote (`origin`); each session's work lands
  on its own branch and merges via PR into `master`.

<hr>

## Decisions already made (don't re-litigate these)
- MVP scope = Daily quests only: the 6 fixed quests + the auto-completing
  "daily cleared" meta-quest + Daily Coins. Weekly/Monthly/Achievements/
  Shop/Deckbox are deliberately deferred — see PLAN.md's roadmap.
- No automated tests in the MVP — deliberate choice. Tests arrive in
  iteration 1, alongside a dedicated test project.
- Two projects from the start, `Lelleplanner.Core` and
  `Lelleplanner.ConsoleApp`, so `Core` stays console-free and is testable
  later without a refactor.
- Stack: C#, .NET 8 (LTS), console app, `System.Text.Json`, no other
  dependencies for the MVP.
- Day-rollover ("game-day") cutover hour = **04:00**, hardcoded constant for
  now. A settings menu to make this configurable at runtime is parked in the
  open-ended later iteration.
- Deckbox draws (iteration 4, not MVP) are without replacement — no
  duplicate cards.
- GUI framework choice (iteration 7: WPF / MAUI / Avalonia) is intentionally
  still undecided.
- State persists as JSON at `%LOCALAPPDATA%\Lelleplanner\gamestate.json`, not
  inside the repo.

<hr>

## Working agreements
- Cadence: ~30 min/day, ~60 min on Fridays.
- Iteration budget: MVP ≤ 4 hours; each iteration after that, 4–16 hours.
- PLAN.md is a *living* document — update its status line and scope
  sections as work progresses, rather than spawning new planning files per
  iteration.
- This is a personal-motivation project (see VISION.md's Introduction) —
  keep collaboration practical and encouraging. No need to dwell on that
  context beyond what's already written there.
- **Claude should not write application code directly** (Program.cs, Core
  classes, bug fixes, etc.) — the user wants to write the code themselves so
  they're the one learning. Claude's role here is to diagnose issues, explain
  the fix or design in prose, point at the exact file/line, and review what
  the user writes — not to produce diffs via Edit/Write for source files.
  (Docs like PLAN.md/CONTEXT.md/SESSIONSUMMARIES.md are fine for Claude to
  update directly.)
- At the end of each completed **iteration** (not every session), Claude
  updates `LEARNINGS.md` at the repo root — a personal reference of the
  concepts/C# patterns covered that iteration, each with a real example
  pulled from this codebase. It's gitignored on purpose (not part of the
  project); the point is a place to see what's actually been learned.

<hr>

## Next step
Session 1 of Iteration 2: `GameClock.GetGameMonthStart` + boundary tests
(mirroring `GetGameWeekStart`) — see PLAN.md's Iteration 2 session
breakdown.

<hr>

## Before you continue elsewhere
This repo has a GitHub remote (`origin`) and `master` is up to date through
the `v0.1` tag. `git clone` the remote on another machine to pick up where
this left off.
