# Lelleplanner — Vision

> This is the north star. It shouldn't change often. For what we're actually
> building right now, see [PLAN.md](PLAN.md).

## Introduction
Currently, I'm a 28 year old software engineer struggling with depression, anxiety, and burnout. As such, I'd like to encourage myself to build strong, healthy habits again, and reignite my passion for work. This project is meant to encourage me to learn, to reward maintaining daily habits, and ultimately, to give me "breadcrumbs" ('Missing Eddie Card' in the shop), and finally a "cake" ('Daddy Markov [Serialized]' in the shop) at the end of it all.

<hr>

## Constraints
- Have fun! | This is meant to be fun, and to encourage learning.
- The MVP should be up within 4 hours | I need to start feeding my habits and gradually rewarding myself as soon as possible.
- Iterations should take 4-16 hours per iteration | It's easy to overplan and overthink. Start small, and make a greater plan.
- Good structures! | I want this project to have a good project structure and to incentivise learning, as well as good habits, not just in life, but in software engineering too.
- C# | Because I work with C# at work, I want this project to teach me about C# on the side, to feed into getting better at my job, and getting better at my job helping me with my side-project. Positive loops!

<hr>

## Vision
I'm a big fan of gamification in general. I have a large passion for games, and to-do lists as well as "progression quests" help me immensely when it comes to motivation. If I believe I am playing a game, I tend to min-max. As such, I want these routines to be separated into quests.

<hr>

### Quests & Achievements

The quest description structure will be as follows: Name | Goal.
I want three types of quests:
- Daily
- Weekly
- Monthly

The daily quests should be as follows:
- Food For Thought! | Eat breakfast & dinner
- Shiny Pearly Whites! | Brush teeth morning & night
- Buff Papaya | Workout
- Walk For Life | Do 30 min on the treadmill
- Pretty Boy Papaya | Wash your face
- Smartypants huh? | Study for 15-30min
- Excellent work, daily cleared! | Finish all of the above quests

The weekly quests should be as follows:
- Shiny Sparkly! | Do the dishes
- Tidy Room, Tidy Mind | Clean a room
- Week Survived | Finish the weekly quests

The monthly quests should be as follows:
- Habit Handled! | 'Excellent work, daily cleared!' cleared 25 times
- Plates For Days! | 'Shiny Sparkly!' cleared 4 times
- Tidy Home, Tidy Life | 'Tidy Room, Tidy Mind' cleared 4 times

The Achievements should symbolize that I've managed to not only build these habits, but maintain them. I want three achievements that build off completing the monthly quests. These are only completed once.

The Achievements should be as follows:
- Habits Maintained! | 'Habit Handled!' cleared 4 times
- Mythical Kitchen | 'Plates For Days!' cleared 4 times
- Always Ready | 'Tidy Home, Tidy Life' cleared 4 times

#### Quest Rewards
There should be a total of two quest rewards:
- Daily Coins - One Daily Coin is awarded when clearing 'Excellent work, daily cleared!'
- Weekly Coins - One Weekly Coin is awarded when 'Week Survived' is cleared
- Markov Fragments - One Markov Fragment is awarded for each achievement ('Habits Maintained!', 'Mythical Kitchen', 'Always Ready')

### Deckbox
Deckbox can be viewed as an achievement, of sorts. I'm breadcrumbing myself to give myself small treats along the way as I build these habits, with a big treat at the end. The item in the shop 'Missing Eddie Card' should randomly reward me with one of the cards in the list below, with the exception of 'Edgar Markov [Serialized]'. 'Edgar Markov [Serialized]' should not be a potential reward from the 'Missing Eddie Card' pool, and should instead be reserved as a reward from 'Daddy Markov [Serialized]'.

The Deckbox should be a list of a grand total of 48 cards.
- Edgar Markov [Serialized]
- Legion Lieutenant
- Stromkirk Captain
- Cordial Vampire
- Oathsworn Knight
- Arnyn, Deathbloom Botanist
- Blood Artist
- Cruel Celebrant
- Sephiroth, Fabled SOLDIER
- Baron Bertram Graywater
- Bolas's Citadel
- Clavileno, First of the Blessed
- Corrupted Conviction
- Crossway Troublemakers
- Emeritus of Woe
- Falkenrath Pit Fighter
- Gas Guzzler
- Necropotence
- Preacher of the Schism
- Skullclamp
- Vampiric Rites
- Voldaren Epicure
- Welcoming Vampire
- Phyrexian Tower
- Nighthawk Scavenger
- Vampire Cutthroat
- Vampire of the Dire Moon
- Vampire Socialite
- Legion's Landing
- Master of Dark Rites
- Phyrexian Altar
- Pitiless Plunder
- Sorin, Imperious Bloodlord
- The Golden Throne
- Carmen, Cruel Skymarcher
- Captivating Vampire
- Kindred Dominance
- New Blood
- Ruthless Lawbringer
- Bloodline Keeper
- Elenda, the Dusk Rose
- Forerunner of the Legion
- Evereth, Viceroy of Plunder
- Indulgent Aristocrat
- Pitiless Pontiff
- Viscera Seer
- Yahenni, Undying Partisan
- Guul Draz Assassin

### Shop
The shop should use the currencies awarded from the Quest Rewards. I'd like four items for sale in the shop. They'll be described as Name | Cost | In Stock.

- Missing Eddie Card | 3 Daily Coins | 47
- Shortcut! | 1 Weekly Coin | 999
- Cheat Day! | 1 Weekly Coin | 999
- Daddy Markov [Serialized] | 3 Markov Fragments | 1

'Shortcut!' is an item which, when bought, clears the 'Walk For Life' and 'Smartypants huh?' quests for the day. Sometimes you just need a break, and this item will help you skip those two things.

'Cheat Day!' is an item which, when bought, permits the user to go buy snacks and such for 100 SEK.

'Missing Eddie Card' should randomly tick off one of the cards in the list in the Deckbox section, with consideration to the logic described in that section.

'Daddy Markov [Serialized]' should tick off 'Edgar Markov [Serialized]' from the Deckbox list.

<hr>

## Tech Spec (high level)
Written in C#, targeting .NET 8 (LTS). Starts as a console app with ASCII art, with a local JSON "database". See [PLAN.md](PLAN.md) for the concrete MVP architecture and the iteration roadmap toward the rest of this vision.
