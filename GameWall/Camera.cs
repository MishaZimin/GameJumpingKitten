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
                TransformMatrix = Matrix.CreateTranslation(new Vector3(-position.X + 700, -position.Y + 500, 0));
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













//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using System.Collections.Generic;

//namespace GameWall
//{
//    public class Game1 : Game
//    {
//        private GraphicsDeviceManager graphics;
//        private Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;

//        readonly Background background = new();

//        private Kitten kitten;
//        private Texture2D kittenTexture;

//        private List<Wall> walls;
//        private Texture2D wallTexture;

//        private Texture2D spikeTexture;

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
//            kittenTexture = Content.Load<Texture2D>("kitten9");
//            wallTexture = Content.Load<Texture2D>("wall11");
//            spikeTexture = Content.Load<Texture2D>("spike5");

//            kitten = new Kitten(new(32f, 400f), kittenTexture);

//            walls = new List<Wall>();
//            //spikes = new List<Spike>();

//            var rnd = new Random(); //рандомная генерация позиций Y стен

//            walls.Add(new Wall(new(50, 400), wallTexture, new Spike(new(40, 400), spikeTexture)));

//            for (int i = 1; i < 4; i++)
//            {
//                int wallPositionX = i * 400;
//                int wallPositionY = rnd.Next(1, 6) * 100;
//                int spikePositionY = rnd.Next(1, 18) * 10;

//                walls.Add(new Wall(new(wallPositionX, wallPositionY), 
//                                   wallTexture,
//                                   new Spike(new(wallPositionX - spikeTexture.Width + 2, wallPositionY + spikePositionY),
//                                   spikeTexture)));
//            }           
//        }


//        protected override void Update(GameTime gameTime)
//        {
//            KeyboardState keyboardState = Keyboard.GetState();

//            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
//                Exit();

//            for(var i = 0; i < 4; i++)
//            {
//                var wall = walls[i];

//                wall.position = new Vector2(wall.position.X, wall.position.Y + wall.speed);

//                wall.spike.position = new Vector2(wall.spike.position.X, wall.spike.position.Y + wall.spike.speed + wall.speed);

//                if (wall.spike.position.Y + wall.spike.texture.Height >= wall.position.Y + wall.texture.Height ||
//                    wall.spike.position.Y <= wall.position.Y)
//                {
//                    wall.spike.speed *= -1;
//                }

//                if (wall.position.Y + wall.texture.Height >= 900 ||
//                    wall.position.Y <= 0)
//                {
//                    wall.speed *= -1;
//                }

//            }



//            //дополнительное управление wasd
//            WASDMove(keyboardState);

//            //передвижение и повороты на стене
//            MovesOnWall(keyboardState);

//            //механика прыжка
//            Jumping();

//            //зацепиться за стену
//            JumpOnWall();

//            CollisionWithSpike();

//            base.Update(gameTime);
//        }

//        private void WASDMove(KeyboardState keyboardState)
//        {
//            if (keyboardState.IsKeyDown(Keys.W))
//            {
//                if (kitten.position.Y > 0)
//                {
//                    kitten.position.Y -= kitten.speed * 2;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.S))
//            {
//                if (kitten.position.Y < background.size.Height - kitten.Rectangle.Height)
//                {
//                    kitten.position.Y += kitten.speed * 2;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.A))
//            {
//                if (kitten.position.X > 0)
//                {
//                    kitten.position.X -= kitten.speed * 2;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.D))
//            {
//                if (kitten.position.X < background.size.Width - kitten.Rectangle.Width)
//                {
//                    kitten.position.X += kitten.speed * 2;
//                }
//            }
//        }

