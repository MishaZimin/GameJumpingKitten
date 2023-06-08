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








//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using System.Linq;

//namespace GameWall
//{
//    public class Game1 : Game
//    {
//        private GraphicsDeviceManager graphics;
//        private Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;

//        readonly Background background = new();

//        readonly Kitten kitten = new();

//        private Wall[] walls;
//        private Texture2D wallTexture;

//        public Game1()
//        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
//        }

//        protected override void Initialize()
//        {
//            // TODO: Add your initialization logic here
//            graphics.PreferredBackBufferWidth = 1500;
//            graphics.PreferredBackBufferHeight = 900;
//            graphics.IsFullScreen = false;
//            graphics.ApplyChanges();

//            base.Initialize();
//        }

//        protected override void LoadContent()
//        {
//            spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

//            background.texture = Content.Load<Texture2D>("background2");
//            kitten.texture = Content.Load<Texture2D>("kitten9");
//            wallTexture = Content.Load<Texture2D>("wall10");

//            Random rnd = new Random(); //рандомная генерация позиций Y стен
//            walls = new Wall[]
//            {
//                new Wall(new Rectangle(200, rnd.Next(3, 5)*100, 32, 192), wallTexture),
//                new Wall(new Rectangle(600, rnd.Next(2, 4)*100, 32, 192), wallTexture),
//                new Wall(new Rectangle(1000, rnd.Next(1, 3)*100, 32, 192), wallTexture),
//                new Wall(new Rectangle(1400, rnd.Next(1, 3)*100, 32, 192), wallTexture),
//                //new Wall(new Rectangle(1400, rnd.Next(2, 5)*100, 20, 128), wallTexture),
//            };
//            //walls = new Wall[] { 0 };
//            //walls.Append(new Wall(new Rectangle(600, rnd.Next(2, 4) * 100, 32, 192), wallTexture));


//            //Random rnd = new Random();
//            //walls = new Wall[]
//            //{
//            //    new Wall(new Rectangle(600, 200, 50, 300), wallTexture),
//            //    new Wall(new Rectangle(1000, 400, 50, 300), wallTexture),
//            //    new Wall(new Rectangle(0, 0, 50, 300), wallTexture),
//            //    new Wall(new Rectangle(300, 300, 50, 300), wallTexture),
//            //};

//            // TODO: use this.Content to load your game content here
//        }

//        protected override void Update(GameTime gameTime)
//        {
//            KeyboardState keyboardState = Keyboard.GetState();

//            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
//                Exit();

//            //дополнительное управление wasd
//            WASDMove(keyboardState);

//            //передвижение и повороты на стене
//            MovesOnWall(keyboardState);

//            //прыжок вправо
//            JumpRight(keyboardState);

//            //прыжок влево
//            JumpLeft(keyboardState);

//            //механика прыжка
//            Jumping();

//            //зацепиться за стену
//            JumpONWall();

//            base.Update(gameTime);
//        }

//        private void WASDMove(KeyboardState keyboardState)
//        {
//            if (keyboardState.IsKeyDown(Keys.W))
//            {
//                if (kitten.position.Y > 0)
//                {
//                    kitten.position.Y -= kitten.speed;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.S))
//            {
//                if (kitten.position.Y < background.size.Height - kitten.texture.Height)
//                {
//                    kitten.position.Y += kitten.speed;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.A))
//            {
//                if (kitten.position.X > 0)
//                {
//                    kitten.position.X -= kitten.speed;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.D))
//            {
//                if (kitten.position.X < background.size.Width - kitten.texture.Width)
//                {
//                    kitten.position.X += kitten.speed;
//                }
//            }
//        }

//        private void MovesOnWall(KeyboardState keyboardState)
//        {
//            foreach (var wall in walls)
//            {
//                if (kitten.position.X == wall.Rectangle.X - kitten.texture.Width ||
//                    kitten.position.X == wall.Rectangle.X + wall.Rectangle.Width)
//                {
//                    if (keyboardState.IsKeyDown(Keys.Up))
//                    {
//                        if (kitten.position.Y + kitten.texture.Height / 2 > wall.Rectangle.Y) // возможно деление на 2 высоты текстуры котенка
//                        {
//                            kitten.position.Y -= kitten.speed;
//                        }
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Down))
//                    {
//                        if (kitten.position.Y + kitten.texture.Height / 2 < wall.Rectangle.Y + wall.Rectangle.Height)
//                        {
//                            kitten.position.Y += kitten.speed;
//                        }
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Z))
//                    {
//                        kitten.position.X = wall.Rectangle.X - kitten.texture.Width;
//                    }

