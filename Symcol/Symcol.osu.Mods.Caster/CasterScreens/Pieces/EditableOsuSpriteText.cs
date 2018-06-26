using osu.Framework.Configuration;
using Symcol.Core.Graphics.Containers;

namespace Symcol.osu.Mods.Caster.CasterScreens.Pieces
{
    public class EditableOsuSpriteText : SymcolContainer
    {
        public Bindable<bool> Editable = new Bindable<bool>();

        public Bindable<string> Current = new Bindable<string>();

        public EditableOsuSpriteText()
        {

        }
    }
}
