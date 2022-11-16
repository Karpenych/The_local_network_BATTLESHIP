using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System;

namespace ButtleShip
{
    public partial class Form1 : Form
    {
        Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket socketGuest;
        byte[] data = new byte[10];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawField(280);
            DrawField(850);

            lbMyShip1Counter.Text = Ships.myShip1Counter.ToString();
            lbMyShip2Counter.Text = Ships.myShip2Counter.ToString();
            lbMyShip3Counter.Text = Ships.myShip3Counter.ToString();
            lbMyShip4Counter.Text = Ships.myShip4Counter.ToString();

            lbEnemyShip1Counter.Text = Ships.enemyShip1Counter.ToString();
            lbEnemyShip2Counter.Text = Ships.enemyShip2Counter.ToString();
            lbEnemyShip3Counter.Text = Ships.enemyShip3Counter.ToString();
            lbEnemyShip4Counter.Text = Ships.enemyShip4Counter.ToString();
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
            if (mouse.X >= 850 && mouse.X <= 1350 && mouse.Y >= 300 && mouse.Y <= 800)          // ONLY ENEMY FIELD CLICK_LISTENER
            {
                byte column = (byte)((mouse.X - 850) / 50 + 1);
                byte row = (byte)((mouse.Y - 300) / 50 + 1);
               
                if (Cells.enemyFieldCondition[row, column] != 2 && Cells.enemyFieldCondition[row, column] != 3)
                {
                    if (rbServer.Checked)
                    {
                        Shoot(ref data, socketGuest, row, column, row.ToString()+","+column.ToString());
                    }
                    else // rbGuest checked
                    {
                        Shoot(ref data, socket, row, column, row.ToString() + "," + column.ToString());
                    }
                }
            }
        }
        
        private void btArrangeTheShips_Click(object sender, EventArgs e)
        {
            rbGuest.Visible = true;
            rbServer.Visible = true;

            if (Ships.myPbShipsList.Count > 0)
                for (byte i = 0; i < Ships.myPbShipsList.Count; i++)
                    Controls.Remove(Ships.myPbShipsList[i]);

            Ships.ArrandeTheShip();

            for (byte i = 0; i < Ships.myPbShipsList.Count; i++)
                Controls.Add(Ships.myPbShipsList[i]);
        }

        private void rbHost_CheckedChanged(object sender, EventArgs e)  { btConnect.Visible = true; tbServerIP.Visible = false; }

        private void rbGuest_CheckedChanged(object sender, EventArgs e) { btConnect.Visible = true; tbServerIP.Visible = true;  }

        private void btConnect_Click(object sender, EventArgs e)
        {
            btArrangeTheShips.Enabled = false;

            if (rbServer.Checked == true)
            {
                rbGuest.Enabled = false;
                rbServer.Enabled = false;
                btConnect.Enabled = false;
                
                socket.Bind(new IPEndPoint(IPAddress.Loopback, 7064));
                socket.Listen(10);
                socketGuest = socket.Accept();
            }
            else
            {
                try 
                {
                    socket.Connect(new IPEndPoint(IPAddress.Parse(tbServerIP.Text), 7064));

                    rbGuest.Enabled = false;
                    rbServer.Enabled = false;
                    btConnect.Enabled = false;
                    tbServerIP.Visible = false;
                }
                catch { MessageBox.Show("Server no respond"); return; }

                Wait(ref data, socket);
            }
        }

        public void Shoot(ref byte[] data, Socket socket, byte row, byte column, string row_column)
        {
            data = new byte[10];
            data = Encoding.Unicode.GetBytes(row_column);
            socket.Send(data);
            data = new byte[10];
            int bytes = socket.Receive(data);

            if (Encoding.Unicode.GetString(data, 0, bytes) == "0") // if SPLASH
            {
                Effects.AddEnemyFieldEffect(out PictureBox effect, row, column, "splash");
                Controls.Add(effect);
                Cells.enemyFieldCondition[row, column] = 2;

                MessageBox.Show("I_Splash");

                Wait(ref data, socket);
            }
            else  // if BOOM
            {
                string[] info = Encoding.Unicode.GetString(data, 0, bytes).Split(',');
                string isShipDead = info[1];

                Effects.AddEnemyFieldEffect(out PictureBox effect, row, column, "boom");
                Controls.Add(effect);
                Cells.enemyFieldCondition[row, column] = 3;

                MessageBox.Show("I_Boom");

                if (isShipDead == "1")
                {
                    Effects.SplashBorderEnemy(out List<PictureBox> border, row, column);
                    for (byte i = 0; i < border.Count; i++)
                        Controls.Add(border[i]);

                    MessageBox.Show("I Kill ship!!!");

                    if (Ships.enemyShipTotal == 0)
                    {
                        MessageBox.Show("Dude, you win", "", MessageBoxButtons.OK);
                        ActiveForm.Close();
                    }
                }
            }
        }

        public void Wait(ref byte[] data, Socket socket)
        {
            data = new byte[10];
            int bytes = socket.Receive(data);
            string[] row_col = Encoding.Unicode.GetString(data, 0, bytes).Split(',');
            byte row = byte.Parse(row_col[0]);
            byte column = byte.Parse(row_col[1]);

            if (Cells.myFieldCondition[row, column] == 0)  // if enemy splash
            {
                Effects.AddMyFieldEffect(out PictureBox effect, row, column, "splash");
                Controls.Add(effect);

                Cells.myFieldCondition[row, column] = 2;

                data = new byte[10];
                data = Encoding.Unicode.GetBytes("0");
                socket.Send(data);
                MessageBox.Show("LoL he miss)))");
                return;
            }
            else // if enemy boom
            {
                Effects.AddMyFieldEffect(out PictureBox effect, row, column, "boom");
                Controls.Add(effect);
                effect.BringToFront();

                Cells.myFieldCondition[row, column] = 3;

                Effects.SplashBorderMy(out List<PictureBox> border, row, column, out bool isShipDead);
                if (isShipDead)
                {
                    for (byte i = 0; i < border.Count; i++)
                        Controls.Add(border[i]);

                    data = new byte[10];
                    data = Encoding.Unicode.GetBytes("1,1");
                    socket.Send(data);

                    MessageBox.Show("he kill my ship!!!!!!!");

                    if (Ships.myShipTotal == 0)
                    {
                        MessageBox.Show("Dude, you lose", "", MessageBoxButtons.OK);
                        ActiveForm.Close();
                    }

                    Wait(ref data, socket);
                    return;
                }
                else
                {
                    data = new byte[10];
                    data = Encoding.Unicode.GetBytes("1,0");
                    socket.Send(data);
                    MessageBox.Show("Oh, he boom me((");
                    Wait(ref data, socket);
                    return;
                }
            }
        }

    }
}