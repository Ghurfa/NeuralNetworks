using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoNeuralNetwork
{
    public enum DinoState
    {
        Standing,
        Running,
        Jumping,
        Ducking,
        Dead
    }
    public class Dinosaur : AnimatedSprite
    {
        public DinoState State;
        public int Score
        {
            get
            {
                return (int)score;
            }
        }
        private double score;
        readonly Rectangle startPosition;
        readonly Point runningPosition;
        private double preciseY;
        public Dinosaur(Rectangle position, Texture2D spriteSheet, Vector2 velocity, params AnimationFrame[] animationPositions)
            : base(position, spriteSheet, velocity, animationPositions)
        {
            runningPosition = position.Location;
            startPosition = position;
            State = DinoState.Standing;
            FrameIndex = 2;
            CurrentFrame = Frames[0];
            score = 0;
        }
        public void Jump()
        {
            if(State == DinoState.Running || State == DinoState.Standing)
            {
                preciseY = runningPosition.Y;
                Velocity = new Vector2(0, -700);
                State = DinoState.Jumping;
                CurrentFrame = Frames[0];
            }
        }
        public void Duck()
        {
            State = DinoState.Ducking;
            CurrentFrame = Frames[4];
            Position = new Rectangle(Position.Location, new Point(CurrentFrame.Position.Width, CurrentFrame.Position.Height));
        }
        public override void Update(GameTime gameTime)
        {
            throw new InvalidOperationException();
        }
        public virtual void Update(GameTime gameTime, Queue<Obstacle> obstacles)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime;
            if(State == DinoState.Dead)
            {

            }
            else
            {
                score += gameTime.ElapsedGameTime.TotalMilliseconds/25;
                foreach (Obstacle osbtacle in obstacles)
                {
                    if(Position.Intersects(osbtacle.Position))
                    {
                        State = DinoState.Dead;
                        break;
                    }
                }
                if (State == DinoState.Running)
                {
                    Position = startPosition;
                    if (timeSinceLastFrame > CurrentFrame.Length)
                    {
                        timeSinceLastFrame = TimeSpan.Zero;
                        if (FrameIndex == 2)
                        {
                            FrameIndex = 3;
                        }
                        else if (FrameIndex == 3)
                        {
                            FrameIndex = 2;
                        }
                        CurrentFrame = Frames[FrameIndex];
                    }
                }
                else if (State == DinoState.Jumping)
                {
                    float gravity = 1600;
                    Velocity = new Vector2(0, Velocity.Y + (float)(gravity * gameTime.ElapsedGameTime.TotalSeconds));
                    preciseY += (Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
                    if (preciseY < runningPosition.Y)
                    {
                        Position = new Rectangle(runningPosition.X, (int)preciseY, Position.Width, Position.Height);
                    }
                    else
                    {
                        CurrentFrame = Frames[2];
                        Velocity = new Vector2(0, 0);
                        Position = new Rectangle(runningPosition, CurrentFrame.Position.Size);
                        State = DinoState.Running;
                    }
                }
                else if (State == DinoState.Ducking)
                {
                    if(Position.Bottom < startPosition.Bottom)
                    {
                        float gravity = 2500;
                        Velocity = new Vector2(0, Velocity.Y + (float)(gravity * gameTime.ElapsedGameTime.TotalSeconds));
                        preciseY += (Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
                        if ((preciseY + Texture.Height) < startPosition.Bottom) //Not touched ground
                        {
                            Position = new Rectangle(runningPosition.X, (int)preciseY, Position.Width, Position.Height);
                        }
                        else
                        {
                            CurrentFrame = Frames[2];
                            Velocity = new Vector2(0, 0);
                            Position = new Rectangle(new Point(startPosition.X, startPosition.Bottom - CurrentFrame.Position.Height), CurrentFrame.Position.Size);
                        }
                    }
                }
            }
        }
        public void Reset()
        {
            Position = startPosition;
            State = DinoState.Running;
            Velocity = Vector2.Zero;
            CurrentFrame = Frames[0];
            score = 0;
        }
    }
}
