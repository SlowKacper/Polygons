using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Lab1
{
    class Colors
    {
        public Color[] Colo = new Color[11];
        public Colors()
        {
            //Colo[0] = (255, 0, 0);   //Red
            //Colo[1] = (30, 144, 255); //DogerBlue
            //Colo[2] = (0, 0, 128);   //Navy
            //Colo[3] = (139, 0, 139);  //DarkMagena
            //Colo[4] = (255, 140, 0);  //DarkOrange
            //Colo[5] = (0, 128, 0);   //Green
            //Colo[6] = (173, 255, 47);  //GreenYellow
            //Colo[7] = (255, 105, 180); //HotPink
            //Colo[8] = (255, 0, 255);  //Magenta
            //Colo[9] = (0, 255, 0);  //Lime
            //Colo[10] = (0, 255, 255);  //Aqua
            Colo[0] = Color.Red;
            Colo[1] = Color.Blue;
            Colo[2] = Color.Navy;
            Colo[3] = Color.Purple;
            Colo[4] = Color.DarkOrange;
            Colo[5] = Color.Green;
            Colo[6] = Color.GreenYellow;
            Colo[7] = Color.HotPink;
            Colo[8] = Color.Magenta;
            Colo[9] = Color.Lime;
            Colo[10] = Color.Aqua;
        }
    }
   
}
