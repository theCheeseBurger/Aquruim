
using System;
using System.Windows.Forms;
using OpenGL;

namespace AquriumProject2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timerRUN;
        private System.Windows.Forms.Button btnAddFish;
        private System.Windows.Forms.Button btnRemoveFish;
        private TrackBar fishSizeBar;
        private TrackBar fishRotationBar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timerRUN = new System.Windows.Forms.Timer(this.components);
            this.fishSizeBar = new System.Windows.Forms.TrackBar();
            this.fishRotationBar = new System.Windows.Forms.TrackBar();
            this.btnAddFish = new System.Windows.Forms.Button();
            this.btnRemoveFish = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fishSizeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fishRotationBar)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(20, 31);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(834, 961);
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // timerRUN
            // 
            this.timerRUN.Interval = 30;
            this.timerRUN.Tick += new System.EventHandler(this.timerRUN_Tick);
            // 
            // fishSizeBar
            // 
            this.fishSizeBar.AccessibleRole = System.Windows.Forms.AccessibleRole.Slider;
            this.fishSizeBar.Location = new System.Drawing.Point(978, 31);
            this.fishSizeBar.Margin = new System.Windows.Forms.Padding(4);
            this.fishSizeBar.Maximum = 50;
            this.fishSizeBar.Name = "fishSizeBar";
            this.fishSizeBar.Size = new System.Drawing.Size(162, 69);
            this.fishSizeBar.TabIndex = 7;
            this.fishSizeBar.TickFrequency = 0;
            this.fishSizeBar.Scroll += new System.EventHandler(this.fishSizeBar_Scroll);
            // 
            // fishRotationBar
            // 
            this.fishRotationBar.AccessibleRole = System.Windows.Forms.AccessibleRole.Slider;
            this.fishRotationBar.Location = new System.Drawing.Point(978, 128);
            this.fishRotationBar.Margin = new System.Windows.Forms.Padding(4);
            this.fishRotationBar.Maximum = 360;
            this.fishRotationBar.Name = "fishRotationBar";
            this.fishRotationBar.Size = new System.Drawing.Size(162, 69);
            this.fishRotationBar.TabIndex = 8;
            this.fishRotationBar.TickFrequency = 0;
            this.fishRotationBar.Scroll += new System.EventHandler(this.fishRotationBar_Scroll);  // ✅ Ensure event handler is connected
            // 
            // btnAddFish
            // 
            this.btnAddFish.Location = new System.Drawing.Point(978, 187);
            this.btnAddFish.Name = "btnAddFish";
            this.btnAddFish.Size = new System.Drawing.Size(50, 30);
            this.btnAddFish.TabIndex = 0;
            this.btnAddFish.Text = "+";
            this.btnAddFish.UseVisualStyleBackColor = true;
            this.btnAddFish.Click += new System.EventHandler(this.btnAddFish_Click);
            // 
            // btnRemoveFish
            // 
            this.btnRemoveFish.Location = new System.Drawing.Point(1090, 187);
            this.btnRemoveFish.Name = "btnRemoveFish";
            this.btnRemoveFish.Size = new System.Drawing.Size(50, 30);
            this.btnRemoveFish.TabIndex = 0;
            this.btnRemoveFish.Text = "-";
            this.btnRemoveFish.UseVisualStyleBackColor = true;
            this.btnRemoveFish.Click += new System.EventHandler(this.btnRemoveFish_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 1039);
            this.Controls.Add(this.fishRotationBar);
            this.Controls.Add(this.fishSizeBar);
            this.Controls.Add(this.btnAddFish);
            this.Controls.Add(this.btnRemoveFish);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "Form1";
            this.Text = "3D Fish Aquarium";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fishSizeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fishRotationBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


    }
}

