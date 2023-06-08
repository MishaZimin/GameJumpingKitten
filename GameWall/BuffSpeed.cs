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






//using Microsoft.Xna.Framework;

//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using System;
//using System.Collections.Generic;

//namespace GameWall
//{
//    public partial class Game1 : Game
//    {
//        private readonly GraphicsDeviceManager graphics;
//        private SpriteBatch spriteBatch;

//        readonly Background backgroundGame = new();
//        readonly Background backgroundMenu = new();
//        readonly Background backgroundEndGame = new();
//        readonly Background backgroundPause = new();

//        private SpriteFont scoreFont;

//        private Kitten kitten;
//        private Texture2D kittenTexture;
//        private Texture2D kittenTextureStaticRight;
//        private Texture2D kittenTextureStaticLeft;
//        private Texture2D kittenTextureUpRight;
//        private Texture2D kittenTextureUpLeft;
//        private Texture2D kittenTextureDownRight;
//        private Texture2D kittenTextureDownLeft;

//        private Owl owl;
//        private Texture2D owlTexture;


//        private readonly List<Wall> walls = new();
//        private Texture2D wallTexture;
//        private Texture2D badWallTexture;

//        private Texture2D BigSpikeLeftTexture;
//        private Texture2D SmallSpikeLeftTexture;
//        private Texture2D BigSpikeRightTexture;
//        private Texture2D SmallSpikeRightTexture;
//        private Texture2D cameraTexture;

//        private Texture2D playButton;
//        private Texture2D scoreButton;

//        private Texture2D gameOverButton;

//        private readonly List<Buff> buffsLowSpeed = new();
//        private readonly List<Buff> buffsSave = new();
//        private readonly List<Buff> owls = new();

//        private Texture2D buffMilkTexture;
//        private Texture2D buffSaveTexture;



//        private readonly Random rnd = new();

//        public Camera camera;

//        private Song backgroundMusic;

//        enum GameState
//        {
//            Menu,
//            Gameplay,
//            EndOfGame,
//            Pause
//        }

//        private GameState state = GameState.Menu;

//        private bool playerDied;

//        public Game1()
//        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
//            Window.Title = "Kitten";
//        }

//        protected override void Initialize()
//        {
//            graphics.PreferredBackBufferWidth = 1500;
//            graphics.PreferredBackBufferHeight = 840;

//            graphics.ApplyChanges();

//            base.Initialize();
//        }

//        protected override void LoadContent()
//        {
//            spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

//            backgroundGame.texture = Content.Load<Texture2D>("treeBackground5");
//            backgroundEndGame.texture = Content.Load<Texture2D>("treeBackground5");
//            backgroundMenu.texture = Content.Load<Texture2D>("treeBackground5");
//            backgroundPause.texture = Content.Load<Texture2D>("treeBackground5");

//            playButton = Content.Load<Texture2D>("playButton4");
//            gameOverButton = Content.Load<Texture2D>("gameover3");
//            scoreButton = Content.Load<Texture2D>("scoreAndBestScore1");

//            cameraTexture = Content.Load<Texture2D>("camera1");

//            kittenTextureStaticLeft = Content.Load<Texture2D>("kitten14StaticLeft");
//            kittenTextureStaticRight = Content.Load<Texture2D>("kitten14StaticRight");
//            kittenTextureUpLeft = Content.Load<Texture2D>("kitten14UpLeft");
//            kittenTextureDownLeft = Content.Load<Texture2D>("kitten14DownLeft");
//            kittenTextureUpRight = Content.Load<Texture2D>("kitten14UpRight");
//            kittenTextureDownRight = Content.Load<Texture2D>("kitten14DownRight");



//            //owlTexture = Content.Load<Texture2D>("kitten10");

//            wallTexture = Content.Load<Texture2D>("wall13");
//            badWallTexture = Content.Load<Texture2D>("wall12");

