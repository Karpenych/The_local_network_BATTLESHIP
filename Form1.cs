using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;

namespace ButtleShip
{
    public partial class Form1 : Form
    {
        public readonly Socket Socket1 = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Socket? Socket2;

        public Form1()
        {
            InitializeComponent();
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
                Effects.ClickEffect(out PictureBox miss, mouse, "splash");
                Controls.Add(miss);
            }
        }

        private void btArrangeTheShips_Click(object sender, EventArgs e)
        {
            rbGuest.Visible = true;
            rbHost.Visible = true;

            if (Ships.myPbShipsList.Count > 0)
                for (int i = 0; i < Ships.myPbShipsList.Count; i++)
                    Controls.Remove(Ships.myPbShipsList[i]);

            Ships.ArrandeTheShip();

            for (int i = 0; i < Ships.myPbShipsList.Count; i++)
                Controls.Add(Ships.myPbShipsList[i]);
        }

        private void rbHost_CheckedChanged(object sender, EventArgs e) => btConnect.Visible = true;

        private void rbGuest_CheckedChanged(object sender, EventArgs e) => btConnect.Visible = true; 

        private void btConnect_Click(object sender, EventArgs e)
        {
            if (rbHost.Checked == true)
            {
                Socket1.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5064));
                Socket1.Listen(10);
                Socket2 = Socket1.Accept();
            }
            else
            {
                try { Socket1.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5064)); }
                catch { MessageBox.Show("Сервер не отвечает"); }
            }
        }

    }
}