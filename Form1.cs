using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string Username { get; set; }
        public string PhoneNumber { get; set; }



        private void Form1_Load(object sender, EventArgs e)
        {
            SmoothRoundedPanel1(panel1, 50, Color.FromArgb(255, 255, 255));
            SmoothRoundedPanel1(panel2, 50, Color.FromArgb(255, 255, 255));
            SmoothRoundedPanel1(panel3, 50, Color.FromArgb(248, 81, 0));
        }



        private void SmoothRoundedPanel1(Panel panel, int cornerRadius, Color baseColor)
        {
            panel.BackColor = Color.Transparent;

            // Override WndProc untuk menggambar latar belakang panel dengan efek sudut yang terbulat yang lebih halus
            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90);
                    path.AddArc(rect.Width - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90);
                    path.AddArc(rect.Width - cornerRadius, rect.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                    path.AddArc(rect.X, rect.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                    path.CloseAllFigures();

                    using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(255, baseColor), Color.FromArgb(255, baseColor), LinearGradientMode.Horizontal))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };
        }

       

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); 
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Dashboard dashboardForm = new Dashboard();


            dashboardForm.Username = textBox1.Text;
            dashboardForm.PhoneNumber = textBox2.Text;
           
            // Show the dashboard form
            dashboardForm.Show();
            
        }
    }
}

