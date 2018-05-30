using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.NeuralNetworking;
using System.Collections.Generic;
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
            List<DrawableBullet> bullets = new List<DrawableBullet>();

            foreach (Drawable draw in vitaruPlayfield.GameField.Current)
                if (draw is DrawableBullet drawableBullet)
                    bullets.Add(drawableBullet);

            return session.Graph.Constant(new Info(bullets, action), new TFShape(), TFDataType.Double);
        }

        private class Info
        {
            public readonly List<DrawableBullet> DrawableBullets;

            public readonly VitaruAction VitaruAction;

            public Info(List<DrawableBullet> bullets, VitaruAction action)
            {
                DrawableBullets = bullets;
                VitaruAction = action;
            }
        }
    }
}
