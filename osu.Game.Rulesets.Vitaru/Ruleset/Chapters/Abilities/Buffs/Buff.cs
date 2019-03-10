using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Vitaru.Mods.Chaptersets;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Cursor;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;
using osuTK.Graphics;
using Symcol.Base.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Abilities.Buffs
{
    public class Buff : SymcolContainer, ITuneable
    {
        public AspectLockedPlayfield CurrentPlayfield { get; set; }

        private readonly VitaruGamemode gamemode = ChapterStore.GetGamemode(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode));

        public virtual bool Untuned
        {
            get => untuned;
            set
            {
                if (value == untuned) return;

                untuned = value;

                if (value)
                {
                    playfield.Gamefield.Remove(this);
                    playfield.VitaruInputManager.BlurredPlayfield.Add(this);
                    CurrentPlayfield = playfield.VitaruInputManager.BlurredPlayfield;
                }
                else
                {
                    playfield.VitaruInputManager.BlurredPlayfield.Remove(this);
                    playfield.Gamefield.Add(this);
                    CurrentPlayfield = playfield.Gamefield;
                }
            }
        }

        private bool untuned;

        private readonly VitaruPlayfield playfield;

        public Buff(VitaruPlayfield playfield)
        {
            this.playfield = playfield;
            CurrentPlayfield = playfield.Gamefield;

            AlwaysPresent = true;
            Masking = true;
            Size = new Vector2(12, 18);
            CornerRadius = 4;
            BorderThickness = 2;
            BorderColour = Color4.White;

            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Red,
            };
            EdgeEffect = new EdgeEffectParameters
            {
                Colour = Color4.Yellow.Opacity(0.5f),
                Type = EdgeEffectType.Shadow,
                Radius = Size.X * 2,
            };
        }

        private bool killed;

        protected override void Update()
        {
            base.Update();

            if (Position.Y >= gamemode.PlayfieldBounds.W + 10 && !killed)
            {
                killed = true;
                this.FadeOut(100)
                    .OnComplete(b => Delete());
            }
            else if (!killed)
            {
                Vector2 cursorPos = SymcolCursor.VitaruCursor.CenterCircle.ToSpaceOfOtherDrawable(Vector2.Zero, this);
                double distance = Math.Sqrt(Math.Pow(cursorPos.X, 2) + Math.Pow(cursorPos.Y, 2));
                Alpha = (float)GetAlpha(distance);
            }
        }

        protected double GetAlpha(double distance)
        {
            const double alpha_max = 0.6d;
            const double alpha_min = 0.05d;
            const double range = 160;
            const double scale = (alpha_max - alpha_min) / (0 - range);

            return Math.Max(alpha_min + (distance - range) * scale, alpha_min);
        }

        public override void Delete()
        {
            CurrentPlayfield.Remove(this);
            base.Delete();
        }
    }
}