//            BigSpikeLeftTexture = Content.Load<Texture2D>("BigSpikeLeft");
//            SmallSpikeLeftTexture = Content.Load<Texture2D>("SmallSpikeLeft");
//            BigSpikeRightTexture = Content.Load<Texture2D>("BigSpikeRight");
//            SmallSpikeRightTexture = Content.Load<Texture2D>("SmallSpikeRight");

//            buffMilkTexture = Content.Load<Texture2D>("buffMilk");
//            buffSaveTexture = Content.Load<Texture2D>("buffSave");

//            scoreFont = Content.Load<SpriteFont>("score4");

//            // music
//            backgroundMusic = Content.Load<Song>("music1");
//            MediaPlayer.Volume = 0.1f;
//            MediaPlayer.IsRepeating = true;
//            MediaPlayer.Play(backgroundMusic);

//            graphics.IsFullScreen = false;

//            //kitten.texture = kittenTextureUpRight;

//            kitten = new Kitten(new(700 + 27, 500f), kittenTextureUpRight);
//            camera = new Camera(new(700, 500f), cameraTexture);

//            //генерация стен
//            GenerationWalls();
//        }

//        protected override void Update(GameTime gameTime)
//        {
//            KeyboardState keyboardState = Keyboard.GetState();

//            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
//                Exit();

//            UpdateState(keyboardState);

//            UpdateWalls(keyboardState);

//            UpdateBuffs(gameTime);

//            CheckJumping();

//            UpdateCamera();

//            UpdateScore();

//            base.Update(gameTime);
//        }

//        protected override void Draw(GameTime gameTime)
//        {
//            base.Draw(gameTime);

//            GraphicsDevice.Clear(Color.Black);

//            DrawState();

//            void DrawMenu()
//            {
//                spriteBatch.Begin();
//                spriteBatch.Draw(backgroundMenu.texture, Vector2.Zero, Color.White);
//                spriteBatch.Draw(playButton, new Vector2(550, 150), Color.White);
//                spriteBatch.End();
//            }

//            void DrawPause()
//            {
//                spriteBatch.Begin();
//                spriteBatch.Draw(backgroundPause.texture, Vector2.Zero, Color.White);
//                spriteBatch.DrawString(scoreFont, "Best score: " + kitten.bestScore, new Vector2(1070, 110F), Color.White);
//                spriteBatch.End();
//            }

//            void DrawGameplay()
//            {
//                // Отрисовка игрs, счета  
//                spriteBatch.Begin();
//                spriteBatch.Draw(backgroundGame.texture, Vector2.Zero, Color.White);
//                spriteBatch.End();

//                spriteBatch.Begin(transformMatrix: camera.TransformMatrix);

//                camera.Update();

//                foreach (var wall in walls)
//                {
//                    wall.Draw(spriteBatch);
//                    wall.spike.Draw(spriteBatch);
//                }

//                foreach (var buff in buffsLowSpeed)
//                {
//                    buff.Draw(spriteBatch);
//                }

//                foreach (var buff in buffsSave)
//                {
//                    buff.Draw(spriteBatch);
//                }

//                kitten.Draw(spriteBatch);

//                camera.Draw(spriteBatch);

//                spriteBatch.End();

//                spriteBatch.Begin();

//                spriteBatch.DrawString(scoreFont, "Score: " + kitten.score, new Vector2(50, 700), Color.White);
//                spriteBatch.DrawString(scoreFont, "Best score: " + kitten.bestScore, new Vector2(50, 750), Color.White);

//                //spriteBatch.Draw(playButton, new Vector2(1280, 680), Color.White);

//                if (BuffSave.Timer != 0)
//                {
//                    spriteBatch.Draw(buffSaveTexture, new Vector2(1300 + 20, 700), Color.White);
//                    spriteBatch.DrawString(scoreFont, "" + (BuffSave.Duration - (int)BuffSave.Timer) + " sec", new Vector2(1350 + 20, 700), Color.White);
//                }
//                if (BuffLowSpeed.Timer != 0)
//                {
//                    spriteBatch.Draw(buffMilkTexture, new Vector2(1305 + 20, 750), Color.White);
//                    spriteBatch.DrawString(scoreFont, "" + (BuffLowSpeed.Duration - (int)BuffLowSpeed.Timer) + " sec", new Vector2(1350 + 20, 750), Color.White);
//                }

