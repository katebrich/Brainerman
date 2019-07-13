using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Bomberman.MainClasses
{
    class Monster3 : Monster
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Map"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="Direction">0 = doleva, 1 = nahoru, 2 = doprava, 3 = dolů</param>
        /// <param name="Bitmap"></param>
        /// <param name="BitmapDead"></param>
        /// <param name="Imunity">0, pokud normální level.</param>
        public Monster3(Map Map, int Row, int Col, int Direction, Bitmap Bitmap, Bitmap BitmapDead, int Imunity)
        {
            this.Map = Map;
            this.Row = Row;
            this.Col = Col;
            this.X = Row * Map.TileSize;
            this.Y = Col * Map.TileSize;
            this.Direction = Direction;
            this.Speed = 3;
            this.Image = Bitmap;
            this.ImageDead = BitmapDead;
            this.Score = 14;
            this.Imunity = Imunity;
        }
       
        /// <summary>
        /// V každém kroce zjistí možnosti, kudy jít a náhodně vybere. Pokud nemůže doleva, doprava ani rovně, otočí se zpátky.
        /// Tato příšera může chodit přes zdi. 
        /// </summary>
        public override void Step()
        {
            {
                Random rnd = new Random();
                int[] possibilities = new int[30];
                int count = 0;
                if (CanGoStraight())
                {
                    possibilities[count] = 0;
                    count++;
                }
                if (CanGoLeft())
                {
                    possibilities[count] = 1;
                    count++;
                }
                if (CanGoRight())
                {
                    possibilities[count] = 2;
                    count++;
                }

                int a = rnd.Next(0, count);
                if (count == 0)
                {
                    TurnLeft();
                    TurnLeft();
                }
                else
                {
                    switch (possibilities[a])
                    {
                        case 0:
                            GoStraight();
                            Move();
                            break;
                        case 1:
                            TurnLeft();
                            GoStraight();
                            Move();
                            break;
                        case 2:
                            TurnRight();
                            GoStraight();
                            Move();
                            break;
                    }
                }
            }
        }
    }
}
