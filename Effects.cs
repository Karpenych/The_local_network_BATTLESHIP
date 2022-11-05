using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtleShip
{
    internal class Effects
    {
        static public void AddEnemyFieldEffect(out PictureBox effect, byte row, byte column, string splash_boom)
        {
            effect = new()
            {
                Left =(column - 1) * 50 + 851,
                Top = (row - 1) * 50 + 301,
                Width = 48,
                Height = 48,
                Image = new Bitmap(@"..\..\..\pictures\" + splash_boom + ".png"),
                SizeMode = PictureBoxSizeMode.Normal,
                BackColor = Color.Transparent
            }; 
        }

        static public void AddMyFieldEffect(out PictureBox effect, byte row, byte column, string splash_boom)
        {
            effect = new()
            {
                Left = (column - 1) * 50 + 281,
                Top = (row - 1) * 50 + 301,
                Width = 48,
                Height = 48,
                Image = new Bitmap(@"..\..\..\pictures\" + splash_boom + ".png"),
                SizeMode = PictureBoxSizeMode.Normal,
                BackColor = Color.Transparent
            };
        }

        static public void SplashBorderEnemy(out List<PictureBox> splashBorder, byte row, byte column)
        {
            splashBorder = new();
            bool isVertical = false, isError = false;
            byte bottom, right, shipLength = 1;

            if (Cells.enemyFieldCondition[row - 1, column] != 1 && Cells.enemyFieldCondition[row, column + 1] != 1 &&    // SPLASHS AROUNG DEAD SHIP
                    Cells.enemyFieldCondition[row + 1, column] != 1 && Cells.enemyFieldCondition[row, column - 1] != 1 &&
                    Cells.enemyFieldCondition[row, column] == 3)
            {
                for (byte i = 0; i < 4; i++)          // SET ROW AND COLUMN TOP_LEFT_ANGLE VALUES. SET DIRECTION
                {
                    if (Cells.enemyFieldCondition[row - 1, column] == 3) row--;
                    if (Cells.enemyFieldCondition[row, column - 1] == 3) column--; 
                }
                if (Cells.enemyFieldCondition[row + 1, column] == 3) isVertical = true;
                if (Cells.enemyFieldCondition[row, column + 1] == 3) isVertical = false;

                if (isVertical)     // FIND BOTTOM AND RIGHT FOR VERTICAL SHIP
                {
                    for (byte i = 0; i < 4; i++)
                        if (Cells.enemyFieldCondition[row + shipLength, column] == 3)
                            shipLength++;

                    if (Cells.enemyFieldCondition[row + shipLength, column] == 1 || Cells.enemyFieldCondition[row - 1, column] == 1) isError = true;
                       
                    bottom = (byte)(row + shipLength);
                    right = (byte)(column + 1);
                }
                else               // FIND BOTTOM AND RIGHT FOR HORIZONTAL SHIP
                {
                    for (byte i = 0; i < 4; i++)
                        if (Cells.enemyFieldCondition[row, column + shipLength] == 3)
                            shipLength++;

                    if (Cells.enemyFieldCondition[row, column + shipLength] == 1 || Cells.enemyFieldCondition[row, column - 1] == 1) isError = true;

                    bottom = (byte)(row + 1);
                    right = (byte)(column + shipLength);
                }

                if (!isError)
                    for (byte angleRow = (byte)(row - 1); angleRow <= bottom; angleRow++)
                        for (byte angleColumn = (byte)(column - 1); angleColumn <= right; angleColumn++)
                            if (angleRow > 0 && angleRow < 11 && angleColumn > 0 && angleColumn < 11 && Cells.enemyFieldCondition[angleRow, angleColumn] == 0)
                            {
                                splashBorder.Add(new PictureBox()
                                {
                                    Left = (angleColumn - 1) * 50 + 851,
                                    Top = (angleRow - 1) * 50 + 301,
                                    Width = 48,
                                    Height = 48,
                                    Image = new Bitmap(@"..\..\..\pictures\splash.png"),
                                    SizeMode = PictureBoxSizeMode.Normal
                                });
                                //Controls.Add(miss);
                                Cells.enemyFieldCondition[row, column] = 2;
                            }
            }
        }

        static public void SplashBorderMy(out List<PictureBox> splashBorder, byte row, byte column)
        {
            splashBorder = new();
            bool isVertical = false, isError = false;
            byte bottom, right, shipLength = 1;

            if (Cells.myFieldCondition[row - 1, column] != 1 && Cells.myFieldCondition[row, column + 1] != 1 &&    // SPLASHS AROUNG DEAD SHIP
                    Cells.myFieldCondition[row + 1, column] != 1 && Cells.myFieldCondition[row, column - 1] != 1 &&
                    Cells.myFieldCondition[row, column] == 3)
            {
                for (byte i = 0; i < 4; i++)          // SET ROW AND COLUMN TOP_LEFT_ANGLE VALUES. SET DIRECTION
                {
                    if (Cells.myFieldCondition[row - 1, column] == 3) row--;
                    if (Cells.myFieldCondition[row, column - 1] == 3) column--;
                }
                if (Cells.myFieldCondition[row + 1, column] == 3) isVertical = true;
                if (Cells.myFieldCondition[row, column + 1] == 3) isVertical = false;

                if (isVertical)     // FIND BOTTOM AND RIGHT FOR VERTICAL SHIP
                {
                    for (byte i = 0; i < 4; i++)
                        if (Cells.myFieldCondition[row + shipLength, column] == 3)
                            shipLength++;

                    if (Cells.myFieldCondition[row + shipLength, column] == 1 || Cells.myFieldCondition[row - 1, column] == 1) isError = true;

                    bottom = (byte)(row + shipLength);
                    right = (byte)(column + 1);
                }
                else               // FIND BOTTOM AND RIGHT FOR HORIZONTAL SHIP
                {
                    for (byte i = 0; i < 4; i++)
                        if (Cells.myFieldCondition[row, column + shipLength] == 3)
                            shipLength++;

                    if (Cells.myFieldCondition[row, column + shipLength] == 1 || Cells.myFieldCondition[row, column - 1] == 1) isError = true;

                    bottom = (byte)(row + 1);
                    right = (byte)(column + shipLength);
                }

                if (!isError)
                    for (byte angleRow = (byte)(row - 1); angleRow <= bottom; angleRow++)
                        for (byte angleColumn = (byte)(column - 1); angleColumn <= right; angleColumn++)
                            if (angleRow > 0 && angleRow < 11 && angleColumn > 0 && angleColumn < 11 && Cells.enemyFieldCondition[angleRow, angleColumn] == 0)
                            {
                                splashBorder.Add(new PictureBox()
                                {
                                    Left = (angleColumn - 1) * 50 + 281,
                                    Top = (angleRow - 1) * 50 + 301,
                                    Width = 48,
                                    Height = 48,
                                    Image = new Bitmap(@"..\..\..\pictures\splash.png"),
                                    SizeMode = PictureBoxSizeMode.Normal
                                });
                                Cells.myFieldCondition[row, column] = 2;
                            }
            }
        }

    }
}
