using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Graphics.Containers;

namespace Symcol.Rulesets.Core.VectorVideos
{
    public class VectorVideo : BeatSyncedContainer
    {
        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {

        }

        protected void LoadContent(string args)
        {
            string[] parameters = args.Split(',');
            /*
            ObjectType objectType = ObjectType.CircleVisualizer;
            Anchor anchor = Anchor.Centre;
            Anchor origin = Anchor.Centre;

            bool checkingType = false;

            foreach (string parameter in parameters)
            {
                string[] subParameters = parameter.Split('=');

                foreach (string subParameter in subParameters)
                {
                    if (subParameter == "Type")
                        checkingType = true;

                    if (checkingType)
                        switch (subParameter)
                        {
                            case "LogoVisualizer":
                                objectType = ObjectType.CircleVisualizer;
                                break;
                        }
                }
            }
            */
        }

        private void loadLogoVisualizer()
        {

        }
    }

    public enum ObjectType
    {
        CircleVisualizer,
        StraightVisualizer,
        Clock,
    }
}
