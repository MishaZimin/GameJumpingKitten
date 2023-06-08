using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpingKitten
{
    public partial class Object
    {
        public Texture2D texture;
        public Vector2 position;

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}