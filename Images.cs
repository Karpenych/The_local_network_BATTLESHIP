namespace ButtleShip
{
    internal class Images
    {
        static public void MissClick(ref List<PictureBox> lpbs, MouseEventArgs mouse, string path)
        {
            lpbs.Add(new PictureBox());
            lpbs.Last().Left = ((mouse.X - 850) / 50) * 50 + 851;
            lpbs.Last().Top = ((mouse.Y - 300) / 50) * 50 + 301;

            lpbs.Last().Width = 48;
            lpbs.Last().Height = 48;
            lpbs.Last().Image = new Bitmap(path);
            lpbs.Last().SizeMode = PictureBoxSizeMode.Normal;
        }

        static public void HitClick(ref List<PictureBox> lpbs, MouseEventArgs mouse, string path)
        {
            lpbs.Add(new PictureBox());
            lpbs.Last().Left = ((mouse.X - 850) / 50) * 50 + 851;
            lpbs.Last().Top = ((mouse.Y - 300) / 50) * 50 + 301;

            lpbs.Last().Width = 48;
            lpbs.Last().Height = 48;
            lpbs.Last().Image = new Bitmap(path);
            lpbs.Last().SizeMode = PictureBoxSizeMode.Normal;
        }

        static public void ArrandeTheShip()
        {
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
            {
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

                        DrawTheShip();
                    }
                }
            }
        }

        static public void DrawTheShip()
        {

        }

    }
}
