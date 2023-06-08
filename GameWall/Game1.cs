using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using static JumpingKitten.Wall;
using static JumpingKitten.Kitten;

namespace JumpingKitten
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public partial class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static Background backgroundGame = new();
        public static Background backgroundMenu = new();
        public static Background backgroundEndGame = new();
        public static Background backgroundPause = new();

        public static Texture2D kittenTexture;
        public static Texture2D kittenTextureStaticRight;
        public static Texture2D kittenTextureStaticLeft;
        public static Texture2D kittenTextureUpRight;
        public static Texture2D kittenTextureUpLeft;
        public static Texture2D kittenTextureDownRight;
        public static Texture2D kittenTextureDownLeft;
        public static Texture2D wallTexture;
        public static Texture2D badWallTexture;

        public static Texture2D BigSpikeLeftTexture;
        public static Texture2D SmallSpikeLeftTexture;
        public static Texture2D BigSpikeRightTexture;
        public static Texture2D SmallSpikeRightTexture;
        public static Texture2D cameraTexture;

        public static Texture2D playButton;
        public static Texture2D scoreButton;
        public static Texture2D gameOverButton;

        public static Texture2D buffLowSpeedTexture;
        public static Texture2D buffSaveTexture;
        public static Texture2D buffSpeedTexture;

        public static Random rnd = new();

        public static Camera camera;

        public static SpriteFont scoreFont;

        private Song backgroundMusic;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "NameGame";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 840;

            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //текстуры фона
            backgroundGame.texture = Content.Load<Texture2D>("treeBackground5");
            backgroundEndGame.texture = Content.Load<Texture2D>("treeBackground5");
            backgroundMenu.texture = Content.Load<Texture2D>("treeBackground5");
            backgroundPause.texture = Content.Load<Texture2D>("treeBackground5");

            //текстуры кнопок
            playButton = Content.Load<Texture2D>("playButton4");
            gameOverButton = Content.Load<Texture2D>("gameover3");
            scoreButton = Content.Load<Texture2D>("scoreAndBestScore1");

            //текстуры котенка
            kittenTextureStaticLeft = Content.Load<Texture2D>("kitten14StaticLeft");
            kittenTextureStaticRight = Content.Load<Texture2D>("kitten14StaticRight");
            kittenTextureUpLeft = Content.Load<Texture2D>("kitten14UpLeft");
            kittenTextureDownLeft = Content.Load<Texture2D>("kitten14DownLeft");
            kittenTextureUpRight = Content.Load<Texture2D>("kitten14UpRight");
            kittenTextureDownRight = Content.Load<Texture2D>("kitten14DownRight");

            //текстуры стен
            wallTexture = Content.Load<Texture2D>("wall13");
            badWallTexture = Content.Load<Texture2D>("wall12");

            //текстуры шипов
            BigSpikeLeftTexture = Content.Load<Texture2D>("BigSpikeLeft");
            SmallSpikeLeftTexture = Content.Load<Texture2D>("SmallSpikeLeft");
            BigSpikeRightTexture = Content.Load<Texture2D>("BigSpikeRight");
            SmallSpikeRightTexture = Content.Load<Texture2D>("SmallSpikeRight");

            //текстуры баффов
            buffLowSpeedTexture = Content.Load<Texture2D>("buffMilk");
            buffSaveTexture = Content.Load<Texture2D>("buffSave");
            buffSpeedTexture = Content.Load<Texture2D>("buffSpeed2");

            //шрифт
            scoreFont = Content.Load<SpriteFont>("score4");

            // музыкв
            backgroundMusic = Content.Load<Song>("music1");
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

            graphics.IsFullScreen = false;

            kitten = new Kitten(new(700 + 27, 500f), kittenTextureUpRight, 5f);
            camera = new Camera(new(0, 0), cameraTexture);

            //генерация стен
            GenerationWalls();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            kitten.Update();
            camera.Update();
            State.Update(keyboardState);
            Wall.Update(keyboardState);
            Buff.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.Black);
            State.Draw();
        }
    }
}