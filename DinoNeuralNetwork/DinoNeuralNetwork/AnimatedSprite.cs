using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoNeuralNetwork
{
    public struct AnimationFrame
    {
        public Rectangle Position;
        public TimeSpan Length;

        public AnimationFrame(Rectangle position, TimeSpan length)
        {
            Position = position;
            Length = length;
        }

        public AnimationFrame(int x, int y, int width, int height, TimeSpan length)
        {
            Position = new Rectangle(x, y, width, height);
            Length = length;
        }
    }

    public class AnimatedSprite : Sprite
    {
        public readonly AnimationFrame[] Frames;
        public int FrameIndex;
        public AnimationFrame CurrentFrame { get; protected set; }
        protected TimeSpan timeSinceLastFrame;

        public AnimatedSprite(Rectangle position, Texture2D spriteSheet, Vector2 velocity, params AnimationFrame[] frames)
            :base(position, spriteSheet, velocity)
        {
            Frames = frames;
            FrameIndex = 0;
            CurrentFrame = Frames[FrameIndex];
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime;
            while (timeSinceLastFrame >= CurrentFrame.Length)
            {
                FrameIndex = (FrameIndex + 1) % Frames.Length;
                CurrentFrame = Frames[FrameIndex];
                timeSinceLastFrame -= CurrentFrame.Length;
            }
            Position = new Rectangle((int)(Position.X + Velocity.X), (int)(Position.Y + (Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds)), Position.Width, Position.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, CurrentFrame.Position, Color.White);
        }
    }
}
