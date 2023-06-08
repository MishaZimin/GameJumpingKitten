using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpingKitten
{
    public class Spike : Sprite
    {
        public float speed = 1f; // 2

        public Spike(Vector2 position, Texture2D texture, float speed)
            : base(texture, position, speed)
        {
            this.position = position;
            this.texture = texture;
            this.speed = speed;
        }

        public void GoSpikeDownAndUp(Wall wall)
        {
            position = new Vector2(position.X, position.Y + speed);

            if (position.Y + texture.Height + speed > wall.position.Y + wall.texture.Height ||
                position.Y + speed < wall.position.Y)
            {
                speed *= -1;
            }
        }
    }
}