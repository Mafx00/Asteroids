using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids
{
    public partial class Form1 : Form
    {
        public bool running;
        private readonly List<Keys> input = new List<Keys>();

        private bool goUp, goDown, goLeft, goRight, gameOver = false;

        private float timer, points;

        private float nextSpawn = 5;

        private Player player;
        private List<Bullet> bullets = new List<Bullet>();
        private List<Enemy> enemies = new List<Enemy>();

        List<Bullet> destroyBullets = new List<Bullet>();
        List<Enemy> destroyEnemies = new List<Enemy>();

        public Form1()
        {
            InitializeComponent();
            player = new Player();
            player.size = 40;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            input.Add(e.KeyCode);

            if (e.KeyCode == Keys.W)
            {
                goUp = true;
            }

            if (e.KeyCode == Keys.S)
            {
                goDown = true;
            }

            if (e.KeyCode == Keys.A)
            {
                goLeft = true;
            }

            if (e.KeyCode == Keys.D)
            {
                goRight = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                goUp = false;
            }

            if (e.KeyCode == Keys.S)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.A)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.D)
            {
                goRight = false;
            }
        }

        public void RunGameLoop()
        {
            running = true;
            while(running)
            {
                if (!gameOver)
                {
                    HandleInput();
                    Update();
                }
                Render();
            }
        }

        private void HandleInput()
        {
            List<Keys> tempInput = new List<Keys>(input);
            input.Clear();

            foreach(Keys key in tempInput)
            {
                switch (key)
                {
                    case Keys.Space:
                        Bullet bullet = new Bullet();
                        bullet.speed = 5;
                        bullet.horizontalPosition = player.horizontalPosition + 20;
                        bullet.verticalPosition = player.verticalPosition;
                        bullets.Add(bullet);
                        break;
                }
            }

        }

        private void Update()
        {
            timer += 0.01f;

            if(timer > nextSpawn)
            {
                SpawnEnemy();
            }

            if(goUp && player.verticalPosition > 0)
            {
                player.verticalPosition -= 1;
            }

            if (goDown && player.verticalPosition < 640)
            {
                player.verticalPosition += 1;
            }

            if (goRight && player.horizontalPosition < 1020)
            {
                player.horizontalPosition += 1;
            }

            if (goLeft && player.horizontalPosition > 0)
            {
                player.horizontalPosition -= 1;
            }

            player.BoundingVolume();

            foreach (Bullet bullet in bullets)
            {
                bullet.verticalPosition -= bullet.speed;
                bullet.BoundingVolume();

                if(bullet.verticalPosition < 0)
                {
                    destroyBullets.Add(bullet);                   
                }
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.verticalPosition += enemy.speed;
                enemy.BoundingVolume();

                foreach(Bullet bullet in bullets)
                {
                   if(enemy.CompareVolumes(bullet.verticalMin, bullet.verticalMax, bullet.horizontalMin, bullet.horizontalMax))
                    {
                        destroyEnemies.Add(enemy);
                        destroyBullets.Add(bullet);
                        points++;
                    }
                }

                if (enemy.CompareVolumes(player.verticalMin, player.verticalMax, player.horizontalMin, player.horizontalMax))
                {
                    destroyEnemies.Add(enemy);
                    player.health--;
                }

                if (enemy.verticalPosition > 600)
                {
                    destroyEnemies.Add(enemy);
                    player.health -= 1;
                }
            }

            foreach (Enemy enemy in destroyEnemies)
            {
                enemies.Remove(enemy);
            }

            foreach (Bullet bullet in destroyBullets)
            {
                bullets.Remove(bullet);
            }

            destroyBullets.Clear();

            destroyEnemies.Clear();

            if (player.health < 1)
                gameOver = true;

        }

        void SpawnEnemy()
        {
            timer = 0;
            Random random = new Random();
            nextSpawn = random.Next(1, 8);
          
            Enemy enemy = new Enemy();
            enemy.speed = 0.5f;
            enemy.verticalPosition = 0;
            enemy.size = random.Next(40, 150);
            enemy.horizontalPosition = random.Next(0, 900);

            enemies.Add(enemy);
        }

        private void Render()
        {
            Bitmap bitmap = new Bitmap(Canvas.Width, Canvas.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            if (gameOver)
            {
                graphics.DrawString("GAME OVER", new Font("Arial", 64), new SolidBrush(Color.Red), 300, 325);
            }
            graphics.DrawString("health " + player.health, new Font("Arial", 16), new SolidBrush(Color.White), 465, 650);
            graphics.DrawString("total points " + points, new Font("Arial", 16), new SolidBrush(Color.White), 450, 550);

            graphics.DrawImage(Image.FromFile("C:/Users/gogre/source/repos/Asteroids/Asteroids/graphics/player.png"), player.horizontalPosition, player.verticalPosition, player.size, player.size);

            foreach (Bullet bullet in bullets)
            {
                graphics.DrawImage(Image.FromFile("C:/Users/gogre/source/repos/Asteroids/Asteroids/graphics/bullet.png"), bullet.horizontalPosition, bullet.verticalPosition, 10, 10);
            }

            foreach (Enemy enemy in enemies)
            {
                graphics.DrawImage(Image.FromFile("C:/Users/gogre/source/repos/Asteroids/Asteroids/graphics/asteroid.png"), enemy.horizontalPosition, enemy.verticalPosition, enemy.size, enemy.size);
            }

            Canvas.Image = bitmap;

            Application.DoEvents();
        }
    }
}
