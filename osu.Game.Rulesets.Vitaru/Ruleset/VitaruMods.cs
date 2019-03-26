#region usings

using System;
using System.Collections.Generic;
using osu.Framework.Audio;
using osu.Framework.Timing;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset
{
    public class VitaruModEasy : ModEasy
    {

    }

    public class VitaruModNoFail : ModNoFail
    {

    }

    public class VitaruModCharged : Mod, IApplicableToDrawableHitObjects
    {
        public override string Name => "Charged";
        public override string Acronym => "CD";
        //public override FontAwesome Icon => ree;
        public override ModType Type => ModType.DifficultyReduction;
        public override string Description => "Full energy, all the time.";
        public override double ScoreMultiplier => 0.2f;
        public override bool Ranked => true;

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            VitaruPlayfield.CHARGED = true;
        }
    }

    public class VitaruModHalfTime : ModHalfTime
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModDoubleTime),
            typeof(VitaruModTrueDoubleTime),
            typeof(VitaruModNightcore),
            typeof(VitaruModTrueNightcore),
        };

        public override double ScoreMultiplier => 0.4;
    }

    public class VitaruModTrueHalfTime : ModHalfTime
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModDoubleTime),
            typeof(VitaruModTrueDoubleTime),
            typeof(VitaruModNightcore),
            typeof(VitaruModTrueNightcore),
        };

        public override string Name => "True Half Time";
        public override string Acronym => "THT";
        public override string Description => "Even less zoom...";
        public override double ScoreMultiplier => 0.2;
        public override void ApplyToClock(IAdjustableClock clock)
        {
            clock.Rate = 0.5f;
        }
    }

    public class VitaruModDaycore : ModDaycore
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModDoubleTime),
            typeof(VitaruModTrueDoubleTime),
            typeof(VitaruModNightcore),
            typeof(VitaruModTrueNightcore),
        };

        public override double ScoreMultiplier => 0.4;
    }

    public class VitaruModTrueDaycore : ModDaycore
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModDoubleTime),
            typeof(VitaruModTrueDoubleTime),
            typeof(VitaruModNightcore),
            typeof(VitaruModTrueNightcore),
        };

        public override string Name => "True Daycore";
        public override string Acronym => "TDC";
        public override string Description => "Whoaaaaaaaaaa...";
        public override double ScoreMultiplier => 0.2;
        public override void ApplyToClock(IAdjustableClock clock)
        {
            IHasPitchAdjust pitchAdjust = clock as IHasPitchAdjust;
            if (pitchAdjust != null)
                pitchAdjust.PitchAdjust = 0.5;
            else
                base.ApplyToClock(clock);
        }
    }

    public class VitaruModHardRock : ModHardRock
    {
        public override double ScoreMultiplier => 1.12;
    }

    public class VitaruModSuddenDeath : ModSuddenDeath
    {
        public override string Description => "Don't get hit";
    }

    public class VitaruModPerfect : ModPerfect
    {
        public override string Description => "Leave no survivors";
    }

    public class VitaruModTruePerfect : ModPerfect
    {
        public override string Name => "True Perfect";
        public override string Acronym => "TPF";
        public override string Description => "Good Luck!";
        public override double ScoreMultiplier => 1000000;
    }

    public class VitaruModDoubleTime : ModDoubleTime
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModHalfTime),
            typeof(VitaruModTrueHalfTime),
            typeof(VitaruModDaycore),
            typeof(VitaruModTrueDaycore),
        };

        public override double ScoreMultiplier => 1.16;
    }

    public class VitaruModTrueDoubleTime : ModDoubleTime
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModHalfTime),
            typeof(VitaruModTrueHalfTime),
            typeof(VitaruModDaycore),
            typeof(VitaruModTrueDaycore),
        };

        public override string Name => "True Double Time";
        public override string Acronym => "TDT";
        public override string Description => "Zoooooooooooooooooooom...";
        public override double ScoreMultiplier => 1.48;
        public override void ApplyToClock(IAdjustableClock clock)
        {
            clock.Rate = 2;
        }
    }

    public class VitaruModNightcore : ModNightcore
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModHalfTime),
            typeof(VitaruModTrueHalfTime),
            typeof(VitaruModDaycore),
            typeof(VitaruModTrueDaycore),
        };

        public override double ScoreMultiplier => 1.16;
    }

    public class VitaruModTrueNightcore : ModNightcore
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModHalfTime),
            typeof(VitaruModTrueHalfTime),
            typeof(VitaruModDaycore),
            typeof(VitaruModTrueDaycore),
        };

        public override string Name => "True Nightcore";
        public override string Acronym => "TNC";
        public override string Description => "Uguuuuuuuuuuuuuuuu...";
        public override double ScoreMultiplier => 1.48;
        public override void ApplyToClock(IAdjustableClock clock)
        {
            IHasPitchAdjust pitchAdjust = clock as IHasPitchAdjust;
            if (pitchAdjust != null)
                pitchAdjust.PitchAdjust = 2;
            else
                base.ApplyToClock(clock);
        }
    }

    public class VitaruModAccel : Mod, IApplicableToDrawableHitObjects
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModHalfTime),
            typeof(VitaruModTrueHalfTime),
            typeof(VitaruModDaycore),
            typeof(VitaruModTrueDaycore),
            typeof(VitaruModDoubleTime),
            typeof(VitaruModTrueDoubleTime),
            typeof(VitaruModNightcore),
            typeof(VitaruModTrueNightcore),
        };

        public override string Name => "Accel";
        public override string Acronym => "AC";
        public override string Description => "Things are changing...";
        public override double ScoreMultiplier => 1.12;
        public override bool Ranked => false;

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            VitaruPlayfield.ACCEL = true;
        }
    }

    public class VitaruModDeccel : Mod, IApplicableToDrawableHitObjects
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModHalfTime),
            typeof(VitaruModTrueHalfTime),
            typeof(VitaruModDaycore),
            typeof(VitaruModTrueDaycore),
            typeof(VitaruModDoubleTime),
            typeof(VitaruModTrueDoubleTime),
            typeof(VitaruModNightcore),
            typeof(VitaruModTrueNightcore),
        };

        public override string Name => "Deccel";
        public override string Acronym => "DC";
        public override string Description => "Things are changing...";
        public override double ScoreMultiplier => 1.12;
        public override bool Ranked => false;

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            //VitaruPlayfield.DECCEL = true;
        }
    }

    public class VitaruModHidden : ModHidden
    {
        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModFlashlight)
        };

        public override string Description => @"Play with bullets disappearing when they are close";
        public override double ScoreMultiplier => 2;

        public override void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            VitaruPlayfield.HIDDEN = true;
        }
    }

    public class VitaruModTrueHidden : ModHidden
    {
        public override string Name => "True Hidden";
        public override string Acronym => "THD";
        public override string Description => @"Play with bullets dissapearing once they leave enemies immediate area";
        public override double ScoreMultiplier => 10;

        public override void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            VitaruPlayfield.TRUEHIDDEN = true;
        }
    }

    public class VitaruModFlashlight : Mod, IApplicableToDrawableHitObjects
    {
        public override string Name => "Flashlight";
        public override string Acronym => "FL";
        public override FontAwesome Icon => FontAwesome.fa_osu_mod_flashlight;
        public override ModType Type => ModType.DifficultyIncrease;

        public override Type[] IncompatibleMods => new[]
        {
            typeof(VitaruModHidden)
        };

        public override string Description => @"Play with bullets only appearing when they are close";
        public override double ScoreMultiplier => 1.32;

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            VitaruPlayfield.FLASHLIGHT = true;
        }
    }

    public class VitaruRelax : ModRelax
    {
        public override string Description => @"Player moves to the cursor instantly";
        public override bool Ranked => false;
    }

    public class VitaruModAutoplay : ModAutoplay<VitaruHitObject>
    {/*
        protected override Score CreateReplayScore(Beatmap<VitaruHitObject> beatmap) => new Score
        {
            User = new User { Username = "reimosu!" },
            Replay = new VitaruAutoGenerator(beatmap).Generate(),
        };
    */}
}
