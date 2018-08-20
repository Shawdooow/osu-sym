﻿using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using Symcol.NeuralNetworking;
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

        public override TFTensor GetTensor(TFSession session, VitaruAction action)
        {
            const int s = 2;
            TFShape shape = new TFShape(s, s);
            TFTensor tensor = new TFTensor(s);

            TFTensor output;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < vitaruPlayfield.Gamefield.Children.Count; i++)
                if (vitaruPlayfield.Gamefield.Children[i] is DrawableBullet drawableBullet && drawableBullet.Bullet.Team != player.Team)
                {
                    float xPow = (float)Math.Pow(drawableBullet.Position.X - player.Position.X, 2);
                    float yPow = (float)Math.Pow(drawableBullet.Position.Y - player.Position.Y, 2);

                    float sqrt = (float)Math.Sqrt(xPow + yPow);

                    session.GetRunner().AddInput(session.Graph.Constant(sqrt, shape, TFDataType.Float), tensor);
                }

            //TODO: see if this leaks
            output = session.GetRunner().Run(session.Graph.Constant((float)action, shape, TFDataType.Float));

            return output;
        }
    }
}
