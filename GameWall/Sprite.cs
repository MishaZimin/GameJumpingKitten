using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JumpingKitten
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 position;
        public float speed;

        public Sprite(Texture2D texture, Vector2 position, float speed)
        {
            this.position = position;
            this.texture = texture;
            this.speed = speed;   
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public bool CollisionRight(Sprite sprite)
        {
            var Object2Right = sprite.position.X + sprite.texture.Width;
            var Object2Top = sprite.position.Y;
            var Object2Buttom = sprite.position.Y + sprite.texture.Height;

            var Object1Left = position.X;
            var Object1Right = position.X + texture.Width;
            var Object1Top = position.Y;
            var Object1Buttom = position.Y + texture.Height;

            return Object1Left < Object2Right &&
                   Object1Right > Object2Right &&
                   Object1Buttom > Object2Top &&
                   Object1Top < Object2Buttom;
        }

        public bool CollisionLeft(Sprite sprite)
        {
            var Object2Left = sprite.position.X;
            var Object2Top = sprite.position.Y;
            var Object2Buttom = sprite.position.Y + sprite.texture.Height;

            var Object1Left = this.position.X;
            var Object1Right = this.position.X + this.texture.Width;
            var Object1Top = this.position.Y;
            var Object1Buttom = this.position.Y + this.texture.Height;

            return Object1Right > Object2Left &&
                   Object1Left < Object2Left &&
                   Object1Buttom > Object2Top &&
                   Object1Top < Object2Buttom;
        }

        public static bool IsTouchObjects(Sprite object1, Sprite object2)
        {
            Rectangle Rectangle1 = new((int)object1.position.X,
                                       (int)object1.position.Y,
                                       object1.texture.Width,
                                       object1.texture.Height);

            Rectangle Rectangle2 = new((int)object2.position.X,
                                       (int)object2.position.Y,
                                       object2.texture.Width,
                                       object2.texture.Height);

            return Rectangle1.Intersects(Rectangle2);
        }

        public static bool PresenceObject(Sprite object1, Sprite object2)
        {
            return (object1.position.X == object2.position.X - object1.texture.Width ||
                    object1.position.X == object2.position.X + object2.texture.Width) &&
                    (object1.position.Y + object1.texture.Height > object2.position.Y) &&
                    object1.position.Y < object2.position.Y + object2.texture.Height;
        }
    }
}
