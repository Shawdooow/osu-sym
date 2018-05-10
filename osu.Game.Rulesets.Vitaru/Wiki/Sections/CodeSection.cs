using osu.Framework.Allocation;
using osu.Framework.Configuration;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class CodeSection : WikiSection
    {
        public override string Title => "Code";

        private Bindable<Code> selectedEquations = new Bindable<Code> { Default = Code.ConvertDifficultySettings };

        private WikiParagraph rightParagraph = new WikiParagraph("Check back later!");
        private WikiParagraph leftParagraph = new WikiParagraph("Check back later!");

        [BackgroundDependencyLoader]
        private void load()
        {
            WikiOptionEnumSplitExplanation<Code> equationsDescription;

            Content.Add(new WikiParagraph("Don't worry, you don't have to speak C# to understand this section. " +
                        "This is just a place for people who want to know exactly whats going on under the hood without having to go digging through the code themselves. " +
                        "Its a place for the programmers to display things like the PP algorithm or exactly how certain spells calculate certain things to you, or your friends if you don't care.\n"));
            Content.Add(equationsDescription = new WikiOptionEnumSplitExplanation<Code>(selectedEquations, leftParagraph, rightParagraph));

            selectedEquations.ValueChanged += equations =>
            {
                leftParagraph.Text = "Check back later!";
                rightParagraph.Text = "Check back later!";

                switch (equations)
                {
                    case Code.Difficulty:
                        leftParagraph.Text = "I honestly have no idea how it works or if it actually does. It appears to work so I ain't gonna go back in there till people complain.";
                        break;
                    case Code.PP:
                        leftParagraph.Text = "Where:\n" +
                        "difficulty = map star rating\n" +
                        "Score.TotalScore = score you got\n" +
                        "pp_multiplier = some number of my choosing. This will NEVER change EVER and will be the same for EVERY play EVER!";

                        rightParagraph.Text = "pp = difficulty * Score.TotalScore * pp_multiplier;";
                        break;
                    case Code.Player:
                        leftParagraph.Text = "Basically we are checking where the bullet is relative to the player's hitbox (which would be (0, 0)) and seeing how far away the edge of the bullet is to the edge of the hitbox. " +
                        "If its less than 64 you can heal this beat (which approximates to just within the health ring). " +
                        "This is also how we check if you can gain energy, but instead of it being beat based it is a per frame thing " +
                        "(however the amount is framerate independent, it is based on real time passed. This makes it map independent).";

                        rightParagraph.Text = "Vector2 object2Pos = bullet.ToSpaceOfOtherDrawable(Vector2.Zero, this) + new Vector2(6);\n" +
                        "float distance = (float)Math.Sqrt(Math.Pow(object2Pos.X, 2) + Math.Pow(object2Pos.Y, 2));\n" +
                        "float edgeDistance = distance - (bullet.Width / 2 + Hitbox.Width / 2);\n\n" +
                        "if (edgeDistance < 64 && bullet.Bullet.Team != Team)\n" +
                        "   CanHeal = true;";
                        break;
                }
            };
            selectedEquations.TriggerChange();
        }
    }

    public enum Code
    {
        [System.ComponentModel.Description("Difficulty Settings")]
        ConvertDifficultySettings,
        [System.ComponentModel.Description("Difficulty Calculation")]
        Difficulty,
        [System.ComponentModel.Description("PP Calulation")]
        PP,
        Player,
        Ryokoy,
        Sakuya,
    }
}
