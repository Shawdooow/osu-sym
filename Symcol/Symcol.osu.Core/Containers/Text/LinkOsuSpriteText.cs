using System.Diagnostics;

namespace Symcol.osu.Core.Containers.Text
{
    public class LinkOsuSpriteText : ClickableOsuSpriteText
    {
        public string Url
        {
            set
            {
                if (value != null)
                    Action = () => Process.Start(value);
            }
        }
    }
}
