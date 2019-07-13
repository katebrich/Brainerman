using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bomberman.MainClasses
{
    class HeroState
    {
        public int BombsMaximum = 1;
        public int Speed = 2;
        public int FireRange = 1;
        public int Life = 3;

        /// <summary>
        /// Použije se základní nastavení:
        ///   BombsMaximum = 1;
        ///   Speed = 2;
        ///   FireRange = 1;
        ///   Life = 3;
        /// </summary>
        public HeroState()
        {
        }
        
        public HeroState(int FireRange, int BombsMaximum, int Life, int Speed)
        {
            this.FireRange = FireRange;
            this.BombsMaximum = BombsMaximum;
            this.Life = Life;
            this.Speed = Speed;
        }
    }
}
