using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Bomberman.MainClasses
{
    /// <summary>
    /// Políčko, kam dosáhl plamen bomby a ještě je nějakou dobu aktivní = může zabíjet
    /// </summary>
    class FireTile
    {
        public int Row;
        public int Col;
        public int Time = 0; //Jak dlouho od vybuchnutí bomby
        public int Type; // 0 = free space, 1 = temporary wall
        public Bitmap Bitmap; //jaký obrázek se má vykreslovat

        public FireTile(int row, int col, int type, Bitmap bmp)
        {
            this.Row = row;
            this.Col = col;
            this.Type = type;
            this.Bitmap = bmp;
        }
    }
}
