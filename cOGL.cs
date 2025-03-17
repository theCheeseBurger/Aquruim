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
        private const int FishStart = 2;
        private List<Bubble> bubbles = new List<Bubble>();
        private float bubbleSpeed = 0.02f;
        public int FishCount => fishes.Count; // ✅ Returns the current number of fish

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
            GL.glEnable(GL.GL_COLOR_MATERIAL); // ✅ Enables material color usage
            GL.glEnable(GL.GL_NORMALIZE); // ✅ Ensures correct lighting after transformations

            // ✅ Apply Two-Sided Lighting BEFORE projection setup
            GL.glLightModeli(GL.GL_LIGHT_MODEL_TWO_SIDE, (int)GL.GL_TRUE);

            // ✅ Set light position
            float[] lightPos = { 2.0f, 2.0f, 3.0f, 1.0f };
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, lightPos);

            // ✅ Set how the material color is affected by lighting
            GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);

            // ✅ Set background color
            GL.glClearColor(0.7f, 0.9f, 1.0f, 1.0f);

            // ✅ Projection Setup
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            GLU.gluPerspective(45.0, (double)Width / Height, 1.0, 1000.0);

            // ✅ Switch to ModelView matrix
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
        }

        private void GenerateFishes()
        {
            for (int i = 0; i < FishStart; i++)
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

        public void AddFish()
        {
            if (fishes.Count < 5) // ✅ Limit max fish to 5
            {
                fishes.Add(new Fish(
                    (float)(rand.NextDouble() * 1.5 - 0.75),
                    (float)(rand.NextDouble() * 1.5 - 0.75),
                    (float)(rand.NextDouble() * -1.5),
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble()
                ));
            }
        }
        public void RemoveFish()
        {
            if (fishes.Count > 0) // ✅ Prevents fish count from going below 0
            {
                fishes.RemoveAt(fishes.Count - 1);
            }
        }

        public void Draw()
        {
            if (DC == 0 || RC == 0) return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
            GL.glLoadIdentity();
            GLU.gluLookAt(0, 0, 3, 0, 0, 0, 0, 1, 0);

            DrawAquarium();
            DrawTreasureChest();
            DrawSpongeBobHouse();
            DrawBubbles();
            DrawFishes();
            UpdateFishes();

            GL.glFlush();
            WGL.wglSwapBuffers(DC);
        }
        public void SetFishScale(float scale)
        {
            foreach (var fish in fishes)
            {
                fish.ScaleFactor = scale;
            }
        }
        public void SetFishRotation(float angle)
        {
            foreach (var fish in fishes)
            {
                fish.RotationAngle = angle;
            }
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
                fish.DrawFishShadow(fish);
                fish.DrawFish(); // ✅ Now correctly calls the 3D fish model
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
        public bool FacingRight;
        public float RotationAngle { get; set; } = 2.0f;

        public float ScaleFactor { get; set; } = 1.0f;
        private Random rand = new Random();

        public Fish(float x, float y, float z, float r, float g, float b)
        {
            X = x;
            Y = y;
            Z = z;
            R = r;
            G = g;
            B = b;
            FacingRight = rand.Next(2) == 0;
            ChangeDirection();
        }

        public void Move()
        {
            X += dX;
            Y += dY;
            Z += dZ;

            if (X > 0.75f || X < -0.75f)
            {
                dX = -dX;
                FacingRight = !FacingRight;
            }
            if (Y > 0.75f || Y < -0.75f) dY = -dY;
            if (Z > -0.1f || Z < -1.5f) dZ = -dZ;

            if (rand.NextDouble() < 0.01) ChangeDirection();
        }

        private void ChangeDirection()
        {
            dX = (float)(rand.NextDouble() * 0.02 - 0.01);
            dY = (float)(rand.NextDouble() * 0.02 - 0.01);
            dZ = (float)(rand.NextDouble() * 0.02 - 0.01);
        }

        public void DrawFish()
        {
            GL.glPushMatrix();
            GL.glTranslatef(X, Y, Z);
            GL.glScalef(ScaleFactor, ScaleFactor, ScaleFactor);

            // Apply rotation
            GL.glRotatef(RotationAngle, 0.0f, 1.0f, 0.0f);

            if (!FacingRight)
                GL.glScalef(-1.0f, 1.0f, 1.0f);

            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glShadeModel(GL.GL_SMOOTH);

            // **Body**
            GL.glPushMatrix();
            DrawGradientOvalSphere(0.12f, 0.08f, 0.06f, 16, 16);
            GL.glPopMatrix();

            // ✅ **Tail Positioned Correctly**
            Draw3DTail();

            // ✅ **Left & Right Side Inflated Fins**
            DrawInflatedFin(0.07f, 0.00f, 0.05f);  // Left Fin (Positive Z)
            DrawInflatedFin(0.07f, 0.00f, -0.05f); // Right Fin (Negative Z)

            // **Eyes**
            GL.glColor3f(1.0f, 1.0f, 1.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0.07f, 0.03f, 0.04f);
            DrawSphere(0.02f, 8, 8);
            GL.glPopMatrix();

            GL.glPushMatrix();
            GL.glTranslatef(0.07f, 0.03f, -0.04f);
            DrawSphere(0.02f, 8, 8);
            GL.glPopMatrix();

            // **Pupils**
            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0.075f, 0.03f, 0.05f);
            DrawSphere(0.01f, 8, 8);
            GL.glPopMatrix();

            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0.075f, 0.03f, -0.05f);
            DrawSphere(0.01f, 8, 8);
            GL.glPopMatrix();

            GL.glPopMatrix();
        }

        public void DrawFishShadow(Fish fish)
        {
            GL.glPushMatrix();

            // ✅ **Refined Aquarium Floor Boundaries (Tighter Clamping)**
            float aquariumMinX = -0.85f; // Left boundary
            float aquariumMaxX = 0.85f;  // Right boundary
            float aquariumMinZ = -0.6f; // Back boundary
            float aquariumMaxZ = 0.85f;  // Front boundary

            // ✅ **Clamp Shadow Position** (Ensures it stays inside aquarium floor)
            float shadowX = Math.Max(aquariumMinX, Math.Min(fish.X, aquariumMaxX));
            float shadowZ = Math.Max(aquariumMinZ, Math.Min(fish.Z, aquariumMaxZ));

            // ✅ **Move shadow to the constrained position**
            GL.glTranslatef(shadowX, -0.95f, shadowZ);
            GL.glScalef(1.0f, 0.08f, 0.7f); // Slightly smaller shadow to stay inside floor

            // ✅ **Disable lighting for solid shadow**
            GL.glDisable(GL.GL_LIGHTING);
            GL.glColor4f(0.0f, 0.0f, 0.0f, 0.5f); // Semi-transparent black

            GL.glBegin(GL.GL_TRIANGLE_FAN);
            GL.glVertex3f(0.0f, 0.0f, 0.0f); // Center of the shadow

            int segments = 20; // Smooth oval shape
            float radiusX = 0.08f, radiusZ = 0.04f; // **Reduced to keep inside boundaries**
            for (int i = 0; i <= segments; i++)
            {
                float angle = (float)(2 * Math.PI * i / segments);
                float x = (float)Math.Cos(angle) * radiusX;
                float z = (float)Math.Sin(angle) * radiusZ;
                GL.glVertex3f(x, 0.0f, z);
            }
            GL.glEnd();

            // ✅ **Re-enable lighting after shadow rendering**
            GL.glEnable(GL.GL_LIGHTING);

            GL.glPopMatrix();
        }

        private void Draw3DTail()
        {
            GL.glBegin(GL.GL_TRIANGLES);

            // **Base color (near body)**
            GL.glColor3f(R * 0.6f, G * 0.6f, B * 0.6f);

            // **Left Tail**
            GL.glVertex3f(-0.25f, 0.0f, 0.05f);
            //GL.glVertex3f(-0.15f, 0.05f, 0.03f);
            GL.glVertex3f(-0.15f, -0.05f, 0.03f);

            // **Tip color (fading out)**
            GL.glColor3f(R * 0.4f, G * 0.4f, B * 0.4f);

            // **Right Tail**
            GL.glVertex3f(-0.15f, 0.05f, -0.03f);
            //GL.glVertex3f(-0.25f, 0.0f, -0.05f);
            GL.glVertex3f(-0.15f, -0.05f, -0.03f);

            GL.glEnd();
        }

        private void DrawInflatedFin(float width, float height, float zOffset)
        {
            GL.glPushMatrix();

            // ✅ Position the fin correctly on the side of the fish
            GL.glTranslatef(-0.01f, height, zOffset);

            // ✅ Rotate the fin to be 90 degrees to the body
            GL.glRotatef(90, 0.0f, 1.0f, 0.0f);

            // ✅ Inflate the fin while keeping it flat
            GLUquadric quad = GLU.gluNewQuadric();
            GL.glColor3f(R * 1.1f, G * 1.1f, B * 1.1f); // Slightly lighter
            GLU.gluCylinder(quad, width * 0.3, 0.0, width, 12, 6);

            GLU.gluDeleteQuadric(quad);

            GL.glPopMatrix();
        }
        private void DrawSphere(float radius, int slices, int stacks)
        {
            float x, y, z;
            float alpha, beta;

            for (int i = 0; i < stacks; i++)
            {
                alpha = (float)(Math.PI * i / stacks);
                beta = (float)(Math.PI * (i + 1) / stacks);

                GL.glBegin(GL.GL_TRIANGLE_STRIP);
                for (int j = 0; j <= slices; j++)
                {
                    float theta = (float)(2.0 * Math.PI * j / slices);

                    x = (float)(Math.Cos(theta) * Math.Sin(alpha));
                    y = (float)(Math.Sin(theta) * Math.Sin(alpha));
                    z = (float)(Math.Cos(alpha));

                    GL.glVertex3f(x * radius, y * radius, z * radius);

                    x = (float)(Math.Cos(theta) * Math.Sin(beta));
                    y = (float)(Math.Sin(theta) * Math.Sin(beta));
                    z = (float)(Math.Cos(beta));

                    GL.glVertex3f(x * radius, y * radius, z * radius);
                }
                GL.glEnd();
            }
        }

        private void DrawGradientOvalSphere(float radiusX, float radiusY, float radiusZ, int slices, int stacks)
        {
            float x, y, z;
            float alpha, beta;

            for (int i = 0; i < stacks; i++)
            {
                alpha = (float)(Math.PI * i / stacks);
                beta = (float)(Math.PI * (i + 1) / stacks);

                GL.glBegin(GL.GL_TRIANGLE_STRIP);
                for (int j = 0; j <= slices; j++)
                {
                    float theta = (float)(2.0 * Math.PI * j / slices);

                    // Compute normal
                    float nx = (float)(Math.Cos(theta) * Math.Sin(alpha));
                    float ny = (float)(Math.Sin(theta) * Math.Sin(alpha));
                    float nz = (float)(Math.Cos(alpha));

                    x = nx * radiusX;
                    y = ny * radiusY;
                    z = nz * radiusZ;

                    // ✅ Set normal for consistent lighting
                    GL.glNormal3f(nx, ny, nz);

                    // ✅ Set color ONCE based on position
                    if (y > 0) // Top is lighter
                        GL.glColor3f(R * 1.2f, G * 1.2f, B * 1.2f);
                    else // Bottom is darker
                        GL.glColor3f(R * 0.7f, G * 0.7f, B * 0.7f);

                    GL.glVertex3f(x, y, z);

                    // Compute next normal
                    nx = (float)(Math.Cos(theta) * Math.Sin(beta));
                    ny = (float)(Math.Sin(theta) * Math.Sin(beta));
                    nz = (float)(Math.Cos(beta));

                    x = nx * radiusX;
                    y = ny * radiusY;
                    z = nz * radiusZ;

                    GL.glNormal3f(nx, ny, nz);
                    GL.glVertex3f(x, y, z);
                }
                GL.glEnd();
            }
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


