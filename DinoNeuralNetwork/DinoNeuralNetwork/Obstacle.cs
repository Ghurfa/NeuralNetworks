using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoNeuralNetwork
{
    public class Obstacle : Sprite
    {
        public Obstacle(Vector2 position, Texture2D texture, Rectangle positionOnTexture, Vector2 velocity)
            :base(position, texture, positionOnTexture, velocity)
        {

        }
    }
}
