using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Bomberman.MainClasses
{
    class Hero
    {
        public int Row = 1; //souřadnice políčka v mapě
        public int Col = 1;
        public int X; //souřadnice vykreslovaného obrázku hrdiny
        public int Y;
        public Bitmap Bitmap;
        public int BombsAvailable; //Kolik má bomb v zásobě       
        public bool StandingOnBomb = false; //Pomocná proměnná. Když položí bombu, musí z ní umět odejít, ale už se nevrátit.
        public Point BombWhereStanding; // Pomocná proměnná.
        public HeroState HeroState; //jaký je stav bonusů a životů
        
        public Hero(int X, int Y, Bitmap Image, HeroState HeroState)
        {
           this.Bitmap = Image;
           this.X = X;
           this.Y = Y;
           this.HeroState = HeroState;
           this.BombsAvailable = this.HeroState.BombsMaximum;
        }

        /// <summary>
        /// Upraví se hodnoty HeroState podle příslušného bonusu.
        /// </summary>
        /// <param name="type">0 = fire, 1 = bomb, 2 = life, 3 = speed</param>
        public void TakeBonus(int type)
        {
            switch (type)
            {
                case 0:
                    HeroState.FireRange++;
                    break;
                case 1:
                    HeroState.BombsMaximum++;
                    BombsAvailable++;
                    break;
                case 2:
                    HeroState.Life++;
                    break;
                case 3:
                    HeroState.Speed++;
                    break;
            }
        }
    }
}
