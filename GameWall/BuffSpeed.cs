using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static JumpingKitten.Game1;
using static JumpingKitten.Kitten;

namespace JumpingKitten
{
    public class BuffSpeed : Buff
    {
        private const float timer = 5f;
        public static new bool IsActive = false;
        public static new float Duration = timer;
        public static new float Timer = 0f;

        public static List<Buff> buffsSpeed = new();

        public BuffSpeed(Texture2D texture, Vector2 position, float speed)
        : base(texture, position, speed)
        {
        }

        public static void UpdateBuffSpeed(float deltaTime)
        {
            for (var i = 0; i < buffsSpeed.Count; i++)
            {
                var buff = buffsSpeed[i];

                if (IsTouchObjects(kitten, buff))
                {
                    if (IsActive)
                    {
                        Timer = Duration;
                    }
                    else
                    {
                        IsActive = true;

                        for (int j = 0; j <= i; j++)
                            buffsSpeed.RemoveAt(0);

                        kitten.speed *= 1.8f;
                        kitten.speedFlag = true;
                    }
                }
            }

            if (IsActive) // бафф активен
            {
                Timer += deltaTime;

                if (Timer >= Duration)
                {
                    // действия после конца баффа
                    IsActive = false;
                    Timer = 0f;

                    if (kitten.speedFlag)
                    {
                        kitten.speed /= 1.8f;
                    }
                }
            }
        }
    }
}