//                spriteBatch.End();
//            }

//            void DrawGameOver()
//            {
//                // Отрисовка результатов, кнопок
//                spriteBatch.Begin();
//                spriteBatch.Draw(backgroundEndGame.texture, Vector2.Zero, Color.White);
//                spriteBatch.Draw(gameOverButton, new Vector2(450, 100), Color.White);
//                spriteBatch.Draw(scoreButton, new Vector2(450 - 30, 300), Color.White);

//                spriteBatch.DrawString(scoreFont, "" + kitten.score, new Vector2(450 - 30 + 300, 450), Color.White);
//                spriteBatch.DrawString(scoreFont, "" + kitten.bestScore, new Vector2(450 - 30 + 300, 650), Color.White);
//                spriteBatch.End();
//            }

//            void DrawState()
//            {
//                switch (state)
//                {
//                    case GameState.Menu:
//                        DrawMenu();
//                        break;
//                    case GameState.Gameplay:
//                        DrawGameplay();
//                        break;
//                    case GameState.Pause:
//                        DrawPause();
//                        break;
//                    case GameState.EndOfGame:
//                        DrawGameOver();
//                        break;
//                }
//            }
//        }

//        private void UpdateCamera()
//        {
//            if (kitten.LeftZ && camera.position.X > kitten.position.X)
//            {
//                for (int i = 0; i < 20; i++)
//                    camera.position.X -= 0.5f;
//                kitten.LeftZ = false;
//            }

//            else if (kitten.RightX && camera.position.X < kitten.position.X)
//            {
//                for (int i = 0; i < 20; i++)
//                    camera.position.X += 0.5f;
//                kitten.RightX = false;
//            }

//            else
//            {
//                camera.position = kitten.position;
//            }
//        }

//        private void UpdateScore()
//        {
//            if (kitten.score <= (int)camera.position.Y * -1 / 100 + 5)
//            {
//                kitten.score = (int)camera.position.Y * -1 / 100 + 5;
//            }

//            if (kitten.score > kitten.bestScore)
//            {
//                kitten.bestScore = kitten.score;
//            }
//        }

//        private void UpdateBuffs(GameTime gameTime)
//        {
//            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

//            UpdateBuffLowSpeed(deltaTime);
//            UpdateBuffSave(deltaTime);
//        }

//        private void UpdateBuffSave(float deltaTime)
//        {
//            for (var i = 0; i < buffsSave.Count; i++)
//            {
//                var buff = buffsSave[i];

//                if (kitten.IsTouchObjects(kitten, buff))
//                {
//                    if (BuffSave.IsActive)
//                    {
//                        BuffSave.Timer = BuffSave.Duration;
//                    }
//                    else
//                    {
//                        BuffSave.IsActive = true;

//                        for (int j = 0; j <= i; j++)
//                            buffsSave.RemoveAt(0);

//                        BuffSave.Flag = true;
//                        break;
//                    }
//                }
//            }

//            if (BuffSave.IsActive)
//            {
//                BuffSave.Timer += deltaTime;

//                if (BuffSave.Timer >= BuffSave.Duration)
//                {
//                    // действия после конца баффа
//                    BuffSave.IsActive = false;
//                    BuffSave.Timer = 0f;
//                    BuffSave.Flag = false;
//                }
//            }
//        }

//        private void UpdateBuffLowSpeed(float deltaTime)
//        {
//            for (var i = 0; i < buffsLowSpeed.Count; i++)
//            {
//                var buff = buffsLowSpeed[i];

//                if (kitten.IsTouchObjects(kitten, buff))
//                {
//                    if (BuffLowSpeed.IsActive)
//                    {
//                        BuffLowSpeed.Timer = BuffLowSpeed.Duration;
//                    }

//                    else
//                    {
//                        BuffLowSpeed.IsActive = true;
//                        BuffLowSpeed.Flag = false;

//                        for (int j = 0; j <= i; j++)
//                            buffsLowSpeed.RemoveAt(0);

