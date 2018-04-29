// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using OpenTK;
using osu.Game.Rulesets.Judgements;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.UI;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Classic.Objects.Drawables
{
    public class DrawableClassicJudgement : DrawableJudgement
    {
        private Sprite sprite;

        public DrawableClassicJudgement(Judgement judgement, DrawableHitObject judgedObject)
            : base(judgement, judgedObject)
        {
            Masking = false;
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            if (Judgement.Result == HitResult.Miss)
            {
                Children = new Drawable[]
                {
                    sprite = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0.5f)
                    }
                };
                sprite.Texture = ClassicSkinElement.LoadSkinElement(@"hit0", storage);
            }
            else if (Judgement.Result == HitResult.Meh)
            {
                Children = new Drawable[]
                {
                    sprite = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0.5f)
                    }
                };
                sprite.Texture = ClassicSkinElement.LoadSkinElement(@"hit50", storage);
            }
            else if (Judgement.Result == HitResult.Good)
            {
                Children = new Drawable[]
                {
                    sprite = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0.5f)
                    }
                };
                sprite.Texture = ClassicSkinElement.LoadSkinElement(@"hit100", storage);
            }
            else if (Judgement.Result == HitResult.Great)
            {
                Children = new Drawable[]
                {
                    sprite = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new Vector2(0.5f)
                    }
                };
                sprite.Texture = ClassicSkinElement.LoadSkinElement(@"hit300", storage);
            }
        }
    }
}
