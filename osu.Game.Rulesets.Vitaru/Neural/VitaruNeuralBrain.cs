using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.NeuralNetworking;
using System;
using TensorFlow;

namespace osu.Game.Rulesets.Vitaru.Neural
{
    public class VitaruNeuralBrain : TensorFlowBrain<VitaruAction>
    {
        private readonly VitaruPlayfield vitaruPlayfield;

        public VitaruNeuralBrain(VitaruPlayfield vitaruPlayfield)
        {
            this.vitaruPlayfield = vitaruPlayfield;
        }

        public override TFOutput GetTFOutput(TFSession session, VitaruAction action)
        {
            for (int i = 0; i < vitaruPlayfield.GameField.Current.Count; i++)
                if (vitaruPlayfield.GameField.Current[i] is DrawableBullet drawableBullet)
                {
                    //TFOutput x = session.Graph.Constant(1, new TFShape(), TFDataType.Double)
                }

            TFOutput bulletstuff = session.Graph.Constant(2, new TFShape(4, 4), TFDataType.Int32);
            TFOutput input = session.Graph.Constant((int)action, new TFShape(4, 4), TFDataType.Int32);

            return session.Graph.Mul(bulletstuff, input);
        }
    }
}
