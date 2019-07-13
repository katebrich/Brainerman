using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bomberman.MainClasses
{
    enum BonusState { Covered, Discovered, Destroyed };

    class Bonus
    {
        public int Row;
        public int Col;
        public int Type;
        public BonusState State = BonusState.Covered;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="Type"> 0 = fire, 1 = bomb, 2 = life, 3 = speed </param>
        public Bonus(int Row, int Col, int Type)
        {
            this.Row = Row;
            this.Col = Col;
            this.Type = Type;
        }
    }

}
