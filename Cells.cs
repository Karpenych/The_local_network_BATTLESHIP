
namespace ButtleShip
{
    internal class Cells
    {
        /*static public Point[,] myField = new Point[12, 12];
        static public Point[,] enemyField = new Point[12, 12];

        static public void MatricaFieldsFill()
        {
            // My field fill
            int fieldY = 250;

            for (int i = 0; i < 12; i++)
            {
                int fieldX = 230;

                for (int j = 0; j < 12; j++)
                {
                    myField[i, j].X = fieldX;
                    myField[i, j].Y = fieldY;
                    fieldX += 50;
                }

                fieldY += 50;
            }

            // Enemy field fill
            int fieldEnemyY = 250;

            for (int i = 0; i < 12; i++)
            {
                int fieldEnemyX = 800;

                for (int j = 0; j < 12; j++)
                {
                    enemyField[i, j].X = fieldEnemyX;
                    enemyField[i, j].Y = fieldEnemyY;
                    fieldEnemyX += 50;
                }

                fieldEnemyY += 50;
            }
        }*/

        static public byte[,] myFieldCondition = new byte[12, 12];
        static public byte[,] enemyFieldCondition = new byte[12, 12];

        
    }
}
