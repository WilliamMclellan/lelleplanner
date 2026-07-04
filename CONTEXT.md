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

## Where things stand (as of 2026-07-03)
- **Planning only — no code written yet.** VISION.md and PLAN.md exist; the
  .NET solution hasn't been scaffolded.
- Repo folder name: `lelleplanner` (on this machine:
  `C:\Users\william.mclellan\source\repos\lelleplanner`)
- **Not yet a git repository.** No `git init` has been run and there's no
  remote. See "Before you continue elsewhere" below.

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

<hr>

## Next step
Scaffold the MVP solution — PLAN.md's "Suggested session breakdown,"
session 1: `dotnet new sln`, add `Lelleplanner.Core` (classlib) and
`Lelleplanner.ConsoleApp` (console), wire up the project reference,
`git init` + `.gitignore`, confirm a clean build and run.

<hr>

## Before you continue elsewhere
This folder has never been committed to git, so nothing here syncs
automatically. Before switching machines, make sure `VISION.md`, `PLAN.md`,
and this file actually exist on the other one — whether that's by pushing
this folder to a remote (GitHub, Azure DevOps, etc.) or copying it manually.
