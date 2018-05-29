using osu.Framework.Input.Bindings;

namespace Symcol.Core.NeuralNetworking
{
    public abstract class NeuralInputHandler<T> : KeyBindingContainer<T>
        where T : struct
    {
        public TensorFlowBrain TensorFlowBrain { get; private set; }

        public NeuralInputHandler()
        {
            TensorFlowBrain = new TensorFlowBrain();
        }
    }
}
