#region usings

using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Mods.Rulesets.Core.Skinning;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay
{
    public class VitaruSkinElement : SkinElement
    {
        public static Texture LoadSkinElement(string fileName, Storage storage)
        {
            Bindable<string> skin = new Bindable<string>{ Value = "default" };
            Texture texture = GetSkinElement(VitaruRuleset.VitaruTextures, skin, fileName, storage);
            return texture;
        }

        public static Texture CheckForSkinElement(string fileName, Storage storage)
        {
            Bindable<string> skin = new Bindable<string> { Value = "default" };
            Texture texture = GetElement(skin, fileName, storage);
            return texture;
        }
    }
}
