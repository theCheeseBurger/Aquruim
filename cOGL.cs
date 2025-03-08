using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenGL;

namespace AquriumProject2
{
    class cOGL
    {
        Control p;
        int Width;
        int Height;
        private List<Fish> fishes = new List<Fish>();
        private Random rand = new Random();
        private const int FishCount = 20;
        private List<Bubble> bubbles = new List<Bubble>();
        private float bubbleSpeed = 0.02f;

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            InitRenderingGL();
            GenerateFishes();
            GenerateBubbles();
        }

        ~cOGL()
        {
            WGL.wglDeleteContext(RC);
        }

        public uint RC { get; private set; }
        public uint DC { get; private set; }

        private void InitRenderingGL()
        {
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glEnable(GL.GL_NORMALIZE); // ✅ Ensures proper lighting across transformations
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glClearColor(0.7f, 0.9f, 1.0f, 1.0f); // Light blue background for the aquarium water

            float[] lightPos = { 2.0f, 2.0f, 3.0f, 1.0f };
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, lightPos);

            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            GLU.gluPerspective(45.0, (double)Width / Height, 1.0, 1000.0);
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
        }

        private void GenerateFishes()
        {
            for (int i = 0; i < FishCount; i++)
            {
                fishes.Add(new Fish(
                    (float)(rand.NextDouble() * 1.5 - 0.75), // X position inside the aquarium
                    (float)(rand.NextDouble() * 1.5 - 0.75), // Y position inside the aquarium
                    (float)(rand.NextDouble() * -1.5), // Z position inside the aquarium
                    (float)rand.NextDouble(), // Red color
                    (float)rand.NextDouble(), // Green color
                    (float)rand.NextDouble()  // Blue color
                ));
            }
        }

        public void Draw()
        {
            if (DC == 0 || RC == 0) return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
            GL.glLoadIdentity();
            GLU.gluLookAt(0, 0, 3, 0, 0, 0, 0, 1, 0);

            UpdateFishes();
            DrawAquarium();
            DrawTreasureChest();
            DrawSpongeBobHouse();
            DrawBubbles();
            DrawFishes();

            GL.glFlush();
            WGL.wglSwapBuffers(DC);
        }

        private void DrawAquarium()
        {
            GL.glPushMatrix();
            GL.glColor4f(0.5f, 0.8f, 1.0f, 0.5f); // Light blue transparent water effect

            GL.glBegin(GL.GL_QUADS);
            float size = 2.0f;

            // Bottom face (floor)
            GL.glColor3f(0.6f, 0.6f, 0.6f); // Grey bottom
            GL.glVertex3f(-size / 2, -size / 2, -size / 2);
            GL.glVertex3f(size / 2, -size / 2, -size / 2);
            GL.glVertex3f(size / 2, -size / 2, size / 2);
            GL.glVertex3f(-size / 2, -size / 2, size / 2);

            // Top face (water surface)
            GL.glColor3f(0.6f, 0.6f, 0.6f); // Grey bottom
            GL.glVertex3f(-size / 2, size / 2, -size / 2);
            GL.glVertex3f(size / 2, size / 2, -size / 2);
            GL.glVertex3f(size / 2, size / 2, size / 2);
            GL.glVertex3f(-size / 2, size / 2, size / 2);

            GL.glEnd();
            GL.glPopMatrix();
        }

        private void DrawFishes()
        {
            foreach (var fish in fishes)
            {
                GL.glPushMatrix();
                GL.glPushAttrib(GL.GL_CURRENT_BIT); // ✅ Save color state

                GL.glTranslatef(fish.X, fish.Y, fish.Z);
                GL.glColor3f(fish.R, fish.G, fish.B); // ✅ Ensure fish color is properly set

                // Fish Body
                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(-0.05f, -0.02f, 0.0f);
                GL.glVertex3f(0.05f, -0.02f, 0.0f);
                GL.glVertex3f(0.05f, 0.02f, 0.0f);
                GL.glVertex3f(-0.05f, 0.02f, 0.0f);
                GL.glEnd();

                // Tail (adjust position and flip when moving left)
                GL.glBegin(GL.GL_TRIANGLES);
                float tailX = -0.06f; // ✅ Default tail position for right movement
                float tailTipX = -0.08f; // ✅ Default tail tip position

                if (!fish.FacingRight) // ✅ Moving left → Tail moves to the right side
                {
                    tailX = 0.06f;
                    tailTipX = 0.08f; // ✅ Reverse triangle direction
                }

                GL.glVertex3f(tailX, 0.0f, 0.0f);
                GL.glVertex3f(tailTipX, 0.03f, 0.0f);
                GL.glVertex3f(tailTipX, -0.03f, 0.0f);
                GL.glEnd();

                GL.glPopAttrib(); // ✅ Restore color state
                GL.glPopMatrix();
            }
        }


        private void DrawTreasureChest()
        {
            GL.glPushMatrix();
            GL.glPushAttrib(GL.GL_CURRENT_BIT); // ✅ Save color state

            GL.glTranslatef(0.0f, -0.95f, 0.0f); // Adjust position slightly up

            GL.glColor3f(0.5f, 0.3f, 0.1f); // ✅ Brown for the treasure chest

            // Base of the chest
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.1f, 0.0f, -0.1f);
            GL.glVertex3f(0.1f, 0.0f, -0.1f);
            GL.glVertex3f(0.1f, 0.1f, -0.1f);
            GL.glVertex3f(-0.1f, 0.1f, -0.1f);
            GL.glEnd();

            // Open lid
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.1f, 0.0f);
            GL.glRotatef(-30, 1.0f, 0.0f, 0.0f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.1f, 0.0f, -0.1f);
            GL.glVertex3f(0.1f, 0.0f, -0.1f);
            GL.glVertex3f(0.1f, 0.05f, -0.1f);
            GL.glVertex3f(-0.1f, 0.05f, -0.1f);
            GL.glEnd();
            GL.glPopMatrix();

            GL.glPopAttrib(); // ✅ Restore color state
            GL.glPopMatrix();
        }


        private void DrawBubbles()
        {
            GL.glPushAttrib(GL.GL_CURRENT_BIT); // ✅ Save color state before drawing bubbles
            GL.glColor3f(0.8f, 0.8f, 1.0f); // Light blue for bubbles

            for (int i = 0; i < bubbles.Count; i++)
            {
                bubbles[i].Y += bubbleSpeed;
                if (bubbles[i].Y > 1.0f) bubbles[i].Y = -0.7f; // Reset to bottom

                GL.glPushMatrix();
                GL.glTranslatef(bubbles[i].X, bubbles[i].Y, bubbles[i].Z);

                // ✅ Ensure correct sphere drawing with GLUquadric
                GLUquadric bubbleQuadric = GLU.gluNewQuadric();
                GLU.gluSphere(bubbleQuadric, 0.03, 10, 10);
                GLU.gluDeleteQuadric(bubbleQuadric); // ✅ Free memory after use

                GL.glPopMatrix();
            }

            GL.glPopAttrib(); // ✅ Restore color state after drawing bubbles
        }

        private void GenerateBubbles()
        {
            for (int i = 0; i < 5; i++)
            {
                bubbles.Add(new Bubble(
                    0.0f + (float)(rand.NextDouble() * 0.1 - 0.05),
                    -0.95f, // Start at the bottom
                    -0.0f + (float)(rand.NextDouble() * 0.1 - 0.05)
                ));
            }
        }

        private void DrawSpongeBobHouse()
        {
            GL.glPushMatrix();
            GL.glTranslatef(-0.8f, -0.85f, 0.0f); // ✅ Position house on the left-bottom side
            GL.glColor3f(1.0f, 0.6f, 0.0f); // ✅ Pineapple-like yellow-orange color

            // Pineapple body (scaled sphere for a taller and narrower shape)
            GL.glPushMatrix();
            GL.glScalef(0.8f, 1.5f, 0.8f); // ✅ Make it taller and narrower
            GLUquadric houseQuadric = GLU.gluNewQuadric();
            GLU.gluSphere(houseQuadric, 0.15, 20, 20);
            GLU.gluDeleteQuadric(houseQuadric); // ✅ Free memory after use
            GL.glPopMatrix();

            // Add a diamond pattern effect (simulating pineapple texture)
            GL.glColor3f(0.8f, 0.4f, 0.0f); // ✅ Darker orange for pattern
            GL.glBegin(GL.GL_LINES);
            for (float theta = 0; theta < Math.PI; theta += 0.2f) // Draw latitude lines
            {
                for (float phi = 0; phi < 2 * Math.PI; phi += 0.2f) // Draw longitude lines
                {
                    float x = 0.15f * (float)(Math.Sin(theta) * Math.Cos(phi));
                    float y = 0.15f * (float)(Math.Cos(theta));
                    float z = 0.15f * (float)(Math.Sin(theta) * Math.Sin(phi));
                    GL.glVertex3f(x, y, z);
                }
            }
            GL.glEnd();

            // Leaves (larger and taller)
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.3f, 0.0f); // ✅ Move leaves higher
            GL.glColor3f(0.0f, 0.8f, 0.0f); // ✅ Green for leaves

            for (int i = 0; i < 6; i++) // ✅ More leaves for a fuller look
            {
                GL.glPushMatrix();
                GL.glRotatef(i * 60, 0.0f, 1.0f, 0.0f); // Spread leaves more
                GLU.gluCylinder(GLU.gluNewQuadric(), 0.05, 0.0, 0.2, 10, 10); // ✅ Taller and bigger leaves
                GL.glPopMatrix();
            }

            GL.glPopMatrix();

            // Door
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, -0.05f, 0.14f);
            GL.glColor3f(0.5f, 0.3f, 0.2f); // ✅ Brown for the door
            GLU.gluDisk(GLU.gluNewQuadric(), 0, 0.03, 10, 1);
            GL.glPopMatrix();

            GL.glPopMatrix();
        }


        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
            InitRenderingGL();
            Draw();
        }

        private void InitializeGL()
        {
            DC = WGL.GetDC((uint)p.Handle.ToInt32());
            WGL.wglSwapBuffers(DC);
            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.dwFlags = (WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)WGL.PFD_TYPE_RGBA;
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)WGL.PFD_MAIN_PLANE;

            int pixelFormatIndex = WGL.ChoosePixelFormat(DC, ref pfd);
            WGL.SetPixelFormat(DC, pixelFormatIndex, ref pfd);
            RC = WGL.wglCreateContext(DC);
            WGL.wglMakeCurrent(DC, RC);
            InitRenderingGL();
        }

        private void UpdateFishes()
        {
            foreach (var fish in fishes)
            {
                fish.Move();
            }
        }


    }

    class Fish
    {
        public float X, Y, Z;
        public float R, G, B;
        public float dX, dY, dZ; // Movement direction and speed
        public bool FacingRight; // ✅ Determines fish orientation
        private Random rand = new Random();

        public Fish(float x, float y, float z, float r, float g, float b)
        {
            X = x;
            Y = y;
            Z = z;
            R = r;
            G = g;
            B = b;
            FacingRight = rand.Next(2) == 0; // Randomly assign initial direction
            ChangeDirection();
        }

        public void Move()
        {
            X += dX;
            Y += dY;
            Z += dZ;

            // If a fish reaches the boundary, change direction
            if (X > 0.75f || X < -0.75f)
            {
                dX = -dX;
                FacingRight = !FacingRight; // ✅ Flip fish when changing direction
            }
            if (Y > 0.75f || Y < -0.75f) dY = -dY;
            if (Z > -0.1f || Z < -1.5f) dZ = -dZ;

            // Randomly change direction over time
            if (rand.NextDouble() < 0.01) ChangeDirection();
        }

        private void ChangeDirection()
        {
            dX = (float)(rand.NextDouble() * 0.02 - 0.01);
            dY = (float)(rand.NextDouble() * 0.02 - 0.01);
            dZ = (float)(rand.NextDouble() * 0.02 - 0.01);
        }
    }

    class Bubble
    {
        public float X, Y, Z;
        public Bubble(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

}


