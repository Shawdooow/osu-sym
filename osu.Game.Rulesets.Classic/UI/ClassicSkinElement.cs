using osu.Framework.Configuration;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.Settings;
using Symcol.Rulesets.Core.Skinning;

namespace osu.Game.Rulesets.Classic.UI
{
    public class ClassicSkinElement : SkinElement
    {
        public static Texture LoadSkinElement(string fileName, Storage storage)
        {
            Bindable<string> skin = ClassicSettings.ClassicConfigManager.GetBindable<string>(ClassicSetting.Skin);
            Texture texture = GetSkinElement(ClassicRuleset.ClassicTextures, skin, fileName, storage);
            return texture;
        }
    }
}
