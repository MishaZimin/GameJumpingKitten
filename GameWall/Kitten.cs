
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using static JumpingKitten.Game1;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace JumpingKitten
{
    public class Kitten : Sprite
    {
        public Rectangle Rectangle = new(0, 0, 60, 60);

        public int score = 0;
        public int bestScore = 0;
        public bool ScoreFlag = true;
        public bool LeftJump = false;
        public bool RightJump = false;
        public bool LeftZ = false;
        public bool RightX = false;

        public bool speedFlag;

        public bool LeftDubleJump = false;
        public bool RightDubleJump = false;
        public float maxKittenPositionY = 0;

        public float jumpForce = 4f;
        public float gravity = 0.3f;
        public float totalJumpTime = 0.5f;

        public bool jumping = false;
        public bool isJumping = false;
        public bool isWall = true;

        public float direction;
        public float jumpTime;
        public float startJumpHeight;
        public float endJumpHeight;
        public float jumpHeight;
        public float jumpPeakTime;
        public float jumpPeakHeight;

        public bool gameOver;

        public static Kitten kitten;

        public Kitten(Vector2 position, Texture2D texture, float speed)
        : base(texture, position, speed)
        {
            this.position = position;
            this.texture = texture;
            this.speed = speed;
        }

        public void EndJump()
        {
            isJumping = false;
            jumping = false;
            isWall = true;
        }

        public void StartJump()
        {
            isWall = false;
            isJumping = true;
            jumping = true;
        }

        public void CheckJumping()
        {
            if (isJumping)
            {
                // прыжок
                position.Y -= (jumpForce * 5f) + (gravity * 1f);
                position.X += direction;
                jumpForce -= gravity;

                // падение
                if (position.Y >= 840 - texture.Height)
                {
                    GameOver();
                }
            }
        }

        public void GameOver()
        {
            EndJump();

            // установка рекорда
            if (bestScore < score)
            {
                bestScore = score;
            }

            gameOver = true;
            ScoreFlag = true;
        }

        public void Update()
        {
            CheckJumping();
            UpdateScore();
        }

        public void MovesOnWall(KeyboardState keyboardState, Wall wall)
        {
            if (PresenceObject(kitten, wall)) // 
            {
                if (keyboardState.IsKeyDown(Keys.Up) &&
                    position.Y + texture.Height - speed > wall.position.Y)
                {
                    position.Y -= speed;
                    if (texture == kittenTextureDownLeft)
                        texture = kittenTextureUpLeft;
                    else if (texture == kittenTextureDownRight)
                        texture = kittenTextureUpRight;
                }

                if (keyboardState.IsKeyDown(Keys.Down) &&
                    position.Y + speed < wall.position.Y + wall.texture.Height)
                {
                    position.Y += speed;
                    if (texture == kittenTextureUpLeft)
                        texture = kittenTextureDownLeft;
                    else if (texture == kittenTextureUpRight)
                        texture = kittenTextureDownRight;
                }

                if (keyboardState.IsKeyDown(Keys.Z))
                {
                    position.X = wall.position.X - texture.Width;

                    ScoreFlag = false;
                    LeftJump = false;
                    LeftZ = true;

                    if (texture == kittenTextureDownRight)
                        texture = kittenTextureDownLeft;
                    else if (texture == kittenTextureUpRight)
                        texture = kittenTextureUpLeft;
                }

                if (keyboardState.IsKeyDown(Keys.X))
                {
                    position.X = wall.position.X + wall.texture.Width;

                    ScoreFlag = true;
                    RightJump = false;
                    RightX = true;

                    if (texture == kittenTextureDownLeft)
                        texture = kittenTextureDownRight;
                    else if (texture == kittenTextureUpLeft)
                        texture = kittenTextureUpRight;
                }
            }
        }

        public void Jump(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Right) &&
                           (LeftJump == true || isWall == true))
            {
                if (isWall == true)
                {
                    RightJump = true;
                }

                if (LeftJump == true)
                {
                    LeftJump = false;
                }

                texture = kittenTextureStaticRight;

                StartJump();

                jumpForce = 4f;
                direction = 10f;
            }

            if (keyboardState.IsKeyDown(Keys.Left) &&
               (RightJump == true || isWall == true)) //прыжок влево
            {

                if (isWall == true)
                {
                    LeftJump = true;
                }

                if (RightJump == true)
                {
                    RightJump = false;
                }

                texture = kittenTextureStaticLeft;

                StartJump();

                jumpForce = 4f;
                direction = -10f;
            }
        }

        public void TouchSpike(Wall wall)
        {
            if (IsTouchObjects(kitten, wall.spike)) //конец игры
            {
                if (BuffSave.Timer != 0)
                {
                    //BuffSave.Flag = false;
                    BuffSave.Timer = 0f;
                    wall.spike.position.X = 10000;
                    BuffSave.Timer = BuffSave.Duration;
                }
                else
                {
                    kitten.GameOver();
                }
            }
        }

        public void UpdateScore()
        {
            if (kitten.score <= (int)camera.position.Y * -1 / 100 + 5)
            {
                kitten.score = (int)camera.position.Y * -1 / 100 + 5;
            }

            if (kitten.score > kitten.bestScore)
            {
                kitten.bestScore = kitten.score;
            }
        }
    }
}

