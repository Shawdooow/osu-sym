﻿using System.Linq;
using TensorFlow;
using static TensorFlow.TFSession;

namespace Symcol.Core.NeuralNetworking
{
    /// <summary>
    /// Will act like high-level input / output for TensorFlow
    /// Can be set to different NeuralNetworkStates
    /// </summary>
    public abstract class TensorFlowBrain<T>
        where T : struct
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
        public abstract TFTensor GetTensor(TFSession session, T t);

        public void LearnInput(int action)
        {
            if (NeuralNetworkState == NeuralNetworkState.Learning)
            {

            }
        }

        //TODO: fix serious memory leak issues
        public int GetOutput(T t)
        {
            TFSession session = new TFSession();
            Runner runner = session.GetRunner();

            TFTensor result = GetTensor(session, t);
            
            int bestIdx = 0;
            float best = 0;

            object output = result.GetValue(jagged: false);

            float[,] val = (float[,])output;

            // Result is [1,N], flatten array
            for (int i = 0; i < val.GetLength(1); i++)
            {
                if (val[0, i] > best)
                {
                    bestIdx = i;
                    best = val[0, i];
                }
            }

            session.Graph.Dispose();
            session.Dispose();
            return bestIdx;
        }
    }

    public enum NeuralNetworkState
    {
        Idle,
        Learning,
        Active
    }
}