//                        for (var j = 0; j < walls.Count; j++)
//                        {
//                            var wall = walls[j];
//                            wall.spike.speed /= 2; // замедление шипов
//                            wall.speedFlag = true;
//                        }

//                        break;
//                    }
//                }

//            }

//            if (BuffLowSpeed.IsActive) // бафф активен
//            {
//                BuffLowSpeed.Timer += deltaTime;

//                if (BuffLowSpeed.Timer >= BuffLowSpeed.Duration)
//                {
//                    // действия после конца баффа
//                    BuffLowSpeed.IsActive = false;
//                    BuffLowSpeed.Timer = 0f;

//                    for (var j = 0; j < walls.Count; j++)
//                    {
//                        var wall = walls[j];

//                        if (wall.speedFlag == true)
//                        {
//                            wall.spike.speed *= 2; //ускорение замедленных шипов
//                            wall.speedFlag = false;
//                        }
//                        else
//                        {
//                            break;
//                        }
//                    }
//                }
//            }
//        }

//        private void UpdateWalls(KeyboardState keyboardState)
//        {
//            for (var i = 0; i < walls.Count; i++)
//            {
//                var wall = walls[i];

//                if (walls[0].position.Y - kitten.position.Y > 1000)
//                {
//                    AddNewWall();
//                    AddNewBuffs();

//                    walls.RemoveAt(0);
//                }

//                // касание шипов
//                TouchSpike(wall);

//                // движение шипов
//                GoSpikeDownAndUp(wall);

//                //передвижение, повороты и прыжки на стене
//                MovesOnWall(keyboardState, wall);

//                //зацепиться за стену
//                CollisionWithWall(wall);
//            }

//        }

//        private void AddNewBuffLowSpeed()
//        {
//            int rundomNumberBuffLowSpeed = rnd.Next(1, 2); //шанс появления        

//            if (rundomNumberBuffLowSpeed == 1)
//            {
//                float rndLowSpeedPosition = rnd.Next(1, 3);
//                float buffLowSpeedPositionY = rnd.Next(0, 150);

//                if (rndLowSpeedPosition == 1) // бафф справа
//                {
//                    buffsLowSpeed.Add(new BuffLowSpeed(new(walls[^1].position.X + walls[^1].texture.Width + 15,
//                                               walls[^1].position.Y + buffLowSpeedPositionY),
//                                               buffMilkTexture));
//                }

//                if (rndLowSpeedPosition == 2) // бафф слева
//                {
//                    buffsLowSpeed.Add(new BuffLowSpeed(new(walls[^1].position.X - 50,
//                                               walls[^1].position.Y + buffLowSpeedPositionY),
//                                               buffMilkTexture));
//                }
//            }
//        }

//        private void AddNewBuffSave()
//        {
//            int rundomNumberBuffSave = rnd.Next(1, 2); //шанс появления

//            if (rundomNumberBuffSave == 1)
//            {
//                float rndSavePosition = rnd.Next(1, 3);
//                float buffSavePositionY = rnd.Next(0, 100);

//                if (rndSavePosition == 1)
//                    buffsSave.Add(new BuffSave(new(walls[^1].position.X + walls[^1].texture.Width + 150, walls[^1].position.Y + buffSavePositionY), buffSaveTexture));
//                else if (rndSavePosition == 2)
//                    buffsSave.Add(new BuffSave(new(walls[^1].position.X - 135, walls[^1].position.Y + buffSavePositionY), buffSaveTexture));
//            }
//        }

//        private void AddNewWall()
//        {
//            int numberWallPositin = rnd.Next(1, 4);

//            float changeWallX = 0;
//            float changeWallY = 0;

//            if (numberWallPositin == 1)
//            {
//                changeWallX = 0;
//                changeWallY = -200;
//            }

//            if (numberWallPositin == 2)
//            {
//                changeWallX = 300;
//                changeWallY = -85;
//            }

//            if (numberWallPositin == 3)
//            {
//                changeWallX = -300;
//                changeWallY = -85;
//            }

