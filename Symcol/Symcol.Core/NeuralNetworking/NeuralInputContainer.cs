using OpenTK;
using osu.Framework.Input.Bindings;
using System;
using osu.Framework.Graphics.Containers;

namespace Symcol.Core.NeuralNetworking
{
    public abstract class NeuralInputContainer<T> : Container, IKeyBindingHandler<T>
        where T : struct, IConvertible
    {
        public abstract TensorFlowBrain<T> TensorFlowBrain { get; }

        /// <summary>
        /// All currently usable actions in T
        /// </summary>
        public abstract T[] GetActiveActions { get; }

        protected NeuralInputContainer(SimultaneousBindingMode bindingMode = SimultaneousBindingMode.Unique) : base()
        {
        }

        protected override void Update()
        {
            base.Update();

            if (TensorFlowBrain.NeuralNetworkState == NeuralNetworkState.Active)
                foreach (T t in GetActiveActions)
                {
                    int i = TensorFlowBrain.GetOutput(t);

                    if (i % 2 == 1)
                        Pressed(t);
                    else
                        Released(t);
                }
            
        }

        #region Input Handling
        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => true;

        public bool OnPressed(T action)
        {
            if (TensorFlowBrain.NeuralNetworkState < NeuralNetworkState.Active)
            {
                Pressed(action);
                return true;
            }
            else
                return false;
        }

        public Action<T> Pressed;

        public bool OnReleased(T action)
        {
            if (TensorFlowBrain.NeuralNetworkState < NeuralNetworkState.Active)
            {
                Released(action);
                return true;
            }
            else
                return false;
        }

        public Action<T> Released;
        #endregion
    }
}
