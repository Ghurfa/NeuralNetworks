using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeuralNetworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoNeuralNetwork
{
    public class ComputerDino : Dinosaur
    {
        public Network Brain;
        public ComputerDino(Rectangle position, Texture2D spriteSheet, Vector2 velocity, AnimationFrame[] animationPositions, Network brain)
            : base(position, spriteSheet, velocity, animationPositions)
        {
            Brain = brain;
        }
        public override void Update(GameTime gameTime, Queue<Obstacle> obstacles)
        {
            double[] input = new double[] { obstacles.Peek().Position.X,
                                            Position.Y,
                                            Velocity.Y};
            double[] output = Brain.Compute(input);
            if (output[0] == 1)
            {
                Jump();
            }
            else if(output[1] == 1)
            {
                Duck();
            }
            base.Update(gameTime, obstacles);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (State != DinoState.Dead)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
