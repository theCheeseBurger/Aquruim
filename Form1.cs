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

        private int maxFishCount = 5; // ✅ Maximum number of fish
        private int minFishCount = 0; // ✅ Minimum number of fish

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (panel1 != null) // ✅ Ensure panel1 is created before initializing cGL
            {
                cGL = new cOGL(panel1); // ✅ Now initialized correctly
                timerRUN.Interval = 30;
                timerRUN.Start();
            }
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

        private void fishSizeBar_Scroll(object sender, EventArgs e)
        {
            if (cGL != null)
            {
                float scale = fishSizeBar.Value / 10.0f; // Normalize scale
                cGL.SetFishScale(scale);
                panel1.Invalidate(); // ✅ Redraw scene
            }
        }

        private void btnAddFish_Click(object sender, EventArgs e)
        {
            if (cGL != null && cGL.FishCount < maxFishCount)
            {
                cGL.AddFish(); // ✅ Adds a fish to the aquarium
                panel1.Invalidate(); // ✅ Redraw the scene
            }
        }

        private void btnRemoveFish_Click(object sender, EventArgs e)
        {
            if (cGL != null && cGL.FishCount > minFishCount)
            {
                cGL.RemoveFish(); // ✅ Removes a fish from the aquarium
                panel1.Invalidate(); // ✅ Redraw the scene
            }
        }

        private void fishRotationBar_Scroll(object sender, EventArgs e)
        {
            if (cGL != null)
            {
                float angle = fishRotationBar.Value; // ✅ Get rotation value from slider
                cGL.SetFishRotation(angle); // ✅ Update fish rotation
                panel1.Invalidate(); // ✅ Redraw the OpenGL scene
            }
        }
    }
}
