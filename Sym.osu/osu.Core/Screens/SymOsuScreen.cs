using System;
using System.Collections.Generic;
using System.Diagnostics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Screens;

namespace osu.Core.Screens
{
    public class SymOsuScreen : OsuScreen
    {
        protected virtual Container Content => content;

        private readonly Container content;

        public IReadOnlyList<Drawable> Children
        {
            get => Content.Children;
            set => ChildrenEnumerable = value;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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

        public IEnumerable<Drawable> ChildrenEnumerable
        {
            set
            {
                Clear();
                AddRange(value);
            }
        }

        public SymOsuScreen()
        {
            InternalChild = content = new Container
            {
                RelativeSizeAxes = Axes.Both
            };
        }

        /// <summary>
        /// Adds a child to this container. This amount to adding a child to <see cref="Content"/>'s
        /// <see cref="Children"/>, recursing until <see cref="Content"/> == this.
        /// </summary>
        public virtual void Add(Drawable drawable)
        {
            if (drawable == Content)
                throw new InvalidOperationException("Content may not be added to itself.");

            if (Content == content)
                AddInternal(drawable);
            else
                Content.Add(drawable);
        }

        /// <summary>
        /// Adds a range of children. This is equivalent to calling <see cref="Add(T)"/> on
        /// each element of the range in order.
        /// </summary>
        public void AddRange(IEnumerable<Drawable> range)
        {
            foreach (Drawable d in range)
                Add(d);
        }

        /// <summary>
        /// Removes a given child from this container.
        /// </summary>
        public virtual bool Remove(Drawable drawable) => Content != content ? Content.Remove(drawable) : RemoveInternal(drawable);

        /// <summary>
        /// Removes all children which match the given predicate.
        /// This is equivalent to calling <see cref="Remove(T)"/> for each child that
        /// matches the given predicate.
        /// </summary>
        /// <returns>The amount of removed children.</returns>
        public int RemoveAll(Predicate<Drawable> pred)
        {
            if (Content != content)
                return Content.RemoveAll(pred);

            int removedCount = 0;

            for (int i = 0; i < InternalChildren.Count; i++)
            {
                var tChild = (Drawable)InternalChildren[i];

                if (pred.Invoke(tChild))
                {
                    RemoveInternal(tChild);
                    removedCount++;
                    i--;
                }
            }

            return removedCount;
        }

        /// <summary>
        /// Removes a range of children. This is equivalent to calling <see cref="Remove(T)"/> on
        /// each element of the range in order.
        /// </summary>
        public void RemoveRange(IEnumerable<Drawable> range)
        {
            if (range == null)
                return;

            foreach (Drawable p in range)
                Remove(p);
        }

        /// <summary>
        /// Removes all children.
        /// </summary>
        public void Clear() => Clear(true);

        /// <summary>
        /// Removes all children.
        /// </summary>
        /// <param name="disposeChildren">
        /// Whether removed children should also get disposed.
        /// Disposal will be recursive.
        /// </param>
        public virtual void Clear(bool disposeChildren)
        {
            if (Content != null && Content != content)
                Content.Clear(disposeChildren);
            else
                ClearInternal(disposeChildren);
        }

        public void Push(Screen screen) => ScreenExtensions.Push(this, screen);
    }
}
