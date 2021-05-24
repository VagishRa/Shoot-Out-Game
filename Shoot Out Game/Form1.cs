using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shoot_Out_Game
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, goUp, goDown, gameOver;               //deklarera variablerna
        string facing = "up";                                        //man börjar titta uppåt
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int zombieSpeed = 3;
        int score;
        Random randNum = new Random();

        List<PictureBox> zombieList = new List<PictureBox>();                          //skapar list så att man int ebehöver spawna zombies hela tiden

        
        public Form1()
        {
            InitializeComponent();
        }

        private void MainTImerEvent(object sender, EventArgs e)
        {
            if (playerHealth > 1)                                                         //Stopping crashes when healthbar goes below 0
            {
                healthBar.Value = playerHealth;
            }
            else
            {
                gameOver = true;
            }

            txtAmmo.Text = "Ammo: " + ammo;
            txtScore.Text = "Kills: " + score;

         


            if (goLeft == true && player.Left > 0)                                           //Prevents player from going of screen
            {
                player.Left -= speed;
            }

            if( goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += speed;

            }

            if ( goUp == true && player.Top > 0)
            {
                player.Top -= speed;
            }

            if( goDown== true && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += speed;
            }


        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)                                                      // if key is pressed then do following
            {   
                goLeft = true; // change go left to true
                facing = "left"; //change facing to left
                player.Image = Properties.Resources.left; // change the player image to LEFT image
            }

            if (e.KeyCode == Keys.Right)
            {
                score++;
                goRight = true; // change go right to true
                facing = "right"; // change facing to right
                player.Image = Properties.Resources.right; // change the player image to right
            }

            if (e.KeyCode == Keys.Up)
            {
                facing = "up"; // change facing to up
                goUp = true; // change go up to true
                player.Image = Properties.Resources.up; // change the player image to up
            }

            if (e.KeyCode == Keys.Down)
            {
                facing = "down"; // change facing to down
                goDown = true; // change go down to true
                player.Image = Properties.Resources.down; //change the player image to down
            }


        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
                                                                                           // if keys are not pressed, do following
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false; 
               
            }

            
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
                
            }
            
            if (e.KeyCode == Keys.Up)
            {
                goUp = false; 
                
            }
     
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;

            }

            if(e.KeyCode == Keys.Space)                                                  // shoot a bullet in facing direction, with space
            {
                ShootBullet(facing);
            }

        }

        private void ShootBullet(string direction)                                      // funtion for shooting a bullet
        {

        }

        private void MakeZombies()
        {

        }

        private void RestartGame()
        {

        }

    }
}