//            if (numberWallPositin == 4)
//            {
//                changeWallX = 0;
//                changeWallY = 0;
//            }

//            float wallPositionX = (int)walls[^1].position.X + changeWallX;
//            float wallPositionY = (int)walls[^1].position.Y - wallTexture.Height + changeWallY;

//            float spikePositionY = rnd.Next(1, 10) * 10;

//            float randomSpeedSpike = rnd.Next(3, 6);

//            var newWallTextur = wallTexture;
//            var newSpikeTexture = SmallSpikeLeftTexture;
//            float spikePositionX = 0;

//            float numberSpike = rnd.Next(1, 5);

//            if (numberSpike == 1)
//            {
//                newWallTextur = wallTexture;
//                newSpikeTexture = SmallSpikeLeftTexture;
//                spikePositionX = wallPositionX - SmallSpikeLeftTexture.Width + 2;
//            }

//            if (numberSpike == 2)
//            {
//                newWallTextur = badWallTexture;
//                newSpikeTexture = BigSpikeLeftTexture;
//                spikePositionX = wallPositionX - BigSpikeLeftTexture.Width + 2;
//            }

//            if (numberSpike == 3)
//            {
//                newWallTextur = wallTexture;
//                newSpikeTexture = SmallSpikeRightTexture;
//                spikePositionX = wallPositionX + wallTexture.Width - 2;
//            }

//            if (numberSpike == 4)
//            {
//                newWallTextur = wallTexture;
//                newSpikeTexture = BigSpikeRightTexture;
//                spikePositionX = wallPositionX + wallTexture.Width - 2;
//            }

//            walls.Add(new Wall(new(wallPositionX, wallPositionY),
//                               newWallTextur,
//                               new Spike(new(spikePositionX, wallPositionY + spikePositionY), newSpikeTexture, randomSpeedSpike)));
//        }

//        private void AddNewBuffs()
//        {
//            AddNewBuffLowSpeed();
//            AddNewBuffSave();
//        }

//        void UpdateState(KeyboardState keyboardState)
//        {
//            switch (state)
//            {
//                case GameState.Menu:
//                    UpdateMenu(keyboardState);
//                    break;
//                case GameState.Gameplay:
//                    UpdateGameplay(keyboardState);
//                    break;
//                case GameState.Pause:
//                    UpdatePause(keyboardState);
//                    break;
//                case GameState.EndOfGame:
//                    UpdateEndOfGame(keyboardState);
//                    break;
//            }
//        }

//        void UpdateMenu(KeyboardState keyboardState)
//        {
//            if (keyboardState.IsKeyDown(Keys.Enter))
//                state = GameState.Gameplay;
//        }

//        void UpdateGameplay(KeyboardState keyboardState)
//        {
//            if (playerDied)
//            {
//                state = GameState.EndOfGame;
//                playerDied = false;
//            }

//            if (keyboardState.IsKeyDown(Keys.RightShift))
//                state = GameState.Pause;
//        }

//        void UpdatePause(KeyboardState keyboardState)
//        {
//            if (keyboardState.IsKeyDown(Keys.Enter))
//                state = GameState.Gameplay;
//        }

//        void UpdateEndOfGame(KeyboardState keyboardState)
//        {
//            if (keyboardState.IsKeyDown(Keys.Enter))
//            {
//                ResetLevel();
//                state = GameState.Gameplay;
//            }
//        }

//        private void ResetLevel()
//        {
//            kitten.position = new Vector2(700 + 27, 500);
//            camera.position = kitten.position;
//            kitten.texture = kittenTextureUpRight;

//            // Удаление всех стен и баффов
//            walls.Clear();
//            buffsLowSpeed.Clear();
//            buffsSave.Clear();

//            // генерация стен
//            GenerationWalls();
//        }

//        private void GenerationWalls()
//        {
//            walls.Add(new Wall(new Vector2(700, 500), wallTexture, new Spike(new Vector2(-140, 4000), BigSpikeLeftTexture, 0)));

//            for (int i = 1; i < 10; i++)
//            {
//                AddNewWall();
//                AddNewBuffs();
//            }
//        }

