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

        public Form1()
        {
            InitializeComponent();

            new Settings(); //сброс настроек

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            maxXPos = pbCanvas.Size.Width / Settings.Width;
            maxYPos = pbCanvas.Size.Height / Settings.Height;

            StartGame();
        }

        private void StartGame()
        {
            lblGameOverf.Visible = false;

            new Settings(); //сброс настроек

            Snake.Clear();
            Circle head = new Circle();
            Random random = new Random();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }


        private void GenerateFood()
        {
            Random random = new Random();

            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (!Settings.GameOver)
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pbCanvas_Click(object sender, EventArgs e)
        {

        }
    }
}
