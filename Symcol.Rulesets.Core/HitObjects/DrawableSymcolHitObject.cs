﻿using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using System;
using System.Collections.Generic;

namespace Symcol.Rulesets.Core.HitObjects
{
    /// <summary>
    /// Mostly stuff copied from Container
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public abstract class DrawableSymcolHitObject<TObject> : DrawableHitObject<TObject>
        where TObject : HitObject
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
                Add(d);
        }

        public Drawable Child
        {
            get
            {
                if (Children.Count != 1)
                    throw new InvalidOperationException($"{nameof(Child)} is only available when there's only 1 in {nameof(Children)}!");

                return Children[0];
            }
            set
            {
                Clear();
                Add(value);
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

        public void Add(Drawable drawable)
        {
            AddInternal(drawable);
        }

        public void Remove(Drawable drawable)
        {
            RemoveInternal(drawable);
        }
        #endregion

        protected DrawableSymcolHitObject(TObject hitObject)
    : base(hitObject)
        { }

        // Todo: At some point we need to move these to DrawableHitObject after ensuring that all other Rulesets apply
        // transforms in the same way and don't rely on them not being cleared
        public override void ClearTransformsAfter(double time, bool propagateChildren = false, string targetMember = null) { }
        public override void ApplyTransformsAt(double time, bool propagateChildren = false) { }
    }
}
