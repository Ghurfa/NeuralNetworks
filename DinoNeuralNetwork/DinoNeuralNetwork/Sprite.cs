using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoNeuralNetwork
{
    public class Sprite
    {
        public Rectangle Position { get; set; }
        public Texture2D Texture { get; protected set; }
        public Vector2 Velocity { get; set; }
        private Rectangle positionOnTexture;
        private bool fullTexture;

        public Sprite(Rectangle position, Texture2D texture, Vector2 velocity)
        {
            Position = position;
            Texture = texture;
            Velocity = velocity;
            fullTexture = true;
        }

        public Sprite(Vector2 position, Texture2D texture, Vector2 velocity)
        {
            Position = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            Texture = texture;
            Velocity = velocity;
            fullTexture = true;
        }

        public Sprite(Rectangle position, Texture2D texture, Rectangle positionOnTexture, Vector2 velocity)
        {
            Position = position;
            Texture = texture;
            this.positionOnTexture = positionOnTexture;
            Velocity = velocity;
            fullTexture = false;
        }

        public Sprite(Vector2 position, Texture2D texture, Rectangle positionOnTexture, Vector2 velocity)
        {
            Position = new Rectangle((int)position.X, (int)position.Y, positionOnTexture.Width, positionOnTexture.Height);
            Texture = texture;
            this.positionOnTexture = positionOnTexture;
            Velocity = velocity;
            fullTexture = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            Position = new Rectangle((int)(Position.X + Velocity.X), (int)(Position.Y + (Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds)), Position.Width, Position.Height);
        }

        public bool Intersects(Sprite other)
        {
            return Position.Intersects(other.Position);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(fullTexture)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
            else
            {
                spriteBatch.Draw(Texture, Position, positionOnTexture, Color.White);
            }
        }
    }
}
