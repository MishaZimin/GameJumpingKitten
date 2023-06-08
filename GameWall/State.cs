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
    public class State : Game
    {
        public static GameState state = GameState.Menu;

        public enum GameState
        {
            Menu,
            Gameplay,
            EndOfGame
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
            spriteBatch.Draw(playButton, new Vector2(550 + 220, 150 + 450), Color.White);
            spriteBatch.End();
        }

        public static void UpdateGameplay(KeyboardState keyboardState)
        {
            if (kitten.gameOver)
            {
                state = GameState.EndOfGame;
                kitten.gameOver = false;
            }
        }

        public static void UpdateEndOfGame(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                ResetLevel();
                state = GameState.Gameplay;
            }
        }

        public static void DrawGameplay()
        {
            // Отрисовка игрs, счетаб баффов  
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundGame.texture, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: camera.TransformMatrix);

            camera.UpdateMatrix();

            DrawWalls(spriteBatch, Walls);

            DrawBuff(spriteBatch, buffsSpeed);
            DrawBuff(spriteBatch, buffsLowSpeed);
            DrawBuff(spriteBatch, buffsSave);

            kitten.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            spriteBatch.DrawString(scoreFont, "Score: " + kitten.score, new Vector2(50, backgroundGame.texture.Height - 300), Color.White);
            spriteBatch.DrawString(scoreFont, "Best score: " + kitten.bestScore, new Vector2(50, backgroundGame.texture.Height - 250), Color.White);

            if (BuffSave.Timer != 0)
            {
                spriteBatch.Draw(buffSaveTexture, new Vector2(1750, 950), Color.White);
                spriteBatch.DrawString(scoreFont, "" + (BuffSave.Duration - (int)BuffSave.Timer) + " sec", new Vector2(1800, 950), Color.White);
            }
            if (BuffLowSpeed.Timer != 0)
            {
                spriteBatch.Draw(buffLowSpeedTexture, new Vector2(1755, 1000), Color.White);
                spriteBatch.DrawString(scoreFont, "" + (BuffLowSpeed.Duration - (int)BuffLowSpeed.Timer) + " sec", new Vector2(1800, 1000), Color.White);
            }
            if (BuffSpeed.Timer != 0)
            {
                spriteBatch.Draw(buffSpeedTexture, new Vector2(1755, 900), Color.White);
                spriteBatch.DrawString(scoreFont, "" + (BuffSpeed.Duration - (int)BuffSpeed.Timer) + " sec", new Vector2(1800, 900), Color.White);
            }

            spriteBatch.End();
        }

        public static void DrawGameOver()
        {
            // Отрисовка результатов, кнопок
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundEndGame.texture, Vector2.Zero, Color.White);
            spriteBatch.Draw(gameOverButton, new Vector2(685, 200), Color.White);
            spriteBatch.Draw(scoreButton, new Vector2(650, 400), Color.White);

            spriteBatch.DrawString(scoreFont, "" + kitten.score, new Vector2(950, 550), Color.White);
            spriteBatch.DrawString(scoreFont, "" + kitten.bestScore, new Vector2(950, 750), Color.White);
            spriteBatch.End();
        }

        private static void ResetLevel()
        {
            // Удаление всех стен и баффов
            Walls.Clear();
            buffsLowSpeed.Clear();
            buffsSpeed.Clear();
            buffsSave.Clear();

            kitten.position = new Vector2(900 + 27, 600);          
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
