using System.Runtime.CompilerServices;

namespace ButtleShip
{
    public partial class Form1 : Form
    { 
        
        public List<PictureBox> lpbs = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawField(280);
            DrawField(850);
        }

        private void DrawField(int margin)
        {
            Graphics graphics = CreateGraphics();
            Pen pen = new(Color.Navy, 2.0f);

            Point pt1 = new(margin, 300);
            Point pt2 = new(margin, 800);
            for (int i = 0; i < 11; ++i)
            {
                graphics.DrawLine(pen, pt1, pt2);
                pt1.X += 50;
                pt2.X += 50;
            }

            pt1 = new(margin, 300);
            pt2 = new(margin + 500, 300);
            for (int i = 0; i < 11; ++i)
            {
                graphics.DrawLine(pen, pt1, pt2);
                pt1.Y += 50;
                pt2.Y += 50;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs mouse)
        {
            if (mouse.X >= 850 && mouse.X <= 1350 && mouse.Y >= 300 && mouse.Y <= 800)
            {
                Images.MissClick(ref lpbs, mouse);
                Controls.Add(lpbs.Last());
            }
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            
        }

        private void btArrangeTheShips_Click(object sender, EventArgs e)
        {
            if (Images.myPbShipsList.Count > 0)
                for (int i = 0; i < Images.myPbShipsList.Count; i++)
                    Controls.Remove(Images.myPbShipsList[i]);

            Images.ArrandeTheShip();

            for (int i = 0; i < Images.myPbShipsList.Count; i++)
                Controls.Add(Images.myPbShipsList[i]);
        }


    }
}