//        private void MovesOnWall(KeyboardState keyboardState)
//        {
//            foreach (var wall in walls)
//            {
//                if (kitten.position.X == wall.position.X - kitten.texture.Width ||
//                    kitten.position.X == wall.position.X + wall.texture.Width)
//                {
//                    if (keyboardState.IsKeyDown(Keys.Up))
//                    {
//                        if (kitten.position.Y + kitten.texture.Height > wall.position.Y) // возможно деление на 2 высоты текстуры котенка
//                        {
//                            kitten.position.Y -= kitten.speed;
//                        }
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Down))
//                    {
//                        if (kitten.position.Y < wall.position.Y + wall.texture.Height)
//                        {
//                            kitten.position.Y += kitten.speed;
//                        }
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Right) && kitten.isGrounded)
//                    {
//                        kitten.isGrounded = false;
//                        kitten.isJumping = true;
//                        kitten.jumping = true;
//                        kitten.jumpForce = 4.5f; // начальная скорость прыжка
//                        kitten.direction = 11f;
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Left) && kitten.isGrounded)
//                    {
//                        kitten.isGrounded = false;
//                        kitten.isJumping = true;
//                        kitten.jumping = true;
//                        kitten.jumpForce = 4.5f; // начальная скорость прыжка
//                        kitten.direction = -10f;
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Z))
//                    {
//                        kitten.position.X = wall.position.X - kitten.texture.Width;
//                    }

//                    if (keyboardState.IsKeyDown(Keys.X))
//                    {
//                        kitten.position.X = wall.position.X + wall.texture.Width;
//                    }
//                }
//            }
//        }

//        private void CollisionWithSpike()
//        {
//            foreach (var wall in walls)
//            {
//                var spikeX = wall.spike.position.X;
//                var spikeY = wall.spike.position.Y;
//                var spikeLeft = spikeX;
//                var spikeRight = spikeX + wall.spike.texture.Width;
//                var spikeTop = spikeY;
//                var spikeButtom = spikeY + wall.spike.texture.Height;

//                var kittenLeft = kitten.position.X;
//                var kittenRight = kitten.position.X + kitten.texture.Width;
//                var kittenTop = kitten.position.Y;
//                var kittenButtom = kitten.position.Y + kitten.texture.Height;

//                CollisionWithSpikeLeft(spikeLeft, spikeTop, spikeButtom, kittenLeft, kittenRight, kittenTop, kittenButtom);

//                CollisionWithSpikeRight(spikeRight, spikeTop, spikeButtom, kittenLeft, kittenRight, kittenTop, kittenButtom);
//            }
//        }

//        private void CollisionWithSpikeLeft(float spikeLeft, float spikeTop, float spikeButtom,
//            float kittenLeft, float kittenRight, float kittenTop, float kittenButtom)
//        {
//            if (kittenRight > spikeLeft &&
//                kittenLeft < spikeLeft &&
//                kittenButtom > spikeTop &&
//                kittenTop < spikeButtom)
//            {
//                //останавливаем прыжок
//                kitten.isJumping = false;
//                kitten.jumping = false;
//                kitten.isGrounded = true;

//                // новая позиция котенка
//                kitten.position.X = 32;

//                kitten.position.Y = 400;
//            }
//        }

//        // столкновение справа
//        private void CollisionWithSpikeRight(float spikeRight, float spikeTop, float spikeButtom,
//            float kittenLeft, float kittenRight, float kittenTop, float kittenButtom)
//        {
//            if (kittenLeft < spikeRight &&
//                kittenRight > spikeRight &&
//                kittenButtom > spikeTop &&
//                kittenTop < spikeButtom)
//            {
//                kitten.isJumping = false;
//                kitten.jumping = false;
//                kitten.isGrounded = true;

//                kitten.position.X = 32;
//                kitten.position.Y = 400;
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
//                    //kitten.position.Y = background.size.Height - kitten.texture.Height;
//                    kitten.position.X = 32;
//                    kitten.position.Y = 400;

//                    kitten.isJumping = false;
//                    kitten.jumping = false;
//                    kitten.isGrounded = true;
//                }
//            }
//        }

//        private void JumpOnWall()
//        {
//            foreach (var wall in walls)
//            {
//                var wallX = wall.position.X;
//                var wallY = wall.position.Y;
//                var wallLeft = wallX;
//                var wallRight = wallX + wall.texture.Width;
//                var wallTop = wallY;
//                var wallButtom = wallY + wall.texture.Height;

