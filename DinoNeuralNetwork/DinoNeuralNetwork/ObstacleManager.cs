using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinoNeuralNetwork
{
    class ObstacleManager
    {
        public Queue<Obstacle> Minions;
        Random random;
        Texture2D spriteSheet;
        (Rectangle texturePosition, int heightOffGround)[] obstacleInfo;
        int groundHeight;
        public ObstacleManager(Texture2D spriteSheet, (Rectangle texturePosition, int heightOffGround)[] obstacleInfo, int groundHeight)
        {
            this.spriteSheet = spriteSheet;
            this.obstacleInfo = obstacleInfo;
            this.groundHeight = groundHeight;

            random = new Random();

            Minions = new Queue<Obstacle>();
            for(int i = 0; i < 5; i++)
            {
                spawnNewObstacle();
            }
        }
        public void Reset()
        {
            Minions = new Queue<Obstacle>();
            for (int i = 0; i < 5; i++)
            {
                spawnNewObstacle();
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (Obstacle cactus in Minions)
            {
                cactus.Update(gameTime);
            }
            Obstacle leftMostObtacle = Minions.Peek();
            if (leftMostObtacle.Position.X < -leftMostObtacle.Position.Width)
            {
                Minions.Dequeue();
                spawnNewObstacle();
            }
        }

        private void spawnNewObstacle()
        {
            int minSpacing = 250;
            int maxSpacing = 700;
            Vector2 speed = new Vector2(-10, 0);

            int newObstacleX;
            int newObstacleType = random.Next(obstacleInfo.Length);
            if (Minions.Count > 0)
            {
                bool isSmallCactus = newObstacleType < 3;
                if (isSmallCactus)
                {
                    newObstacleX = Minions.Last().Position.X + random.Next(minSpacing, maxSpacing);
                }
                else
                {
                    int randomVal = random.Next(16, 23);
                    newObstacleX = Minions.Last().Position.X + randomVal * randomVal;
                }
            }
            else
            {
                newObstacleX = 1000;
            }
            Minions.Enqueue(new Obstacle(new Vector2(newObstacleX, groundHeight - obstacleInfo[newObstacleType].heightOffGround - obstacleInfo[newObstacleType].texturePosition.Height), spriteSheet, obstacleInfo[newObstacleType].texturePosition, speed));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Obstacle minion in Minions)
            {
                minion.Draw(spriteBatch);
            }
        }
    }
}
