using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.NeuralNetworking;

namespace osu.Game.Rulesets.Vitaru.Neural
{
    public class VitaruNeuralContainer : NeuralInputContainer<VitaruAction>
    {
        public override TensorFlowBrain<VitaruAction> TensorFlowBrain => vitaruNeuralBrain;

        public override VitaruAction[] GetActiveActions => new[]
        {
            VitaruAction.Up,
            VitaruAction.Down,
            VitaruAction.Left,
            VitaruAction.Right,
            VitaruAction.Slow
        };

        private readonly VitaruNeuralBrain vitaruNeuralBrain;

        private readonly VitaruPlayfield vitaruPlayfield;

        public VitaruNeuralContainer(VitaruPlayfield vitaruPlayfield, DrawableVitaruPlayer player)
        {
            this.vitaruPlayfield = vitaruPlayfield;
            vitaruNeuralBrain = new VitaruNeuralBrain(vitaruPlayfield, player);

            Bindable<NeuralNetworkState> bindable = VitaruSettings.VitaruConfigManager.GetBindable<NeuralNetworkState>(VitaruSetting.NeuralNetworkState);
            bindable.ValueChanged += state =>
            {
                TensorFlowBrain.NeuralNetworkState = state;
            };
            bindable.TriggerChange();
        }

        //public override IEnumerable<KeyBinding> DefaultKeyBindings { get; }
    }
}
