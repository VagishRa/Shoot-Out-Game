using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Shoot_Out_Game.FireBase;


namespace Shoot_Out_Game
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, goUp, goDown, gameOver, gameIsActive;
        string facing = "up";
        //man börjar titta uppåt
        string username = "";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int zombieSpeed = 3;
        int score;
        Random randNum = new Random();
        List<PictureBox> zombieList = new List<PictureBox>();
        //skapar list så att man int ebehöver spawna zombies hela tiden
        FireBase FireBaseConection;

        public Form1()
        {
            InitializeComponent();
            //connecting to database
            FireBaseConection = new FireBase();
            ServerConnectionStatus.Text = FireBaseConection.ConnectToDataBase();

            var Highscores = FireBaseConection.Highscores;

            if (Highscores != null)
                foreach (var user in Highscores)
                    Console.WriteLine($"Date: {user.Value.date} Username: {user.Value.username} Score:{user.Value.score}");
        }

        public void ShowMainMenu(bool showmenu)
        {
            gameIsActive = !showmenu;
            HighScoreList.Visible = showmenu;
            HighScoreList.BringToFront();
            StartGame.Visible = showmenu;
            StartGame.BringToFront();
            ServerConnectionStatus.Visible = showmenu;
            ServerConnectionStatus.BringToFront();
            UsernameTextBox.Visible = showmenu;
            UsernameTextBox.BringToFront();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            txtAmmo.Text = "Ammo: " + ammo;
            txtScore.Text = "Kills: " + score;

            //Stopping crashes when healthbar goes below 0  
            if (playerHealth > 1) healthBar.Value = playerHealth;
            else
            {
                gameOver = true;
                // Switch to player dead image
                player.Image = Properties.Resources.dead;
                // stop game
                GameTimer.Stop();
                ShowMainMenu(true);

                try
                {
                    // sending username, score and date to database
                    FireBaseConection.SendPackage(username, score, DateTime.Now.ToString("MM/dd/yyyy"));
                }
                catch
                {

                }
            }

            //Prevents player from going of screen
            if (goLeft == true && player.Left > 0) player.Left -= speed;
            if (goRight == true && player.Left + player.Width < this.ClientSize.Width) player.Left += speed;
            if (goUp == true && player.Top > 0) player.Top -= speed;
            if (goDown == true && player.Top + player.Height < this.ClientSize.Height) player.Top += speed;

            // loop for if we can collect ammo
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")         
                    // contact with a picturebox with tag ammo
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))              
                        // if player contacts ammo, then dispose the picturebox and add 5 ammo
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                    }
                }

                if (x is PictureBox && (string)x.Tag == "zombie")           
                    // using method to direct zombies towards player
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) playerHealth -= 1;                     
                    // IF player is hit by zombie, then health minus 1

                    if (x.Left > player.Left)            
                        //if zombie is right of player, zombie go left
                    {
                        x.Left -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zleft;         
                        // zombie moving left sprite
                    }

                    if (x.Left < player.Left)            
                        //if zombie is left of player, zombie go right
                    {
                        x.Left += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zright;         
                        // zombie moving right sprite
                    }

                    if (x.Top > player.Top)            
                        //if zombie is below of player, zombie go up
                    {
                        x.Top -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zup;         
                        // zombie moving up sprite
                    }

                    if (x.Top < player.Top)           
                        //if zombie is over of player, zombie go down
                    {
                        x.Top += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zdown;         
                        // zombie moving down sprite
                    }
                }

                foreach (Control j in this.Controls)                     
                    //checking if bullet and zombies are hitting eachother
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))                   
                            // IF BULLET BORDER INTERESECTS WITH ZOMBIE BORDER THEN
                        {
                            score++;                            
                            // ADD SCORE
                            this.Controls.Remove(j);                
                            // remove zombie                  
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zombieList.Remove(((PictureBox)x));
                            MakeZombies();                          
                            // MAKE ZOMBIES after killing 1
                        }
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("DINA ");
            if (gameIsActive)
            {
                if (e.KeyCode == Keys.Left)                                                      
                    // if key is pressed then do following
                {
                    goLeft = true; // change go left to true
                    facing = "left"; //change facing to left
                    player.Image = Properties.Resources.left; 
                    // change the player image to LEFT image
                }

                if (e.KeyCode == Keys.Right)
                {
                    goRight = true; // change go right to true
                    facing = "right"; // change facing to right
                    player.Image = Properties.Resources.right; 
                    // change the player image to right
                }

                if (e.KeyCode == Keys.Up)
                {
                    facing = "up"; // change facing to up
                    goUp = true; // change go up to true
                    player.Image = Properties.Resources.up; 
                    // change the player image to up
                }

                if (e.KeyCode == Keys.Down)
                {
                    facing = "down"; // change facing to down
                    goDown = true; // change go down to true
                    player.Image = Properties.Resources.down; 
                    //change the player image to down
                }
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("DINA ");
            if (gameIsActive)
            {
                if (e.KeyCode == Keys.Left) goLeft = false;

                if (e.KeyCode == Keys.Right) goRight = false;

                if (e.KeyCode == Keys.Up) goUp = false;

                if (e.KeyCode == Keys.Down) goDown = false;

                if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)             
                    // only able to shoot if ammo >0                                                
                    // shoot a bullet in facing direction, with space
                {
                    ammo--;                                                                
                    // reduce ammo for every shot
                    ShootBullet(facing);
                    if (ammo < 1) DropAmmo();                                           
                    // spawn ammo if ammo is equal to 0               
                }
            }
        }

        private void ShootBullet(string direction)                             
            // shooting bullet                                
            // funtion for shooting a bullet
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);              
            // bullet creates from kiddle of player
            shootBullet.bulletTop = player.Top + (player.Height / 2);
            shootBullet.MakeBullet(this);
        }

        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.zdown;
            zombie.Left = randNum.Next(0, 900);             
            // zombie spawns random but not outside frame
            zombie.Top = randNum.Next(60, 800);
            zombie.SizeMode = PictureBoxSizeMode.AutoSize; 
            //sizing up with picture
            zombieList.Add(zombie);                             
            // adding zombie to zombielist
            this.Controls.Add(zombie);                              
            //adds zombie to frame
            player.BringToFront();                                      
            // keeps player on top of zombies.
        }

        private void DropAmmo()                             
            // dropping ammo
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;                       
            //importing ammo pic
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);    
            // ammo does not spawn outside frame
            ammo.Top = randNum.Next(10, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);
            ammo.BringToFront();                                        
            // ammo stays visible on top   
            player.BringToFront();                                              
            // player stays visible on top
        }

        private void RestartGame()
        {
            player.Image = Properties.Resources.up;                                 
            //  removing all zombies from the scene

            foreach (PictureBox i in zombieList) this.Controls.Remove(i);                    
            // remove zombies from list
            zombieList.Clear();                                             
            // CREATE 3 ZOMBIES

            for (int i = 0; i < 3; i++) MakeZombies();

            goUp = false;                                         
            //reseting values. 
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;
            gameIsActive = true;
            playerHealth = 100;
            ammo = 10;
            score = 0;
            GameTimer.Start();
        }
        //start game button
        private void StartGameButton(object sender, EventArgs e)
        {

            if (UsernameTextBox.Text.Length < 1) ServerConnectionStatus.Text = "Enter username with atleast 1 character!";

            else
            {
                username = UsernameTextBox.Text;
                RestartGame();
                ShowMainMenu(false);
            }
        }
    }
}

