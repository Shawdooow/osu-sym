namespace Symcol.Core.NeuralNetworking
{
    public class TensorFlowBrain
    {
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
    }

    public enum NeuralNetworkState
    {
        Idle,
        Learning,
        Active
    }
}
