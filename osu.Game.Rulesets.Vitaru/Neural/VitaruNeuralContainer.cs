using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.NeuralNetworking;

namespace osu.Game.Rulesets.Vitaru.Neural
{
    public class VitaruNeuralContainer : NeuralInputContainer<VitaruAction>
    {
        public override TensorFlowBrain TensorFlowBrain => vitaruNeuralBrain;

        public override VitaruAction[] GetActiveActions => throw new System.NotImplementedException();

        private readonly VitaruNeuralBrain vitaruNeuralBrain;

        private readonly VitaruPlayfield vitaruPlayfield;

        public VitaruNeuralContainer(VitaruPlayfield vitaruPlayfield)
        {
            this.vitaruPlayfield = vitaruPlayfield;
            vitaruNeuralBrain = new VitaruNeuralBrain(vitaruPlayfield);
        }
    }
}
