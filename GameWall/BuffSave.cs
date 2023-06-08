using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static JumpingKitten.Kitten;

namespace JumpingKitten
{
    public class BuffSave : Buff
    {
        private const float timer = 20f;
        public static new bool IsActive = false;
        public static new float Duration = timer;
        public static new float Timer = 0f;

        public static List<Buff> buffsSave = new();

        public BuffSave(Texture2D texture, Vector2 position, float speed)
            : base(texture, position, speed)
        {
        }

        public static void UpdateBuffSave(float deltaTime)
        {
            for (var i = 0; i < buffsSave.Count; i++)
            {
                var buff = buffsSave[i];

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
                            buffsSave.RemoveAt(0);
                    }
                }
            }

            if (IsActive)
            {
                Timer += deltaTime;

                if (Timer >= Duration)
                {
                    // действия после конца баффа
                    IsActive = false;
                    Timer = 0f;
                }
            }
        }
    }
}