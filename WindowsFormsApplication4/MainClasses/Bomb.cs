using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bomberman.MainClasses
{
    class Bomb
    {
        public int CurrentTime; //Jak dlouho od doby, co byla položená
        public int Row;
        public int Col;

        public Bomb(int Row, int Col)
        {
            this.Row = Row;
            this.Col = Col;
            this.CurrentTime = 0;
        }
    }
}
