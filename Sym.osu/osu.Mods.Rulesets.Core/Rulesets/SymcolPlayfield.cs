#region usings

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.UI;
using osuTK;

#endregion

namespace osu.Mods.Rulesets.Core.Rulesets
{
    public class SymcolPlayfield : Playfield
    {
        #region Container

        protected virtual Container<Drawable> Content => new Container();

        public IReadOnlyList<Drawable> Children
        {
            get
            {
                return InternalChildren;
            }
            set
            {
                ChildrenEnumerable = value;
            }
        }

        public IEnumerable<Drawable> ChildrenEnumerable
        {
            set
            {
                Clear();
                AddRange(value);
            }
        }

        public void AddRange(IEnumerable<Drawable> range)
        {
            foreach (Drawable d in range)
                AddDrawable(d);
        }

        public Drawable Child
        {
            get
            {
                //if (Children.Count != 1)
                    //throw new InvalidOperationException($"{nameof(Child)} is only available when there's only 1 in {nameof(Children)}!");

                return Children[0];
            }
            set
            {
                Clear();
                AddDrawable(value);
            }
        }

        public void Clear() => Clear(true);

        public virtual void Clear(bool disposeChildren)
        {
            if (Content != null)
                Content.Clear(disposeChildren);
            else
                ClearInternal(disposeChildren);
        }

        public void AddDrawable(Drawable drawable)
        {
            AddInternal(drawable);
        }

        public void RemoveDrawable(Drawable drawable)
        {
            RemoveInternal(drawable);
        }

        #endregion

        public new virtual float Margin => 0.8f;

        protected virtual Vector2 AspectRatio => new Vector2(4, 3);

        public SymcolPlayfield()
        {
            RelativeSizeAxes = Axes.None;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            Scale = new Vector2(Parent.DrawSize.Y * AspectRatio.X / AspectRatio.Y / Size.X, Parent.DrawSize.Y / Size.Y) * Margin;
        }
    }
}
