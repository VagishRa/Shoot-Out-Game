using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing; // Using colors
using System.Windows.Forms; 


namespace Shoot_Out_Game
{
    class Bullet
    {
        public string direction; // Varibables
        public int bulletLeft;     //Top and left location of bullet
        public int bulletTop;

        private int speed = 20;
        private PictureBox bullet = new PictureBox(); // Create a new instance of picturebox
        private Timer bulletTimer = new Timer();

        public void MakeBullet(Form form)               // function for bullet, bullet gets added to form
        {


            bullet.BackColor = Color.White;                 // using drawing namespace, color of bullet
            bullet.Size = new Size(5, 5);                        // size of bullet
            bullet.Tag = "bullet";
            bullet.Left = bulletLeft;
            bullet.Top = bulletTop;
            bullet.BringToFront();

            form.Controls.Add(bullet);                  // CALLING FORM and adding bullet to the form.

            bulletTimer.Interval = speed;
            bulletTimer.Tick += new EventHandler(BulletTimerEvent);         //each bullet comes wioth its own timer, its going to know which way to move inside the timer function
            bulletTimer.Start();
        }

        private void BulletTimerEvent(object sender, EventArgs e)
        {

            if(direction == "left")                  // move bullet to direction it came in from, up up side side etc.
            {
                bullet.Left -= speed;

            }

            if (direction == "right")                 
            {
                bullet.Left += speed;

            }

            if (direction == "up")                 
            {
                bullet.Top -= speed;

            }

            if (direction == "down")                  
            {
                bullet.Top += speed;

            }

            if(bullet.Left < 10 || bullet.Left > 860 || bullet.Top < 10 || bullet.Top > 600)                    // if bullet goes outside border, dispose timer, bullet etc
            {
                bulletTimer.Stop();                     // memmory manegement
                bulletTimer.Dispose();
                bullet.Dispose();
                bulletTimer = null;
                bullet = null;


            }

        }

    }
}
