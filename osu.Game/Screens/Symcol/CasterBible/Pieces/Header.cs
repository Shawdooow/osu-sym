using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Platform;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using Symcol.Core.Graphics.Containers;
using System;
using System.Collections.Generic;
using System.Text;

namespace osu.Game.Screens.Symcol.CasterBible.Pieces
{
    public class Header : Container
    {
        private const double transition_time = 500;

        public readonly Bindable<BibleScreen> CurrentBibleScreen = new Bindable<BibleScreen>() { Default = BibleScreen.Teams };

        public readonly OsuTabControl<BibleScreen> TabControl;

        private SymcolClickableContainer open;
        private SymcolClickableContainer edit;

        public Header()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            RelativeSizeAxes = Axes.Both;
            Height = 0.04f;

            AlwaysPresent = true;

            OsuColour color = new OsuColour();

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.8f
                },
                new UTCClock
                {
                    Anchor = Anchor.CentreRight,
                    Position = new Vector2(-200, 0)
                },
                TabControl = new OsuTabControl<BibleScreen>
                {
                    Position = new Vector2(200, 0),
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.4f,
                    
                },
                open = new SymcolClickableContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,

                    RelativeSizeAxes = Axes.Y,
                    Width = 80,

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Green,
                        Alpha = 0.8f
                    }
                },
                edit = new SymcolClickableContainer
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,

                    RelativeSizeAxes = Axes.Y,
                    Width = 80,
                    Action = () => toggleEdit(),

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = color.Yellow,
                        Alpha = 0.8f
                    }
                }
            };
            CurrentBibleScreen.BindTo(TabControl.Current);
        }

        public Action<bool> OnEditToggle;
        private bool editMode;

        private void toggleEdit()
        {
            editMode = !editMode;
            OnEditToggle?.Invoke(editMode);
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            open.Action = storage.OpenInNativeExplorer;
        }

        private bool ctrl;
        private bool u;

        protected override bool OnKeyDown(InputState state, KeyDownEventArgs args)
        {
            if (args.Key == Key.U)
                u = true;
            if (args.Key == Key.ControlLeft || args.Key == Key.ControlRight)
                ctrl = true;

            if (u && ctrl && Alpha < 1)
                PopIn();
            else if (u && ctrl)
                PopOut();

            return base.OnKeyDown(state, args);
        }

        protected override bool OnKeyUp(InputState state, KeyUpEventArgs args)
        {
            if (args.Key == Key.U)
                u = false;
            if (args.Key == Key.ControlLeft || args.Key == Key.ControlRight)
                ctrl = false;

            return base.OnKeyUp(state, args);
        }

        private void PopIn()
        {
            this.MoveToY(0, transition_time, Easing.OutQuint);
            this.FadeIn(transition_time / 2, Easing.OutQuint);
        }

        private void PopOut()
        {
            this.MoveToY(-DrawSize.Y, transition_time, Easing.OutQuint);
            this.FadeOut(transition_time);
        }
    }

    public enum BibleScreen
    {
        Teams,
        MapPool,
        MatchResults
    }
}
