using TensorFlow;
using static TensorFlow.TFSession;

namespace Symcol.Core.NeuralNetworking
{
    /// <summary>
    /// Will act like high-level input / output for TensorFlow
    /// Can be set to different NeuralNetworkStates
    /// </summary>
    public abstract class TensorFlowBrain
    {
        /// <summary>
        /// Current state that this Neural Network is set to
        /// </summary>
        public NeuralNetworkState NeuralNetworkState
        {
            get { return neuralNetworkState; }
            set
            {
                if (value != neuralNetworkState)
                {
                    neuralNetworkState = value;
                }
            }
        }

        private NeuralNetworkState neuralNetworkState;

        /// <summary>
        /// Get information to react to
        /// </summary>
        public abstract TFOutput GetTFOutput(TFSession session);

        public void LearnInput(int action)
        {
            if (NeuralNetworkState == NeuralNetworkState.Learning)
            {

            }
        }

        public int GetOutput()
        {
            TFSession session = new TFSession();
            Runner runner = session.GetRunner();

            TFTensor tensor = runner.Run(GetTFOutput(session));

            return (int)tensor.GetValue();
        }
    }

    public enum NeuralNetworkState
    {
        Idle,
        Learning,
        Active
    }
}
