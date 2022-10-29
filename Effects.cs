using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButtleShip
{
    internal class Effects
    {


        static public void ClickEffect(out PictureBox effect, MouseEventArgs mouse, string splash_boom)
        {
            effect = new()
            {
                Left = (mouse.X - 850) / 50 * 50 + 851,
                Top = (mouse.Y - 300) / 50 * 50 + 301,
                Width = 48,
                Height = 48,
                Image = new Bitmap(@"..\..\..\pictures\" + splash_boom + ".png"),
                SizeMode = PictureBoxSizeMode.Normal
            };
            
        }

    }
}
