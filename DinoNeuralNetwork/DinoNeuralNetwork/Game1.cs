using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetworkLibrary;

namespace DinoNeuralNetwork
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite Ground1;
        Sprite Ground2;
        int groundHeight = 300;

        //Dinosaur dinosaur;
        ComputerDino[] dinoArmy;

        DinoTrainer dinoTrainer;

        ObstacleManager enemyOverlord;
        /*Rectangle[] cactusTexturePositions;
        Queue<Obstacle> cacti;*/

        SpriteFont debugFont;
        Random random;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D spriteSheet = Content.Load<Texture2D>("spritesheet");

            Rectangle groundSrcRectangle = new Rectangle(2, 54, 1200, 12);
            Ground1 = new Sprite(new Vector2(0, groundHeight - groundSrcRectangle.Height), spriteSheet, groundSrcRectangle, new Vector2(-5, 0));
            Ground2 = new Sprite(new Vector2(1200, groundHeight - groundSrcRectangle.Height), spriteSheet, groundSrcRectangle, new Vector2(-5, 0));

            AnimationFrame[] dinoFrames =
            {
                new AnimationFrame(new Rectangle(849, 2, 44, 47), new System.TimeSpan(0, 0, 0, 0, 200)),    //Standing/Jumping Eyes open
                new AnimationFrame(new Rectangle(892, 2, 44, 47), new System.TimeSpan(0, 0, 0, 0, 200)),    //Standing Eyes closed
                new AnimationFrame(new Rectangle(936, 2, 44, 47), new System.TimeSpan(0, 0, 0, 0, 200)),    //Running-1
                new AnimationFrame(new Rectangle(980, 2, 44, 47), new System.TimeSpan(0, 0, 0, 0, 200)),     //Running-2
                new AnimationFrame(new Rectangle(1115, 22, 54, 25), new System.TimeSpan(0, 0, 0, 0, 200))     //Ducking
            };
            Rectangle dinosaurPosition = new Rectangle(40, 300 - dinoFrames[1].Position.Height, dinoFrames[1].Position.Width, dinoFrames[1].Position.Height);

            random = new Random();
            dinoTrainer = new DinoTrainer(dinosaurPosition, spriteSheet, dinoFrames);
            dinoArmy = dinoTrainer.RecruitArmy(100);
            dinoTrainer.RandomizeArmy(dinoArmy, random);

            var obstacleInfo = new (Rectangle, int)[]
            {
                (new Rectangle(228, 2, 17, 35), 2),  //Single small
                (new Rectangle(245, 2, 34, 35), 2),  //Double small
                (new Rectangle(279, 2, 50, 35), 2),  //Triple small
                (new Rectangle(332, 2, 25, 50), 2),  //Single large
                (new Rectangle(357, 2, 50, 50), 2),  //Double large
                (new Rectangle(407, 2, 75, 50), 2)  //Cactus family
            };

            enemyOverlord = new ObstacleManager(spriteSheet, obstacleInfo, 300);

            debugFont = Content.Load<SpriteFont>("Debug");
            // TODO: use this.Content to load your game content here
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            enemyOverlord.Update(gameTime);

            if (Ground1.Position.X < 0 - Ground1.Texture.Width)
            {
                Ground1.Position = new Rectangle(Ground2.Position.X + Ground2.Position.Width, groundHeight - Ground1.Position.Height, Ground1.Position.Width, Ground1.Position.Height);
            }
            else if (Ground2.Position.X < 0 - Ground2.Texture.Width)
            {
                Ground2.Position = new Rectangle(Ground1.Position.X + Ground1.Position.Width, groundHeight - Ground2.Position.Height, Ground2.Position.Width, Ground2.Position.Height);
            }

            bool allAreDead = true;
            foreach (ComputerDino dino in dinoArmy)
            {
                dino.Update(gameTime, enemyOverlord.Minions);
                if (dino.State != DinoState.Dead)
                {
                    allAreDead = false;
                }
            }

            if (allAreDead)
            {
                dinoTrainer.EvolveArmy(dinoArmy, random, 0.1);
                foreach (ComputerDino dino in dinoArmy)
                {
                    dino.Reset();
                }
                enemyOverlord.Reset();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(247, 247, 247, 0));

            spriteBatch.Begin();

            Ground1.Draw(spriteBatch);
            Ground2.Draw(spriteBatch);

            foreach (ComputerDino dino in dinoArmy)
            {
                dino.Draw(spriteBatch);
            }
            //spriteBatch.DrawString(debugFont, $"velocity = {dinosaur.Velocity}", new Vector2(0, 0), Color.Black);
            //spriteBatch.DrawString(debugFont, $"Score = {dinosaur.Score}", new Vector2(0, 0), Color.Black);

            enemyOverlord.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
