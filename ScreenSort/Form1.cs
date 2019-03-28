using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSort
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void buttonScreenshot_Click(object sender, EventArgs e)
        {
            ImageManipulator im = new ImageManipulator();
            im.CreateScreenshot(); 
        }

        private void buttonShowScreen_Click(object sender, EventArgs e)
        {
            ImageManipulator im = new ImageManipulator();
            im.ShowPreview(pictureBoxPreview);
        }
    }
}