//                    if (keyboardState.IsKeyDown(Keys.X))
//                    {
//                        kitten.position.X = wall.Rectangle.X + wall.Rectangle.Width;
//                    }
//                }
//            }
//        }

//        private void JumpRight(KeyboardState keyboardState)
//        {
//            if (keyboardState.IsKeyDown(Keys.Right) && kitten.isGrounded)
//            {
//                kitten.isGrounded = false;
//                kitten.isJumping = true;
//                kitten.jumping = true;
//                kitten.jumpForce = 5f; // начальная скорость прыжка
//                kitten.direction = 10f;
//            }
//        }

//        private void JumpLeft(KeyboardState keyboardState)
//        {
//            if (keyboardState.IsKeyDown(Keys.Left) && kitten.isGrounded)
//            {
//                kitten.isGrounded = false;
//                kitten.isJumping = true;
//                kitten.jumping = true;
//                kitten.jumpForce = 5f; // начальная скорость прыжка
//                kitten.direction = -10f;
//            }
//        }

//        private void Jumping()
//        {
//            if (kitten.isJumping)
//            {
//                // Изменяем положение на основе вертикальной и гризонтальной скорости и гравитации
//                kitten.position.Y -= (kitten.jumpForce * 5f) - (kitten.gravity * 1f);
//                kitten.position.X += kitten.direction;
//                kitten.jumpForce -= kitten.gravity;

//                // Если кошка достигла земли, остановите прыжок
//                if (kitten.position.Y >= background.size.Height - kitten.texture.Height)
//                {
//                    kitten.position.Y = background.size.Height - kitten.texture.Height;
//                    kitten.isJumping = false;
//                    kitten.jumping = false;
//                    kitten.isGrounded = true;
//                }
//            }
//        }

//        private void JumpONWall()
//        {
//            foreach (var wall in walls)
//            {
//                var wallX = wall.Rectangle.X;
//                var wallY = wall.Rectangle.Y;
//                var wallLeft = wallX;
//                var wallRight = wallX + wall.Rectangle.Width;
//                var wallTop = wallY;
//                var wallButtom = wallY + wall.Rectangle.Height;

//                var kittenLeft = kitten.position.X;
//                var kittenRight = kitten.position.X + kitten.texture.Width;
//                var kittenTop = kitten.position.Y;
//                var kittenButtom = kitten.position.Y + kitten.texture.Height;

//                CollisionWithWallLeft(wallLeft, wallTop, wallButtom, kittenLeft, kittenRight, kittenTop, kittenButtom);

//                CollisionWithWallRight(wallRight, wallTop, wallButtom, kittenLeft, kittenRight, kittenTop, kittenButtom);
//            }
//        }

//        // столкновение слева
//        private void CollisionWithWallLeft(int wallLeft, int wallTop, int wallButtom,
//            float kittenLeft, float kittenRight, float kittenTop, float kittenButtom)
//        {
//            if (kittenRight > wallLeft &&
//                kittenLeft < wallLeft &&
//                kittenButtom > wallTop &&
//                kittenTop < wallButtom)
//            {
//                //останавливаем прыжок
//                kitten.isJumping = false;
//                kitten.jumping = false;
//                kitten.isGrounded = true;

//                // новая позиция котенка
//                kitten.position.X = wallLeft - kitten.texture.Width;
//            }
//        }

//        // столкновение справа
//        private void CollisionWithWallRight(int wallRight, int wallTop, int wallButtom,
//            float kittenLeft, float kittenRight, float kittenTop, float kittenButtom)
//        {
//            if (kittenLeft < wallRight &&
//                kittenRight > wallRight &&
//                kittenButtom > wallTop &&
//                kittenTop < wallButtom)
//            {
//                //останавливаем прыжок
//                kitten.isJumping = false;
//                kitten.jumping = false;
//                kitten.isGrounded = true;

//                // новая позиция котенка
//                kitten.position.X = wallRight;
//            }
//        }

//        protected override void Draw(GameTime gameTime)
//        {
//            GraphicsDevice.Clear(Color.White);

//            spriteBatch.Begin();

//            spriteBatch.Draw(background.texture,   // фон
//                              background.size,
//                              Color.White);

//            spriteBatch.Draw(kitten.texture,
//                              kitten.position,  // котенок
//                              Color.White);

//            foreach (var wall in walls)         // стены
//            {
//                wall.DrawWall(spriteBatch);
//            }

//            spriteBatch.End();

//            base.Draw(gameTime);
//        }
//    }
//}