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

        public override TFTensor[] GetTFTensors(TFSession session, VitaruAction action)
        {
            TFOutput input = session.Graph.Constant((int)action, new TFShape(4, 4), TFDataType.Int32);

            TFOutput xPlayer = session.Graph.Constant(player.Position.X, new TFShape(4, 4), TFDataType.Float);
            TFOutput yPlayer = session.Graph.Constant(player.Position.Y, new TFShape(4, 4), TFDataType.Float);

            TFOutput two = session.Graph.Constant(2, new TFShape(4, 4), TFDataType.Int32);

            TFTensor[] output = new TFTensor[]
            {
                session.GetRunner().Run(input)
            };

            for (int i = 0; i < vitaruPlayfield.GameField.Current.Count; i++)
                if (vitaruPlayfield.GameField.Current[i] is DrawableBullet drawableBullet)
                {
                    TFOutput xBullet = session.Graph.Constant(drawableBullet.Position.X, new TFShape(4, 4), TFDataType.Float);
                    TFOutput yBullet = session.Graph.Constant(drawableBullet.Position.Y, new TFShape(4, 4), TFDataType.Float);

                    TFOutput xPow = session.Graph.Pow(xBullet, two);
                    TFOutput yPow = session.Graph.Pow(yBullet, two);

                    TFOutput position = session.Graph.Sqrt(session.Graph.Add(xPow, yPow));

                    TFOutput xRelative = session.Graph.Sub(xBullet, xPlayer);
                    TFOutput yRelative = session.Graph.Sub(yBullet, yPlayer);

                    TFOutput angle = session.Graph.Atan2(yRelative, xRelative);

                    TFTensor p = session.GetRunner().Run(position);
                    TFTensor a = session.GetRunner().Run(position);

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
