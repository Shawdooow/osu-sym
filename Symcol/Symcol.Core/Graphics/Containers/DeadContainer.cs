namespace Symcol.Core.Graphics.Containers
{
    /// <summary>
    /// This SymcolContainer will not handle input
    /// </summary>
    public class DeadContainer : SymcolContainer
    {
        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;
    }
}
