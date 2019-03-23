#region usings

using System.Diagnostics;

#endregion

namespace osu.Core.Containers.Text
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
