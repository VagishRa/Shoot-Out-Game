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




        }

    }
}
