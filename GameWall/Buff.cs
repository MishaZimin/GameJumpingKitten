using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static JumpingKitten.Game1;

namespace JumpingKitten
{
    public class Buff : Sprite
    {
        public static bool Flag;
        public static bool IsActive;
        public static float Duration;
        public static float Timer;

        public enum BuffType
        {
            Save,
            Speed,
            LowSpeed
        }

        public BuffType Type;

        public Buff(Texture2D texture, Vector2 position, float speed = 0)
        : base(texture, position, speed)
        {
        }

        public static void AddNewBuff(List<Buff> buffs, Texture2D buffTexture, int shance)
        {
            int rundomNumberBuff = rnd.Next(0, shance); //шанс появления        

            if (rundomNumberBuff == 0)
            {
                float rndPosition = rnd.Next(1, 3);
                float buffPositionY = rnd.Next(0, 100);
                Vector2 buffPosition = new(0, 0);

                if (rndPosition == 1) // бафф справа
                {
                    buffPosition = new(Wall.Walls[^1].position.X + Wall.Walls[^1].texture.Width + 150, Wall.Walls[^1].position.Y + buffPositionY);
                }

                if (rndPosition == 2) // бафф слева
                {
                    buffPosition = new(Wall.Walls[^1].position.X - 135, Wall.Walls[^1].position.Y + buffPositionY);
                }

                buffs.Add(new BuffSave(buffTexture, buffPosition, 0f));
            }
        }

        public static void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            BuffLowSpeed.UpdateBuffLowSpeed(deltaTime);
            BuffSave.UpdateBuffSave(deltaTime);
            BuffSpeed.UpdateBuffSpeed(deltaTime);
        }

        public static void DrawBuff(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, List<Buff> buffs)
        {
            foreach (var buff in buffs)
            {
                buff.Draw(spriteBatch);
            }
        }

        public static void AddNewBuffs()
        {
            AddNewBuff(BuffLowSpeed.buffsLowSpeed, buffLowSpeedTexture, 10);
            AddNewBuff(BuffSave.buffsSave, buffSaveTexture, 20);
            AddNewBuff(BuffSpeed.buffsSpeed, buffSpeedTexture, 5);
        }
    }
}