using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.NeuralNetworking;
using System.Collections.Generic;
using TensorFlow;

namespace osu.Game.Rulesets.Vitaru.Neural
{
    public class VitaruNeuralBrain : TensorFlowBrain
    {
        private readonly VitaruPlayfield vitaruPlayfield;

        public VitaruNeuralBrain(VitaruPlayfield vitaruPlayfield)
        {
            this.vitaruPlayfield = vitaruPlayfield;
        }

        public override TFOutput GetTFOutput(TFSession session)
        {
            List<DrawableBullet> bullets = new List<DrawableBullet>();

            foreach (Drawable draw in vitaruPlayfield.GameField.Current)
                if (draw is DrawableBullet drawableBullet)
                    bullets.Add(drawableBullet);

            return session.Graph.Constant(bullets, new TFShape(), TFDataType.Double);
        }
    }
}
