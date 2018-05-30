using Symcol.Core.NeuralNetworking;

namespace osu.Game.Rulesets.Shape.Neural
{
    public class ShapeNeuralContainer : NeuralInputContainer<ShapeAction>
    {
        public override ShapeAction[] GetActiveActions => throw new System.NotImplementedException();

        public override TensorFlowBrain TensorFlowBrain => new ShapeNeuralBrain();

        public ShapeNeuralContainer()
        {

        }
    }
}
