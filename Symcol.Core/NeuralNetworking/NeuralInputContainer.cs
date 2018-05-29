using OpenTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;

namespace Symcol.Core.NeuralNetworking
{
    public abstract class NeuralInputContainer<T> : Container, IKeyBindingHandler<T>
        where T : struct
    {
        public TensorFlowBrain TensorFlowBrain { get; private set; }

        public NeuralInputContainer()
        {
            TensorFlowBrain = new TensorFlowBrain();
        }

        #region Input Handling
        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => true;

        public bool OnPressed(T action)
        {
            if (TensorFlowBrain.NeuralNetworkState < NeuralNetworkState.Active)
                return Pressed(action);
            else
                return false;
        }

        protected virtual bool Pressed(T action)
        {
            return true;
        }

        public bool OnReleased(T action)
        {
            if (TensorFlowBrain.NeuralNetworkState < NeuralNetworkState.Active)
                return Released(action);
            else
                return false;
        }

        protected virtual bool Released(T action)
        {
            return true;
        }
        #endregion
    }
}
