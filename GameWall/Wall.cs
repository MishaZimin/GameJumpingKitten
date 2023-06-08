using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static JumpingKitten.Game1;
using static JumpingKitten.Kitten;

namespace JumpingKitten
{
    public partial class Wall : Sprite
    {
        public Spike spike;
        public bool speedFlag;
        public bool touchFlag;
        public static List<Wall> Walls = new();

        public Wall(Vector2 position, Texture2D texture, float speed, Spike spike)
        : base(texture, position, speed)
        {
            this.spike = spike;
        }

        public static void Update(KeyboardState keyboardState)
        {
            for (var i = 0; i < Walls.Count; i++)
            {
                var wall = Walls[i];

                if (Walls[0].position.Y - kitten.position.Y > 1000)
                {
                    AddNewWall();
                    Buff.AddNewBuffs();

                    Walls.RemoveAt(0);
                }

                // касание шипов
                kitten.TouchSpike(wall);

                // движение шипов
                wall.spike.GoSpikeDownAndUp(wall);

                //передвижение, повороты и прыжки на стене
                kitten.MovesOnWall(keyboardState, wall);

                kitten.Jump(keyboardState);

                //зацепиться за стену
                CollisionWithWall(wall);
            }
        }

        public static void DrawWalls(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, List<Wall> walls)
        {
            foreach (var wall in walls)
            {
                wall.Draw(spriteBatch);
                wall.spike.Draw(spriteBatch);
            }
        }

        public static void AddNewWall()
        {
            int numberWallPositin = rnd.Next(1, 4);

            float changeWallX = 0;
            float changeWallY = 0;

            if (numberWallPositin == 1)
            {
                changeWallX = 0;
                changeWallY = -200;
            }

            if (numberWallPositin == 2)
            {
                changeWallX = 300;
                changeWallY = -85;
            }

            if (numberWallPositin == 3)
            {
                changeWallX = -300;
                changeWallY = -85;
            }

            if (numberWallPositin == 4)
            {
                changeWallX = 0;
                changeWallY = 0;
            }

            float wallPositionX = (int)Walls[^1].position.X + changeWallX;
            float wallPositionY = (int)Walls[^1].position.Y - wallTexture.Height + changeWallY;

            float spikePositionY = rnd.Next(1, 10) * 10;

            float randomSpeedSpike = rnd.Next(3, 6);

            var newWallTextur = wallTexture;
            var newSpikeTexture = SmallSpikeLeftTexture;
            float spikePositionX = 0;

            float numberSpike = rnd.Next(1, 5);

            if (numberSpike == 1)
            {
                newWallTextur = wallTexture;
                newSpikeTexture = SmallSpikeLeftTexture;
                spikePositionX = wallPositionX - SmallSpikeLeftTexture.Width + 2;
            }

            if (numberSpike == 2)
            {
                newWallTextur = badWallTexture;
                newSpikeTexture = BigSpikeLeftTexture;
                spikePositionX = wallPositionX - BigSpikeLeftTexture.Width + 2;
            }

            if (numberSpike == 3)
            {
                newWallTextur = wallTexture;
                newSpikeTexture = SmallSpikeRightTexture;
                spikePositionX = wallPositionX + wallTexture.Width - 2;
            }

            if (numberSpike == 4)
            {
                newWallTextur = wallTexture;
                newSpikeTexture = BigSpikeRightTexture;
                spikePositionX = wallPositionX + wallTexture.Width - 2;
            }

            Walls.Add(new Wall(new(wallPositionX, wallPositionY),
                               newWallTextur, 0,
                               new Spike(new(spikePositionX, wallPositionY + spikePositionY), newSpikeTexture, randomSpeedSpike)));
        }

        public static void CollisionWithWall(Wall wall)
        {
            if (kitten.CollisionLeft(wall))
            {
                kitten.EndJump();
                kitten.texture = kittenTextureUpLeft;
                // позиция котенка слева от стены
                kitten.position.X = wall.position.X - kitten.texture.Width;
            }

            if (kitten.CollisionRight(wall))
            {
                kitten.EndJump();
                kitten.texture = kittenTextureUpRight;
                // позиция котенка справа от стены
                kitten.position.X = wall.position.X + wall.texture.Width; ;
            }
        }

        public static void GenerationWalls()
        {
            Walls.Add(new Wall(new Vector2(700, 500), wallTexture, 0, new Spike(new Vector2(-140, 4000), BigSpikeLeftTexture, 0)));

            for (int i = 1; i < 10; i++)
            {
                AddNewWall();
                Buff.AddNewBuffs();
            }
        }
    }
}