using Microsoft.VisualBasic.Devices;
using System.Data.Common;
using System.Windows.Forms;

namespace ButtleShip
{
    internal class Ships
    {
        static public byte myShip4Counter = 1, enemyShip4Counter = 1,
                           myShip3Counter = 2, enemyShip3Counter = 2,
                           myShip2Counter = 3, enemyShip2Counter = 3,
                           myShip1Counter = 4, enemyShip1Counter = 4;
        static public List<PictureBox> myPbShipsList = new();

        static public void ArrandeTheShip()
        {
            myPbShipsList.Clear();

            for (byte i = 1; i < 11; i++)
                for (byte j = 1; j  < 11; j ++)
                    Cells.myFieldCondition[i,j] = 0;

            Random random = new();
            byte row, column, direction, right, bottom;
            bool isError;

            for (byte i = 4; i>0; i--)
                for (byte j = 0; j < 5-i; j++)
                {
                    row = (byte)random.Next(1, 11);
                    column = (byte)random.Next(1, 11);
                    direction = (byte)random.Next(2);

                    if (direction == 0)
                    {
                        if (column + i > 11)
                        {
                            j--;
                            continue;
                        }
                        right = (byte)(column + i);
                        bottom = (byte)(row + 1);
                    }
                    else
                    {
                        if (row + i > 11)
                        {
                            j--;
                            continue;
                        }
                        bottom = (byte)(row + i);
                        right = (byte)(column + 1);
                    }

                    isError = false;

                    for (byte y = (byte)(row - 1); y <= bottom; y++)
                        for (byte u = (byte)(column - 1); u <= right; u++)
                            if (Cells.myFieldCondition[y,u] != 0)
                            {
                                isError = true;
                                j--;
                                y = 15;
                                break;
                            }

                    if (!isError)
                    {
                        for (byte y = row; y < bottom; y++)
                            for (byte u = column; u < right; u++)
                                Cells.myFieldCondition[y, u] = 1;

                        DrawMyShips(i, direction, row, column, myPbShipsList);
                    }
                }
        }

        static private void DrawMyShips(in byte i, in byte direction, in byte row, in byte column, in List<PictureBox> myPbShipsList)
        {
            if (direction == 0)
            {
                if (i == 4)
                    AddShipInList(myPbShipsList, row, column, 198, 48, "ship4hor.png");
                else if (i == 3)
                    AddShipInList(myPbShipsList, row, column, 148, 48, "ship3hor.png");
                else if(i == 2)
                    AddShipInList(myPbShipsList, row, column, 98, 48, "ship2hor.png");
                else
                    AddShipInList(myPbShipsList, row, column, 48, 48, "ship1hor.png");
            }
            else
            {
                if (i == 4)
                    AddShipInList(myPbShipsList, row, column, 48, 198, "ship4vert.png");
                else if (i == 3)
                    AddShipInList(myPbShipsList, row, column, 48, 148, "ship3vert.png");
                else if (i == 2)
                    AddShipInList(myPbShipsList, row, column, 48, 98, "ship2vert.png");
                else
                    AddShipInList(myPbShipsList, row, column, 48, 48, "ship1vert.png");
            }
        }

        static private void AddShipInList(in List<PictureBox> myShipsList, in byte row, in byte column, byte width, byte height, string name)
        {
            myShipsList.Add(new PictureBox()
            {
                Left = (column - 1) * 50 + 281,
                Top = (row - 1) * 50 + 301,
                Height = height,
                Width = width,
                Image = new Bitmap(@"..\..\..\pictures\" + name),
                SizeMode = PictureBoxSizeMode.Normal,
                BackColor = Color.Transparent
            });
        }

        static public void KillMyShip(byte shipLength)
        {
            switch (shipLength)
            {
                case 1:
                    myShip1Counter--;
                    break;
                case 2:
                    myShip2Counter--;
                    break;
                case 3:
                    myShip3Counter--;
                    break;
                case 4:
                    myShip4Counter--;
                    break;
                default:
                    break;
            }
        }

        static public void KillEnemyShip()
        {

        }
    }
}
s