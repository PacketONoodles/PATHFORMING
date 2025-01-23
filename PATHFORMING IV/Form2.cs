using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace PATHFORMING_IV
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //1.53676470588
            //0.65071770334
            this.Size = new Size(1095, 730);
            PictureBox MapImage = new PictureBox();
            MapImage.Location = new Point(0, 0);
            MapImage.Name = "MapImage";
            MapImage.ImageLocation = "U:/final.png";
            //MapImage.SizeMode = PictureBoxSizeMode.AutoSize;
            MapImage.Size = new Size(this.Width - 50, this.Height - 50); // set the panel to scale with the form
            MapImage.SizeMode = PictureBoxSizeMode.AutoSize;
            Controls.Add(MapImage);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
}
