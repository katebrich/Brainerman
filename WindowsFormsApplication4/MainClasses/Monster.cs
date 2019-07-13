using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Bomberman.MainClasses
{
    abstract class Monster
    {
        public int Row; //Souřadnice vzhledem k mapě
        public int Col;
        public int X; //Souřadnice vykreslovaného obrázku
        public int Y;
        public int Direction; // 0 = doleva, 1 = nahoru, 2 = doprava, 3 = dolů
        public Map Map;
        public int Speed;
        public Bitmap Image;
        public Bitmap ImageDead; //Obrázek zobrazovaný po smrti příšery
        public int TimeDead; //Jak dlouho už je po smrti
        public int Score; //Kolik je za její zabití bodů
        public int Imunity; //Používané pouze v posledním levelu - příšery se objevují po zničení zdí a musí být chvíli imunní vůči FireTiles

        private bool moving; //Říká, jestli se příšera právě posouvá na další políčko
        private int movesMade; //Kolikrát už se obrázek příšery posunul
        private int i = 0; //Pomocná proměnná, zpomalení rychlosti (krok se udělá jen každý druhý "tik" timeru
        private Point movingWhere; //Kam se právě příšera posouvá
        private Point movingFrom; //Odkud se příšera posouvá

        /// <summary>
        /// Otáčení příšery podle určitých pravidel.
        /// </summary>
        public abstract void Step();

        /// <summary>
        /// Posun obrázku příšery směrem, kterým je otočená.
        /// </summary>
        /// <param name="howMuch"></param>
        private void moveInDirection(int howMuch)
        {
            switch (Direction)
            {
                case 0:
                    Y = Y - howMuch;
                    break;
                case 1:
                    X = X - howMuch;
                    break;
                case 2:
                    Y = Y + howMuch;
                    break;
                case 3:
                    X = X + howMuch;
                    break;
                default:
                    break;
                    
            }
        }

        /// <summary>
        /// Pokud se příšera hýbe na další políčko, posune její obrázek o Speed (případně tolik, kolik zbývá) pixelů.
        /// Pokud se nehýbe, zavolá Step() pro otočení příšery.
        /// </summary>
        public void Move()
        {
            if (i % 2 == 0)   //for slower speed
            {
                if (moving & Map.IsBomb(movingWhere.X, movingWhere.Y)) //Pokud přímo před ní byla položená bomba, přestane se přesouvat její obrázek a vrátí se na výchozí políčko
                {
                    Row = movingFrom.X;
                    Col = movingFrom.Y;
                    X = movingFrom.X * Map.TileSize;
                    Y = movingFrom.Y * Map.TileSize;
                    moving = false;
                }

                else if (moving && movesMade < Map.TileSize / Speed)
                {
                    moveInDirection(Speed);
                    movesMade++;
                }

                else if (moving && movesMade == Map.TileSize / Speed) // in case less than Speed pixels remaining for moving to another tile
                {
                    int remainer = Map.TileSize % Speed;
                    moveInDirection(remainer);
                    moving = false;
                }
                else
                {
                    Step();
                }
            }
            i++;
        }

        public void TurnLeft()
        {
            Direction = (Direction + 3) % 4;
        }

        public void TurnRight()
        {
            Direction = (Direction + 1) % 4;
        }

        public bool CanGoStraight()
        {
            Point next = nextTile();
            return (Map.IsFreeForMonster(next.X, next.Y, this));
        }

        public bool CanGoLeft()
        {
            TurnLeft();
            bool result = CanGoStraight();
            TurnRight();
            return result;
        }

        public bool CanGoRight()
        {
            TurnRight();
            bool result = CanGoStraight();
            TurnLeft();
            return result;
        }

        public bool CanGoBack()
        {
            TurnRight();
            TurnRight();
            bool result = CanGoStraight();
            TurnRight();
            TurnRight();
            return result;
        }

        public void GoStraight()
        {
                movingWhere = nextTile();
                movingFrom.X = Row;
                movingFrom.Y = Col;
                Row = movingWhere.X;
                Col = movingWhere.Y;
                moving = true;
                movesMade = 0;
        }

        /// <summary>
        /// Vrací další políčko podle směru, kterým je příšera aktuálně natočená
        /// </summary>
        /// <returns></returns>
        Point nextTile()
        {
            switch (Direction)
            {
                case 0:
                    return (new Point(Row, Col - 1));                    
                case 1:
                    return (new Point(Row-1, Col));                    
                case 2:
                    return (new Point(Row, Col + 1));
                case 3:
                    return (new Point(Row + 1, Col));
                default:
                    return (new Point(Row, Col));
            }
        }
    }
}