//        private static void GoSpikeDownAndUp(Wall wall)
//        {
//            wall.spike.position = new Vector2(wall.spike.position.X, wall.spike.position.Y + wall.spike.speed);

//            if (wall.spike.position.Y + wall.spike.texture.Height + wall.spike.speed > wall.position.Y + wall.texture.Height ||
//                wall.spike.position.Y + wall.spike.speed < wall.position.Y)
//            {
//                wall.spike.speed *= -1;
//            }
//        }

//        private void TouchSpike(Wall wall)
//        {
//            if (kitten.IsTouchObjects(kitten, wall.spike)) //конец игры
//            {
//                if (BuffSave.Flag == true)
//                {
//                    BuffSave.Flag = false;

//                    wall.spike.position.X = 10000;
//                    BuffSave.Timer = BuffSave.Duration;
//                }
//                else
//                {
//                    GameOver();
//                }
//            }
//        }

//        //protected static bool IsTouchObjects(Object object1, Object object2)
//        //{
//        //    Rectangle Rectangle1 = new((int)object1.position.X,
//        //                               (int)object1.position.Y,
//        //                               object1.texture.Width,
//        //                               object1.texture.Height);

//        //    Rectangle Rectangle2 = new((int)object2.position.X,
//        //                             (int)object2.position.Y,
//        //                             object2.texture.Width,
//        //                             object2.texture.Height);

//        //    return Rectangle1.Intersects(Rectangle2);
//        //}

//        private void MovesOnWall(KeyboardState keyboardState, Wall wall)
//        {
//            if (IsKittenOnWall(wall)) // 
//            {
//                if (keyboardState.IsKeyDown(Keys.Up) &&
//                    kitten.position.Y + kitten.texture.Height - 10 > wall.position.Y)
//                {
//                    kitten.position.Y -= kitten.speed;
//                    if (kitten.texture == kittenTextureDownLeft)
//                        kitten.texture = kittenTextureUpLeft;
//                    else if(kitten.texture == kittenTextureDownRight)
//                        kitten.texture = kittenTextureUpRight;
//                }

//                if (keyboardState.IsKeyDown(Keys.Down) &&
//                    kitten.position.Y + 10 < wall.position.Y + wall.texture.Height)
//                {
//                    kitten.position.Y += kitten.speed;
//                    if (kitten.texture == kittenTextureUpLeft)
//                        kitten.texture = kittenTextureDownLeft;
//                    else if (kitten.texture == kittenTextureUpRight)
//                        kitten.texture = kittenTextureDownRight;
//                }

//                if (keyboardState.IsKeyDown(Keys.Z))
//                {
//                    kitten.position.X = wall.position.X - kitten.texture.Width;

//                    kitten.ScoreFlag = false;
//                    kitten.LeftJump = false;
//                    kitten.LeftZ = true;

//                    if (kitten.texture == kittenTextureDownRight)
//                        kitten.texture = kittenTextureDownLeft;
//                    else if (kitten.texture == kittenTextureUpRight)
//                        kitten.texture = kittenTextureUpLeft;
//                }

//                if (keyboardState.IsKeyDown(Keys.X))
//                {
//                    kitten.position.X = wall.position.X + wall.texture.Width;

//                    kitten.ScoreFlag = true;
//                    kitten.RightJump = false;
//                    kitten.RightX = true;

//                    if (kitten.texture == kittenTextureDownLeft)
//                        kitten.texture = kittenTextureDownRight;
//                    else if (kitten.texture == kittenTextureUpLeft)
//                        kitten.texture = kittenTextureUpRight;
//                }
//            }

//            if (keyboardState.IsKeyDown(Keys.Right) &&
//               (kitten.LeftJump == true || kitten.isWall == true))
//            {
//                if (kitten.isWall == true)
//                {
//                    kitten.RightJump = true;
//                }

//                if (kitten.LeftJump == true)
//                {
//                    kitten.LeftJump = false;
//                }

//                kitten.texture = kittenTextureStaticRight;

//                StartJump();

