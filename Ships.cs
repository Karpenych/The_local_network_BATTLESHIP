using Microsoft.VisualBasic.Devices;
using System.Data.Common;
using System.Windows.Forms;

namespace ButtleShip
{
    internal class Ships
    {
        static public List<PictureBox> myPbShipsList = new();

        static public void ArrandeTheShip()
        {
            myPbShipsList.Clear();

            for (int i = 1; i < 11; i++)
                for (int j = 1; j  < 11; j ++)
                {
                    Cells.enemyFieldCondition[i, j] = 0;
                    Cells.myFieldCondition[i,j] = 0;
                }

            Random random = new();
            int row, column, direction, right, bottom;
            bool isError;

            for (int i = 4; i>0; i--)
                for (int j = 0; j < 5-i; j++)
                {
                    row = random.Next(1, 11);
                    column = random.Next(1, 11);
                    direction = random.Next(2);

                    if (direction == 0)
                    {
                        if (column + i > 11)
                        {
                            j--;
                            continue;
                        }
                        right = column + i;
                        bottom = row + 1;
                    }
                    else
                    {
                        if (row + i > 11)
                        {
                            j--;
                            continue;
                        }
                        bottom = row + i;
                        right = column + 1;
                    }

                    isError = false;

                    for (int y = row - 1; y <= bottom; y++)
                        for (int u = column - 1; u <= right; u++)
                            if (Cells.myFieldCondition[y,u] != 0)
                            {
                                isError = true;
                                j--;
                                y = 15;
                                break;
                            }

                    if (!isError)
                    {
                        for (int y = row; y < bottom; y++)
                            for (int u = column; u < right; u++)
                                Cells.myFieldCondition[y, u] = 1;

                        DrawMyShips(i, direction, row, column, ref myPbShipsList);
                    }
                }
        }

        static private void DrawMyShips(in int i, in int direction, in int row, in int column, ref List<PictureBox> myPbShipsList)
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

        static private void AddShipInList(in List<PictureBox> myShipsList, in int row, in int column, int width, int height, string name)
        {
            myShipsList.Add(new PictureBox());
            myShipsList.Last().Left = (column - 1) * 50 + 281;
            myShipsList.Last().Top = (row - 1) * 50 + 301;
            myShipsList.Last().Height = height;
            myShipsList.Last().Width = width;
            myShipsList.Last().Image = new Bitmap(@"C:\Users\DmitriiKarp\Desktop\MGU\5_semestr\OpSist\Practica\ButtleShip\pictures\" + name);
            myShipsList.Last().SizeMode = PictureBoxSizeMode.Normal;
        }

    }
}
