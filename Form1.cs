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
                
                if (rbServer.Checked)
                {
                    if (Cells.enemyFieldCondition[row, column] != 2 && Cells.enemyFieldCondition[row, column] != 3)
                    {
                        data = Encoding.Unicode.GetBytes($"{row},{column}");
                        socketGuest.Send(data);

                        int bytes = socketGuest.Receive(data);

                        if (Encoding.Unicode.GetString(data, 0, bytes) == "0")
                        {
                            Effects.AddEnemyFieldEffect(out PictureBox effect, row, column, "splash");
                            Controls.Add(effect);
                            Cells.enemyFieldCondition[row, column] = 2;

                            // Ход переходит у гостю
                        }
                        else
                        {
                            Effects.AddEnemyFieldEffect(out PictureBox effect, row, column, "boom");
                            Controls.Add(effect);
                            Cells.enemyFieldCondition[row, column] = 3;

                            Effects.SplashBorderEnemy(out List<PictureBox> border, row, column);
                            for (byte i = 0; i < border.Count; i++)
                                Controls.Add(border[i]);

                            // Click again
                        }
                    } 
                }
                else
                {
                    data = new byte[10];
                    data = Encoding.Unicode.GetBytes($"{row},{column}");
                    socket.Send(data);

                    int bytes = socket.Receive(data);
                    MessageBox.Show(Encoding.Unicode.GetString(data, 0, bytes));
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

        private void rbHost_CheckedChanged(object sender, EventArgs e) => btConnect.Visible = true;

        private void rbGuest_CheckedChanged(object sender, EventArgs e) => btConnect.Visible = true; 

        private void btConnect_Click(object sender, EventArgs e)
        {
            btArrangeTheShips.Enabled = false;

            if (rbServer.Checked == true)
            {
                rbGuest.Enabled = false;
                rbServer.Enabled = false;
                btConnect.Enabled = false;

                socket.Bind(new IPEndPoint(IPAddress.Loopback, 5064));
                socket.Listen(1);
                socketGuest = socket.Accept();
            }
            else
            {
                try 
                {
                    string serverIP = "127.0.0.1";
                    socket.Connect(new IPEndPoint(IPAddress.Parse(serverIP), 5064));

                    rbGuest.Enabled = false;
                    rbServer.Enabled = false;
                    btConnect.Enabled = false;
                }
                catch { MessageBox.Show("Server no respond"); return; }

                int bytes = socket.Receive(data);
                string[] row_col = Encoding.Unicode.GetString(data, 0, bytes).Split(',');
                byte row = byte.Parse(row_col[0]);
                byte column = byte.Parse(row_col[1]);

                if (Cells.myFieldCondition[row, column] == 0)
                {
                    Effects.AddMyFieldEffect(out PictureBox effect, row, column, "splash");
                    Controls.Add(effect);

                    Cells.myFieldCondition[row, column] = 2;

                    data = Encoding.Unicode.GetBytes("0");
                    socket.Send(data);
                }
                else
                {
                    Effects.AddMyFieldEffect(out PictureBox effect, row, column, "boom");
                    Controls.Add(effect);
                    effect.BringToFront();
                    
                    Cells.myFieldCondition[row, column] = 3;

                    Effects.SplashBorderMy(out List<PictureBox> border, row, column);
                    for (byte i = 0; i < border.Count; i++)
                        Controls.Add(border[i]);

                    data = Encoding.Unicode.GetBytes("1");
                    socket.Send(data);
                }
            }
        }

    }
}