//                kitten.jumpForce = 4f; // начальная скорость прыжка
//                kitten.direction = 10f;
//            }

//            if (keyboardState.IsKeyDown(Keys.Left) &&
//               (kitten.RightJump == true || kitten.isWall == true)) //прыжок влево
//            {

//                if (kitten.isWall == true)
//                {
//                    kitten.LeftJump = true;
//                }

//                if (kitten.RightJump == true)
//                {
//                    kitten.RightJump = false;
//                }

//                kitten.texture = kittenTextureStaticLeft;

//                StartJump();

//                kitten.jumpForce = 4f; // начальная скорость прыжка
//                kitten.direction = -10f;
//            }
//        }

//        private void StartJump()
//        {
//            kitten.isWall = false;
//            kitten.isJumping = true;
//            kitten.jumping = true;
//        }

//        private bool IsKittenOnWall(Wall wall)
//        {
//            return (kitten.position.X == wall.position.X - kitten.texture.Width ||
//                    kitten.position.X == wall.position.X + wall.texture.Width) &&
//                    (kitten.position.Y + kitten.texture.Height > wall.position.Y) &&
//                    kitten.position.Y < wall.position.Y + wall.texture.Height;
//        }

//        private void CheckJumping()
//        {
//            if (kitten.isJumping)
//            {
//                // прыжок
//                kitten.position.Y -= (kitten.jumpForce * 5f) + (kitten.gravity * 1f);
//                kitten.position.X += kitten.direction;
//                kitten.jumpForce -= kitten.gravity;

//                // падение
//                if (kitten.position.Y >= backgroundGame.size.Height - kitten.texture.Height)
//                {
//                    GameOver();
//                }
//            }
//        }

//        private void GameOver()
//        {
//            EndJump();

//            // установка рекорда
//            if (kitten.bestScore < kitten.score)
//            {
//                kitten.bestScore = kitten.score;
//            }

//            playerDied = true;
//            kitten.ScoreFlag = true;
//        }

//        private void CollisionWithWall(Wall wall)
//        {
//            if (CollisionWithWallLeft(wall))
//            {
//                EndJump();
//                kitten.texture = kittenTextureUpLeft;
//                // позиция котенка слева от стены
//                kitten.position.X = wall.position.X - kitten.texture.Width;
//            }

//            if (CollisionWithWallRight(wall))
//            {
//                EndJump();
//                kitten.texture = kittenTextureUpRight;
//                // позиция котенка справа от стены
//                kitten.position.X = wall.position.X + wall.texture.Width; ;
//            }
//        }

//        private void EndJump()
//        {
//            kitten.isJumping = false;
//            kitten.jumping = false;
//            kitten.isWall = true;
//        }

//        // столкновение слева
//        private bool CollisionWithWallLeft(Wall wall)
//        {
//            var wallLeft = wall.position.X;
//            var wallTop = wall.position.Y;
//            var wallButtom = wall.position.Y + wall.texture.Height;

//            var kittenLeft = kitten.position.X;
//            var kittenRight = kitten.position.X + kitten.texture.Width;
//            var kittenTop = kitten.position.Y;
//            var kittenButtom = kitten.position.Y + kitten.texture.Height;

//            return kittenRight > wallLeft &&
//                   kittenLeft < wallLeft &&
//                   kittenButtom >= wallTop &&
//                   kittenTop <= wallButtom;
//        }

//        // столкновение справа
//        private bool CollisionWithWallRight(Wall wall)
//        {
//            var wallRight = wall.position.X + wall.texture.Width;
//            var wallTop = wall.position.Y;
//            var wallButtom = wall.position.Y + wall.texture.Height;

//            var kittenLeft = kitten.position.X;
//            var kittenRight = kitten.position.X + kitten.texture.Width;
//            var kittenTop = kitten.position.Y;
//            var kittenButtom = kitten.position.Y + kitten.texture.Height;

//            return kittenLeft < wallRight &&
//                   kittenRight > wallRight &&
//                   kittenButtom >= wallTop &&
//                   kittenTop <= wallButtom;
//        }
//    }
//}
