using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Bomberman.NullFX.Win32;
using System.Windows.Forms;

namespace Bomberman.MainClasses
{
    public enum MapState { Running, Loss, Win, Start, Waiting };

    class Map
    {   
        public int TileSize = 50; //rozměr jednoho políčka v pixelech
        public MapState State;       
        public HeroState HeroState; //stav svého hrdiny
        public bool Paused;
        public int CurrentScore; //skóre dosažené pouze v tomto levelu
        public bool FinalLevel = false; //Hraje se poslední level?
        public string LevelName;
        public bool HeroIsDead = false;
        public int HeroDeath; // Jakým způsobem umřel hrdina. 0 = fire, 1 = Monster1, 2 = Monster2, 3 = Monster3, 4 = time

        private int width; // Šířka herního plánu v políčkách
        private int height; // Výška
        private char[,] map; // W = pevná zeď, X = dočasná zeď, O = volno, E = exit, B = bomba, ? = bonus
        private Hero hero;
        private Bitmap[] bonusIcons = new Bitmap[5];
        private List<Bomb> activeBombs = new List<Bomb>(); //nevybuchlé bomby
        private List<Monster> monsters = new List<Monster>(); //živé příšery
        private List<Monster> deadMonsters = new List<Monster>(); //mrtvé příšery (kvůli vykreslování)
        private List<FireTile> fireTiles = new List<FireTile>(); //políčka, kde je oheń z vybuchlé bomby
        private int fireTileTime = 30; //Jak dlouho má být políčko s ohněm aktivní
        private Bonus bonus;
        private Exit exit;
        private int bombFireTime = 180; //kolik "tiků" timeru má uplynout, než bomba vybuchne             
        private int deadMonsterTime = 30; //jak dlouho se má vykreslovat mrtvá příšera
        private int scoreTime = 100; //jak dlouho se zobrazuje skóre za mrtvou příšeru
        private int tilesRemaining; //jen pro poslední kolo - kolik dočasných zdí zbývá (nepočítá se zeď nad bonusem a exitem)
        private int imunity = 40; //jen pro poslední kolo - jak dlouho má být příšera nezničitelná po svém vzniku

        //obrázky
        private Bitmap bmpBackground = Bomberman.Properties.Resources.background;
        private Bitmap bmpBomb = Bomberman.Properties.Resources.brain;
        private Bitmap bmpFireCenter = Bomberman.Properties.Resources.fireCenter;
        private Bitmap bmpFireHorizontal = Bomberman.Properties.Resources.fireHorizontal;
        private Bitmap bmpFireVertical = Bomberman.Properties.Resources.fireVertical;
        private Bitmap bmpFireUp = Bomberman.Properties.Resources.fireUp;
        private Bitmap bmpFireDown = Bomberman.Properties.Resources.fireDown;
        private Bitmap bmpFireLeft = Bomberman.Properties.Resources.fireLeft;
        private Bitmap bmpFireRight = Bomberman.Properties.Resources.fireRight;
        private Bitmap bmpFireWall = Bomberman.Properties.Resources.fireWall;
        private Bitmap bmpExitClosed = Bomberman.Properties.Resources.exitClosed;
        private Bitmap bmpExitOpened = Bomberman.Properties.Resources.exitOpened;
        private Bitmap bmpWall = Bomberman.Properties.Resources.wall;
        private Bitmap bmpTempWall = Bomberman.Properties.Resources.tempWall;
        private Bitmap bmpMonster1 = Bomberman.Properties.Resources.monster1;
        private Bitmap bmpMonster2 = Bomberman.Properties.Resources.monster2;
        private Bitmap bmpMonster3 = Bomberman.Properties.Resources.monster3;
        private Bitmap bmpMonster1Dead = Bomberman.Properties.Resources.monster1dead;
        private Bitmap bmpMonster2Dead = Bomberman.Properties.Resources.monster2Dead;
        private Bitmap bmpMonster3Dead = Bomberman.Properties.Resources.monster3Dead;
        private Bitmap bmpMatfyzak = Bomberman.Properties.Resources.matfyzak;
        private Bitmap bmpMatfyzacka = Bomberman.Properties.Resources.matfyzacka;

