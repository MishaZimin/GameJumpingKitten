using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static JumpingKitten.Game1;
using static JumpingKitten.Wall;
using static JumpingKitten.Buff;
using static JumpingKitten.BuffLowSpeed;
using static JumpingKitten.BuffSave;
using static JumpingKitten.BuffSpeed;
using static JumpingKitten.Kitten;

namespace JumpingKitten
{
    public class State
    {
        public static GameState state = GameState.Menu;

        public enum GameState
        {
            Menu,
            Gameplay,
            EndOfGame,
            Pause
        }

        public static void Update(KeyboardState keyboardState)
        {
            switch (state)
            {
                case GameState.Menu:
                    UpdateMenu(keyboardState);
                    break;
                case GameState.Gameplay:
                    UpdateGameplay(keyboardState);
                    break;
                case GameState.Pause:
                    UpdatePause(keyboardState);
                    break;
                case GameState.EndOfGame:
                    UpdateEndOfGame(keyboardState);
                    break;
            }
        }
        public static void Draw()
        {
            switch (state)
            {
                case GameState.Menu:
                    DrawMenu();
                    break;
                case GameState.Gameplay:
                    DrawGameplay();
                    break;
                case GameState.Pause:
                    DrawPause();
                    break;
                case GameState.EndOfGame:
                    DrawGameOver();
                    break;
            }
        }

        public static void UpdateMenu(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Enter))
                state = GameState.Gameplay;
        }
        public static void DrawMenu()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundMenu.texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(playButton, new Vector2(550, 150), Color.White);
            spriteBatch.End();
        }

        public static void UpdateGameplay(KeyboardState keyboardState)
        {
            if (kitten.gameOver)
            {
                state = GameState.EndOfGame;
                kitten.gameOver = false;
            }

            if (keyboardState.IsKeyDown(Keys.RightShift))
                state = GameState.Pause;
        }

        public static void UpdatePause(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Enter))
                state = GameState.Gameplay;
        }

        public static void UpdateEndOfGame(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                ResetLevel();
                state = GameState.Gameplay;
            }
        }

        public static void DrawPause()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundPause.texture, Vector2.Zero, Color.White);
            spriteBatch.DrawString(scoreFont, "Best score: " + kitten.bestScore, new Vector2(1070, 110F), Color.White);
            spriteBatch.End();
        }

        public static void DrawGameplay()
        {
            // Отрисовка игрs, счета  
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundGame.texture, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: camera.TransformMatrix);

            camera.UpdateMatrix();

            DrawWalls(spriteBatch, Walls);

            DrawBuff(spriteBatch, BuffSpeed.buffsSpeed);
            DrawBuff(spriteBatch, BuffLowSpeed.buffsLowSpeed);
            DrawBuff(spriteBatch, BuffSave.buffsSave);

            kitten.Draw(spriteBatch);

            //camera.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            spriteBatch.DrawString(scoreFont, "Score: " + kitten.score, new Vector2(50, 700), Color.White);
            spriteBatch.DrawString(scoreFont, "Best score: " + kitten.bestScore, new Vector2(50, 750), Color.White);

            //spriteBatch.Draw(playButton, new Vector2(1280, 680), Color.White);

            if (BuffSave.Timer != 0)
            {
                spriteBatch.Draw(buffSaveTexture, new Vector2(1300 + 20, 700), Color.White);
                spriteBatch.DrawString(scoreFont, "" + (BuffSave.Duration - (int)BuffSave.Timer) + " sec", new Vector2(1350 + 20, 700), Color.White);
            }
            if (BuffLowSpeed.Timer != 0)
            {
                spriteBatch.Draw(buffLowSpeedTexture, new Vector2(1305 + 20, 750), Color.White);
                spriteBatch.DrawString(scoreFont, "" + (BuffLowSpeed.Duration - (int)BuffLowSpeed.Timer) + " sec", new Vector2(1350 + 20, 750), Color.White);
            }
            if (BuffSpeed.Timer != 0)
            {
                spriteBatch.Draw(buffSpeedTexture, new Vector2(1305 + 20, 650), Color.White);
                //spriteBatch.DrawString(scoreFont, "" + kitten.speed, new Vector2(1350 + 20, 600), Color.White);
                spriteBatch.DrawString(scoreFont, "" + (BuffSpeed.Duration - (int)BuffSpeed.Timer) + " sec", new Vector2(1350 + 20, 650), Color.White);
            }

            spriteBatch.End();
        }

        public static void DrawGameOver()
        {
            // Отрисовка результатов, кнопок
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundEndGame.texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(gameOverButton, new Vector2(450, 100), Color.White);
            spriteBatch.Draw(scoreButton, new Vector2(450 - 30, 300), Color.White);

            spriteBatch.DrawString(scoreFont, "" + kitten.score, new Vector2(450 - 30 + 300, 450), Color.White);
            spriteBatch.DrawString(scoreFont, "" + kitten.bestScore, new Vector2(450 - 30 + 300, 650), Color.White);
            spriteBatch.End();
        }

        private static void ResetLevel()
        {
            // Удаление всех стен и баффов
            Walls.Clear();
            buffsLowSpeed.Clear();
            buffsSpeed.Clear();
            buffsSave.Clear();

            kitten.position = new Vector2(700 + 27, 500);          
            kitten.texture = kittenTextureUpRight;
            kitten.speed = 5f;
            kitten.score = 0;

            camera.position = kitten.position;

            BuffSave.IsActive = false;
            BuffSave.Timer = 0f;

            BuffLowSpeed.IsActive = false;
            BuffLowSpeed.Timer = 0f;

            BuffSpeed.IsActive = false;
            BuffSpeed.Timer = 0f;

            // генерация стен
            GenerationWalls();
        }
    }
}
