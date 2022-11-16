using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtleShip
{
    internal class Local_Network : Form1
    {

        public void Hod(ref byte[] data, Socket socket, byte row, byte column, string row_column)
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

                Jdu(ref data, socket);
            }
            else  // if BOOM
            {
                string[] info = Encoding.Unicode.GetString(data, 0, bytes).Split(',');
                string isShipDead = info[1];

                Effects.AddEnemyFieldEffect(out PictureBox effect, row, column, "boom");
                Controls.Add(effect);
                Cells.enemyFieldCondition[row, column] = 3;

                if (isShipDead == "1")
                {
                    Effects.SplashBorderEnemy(out List<PictureBox> border, row, column);
                    for (byte i = 0; i < border.Count; i++)
                        Controls.Add(border[i]);

                    if (Ships.enemyShipTotal == 0)
                    {
                        MessageBox.Show("Dude, you win", "", MessageBoxButtons.OK);
                        ActiveForm.Close();
                    }
                }
            }
        }




        public void Jdu(ref byte[] data, Socket socket)
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

                    if (Ships.myShipTotal == 0)
                    {
                        MessageBox.Show("Dude, you lose", "", MessageBoxButtons.OK);
                        ActiveForm.Close();
                    }

                    Jdu(ref data, socket);
                    return;
                }
                else
                {
                    data = new byte[10];
                    data = Encoding.Unicode.GetBytes("1,0");
                    socket.Send(data);
                    Jdu(ref data, socket);
                    return;
                }
            }
        }
    }
}
