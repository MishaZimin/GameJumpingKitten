using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using static JumpingKitten.Kitten;

namespace JumpingKitten
{
    public partial class Game1
    {
        public class Camera
        {
            public Vector2 position = new(0, 0);
            public Matrix TransformMatrix { get; private set; }
            public float Scale = 1f;

            public Texture2D texture;

            public Camera(Vector2 position, Texture2D texture)
            {
                this.position = position;
                this.texture = texture;
            }

            public void UpdateMatrix()
            {
                TransformMatrix = Matrix.CreateTranslation(new Vector3(-position.X + 900, -position.Y + 600, 0));
            }

            public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(texture, position, Color.White);
            }

            public void Update()
            {
                if (kitten.LeftZ && position.X > kitten.position.X)
                {
                    for (int i = 0; i < 20; i++)
                        position.X -= 0.5f;
                    kitten.LeftZ = false;
                }

                else if (kitten.RightX && position.X < kitten.position.X)
                {
                    for (int i = 0; i < 20; i++)
                        position.X += 0.5f;
                    kitten.RightX = false;
                }

                else
                {
                    position = kitten.position;
                }
            }
        }
    }
}