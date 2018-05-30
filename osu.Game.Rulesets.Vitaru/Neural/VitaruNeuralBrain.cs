using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
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

        private readonly DrawableVitaruPlayer player;

        public VitaruNeuralBrain(VitaruPlayfield vitaruPlayfield, DrawableVitaruPlayer player)
        {
            this.vitaruPlayfield = vitaruPlayfield;
            this.player = player;
        }

        public override TFTensor[] GetTensors(TFSession session, VitaruAction action)
        {
            TFTensor[] output = new TFTensor[]
            {
                session.GetRunner().Run(session.Graph.Constant((float)action, new TFShape(4, 4), TFDataType.Float))
            };

            for (int i = 0; i < vitaruPlayfield.GameField.Current.Count; i++)
                if (vitaruPlayfield.GameField.Current[i] is DrawableBullet drawableBullet && drawableBullet.Bullet.Team != player.Team)
                {
                    float xPow = (float)Math.Pow(drawableBullet.Position.X, 2);
                    float yPow = (float)Math.Pow(drawableBullet.Position.Y, 2);

                    float sqrt = (float)Math.Sqrt(xPow + yPow);

                    TFOutput position = session.Graph.Constant(sqrt, new TFShape(2, 2), TFDataType.Float);
                    TFOutput angle = session.Graph.Constant((float)Math.Atan2(drawableBullet.Position.Y - player.Position.Y, drawableBullet.Position.X - player.Position.X), new TFShape(2, 2), TFDataType.Float);

                    TFTensor p = session.GetRunner().Run(position);
                    TFTensor a = session.GetRunner().Run(angle);

                    output = addTensorToArray(output, p);
                    output = addTensorToArray(output, a);
                }

            return output;
        }

        private TFTensor[] addTensorToArray(TFTensor[] tensorArray, TFTensor newTensor)
        {
            TFTensor[] newArray = new TFTensor[tensorArray.Length + 1];
            tensorArray.CopyTo(newArray, 1);
            newArray[0] = newTensor;
            return newArray;
        }
    }
}
