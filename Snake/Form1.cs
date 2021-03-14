using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        int maxXPos;
        int maxYPos;
        Random random;

        public Form1()
        {
            InitializeComponent();

            new Settings(); //сброс настроек

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            maxXPos = pbCanvas.Size.Width / Settings.Width;
            maxYPos = pbCanvas.Size.Height / Settings.Height;

            random = new Random();

            StartGame();
        }

        private void StartGame()
        {
            lblGameOverf.Visible = false;

            new Settings(); //сброс настроек

            Snake.Clear();
            Circle head = new Circle();
            head.X = random.Next(0,maxXPos);
            head.Y = random.Next(0, maxYPos);
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }


        private void GenerateFood()
        {
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if (Input.KeyPressed(Keys.Enter))
                    StartGame();
            }
            else
            {
                if(Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                {
                    Settings.direction = Direction.Left;
                }
                else if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                {
                    Settings.direction = Direction.Right;
                }
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                {
                    Settings.direction = Direction.Up;
                }
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                {
                    Settings.direction = Direction.Down;
                }

                MovePlayer();
            }

            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                Brush snakeColor;

                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                        snakeColor = Brushes.Black;
                    else
                        snakeColor = Brushes.Green;

                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width,
                                        Snake[i].Y * Settings.Height,
                                        Settings.Width,
                                        Settings.Height));
                }

                canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                        food.Y * Settings.Height,
                                        Settings.Width,
                                        Settings.Height));
            }
        }

        private void MovePlayer()
        {
            for(int i = Snake.Count - 1; i>= 0; i--)
            {
                if(i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                    }


                    if (Snake[i].X > maxXPos || Snake[i].X < 0 || Snake[i].Y > maxYPos || Snake[i].Y < 0)
                        Die();

                    for(int j = 1; j < Snake.Count; j++)
                    {
                        if(Snake[0].X == Snake[j].X && Snake[0].Y == Snake[j].Y)
                        {
                            Die();
                            break;
                        }
                    }

                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                        Eat();
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Eat()
        {
            Circle eatenFood = new Circle();
            eatenFood.X = Snake[Snake.Count - 1].X;
            eatenFood.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(eatenFood);

            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            Settings.Speed++;
            gameTimer.Interval = 1000 / Settings.Speed;

            GenerateFood();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }
    }
}
