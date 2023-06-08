using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpingKitten
{
    class Button : Sprite
    {
        public Button(Texture2D texture, Vector2 position, float speed)
            : base(texture, position, speed)
        {
        }
    }
}