//                var kittenLeft = kitten.position.X;
//                var kittenRight = kitten.position.X + kitten.texture.Width;
//                var kittenTop = kitten.position.Y;
//                var kittenButtom = kitten.position.Y + kitten.texture.Height;

//                CollisionWithWallLeft(wallLeft, wallTop, wallButtom, kittenLeft, kittenRight, kittenTop, kittenButtom, wall);

//                CollisionWithWallRight(wallRight, wallTop, wallButtom, kittenLeft, kittenRight, kittenTop, kittenButtom, wall);

//                if (kitten.position.X == wallLeft - kitten.texture.Width)
//                {
//                    kitten.position.Y += wall.speed;
//                }

//            }
//        }

//        // столкновение слева
//        private void CollisionWithWallLeft(float wallLeft, float wallTop, float wallButtom,
//            float kittenLeft, float kittenRight, float kittenTop, float kittenButtom, Wall wall)
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
//        private void CollisionWithWallRight(float wallRight, float wallTop, float wallButtom,
//            float kittenLeft, float kittenRight, float kittenTop, float kittenButtom, Wall wall)
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

//            //spriteBatch.Draw(kitten.texture,
//            //                  kitten.position,  // котенок
//            //                  Color.White);

//            kitten.DrawKitten(spriteBatch);

//            foreach (var wall in walls)         // стены
//            {
//                wall.DrawWall(spriteBatch);
//                wall.spike.DrawSpike(spriteBatch);
//            }

//            //foreach (var spike in spikes)         // стены
//            //{
//            //    spike.DrawSpike(spriteBatch);
//            //}

//            spriteBatch.End();

//            base.Draw(gameTime);
//        }
//    }
//}



















//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using System.Collections.Generic;

//namespace GameWall
//{
//    public class Game1 : Game
//    {
//        private GraphicsDeviceManager graphics;
//        private Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;

//        readonly Background background = new();

//        private Kitten kitten;
//        private Texture2D kittenTexture;
//        private List<Wall> walls;
//        private Texture2D wallTexture;

//        private List<Spike> spikes;
//        private Texture2D spikeTexture;
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
//            kittenTexture = Content.Load<Texture2D>("kitten9");
//            wallTexture = Content.Load<Texture2D>("wall10");
//            spikeTexture = Content.Load<Texture2D>("spike3");

//            kitten = new Kitten(new(32f, 400f), kittenTexture);

//            walls = new List<Wall>();
//            spikes = new List<Spike>();

//            var rnd = new Random(); //рандомная генерация позиций Y стен

//            walls.Add(new Wall(new Rectangle(0, 400, 32, 192), wallTexture));
//            for (int i = 2; i <= 4; i++)
//            {
//                int positionX = i * 400 - 400;
//                int positionY = rnd.Next(2, 5) * 100;

//                walls.Add(new Wall(new Rectangle(positionX, positionY, 32, 192), wallTexture));
//                spikes.Add(new Spike(new Rectangle(positionX - 10, positionY + rnd.Next(12, 192 - 64 - 12), 12, 64), spikeTexture));
//            }
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

//            //механика прыжка
//            Jumping();

//            //зацепиться за стену
//            JumpONWall();

//            CollisionWithSpike();

//            base.Update(gameTime);
//        }

