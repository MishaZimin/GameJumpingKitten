using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static JumpingKitten.Kitten;
using static JumpingKitten.Wall;

namespace JumpingKitten
{
    public class BuffLowSpeed : Buff
    {
        private const float timer = 10f;
        public static new bool IsActive = false;
        public static new float Duration = timer;
        public static new float Timer = 0f;

        public static List<Buff> buffsLowSpeed = new();

        public BuffLowSpeed(Texture2D texture, Vector2 position, float speed)
        : base(texture, position, speed)
        {
        }

        public static void UpdateBuffLowSpeed(float deltaTime)
        {
            for (var i = 0; i < buffsLowSpeed.Count; i++)
            {
                var buff = buffsLowSpeed[i];

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
                            buffsLowSpeed.RemoveAt(0);

                        for (var j = 0; j < Walls.Count; j++)
                        {
                            var wall = Wall.Walls[j];
                            wall.spike.speed /= 2; // замедление шипов
                            wall.speedFlag = true;
                        }
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

                    for (var j = 0; j < Walls.Count; j++)
                    {
                        var wall = Walls[j];

                        if (wall.speedFlag)
                        {
                            wall.spike.speed *= 2; //ускорение замедленных шипов
                            wall.speedFlag = false;
                        }
                    }
                }
            }
        }
    }
}