        enum PressedKey { Down, Up, Left, Right, Fire, Pause, None }

        /// <summary>
        /// Vytvoří nový level.
        /// </summary>
        /// <param name="LevelName">Název levelu zobrazující se před začátkem levelu</param>
        /// <param name="Height">Výška bez vnějších zdí</param>
        /// <param name="Width">Šířka bez vnějších zdí</param>
        /// <param name="TemporaryWalls">Počet dočasných zdí</param>
        /// <param name="BonusType">typ bonusu;  0 = fire, 1 = bomb, 2 = life, 3 = speed</param>
        /// <param name="MonstersCount">počet příšer</param>
        /// <param name="MonsterTypes">typy příšer</param>
        /// <param name="HeroState">stav hrdiny</param>
        /// <param name="Character">0 = matfyzák, 1 = matfyzačka</param>
        public Map(string LevelName, int Height, int Width, int TemporaryWalls, 
            int BonusType, int MonstersCount, int[] MonsterTypes, HeroState HeroState, int Character)
        {
            this.LevelName = LevelName;
            this.height = Height + 2;
            this.width = Width + 2;
            tilesRemaining = TemporaryWalls;
            fillMap(TemporaryWalls, BonusType);
            
            loadBonusIcons();

            createMonsters(MonstersCount, MonsterTypes);

            if (Character == 0)
            {
                this.hero = new Hero(50, 50, bmpMatfyzak, HeroState);
            }
            else
            {
                this.hero = new Hero(50, 50, bmpMatfyzacka, HeroState);
            }
        }
         
        /// <summary>
        /// Zaktualizuje HeroState.
        /// Pohne hrdinou a příšerami.
        /// Zjistí, jestli nějaká příšera zabila hrdinu.
        /// Zjistí, které bomby se mají odpálit a odpálí je.
        /// Zkusí otevřít exit, pokud to jde.
        /// Zabije objekty na aktivních FireTiles a odstraní neaktivní.
        /// Odchytává pauzu.
        /// </summary>
        public void MoveObjects()
        {
            this.HeroState = hero.HeroState;
            moveHero();
            moveMonsters();
            checkMonsters();
            checkBombs();
            tryOpenExit();
            checkFireTiles();
            Paused = isPaused();
            destroyObjectsOnFireTiles();
        }

        /// <summary>
        /// Vykreslí celý herní plán na bitmapu bmp, která je předána hernímu oknu a to ji vykresluje do pictureboxu.
        /// - vykreslí políčka z map
        /// - aktivní fireTiles
        /// - živé příšery
        /// - mrtvé příšery
        /// - skóre za příšery
        /// - hrdinu
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bmp"></param>
        public void Portray(Graphics g, ref Bitmap bmp)
        {
            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        char c = map[y, x];
                        Image image;

                        if (c == '?')
                        {
                            if (this.bonus.State == BonusState.Covered)
                            {
                                image = bmpTempWall;
                            }
                            else
                                image = bonusIcons[this.bonus.Type];
                        }
                        else if (c == 'E')
                        {
                            if (this.exit.State == ExitState.covered)
                            {
                                image = bmpTempWall;
                            }
                            else if (this.exit.State == ExitState.closed)
                            {
                                image = bmpExitClosed;
                            }
                            else
                            {
                                image = bmpExitOpened;
                            }
                        }
                        else if (c == 'O')
                        {
                            image = bmpBackground;
                        }

                        else if (c == 'B')
                        {
                            image = bmpBomb;
                        }

                        else if (c == 'W')
                        {
                            image = bmpWall;
                        }
                        else //if (c == 'X')
                        {
                            image = bmpTempWall;
                        }

                        grfx.DrawImage(image, x * TileSize, y * TileSize);
                    }

                }

                foreach (FireTile fireTile in fireTiles)
                {
                    grfx.DrawImage(fireTile.Bitmap, fireTile.Col * TileSize, fireTile.Row * TileSize);
                }

