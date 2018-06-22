//Symcol.Rulesets.Core.Rulesets.SymcolSettingsSubsection
//Symcol.Rulesets.Core.Multiplayer.Pieces.MatchTools
#define SymcolMods

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osu.Game.Graphics.Sprites;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core.Screens;
using Symcol.osu.Core.Screens.Evast;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using Symcol.Rulesets.Core.Rulesets;
#if SymcolMods
#endif

namespace Symcol.Rulesets.Core.LegacyMultiplayer.Screens
{
    public class RulesetMultiplayerSelection : BeatmapScreen
    {
        public static readonly FillFlowContainer<RulesetLobbyItem> LobbyItems = new FillFlowContainer<RulesetLobbyItem>
        {
            RelativeSizeAxes = Axes.Both,
            Width = 0.85f,
            Anchor = Anchor.TopCentre,
            Origin = Anchor.TopCentre,
        };

        public RulesetMultiplayerSelection()
        {
            RelativeSizeAxes = Axes.Both;

            Add(LobbyItems);
        }

        protected override void OnEntering(Screen last)
        {
            base.OnEntering(last);
            foreach (RulesetLobbyItem item in LobbyItems)
                item.Action = () => Push(item.RulesetLobbyScreen);
        }

        protected override bool OnExiting(Screen next)
        {
            Remove(LobbyItems);

#if SymcolMods
            SymcolSettingsSubsection.LegacyRulesetMultiplayerSelection = new RulesetMultiplayerSelection();
            SymcolSettingsSubsection.Lobby = new Lobby();

            SymcolMenu.LegacyRulesetMultiplayerScreen = SymcolSettingsSubsection.LegacyRulesetMultiplayerSelection;
            SymcolMenu.Lobby = SymcolSettingsSubsection.Lobby;
#endif

            return base.OnExiting(next);
        }
    }

    public abstract class RulesetLobbyItem : ClickableContainer
    {
        public abstract string RulesetName { get; }

        public virtual Texture Icon { get; }

        public virtual Texture Background { get; }

        public abstract RulesetLobbyScreen RulesetLobbyScreen { get; }

        public RulesetLobbyItem()
        {
            CornerRadius = 20;
            Masking = true;
            RelativeSizeAxes = Axes.X;
            Height = 100;

            Children = new Drawable[]
            {
                new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode  = FillMode.Fill,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Texture = Background
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f,
                },
                new Sprite
                {
                    Size = new Vector2(Height),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Texture = Icon
                },
                new OsuSpriteText
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Text = RulesetName,
                    TextSize = 60,
                    Position = new Vector2(-20, 0)
                }
            };
        }
    }
}
