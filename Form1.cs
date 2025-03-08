using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL;

namespace AquriumProject2
{
    public partial class Form1 : Form
    {
        private cOGL cGL;
        private bool isNight = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cGL = new cOGL(panel1); // Now initialized after panel1 is created
            timerRUN.Interval = 30;
            timerRUN.Start();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (cGL != null)
                cGL.Draw(); // Ensures cGL is valid before drawing
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            if (cGL != null)
                cGL.OnResize();
        }

        private void timerRUN_Tick(object sender, EventArgs e)
        {
            if (cGL != null)
                cGL.Draw(); // Prevents null reference issues
        }
    }
}