//        private void WASDMove(KeyboardState keyboardState)
//        {
//            if (keyboardState.IsKeyDown(Keys.W))
//            {
//                if (kitten.position.Y > 0)
//                {
//                    kitten.position.Y -= kitten.speed * 2;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.S))
//            {
//                if (kitten.position.Y < background.size.Height - kitten.Rectangle.Height)
//                {
//                    kitten.position.Y += kitten.speed * 2;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.A))
//            {
//                if (kitten.position.X > 0)
//                {
//                    kitten.position.X -= kitten.speed * 2;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.D))
//            {
//                if (kitten.position.X < background.size.Width - kitten.Rectangle.Width)
//                {
//                    kitten.position.X += kitten.speed * 2;
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
//                        if (kitten.position.Y + kitten.texture.Height > wall.Rectangle.Y) // возможно деление на 2 высоты текстуры котенка
//                        {
//                            kitten.position.Y -= kitten.speed;
//                        }
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Down))
//                    {
//                        if (kitten.position.Y < wall.Rectangle.Y + wall.Rectangle.Height)
//                        {
//                            kitten.position.Y += kitten.speed;
//                        }
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Right) && kitten.isGrounded)
//                    {
//                        kitten.isGrounded = false;
//                        kitten.isJumping = true;
//                        kitten.jumping = true;
//                        kitten.jumpForce = 4.5f; // начальная скорость прыжка
//                        kitten.direction = 11f;
//                    }

//                    if (keyboardState.IsKeyDown(Keys.Left) && kitten.isGrounded)
//                    {
//                        kitten.isGrounded = false;
//                        kitten.isJumping = true;
//                        kitten.jumping = true;
//                        kitten.jumpForce = 4.5f; // начальная скорость прыжка
//                        kitten.direction = -10f;
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

//        private void CollisionWithSpike()
//        {
//            foreach (var spike in spikes)
//            {
//                var spikeX = spike.Rectangle.X;
//                var spikeY = spike.Rectangle.Y;
//                var spikeLeft = spikeX;
//                var spikeRight = spikeX + spike.Rectangle.Width;
//                var spikeTop = spikeY;
//                var spikeButtom = spikeY + spike.Rectangle.Height;

//                var kittenLeft = kitten.position.X;
//                var kittenRight = kitten.position.X + kitten.texture.Width;
//                var kittenTop = kitten.position.Y;
//                var kittenButtom = kitten.position.Y + kitten.texture.Height;

//                CollisionWithSpikeLeft(spikeLeft, spikeTop, spikeButtom, kittenLeft, kittenRight, kittenTop, kittenButtom);

//                CollisionWithSpikeRight(spikeRight, spikeTop, spikeButtom, kittenLeft, kittenRight, kittenTop, kittenButtom);
//            }
//        }

//        private void CollisionWithSpikeLeft(int spikeLeft, int spikeTop, int spikeButtom,
//            float kittenLeft, float kittenRight, float kittenTop, float kittenButtom)
//        {
//            if (kittenRight > spikeLeft &&
//                kittenLeft < spikeLeft &&
//                kittenButtom > spikeTop &&
//                kittenTop < spikeButtom)
//            {
//                //останавливаем прыжок
//                kitten.isJumping = false;
//                kitten.jumping = false;
//                kitten.isGrounded = true;

//                // новая позиция котенка
//                kitten.position.X = 32;

//                kitten.position.Y = 400;
//            }
//        }

//        // столкновение справа
//        private void CollisionWithSpikeRight(int spikeRight, int spikeTop, int spikeButtom,
//            float kittenLeft, float kittenRight, float kittenTop, float kittenButtom)
//        {
//            if (kittenLeft < spikeRight &&
//                kittenRight > spikeRight &&
//                kittenButtom > spikeTop &&
//                kittenTop < spikeButtom)
//            {
//                kitten.isJumping = false;
//                kitten.jumping = false;
//                kitten.isGrounded = true;

//                kitten.position.X = 32;
//                kitten.position.Y = 400;
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
//                    //kitten.position.Y = background.size.Height - kitten.texture.Height;
//                    kitten.position.X = 32;
//                    kitten.position.Y = 400;

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

//            //spriteBatch.Draw(kitten.texture,
//            //                  kitten.position,  // котенок
//            //                  Color.White);

//            kitten.DrawKitten(spriteBatch);

//            foreach (var wall in walls)         // стены
//            {
//                wall.DrawWall(spriteBatch);
//            }

//            foreach (var spike in spikes)         // стены
//            {
//                spike.DrawSpike(spriteBatch);
//            }

//            spriteBatch.End();

//            base.Draw(gameTime);
//        }
//    }
//}