                foreach (Monster monster in monsters)
                {
                    grfx.DrawImage(monster.Image, monster.Y, monster.X);
                    if (monster.Imunity > 0)
                        monster.Imunity--;
                }
                List<Monster> toDestroy = new List<Monster>();
                foreach (Monster monster in deadMonsters)
                {
                    monster.TimeDead++;
                    if (monster.TimeDead == deadMonsterTime + scoreTime)
                    {
                        toDestroy.Add(monster);
                    }
                    else if (monster.TimeDead < deadMonsterTime)
                    {
                        grfx.DrawImage(monster.ImageDead, monster.Y, monster.X);
                    }
                    else if (monster.TimeDead < deadMonsterTime + scoreTime)
                    {
                        grfx.DrawString(monster.Score.ToString(), new Font("Comic Sans MS", 18), Brushes.Red, monster.Y + monster.Image.Width / 4, monster.X + monster.Image.Height / 4);
                    }
                }
                foreach (Monster monster in toDestroy)
                {
                    deadMonsters.Remove(monster);
                }

                if (HeroIsDead)
                {
                    hero.Bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    grfx.DrawImage(hero.Bitmap, hero.Y, hero.X);
                }
                else
                {
                    grfx.DrawImage(hero.Bitmap, hero.Y, hero.X);
                }
            }
        }
        
        public bool IsBomb(int Row, int Col)
        {
            return (map[Row, Col] == 'B');
        }

        public bool IsFreeForMonster(int Row, int Col, Monster Monster)
        {
            if ((Monster is Monster3) && (map[Row, Col] == 'X'))
                return true;
            return ((map[Row, Col] == 'O') || (map[Row, Col] == 'E' && exit.State != ExitState.covered)
                || (map[Row, Col] == '?' && bonus.State != BonusState.Covered));
        }

        /// <summary>
        /// Rozmístí políčka herního plánu. 
        /// Vytvoří pro tento level exit a bonus.
        /// Pevné zdi mají pevné rozmístění (po obvodu a na sudých pozicích),
        /// dočasné zdi mají jen pevný počet, ale rozmístění je náhodné.
        /// Umístění bonusu a exitu je také náhodné.
        /// </summary>
        /// <param name="TemporaryWallsNumber"></param>
        /// <param name="BonusType"></param>
        private void fillMap(int TemporaryWallsNumber, int BonusType)
        {
            map = new char[height, width];

            char[] array = shuffleTemporaryWalls(TemporaryWallsNumber);

            int k = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1)
                    {
                        map[i, j] = 'W';
                    }

                    else if (j == 0 || j == width - 1)
                    {
                        map[i, j] = 'W';
                    }

                    else if ((i % 2 == 0) && (j % 2 == 0))
                    {
                        map[i, j] = 'W';
                    }
                    else if ((i == 1 && j == 1) || (i == 1 && j == 2) || (i == 2 && j == 1))
                    {
                        map[i, j] = 'O';
                    }

                    else
                    {
                        map[i, j] = array[k];
                        if (array[k] == '?')   // bonus was placed
                        {
                            this.bonus = new Bonus(i, j, BonusType);
                        }
                        else if (array[k] == 'E')
                        {
                            this.exit = new Exit(i, j);
                        }
                        k++;
                    }
                }
            }
        }
       
        private void loadBonusIcons()
        {
            bonusIcons[0] = Bomberman.Properties.Resources.bonusFire;
            bonusIcons[1] = Bomberman.Properties.Resources.bonusBomb;
            bonusIcons[2] = Bomberman.Properties.Resources.bonusLife;
            bonusIcons[3] = Bomberman.Properties.Resources.bonusSpeed;
        }
      
        /// <summary>
        /// Vrátí náhodně promíchané pole, kde jsou dočasné zdi, volná políčka, 1 bonus a 1 exit
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        private char[] shuffleTemporaryWalls(int Count)
        {
            decimal a = (this.width - 2) / 2;
            decimal b = (this.height - 2) / 2;
            int walls = (int)(Math.Floor(a) * Math.Floor(b));
            int total = (width - 2) * (height - 2) - walls - 3;

            char[] array = new char[total];
            array[0] = '?'; //bonus
            array[1] = 'E'; //exit
            for (int i = 2; i < Count + 2; i++)
            {
                array[i] = 'X';
            }
            for (int i = Count + 2; i < total; i++)
            {
                array[i] = 'O';
            }

            //shuffle array
            Random rnd = new Random();
            for (int i = 0; i < total - 1; i++)
            {
                char temp = array[i];
                int j = rnd.Next(i, total - 1);
                array[i] = array[j];
                array[j] = temp;
            }
            return array;
        }

        /// <summary>
        /// Vytvoří určitý počet příšer určitých typů
        /// </summary>
        /// <param name="MonstersCount"></param>
        /// <param name="MonsterTypes"></param>
        private void createMonsters(int MonstersCount, int[] MonsterTypes)
        {
            Random rnd = new Random();
            for (int i = 0; i < MonstersCount; i++)
            {
                PlaceMonster(MonsterTypes[i], rnd);
            }
        }

        /// <summary>
        /// Umístí příšeru daného typu na náhodné volné políčko v mapě, kromě prvního řádku a sloupce.
        /// Hlídá, aby neumístilo dvě přišery na stejné políčko.
        /// </summary>
        /// <param name="MonsterType"></param>
        /// <param name="rnd"></param>
        private void PlaceMonster(int MonsterType, Random rnd)
        {

            int row = rnd.Next(2, height - 1);
            int col = rnd.Next(2, width - 1);
            while (map[row, col] != 'O' || monsterAlreadyHere(row, col))
            {
                row = rnd.Next(2, height - 1);
                col = rnd.Next(2, width - 1);
            }

            switch (MonsterType)
            {
                case 0:
                    monsters.Add(new Monster1(this, row, col, 3, bmpMonster1, bmpMonster1Dead, 0));
                    break;
                case 1:
                    monsters.Add(new Monster2(this, row, col, 3, bmpMonster2, bmpMonster2Dead, 0));
                    break;
                case 2:
                    monsters.Add(new Monster3(this, row, col, 3, bmpMonster3, bmpMonster3Dead, 0));
                    break;
            }
        }

        /// <summary>
        /// Pouze pro umisťování příšer v posledním levelu. 
        /// Příšera se umístí na políčko právě zničené zdi a nastaví se jí imunita, aby hned neumřela.
        /// </summary>
        /// <param name="MonsterType"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        private void PlaceMonster(int MonsterType, int Row, int Col)
        {
            switch (MonsterType)
            {
                case 0:
                    monsters.Add(new Monster1(this, Row, Col, 3, bmpMonster1, bmpMonster1Dead, imunity));
                    break;
                case 1:
                    monsters.Add(new Monster2(this, Row, Col, 3, bmpMonster2, bmpMonster2Dead, imunity));
                    break;
                case 2:
                    monsters.Add(new Monster3(this, Row, Col, 3, bmpMonster3, bmpMonster3Dead, imunity));
                    break;
            }
        }

        /// <summary>
        /// Zjistí, jestli je v listu monsters už příšera na daném políčku
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <returns></returns>
        private bool monsterAlreadyHere(int Row, int Col)
        {
            Monster monster = monsters.Find(x => Matches(Row, Col, x));
            if (monster == null) return false;
            else return true;
        }

        /// <summary>
        /// Podle stisknuté klávesy se pokusit obrázek hrdiny posunout daným směrem, pokud je tam volno.
        /// Přepočítá řádek a sloupec, kde se nachází hrdina.
        /// </summary>
        private void moveHero()
        {
            if (KeyboardInfo.GetKeyState(Keys.Space).IsPressed)
                placeBomb();

            PressedKey pressedKey = findPressedKey();

            switch (pressedKey)
            {

                case PressedKey.Down:
                    if (isNotCollision(hero.X + hero.HeroState.Speed + hero.Bitmap.Height, hero.Y + 5) &&
                        isNotCollision(hero.X + hero.HeroState.Speed + hero.Bitmap.Height, hero.Y + hero.Bitmap.Width - 5))
                        hero.X = hero.X + hero.HeroState.Speed;
                    break;
                case PressedKey.Up:
                    if (isNotCollision(hero.X - hero.HeroState.Speed, hero.Y + 5) &&
                        isNotCollision(hero.X - hero.HeroState.Speed, hero.Y + hero.Bitmap.Width - 5))
                        hero.X = hero.X - hero.HeroState.Speed;
                    break;
                case PressedKey.Left:
                    if (isNotCollision(hero.X + 10, hero.Y - hero.HeroState.Speed) &&
                        isNotCollision(hero.X + hero.Bitmap.Height, hero.Y - hero.HeroState.Speed))
                        hero.Y = hero.Y - hero.HeroState.Speed;
                    break;
                case PressedKey.Right:
                    if (isNotCollision(hero.X + 10, hero.Y + hero.HeroState.Speed + hero.Bitmap.Width) &&
                        isNotCollision(hero.X + hero.Bitmap.Height, hero.Y + hero.HeroState.Speed + hero.Bitmap.Width))
                        hero.Y = hero.Y + hero.HeroState.Speed;
                    break;
                case PressedKey.None:
                    break;
                default:
                    break;
            }

            hero.Row = ((hero.X + hero.Bitmap.Height / 2) / TileSize);
            hero.Col = ((hero.Y + hero.Bitmap.Width / 2) / TileSize);
        }

        /// <summary>
        /// Zjišťuje stisknuté klávesy.
        /// </summary>
        /// <returns></returns>
        private PressedKey findPressedKey()
        {
            KeyStateInfo down = KeyboardInfo.GetKeyState(Keys.Down);
            KeyStateInfo up = KeyboardInfo.GetKeyState(Keys.Up);
            KeyStateInfo left = KeyboardInfo.GetKeyState(Keys.Left);
            KeyStateInfo right = KeyboardInfo.GetKeyState(Keys.Right);
            KeyStateInfo fire = KeyboardInfo.GetKeyState(Keys.Space);
            KeyStateInfo pause = KeyboardInfo.GetKeyState(Keys.P);
            if (down.IsPressed)
            {
                return PressedKey.Down;
            }
            else if (up.IsPressed)
            {
                return PressedKey.Up;
            }
            else if (left.IsPressed)
            {
                return PressedKey.Left;
            }
            else if (right.IsPressed)
            {
                return PressedKey.Right;
            }
            else if (fire.IsPressed)
            {
                return PressedKey.Fire;
            }
            else if (pause.IsPressed)
            {
                return PressedKey.Pause;
            }

            else return PressedKey.None;
        }

        /// <summary>
        /// Zjistí, jestli na daných souřadnicích x, y je volno pro hrdinu a může tam být posunut jeho obrázek.
        /// Ošetřuje situaci, kdy hrdina položil bombu a stojí na ní.
        /// Hlídá, jestli hrdina nevstoupil do exitu nebo nevzal bonus a pokud ano, provede příslušné akce.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        private bool isNotCollision(int X, int Y)
        {
            int row = (X / TileSize);
            int col = (Y / TileSize);

            if (hero.StandingOnBomb)
            {
                if (hero.StandingOnBomb)
                {
                    if (row == hero.BombWhereStanding.X && col == hero.BombWhereStanding.Y)
                    {
                        return true;
                    }

                    else if ((row != hero.BombWhereStanding.X || col != hero.BombWhereStanding.Y) && (hero.Row != hero.BombWhereStanding.X || hero.Col != hero.BombWhereStanding.Y))
                    {
                        hero.StandingOnBomb = false;
                    }

                }
            }

            if (isBonus(row, col) && bonus.State == BonusState.Discovered)
            {
                hero.TakeBonus(bonus.Type);
                bonus.State = BonusState.Destroyed;
                map[row, col] = 'O';
            }

            else if (isExit(row, col) && exit.State == ExitState.closed)
            {
                return true;
            }
            else if (isExit(row, col) && exit.State == ExitState.opened)
            {
                if (hero.X >= row * TileSize
                    && hero.X + hero.Bitmap.Height <= (row + 1) * TileSize
                    && hero.Y >= col * TileSize
                    && hero.Y + hero.Bitmap.Width <= (col + 1) * TileSize)
                {
                    this.State = MapState.Win;
                }
                return true;
            }

            return (isFree(row, col));
        }

        /// <summary>
        /// Zjistí, které bomby ze seznamu activeBombs mají vybuchnout.
        /// Odpálí je a odstraní ze seznamu a z mapy.
        /// Přidá hrdinovi aktivní bombu.
        /// </summary>
        private void checkBombs()
        {
            List<Bomb> toRemove = new List<Bomb>();
            foreach (Bomb bomb in activeBombs)
            {
                bomb.CurrentTime++;
                if (bomb.CurrentTime >= bombFireTime)
                {
                    fire(bomb);
                    toRemove.Add(bomb);
                    map[bomb.Row, bomb.Col] = 'O';
                    if (hero.BombsAvailable < hero.HeroState.BombsMaximum)
                    {
                        hero.BombsAvailable++;
                    }
                }
            }
            foreach (Bomb bomb in toRemove)
                activeBombs.Remove(bomb);
        }

        /// <summary>
        /// Zničí toto políčko.
        /// Šíří oheň do 4 směrů, volá destroy, dokud se oheň nezastaví a dokud stačí dosah.
        /// </summary>
        /// <param name="bomb"></param>
        private void fire(Bomb bomb)
        {
            bool down = true;
            bool up = true;
            bool left = true;
            bool right = true;

            fireTiles.Add(new FireTile(bomb.Row, bomb.Col, 0, bmpFireCenter));
            if (hero.Col == bomb.Col && hero.Row == bomb.Row)
            {
                HeroIsDead = true;
                HeroDeath = 0;
            }

            for (int i = 1; i < hero.HeroState.FireRange; i++)
            {
                if (down == true)
                {
                    down = destroy(bomb.Row + i, bomb.Col, bmpFireVertical); // destroy vraci bool, jestli ma ohen pokracovat dal
                }
                if (up == true)
                {
                    up = destroy(bomb.Row - i, bomb.Col, bmpFireVertical);
                }
                if (left == true)
                {
                    left = destroy(bomb.Row, bomb.Col - i, bmpFireHorizontal);
                }
                if (right == true)
                {
                    right = destroy(bomb.Row, bomb.Col + i, bmpFireHorizontal);
                }
            }

            //poslední políčko, kam oheň dosáhne. Vykreslení odlišného obrázku.
            int j = hero.HeroState.FireRange;
            {
                if (down == true)
                {
                    down = destroy(bomb.Row + j, bomb.Col, bmpFireDown);
                }
                if (up == true)
                {
                    up = destroy(bomb.Row - j, bomb.Col, bmpFireUp);
                }
                if (left == true)
                {
                    left = destroy(bomb.Row, bomb.Col - j, bmpFireLeft);
                }
                if (right == true)
                {
                    right = destroy(bomb.Row, bomb.Col + j, bmpFireRight);
                }
            }
        }

        /// <summary>
        /// Pokud jde tohle políčko zničit bombou, zničí daný objekt a přidá do seznamu fireTiles toto políčko s obrázkem, který se má vykreslit (podle směru, kterým jde oheň)
        /// Může odkrýt bonus a exit.
        /// V posledním levelu vytváří příšery místo políček.
        /// Vrací bool, jestli se má oheň šířit dál tímto směrem
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="Bmp"></param>
        /// <returns></returns>
        private bool destroy(int Row, int Col, Bitmap Bmp)
        {
            if (hero.Col == Col && hero.Row == Row)
            {
                HeroIsDead = true;
                HeroDeath = 0;
            }

            if (map[Row, Col] == 'W')
            {
                return false;
            }
            else if (map[Row, Col] == 'O')
            {
                fireTiles.Add(new FireTile(Row, Col, 0, Bmp));
                return true;
            }
            else if (map[Row, Col] == 'X')
            {
                tilesRemaining--;
                fireTiles.Add(new FireTile(Row, Col, 1, bmpFireWall));
                if (FinalLevel) //v posledním levelu se pod políčkem může vytvořit příšera
                {
                    Random rnd = new Random();
                    int a = rnd.Next(1, 3);
                    if (a == 2)
                    {
                        PlaceMonster(rnd.Next(0, 3), Row, Col);
                    }
                }
                return false;
            }
            else if (map[Row, Col] == 'B') //bomba aktivuje jinou bombu
            {
                Bomb bombToDestroy = activeBombs.Find(x => Matches(Row, Col, x));
                bombToDestroy.CurrentTime = bombFireTime - 10;
                return false;
            }
            else if (map[Row, Col] == '?' && bonus.State == BonusState.Covered)
            {
                bonus.State = BonusState.Discovered;
                return false;
            }
            else if (map[Row, Col] == '?' && bonus.State == BonusState.Discovered)
            {
                fireTiles.Add(new FireTile(Row, Col, 0, Bmp));
                map[Row, Col] = 'O';
                bonus.State = BonusState.Destroyed;
                return true;
            }
            else if (map[Row, Col] == 'E' && exit.State == ExitState.covered)
            {
                exit.State = ExitState.closed;
                return false;
            }
            else if (map[Row, Col] == 'E')
            {
                fireTiles.Add(new FireTile(Row, Col, 0, Bmp));
                return true;
            }
            else return true;
        }

        private bool isExit(int Row, int Col)
        {
            return (map[Row, Col] == 'E');
        }  

        private bool isBonus(int Row, int Col)
        {
            return (map[Row, Col] == '?');
        }     

        private bool isFree(int Row, int Col)
        {
            return (map[Row, Col] == 'O');
        }

        static bool Matches(int Row, int Col, Bomb Bomb)
        {
            return (Row == Bomb.Row && Col == Bomb.Col);
        }

        static bool Matches(int Row, int Col, Monster Monster)
        {
            return (Row == Monster.Row && Col == Monster.Col);
        }

        /// <summary>
        /// Odstraní příšeru ze seznamu živých příšer a přidá ji do mrtvých.
        /// Přičte skóre za tuto příšeru.
        /// </summary>
        /// <param name="monster"></param>
        private void destroyMonster(Monster monster)
        {
            if (monster.Imunity == 0)
            {
                monsters.Remove(monster);
                deadMonsters.Add(monster);
                monster.TimeDead = 0;
                CurrentScore = CurrentScore + monster.Score;
            }
        }

        /// <summary>
        /// Zjistí, jestli nebyly splněny podmínky pro otevření exitu a pokud ano, otevře ho.
        /// </summary>
        private void tryOpenExit()
        {
            if (FinalLevel)
            {
                if (exit.State == ExitState.closed && tilesRemaining == 0 && monsters.Count == 0)
                {
                    exit.State = ExitState.opened;
                    if (bonus.State == BonusState.Covered)
                    {
                        bonus.State = BonusState.Discovered;
                    }
                }
            }
            else
                if (exit.State == ExitState.closed && monsters.Count == 0) //ToDo time OK
                {
                    exit.State = ExitState.opened;
                    if (bonus.State == BonusState.Covered)
                    {
                        bonus.State = BonusState.Discovered;
                    }
                }
        }

        /// <summary>
        /// Zjistí, jestli je stisknutá klávesa P.
        /// </summary>
        /// <returns></returns>
        private bool isPaused()
        {
            return (findPressedKey() == PressedKey.Pause);
        }   

        /// <summary>
        /// Zjistí, jestli nějaká příšera zabila hrdinu.
        /// </summary>
        private void checkMonsters()
        {
            foreach (Monster monster in monsters)
            {
                if (heroTouches(monster))
                {
                    HeroIsDead = true;
                    if (monster is Monster1)
                        HeroDeath = 1;
                    else if (monster is Monster2)
                        HeroDeath = 2;
                    else
                        HeroDeath = 3;
                }
            }
        }

        /// <summary>
        /// Překrývá se obrázek hrdiny s obrázkem příšery?
        /// </summary>
        /// <param name="Monster"></param>
        /// <returns></returns>
        private bool heroTouches(Monster Monster)
        {
            int tolerance = 5;
            return (
                 ((hero.X + hero.Bitmap.Height - tolerance >= Monster.X && hero.X + hero.Bitmap.Height - tolerance <= Monster.X + Monster.Image.Height)
                  || (hero.X + tolerance >= Monster.X && hero.X + tolerance <= Monster.X + Monster.Image.Height))
                &&
                 ((hero.Y + hero.Bitmap.Width - tolerance >= Monster.Y && hero.Y + hero.Bitmap.Width - tolerance <= Monster.Y + Monster.Image.Width)
                  || (hero.Y + tolerance >= Monster.Y && hero.Y + tolerance <= Monster.Y + Monster.Image.Width))
               );
        }

        /// <summary>
        /// Odstraní FireTiles, kterým už vypršela doba.
        /// </summary>
        private void checkFireTiles()
        {
            List<FireTile> toDelete = new List<FireTile>();
            foreach (FireTile fireTile in fireTiles)
            {
                if (fireTile.Time <= fireTileTime)
                {
                    fireTile.Time++;
                }
                else if (fireTile.Time > fireTileTime)
                {
                    toDelete.Add(fireTile);
                }
            }
            foreach (FireTile fireTile in toDelete)
            {
                if (fireTile.Type == 1)
                {
                    map[fireTile.Row, fireTile.Col] = 'O';
                }
                fireTiles.Remove(fireTile);
            }
        }

        /// <summary>
        /// Zničí zničitelné objekty na aktivních FireTiles
        /// </summary>
        private void destroyObjectsOnFireTiles()
        {
            foreach (FireTile fireTile in fireTiles)
            {
                if (hero.Col == fireTile.Col && hero.Row == fireTile.Row)
                {
                    HeroIsDead = true;
                    HeroDeath = 0;
                }

                List<Monster> toDestroy = new List<Monster>();
                foreach (Monster monster in monsters)
                {
                    if (monsterDiesOn(monster, fireTile.Row, fireTile.Col))
                    {
                        toDestroy.Add(monster);
                    }
                }
                foreach (Monster monster in toDestroy)
                {
                    destroyMonster(monster);
                }
            }
        }

        /// <summary>
        /// Zjistí, jestli na tomhle políčku má příšera umřít
        /// </summary>
        /// <param name="Monster"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <returns></returns>
        private bool monsterDiesOn(Monster Monster, int Row, int Col)
        {
            int tol = 7; // tolerance, how many pixels of monster image on the boarder are not affected with firetile

            return (
                (((Monster.X + tol >= Row * TileSize) && (Monster.X + tol <= (Row + 1) * TileSize))
                    || ((Monster.X + Monster.Image.Height - tol >= Row * TileSize) && (Monster.X + Monster.Image.Height - tol <= (Row + 1) * TileSize)))
                    &&
                (((Monster.Y + tol >= Col * TileSize) && (Monster.Y + tol <= (Col + 1) * TileSize))
                    || ((Monster.Y + Monster.Image.Width - tol >= Col * TileSize) && (Monster.Y + Monster.Image.Width - tol <= (Col + 1) * TileSize)))
               );
        }

        /// <summary>
        /// Pohne se všemi živými příšerami
        /// </summary>
        private void moveMonsters()
        {
            foreach (Monster monster in monsters)
            {
                monster.Move();
            }
        }

        /// <summary>
        /// Umístí bombu na políčko, kde je hrdina.
        /// Ubere hrdinovi bombu.
        /// Nastaví hrdinovi, že stojí na bombě.
        /// </summary>
        private void placeBomb()
        {
            int mapRow = ((hero.X + hero.Bitmap.Height / 2) / TileSize);
            int mapCol = ((hero.Y + hero.Bitmap.Width / 2) / TileSize);
            if (isFree(mapRow, mapCol) && hero.BombsAvailable > 0)
            {
                map[mapRow, mapCol] = 'B';
                activeBombs.Add(new Bomb(mapRow, mapCol));
                hero.BombsAvailable--;
                hero.StandingOnBomb = true;
                hero.BombWhereStanding = new Point(mapRow, mapCol);
            }
        }

    }
}
