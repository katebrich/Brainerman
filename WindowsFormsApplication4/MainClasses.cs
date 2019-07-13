using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Bomberman.NullFX.Win32;
using System.Windows.Forms;
using System.Media;

namespace WindowsFormsApplication4
{
   /*
    abstract class Monster
    {
        public int Row;
        public int Col;
        public int Direction;
        public Map Map;
        public int Speed;
        public Bitmap Image;
        public Bitmap ImageDead;
        private bool moving;
        private int movesMade;
        public int x;
        public int y;
        private int i = 0;
        public int TimeDead;
        public int Score;
        private Point movingWhere;
        private Point movingFrom;
        public int Imunity;

        public abstract void Step();

        private void moveInDirection(int howMuch)
        {
            switch (Direction)
            {
                case 0:
                    y = y - howMuch;
                    break;
                case 1:
                    x = x - howMuch;
                    break;
                case 2:
                    y = y + howMuch;
                    break;
                case 3:
                    x = x + howMuch;
                    break;
                default:
                    break;
                    
            }
        }

        public void Move()
        {
            if (i % 2 == 0)   //for slower speed
            {
                if (moving & Map.IsBomb(movingWhere.X, movingWhere.Y))
                {
                    Row = movingFrom.X;
                    Col = movingFrom.Y;
                    x = movingFrom.X * Map.fieldSize;
                    y = movingFrom.Y * Map.fieldSize;
                    moving = false;
                }

                else if (moving && movesMade < Map.fieldSize / Speed)
                {
                    moveInDirection(Speed);
                    movesMade++;
                }

                else if (moving && movesMade == Map.fieldSize / Speed) // in case less than Speed pixels remaining for moving to another field
                {
                    int remainer = Map.fieldSize % Speed;
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
            Point next = nextField();
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
                movingWhere = nextField();
                movingFrom.X = Row;
                movingFrom.Y = Col;
                Row = movingWhere.X;
                Col = movingWhere.Y;
                moving = true;
                movesMade = 0;
        }

        Point nextField()
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

    class Monster1 : Monster
    {

        public Monster1(Map map, int row, int col, int direction, Bitmap bitmap, Bitmap bitmapDead, int imunity) // potrebuje map?
        {
            this.Map = map;
            this.Row = row;
            this.Col = col;
            this.x = Row * map.fieldSize;
            this.y = Col * map.fieldSize;
            this.Direction = direction;
            this.Speed = 1;
            this.Image = bitmap;
            this.ImageDead = bitmapDead;
            this.Score = 4;
            this.Imunity = imunity;
        }

        public override void Step()
        {
            {
                if (CanGoStraight())
                {
                    GoStraight();
                    Move();
                }
                else
                {
                    Random rnd = new Random();
                    int[] possibilities = new int[30];
                    int count = 0;
                    if (CanGoBack())
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
                                TurnLeft();
                                TurnLeft();
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

    class Monster2 : Monster
    {
        public Monster2(Map map, int row, int col, int direction, Bitmap bitmap, Bitmap bitmapDead, int imunity)
        {
            this.Map = map;
            this.Row = row;
            this.Col = col;
            this.x = Row * map.fieldSize;
            this.y = Col * map.fieldSize;
            this.Direction = direction;
            this.Speed = 2;
            this.Image = bitmap;
            this.ImageDead = bitmapDead;
            this.Score = 7;
            this.Imunity = imunity;
        }

        public override void Step()
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

            for (int i = 0; i < count; i++)
            {
                int temp = possibilities[i];
                int j = rnd.Next(i, count);
                possibilities[i] = possibilities[j];
                possibilities[j] = temp;
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

    class Monster3 : Monster
    {
        public Monster3(Map map, int row, int col, int direction, Bitmap bitmap, Bitmap bitmapDead, int imunity)
        {
            this.Map = map;
            this.Row = row;
            this.Col = col;
            this.x = Row * map.fieldSize;
            this.y = Col * map.fieldSize;
            this.Direction = direction;
            this.Speed = 3;
            this.Image = bitmap;
            this.ImageDead = bitmapDead;
            this.Score = 14;
            this.Imunity = imunity;
        }

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

    class HeroState
    {
        public int BombsMaximum = 1;
        public int Speed = 2;
        public int FireRange = 1;
        public int Life = 3;

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

    class Hero
    {
        public int row = 1; //ToDo
        public int col = 1;
        public int x;
        public int y;
        public Bitmap Image;
        public int BombsAvailable;       
        public bool StandingOnBomb = false;
        public HeroState heroState;
        public Point BombWhereStanding;
        
        public Hero(int x, int y, Bitmap image, HeroState heroState)
        {
           this.Image = image;
           this.x = x;
           this.y = y;
           this.heroState = heroState;
           this.BombsAvailable = this.heroState.BombsMaximum;
        }

        public void TakeBonus(int type)
        {
            switch (type)
            {
                case 0:
                    heroState.FireRange++;
                    break;
                case 1:
                    heroState.BombsMaximum++;
                    BombsAvailable++;
                    break;
                case 2:
                    heroState.Life++;
                    break;
                case 3:
                    heroState.Speed = heroState.Speed + 1;
                    break;
            }

        }
    }
  
    class Bomb
    {
        public int ActualTime;
        public int Row;
        public int Col;

        public Bomb(int row, int col)
        {
            this.Row = row;
            this.Col = col;
            this.ActualTime = 0;
        }
    }
   
    enum BonusState {covered, discovered, destroyed};
    class Bonus
    {
        public int Row;
        public int Col;
        public int Type; // 0 = fire, 1 = bomb, 2 = life, 3 = speed
        public BonusState State = BonusState.covered;

        public Bonus(int row, int col, int type)
        {
            this.Row = row;
            this.Col = col;
            this.Type = type;
        }
    }

    enum ExitState { covered, opened, closed };
    class Exit
    {
        public int Row;
        public int Col;
        public ExitState State = ExitState.covered;

        public Exit(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
    }

    class FireTile
    {
        public int Row;
        public int Col;
        public int Time = 0;
        public int Type; // 0 = free space,  = temporary wall
        public Bitmap Bitmap;

        public FireTile(int row, int col, int type, Bitmap bmp)
        {
            this.Row = row;
            this.Col = col;
            this.Type = type;
            this.Bitmap = bmp;
        }
    }

    public enum MapState { Running, Loss, Win, Start, Waiting, Paused };
    class Map
    {
        int Width;
        int Height;
        private char[,] map;
        private Hero hero;
        public int fieldSize;
        Bitmap[] mapIcons;
        Bitmap[] bonusIcons = new Bitmap[5];
        private List<Bomb> activeBombs = new List<Bomb>();
        private List<Monster> Monsters = new List<Monster>();
        private List<Monster> deadMonsters = new List<Monster>();
        public MapState state;
        private Bonus bonus;
        private Exit exit;
        public HeroState HeroState;
        public bool Paused;
        public int ActualScore;
        public bool FinalLevel = false;
       
        private Bitmap bmpBackground = new Bitmap("background.png");
        private Bitmap bmpBomb = new Bitmap("brain.png");
        private Bitmap bmpFireCenter = new Bitmap("fireCenter.png");
        private Bitmap bmpFireHorizontal = new Bitmap("fireHorizontal.png");
        private Bitmap bmpFireVertical = new Bitmap("fireVertical.png");
        private Bitmap bmpFireUp = new Bitmap("fireUp.png");
        private Bitmap bmpFireDown = new Bitmap("fireDown.png");
        private Bitmap bmpFireLeft = new Bitmap("fireLeft.png");
        private Bitmap bmpFireRight = new Bitmap("fireRight.png");
        private Bitmap bmpFireWall = new Bitmap("fireWall2.png"); //
        private Bitmap bmpExitClosed = new Bitmap("exitClosed.png");
        private Bitmap bmpExitOpened = new Bitmap("exitOpened.png"); 
        private Bitmap bmpWall = new Bitmap("wall.png");
        private Bitmap bmpTempWall = new Bitmap("tempWall1.png"); //
        private Bitmap bmpMonster1 = new Bitmap("monster1.png");
        private Bitmap bmpMonster2 = new Bitmap("monster2.png");
        private Bitmap bmpMonster3 = new Bitmap("monster3.png");
        private Bitmap bmpMonster1Dead = new Bitmap("monster1dead.png");
        private Bitmap bmpMonster2Dead = new Bitmap("monster2dead.png"); 
        private Bitmap bmpMonster3Dead = new Bitmap("monster3dead.png");
        private Bitmap bmpMatfyzak = new Bitmap("matfyzak.png");
        private Bitmap bmpMatfyzacka = new Bitmap("matfyzacka.png");

        private int bombFireTime = 180;
        private List<FireTile> fireFields = new List<FireTile>();
        private int fireFieldTime = 30;
        private int deadMonsterTime = 30;
        private int scoreTime = 100;
        public string levelName;
        private int tilesRemaining;

        public bool HeroIsDead = false;
        public int heroDeath; // 0 = fire, 1 = Monster1, 2 = Monster2, 3 = Monster3, 4 = time

        public Map(string path, HeroState heroState, int character)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(path, Encoding.GetEncoding("iso-8859-2"));
            this.levelName = sr.ReadLine();
            this.Height = int.Parse(sr.ReadLine()) + 2;
            this.Width = int.Parse(sr.ReadLine()) + 2;
            tilesRemaining = int.Parse(sr.ReadLine());
            fillMap(tilesRemaining, int.Parse(sr.ReadLine()));

            loadIcons("ikonky.png", ref mapIcons);
          //  loadIcons("powerups.png", ref bonusIcons);
            loadBonusIcons();
            
            int monstersCount = int.Parse(sr.ReadLine());
            int[] monsterTypes = new int[monstersCount];

            for (int i = 0; i < monstersCount; i++)
            {
                monsterTypes[i] = int.Parse(sr.ReadLine());
            }

            CreateMonsters(monstersCount, monsterTypes);

            if (character == 0)
            {
                this.hero = new Hero(50, 50, bmpMatfyzak, heroState);
            }
            else
            {
                this.hero = new Hero(50, 50, bmpMatfyzacka, heroState);
            }
        }

        private void loadBonusIcons()
        {
            bonusIcons[0] = new Bitmap("bonusFire.png");
            bonusIcons[1] = new Bitmap("bonusBomb.png");
            bonusIcons[2] = new Bitmap("bonusLife.png");
            bonusIcons[3] = new Bitmap("bonusSpeed.png");
        }
   
        private void CreateMonsters(int monstersCount, int[] monsterTypes)
        {
            Random rnd = new Random();
            for (int i = 0; i < monstersCount; i++)
            {
                PlaceMonster(monsterTypes[i], rnd);
            }
        }

        private void PlaceMonster(int monsterType, int Row, int Col)
        {
            switch (monsterType)
            {
                case 0:
                    Monsters.Add(new Monster1(this, Row, Col, 3, bmpMonster1, bmpMonster1Dead, 40));
                    break;
                case 1:
                    Monsters.Add(new Monster2(this, Row, Col, 3, bmpMonster2, bmpMonster2Dead, 40));
                    break;
                case 2:
                    Monsters.Add(new Monster3(this, Row, Col, 3, bmpMonster3, bmpMonster3Dead, 40));
                    break;
            }
         }

        private void PlaceMonster(int monsterType, Random rnd)
        {
            
            int row = rnd.Next(2, Height - 1);
            int col = rnd.Next(2, Width - 1);
            while (map[row, col] != 'O' || monsterAlreadyHere(row, col))
            {
                row = rnd.Next(2, Height - 1);
                col = rnd.Next(2, Width - 1);               
            }

            switch (monsterType)
            {
                case 0:
                    Monsters.Add(new Monster1(this, row, col, 3, bmpMonster1, bmpMonster1Dead, 0));
                    break;
                case 1:
                    Monsters.Add(new Monster2(this, row, col, 3, bmpMonster2, bmpMonster2Dead, 0));
                    break;
                case 2:
                    Monsters.Add(new Monster3(this, row, col, 3, bmpMonster3, bmpMonster3Dead, 0));
                    break;
            }
        }

        private bool monsterAlreadyHere(int row, int col)
        {
            Monster monster = Monsters.Find(x => Matches(row, col, x));
            if (monster == null) return false;
            else return true;
        }

        private void fillMap(int temporaryWallsNumber, int bonusType)
        {
            
            map = new char[Height, Width];

            char[] array = shuffleTemporaryWalls(temporaryWallsNumber);

            int k = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == 0 || i == Height-1)
                    {
                        map[i, j] = 'W';
                    }

                    else if (j == 0 || j == Width-1)
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
                            this.bonus = new Bonus(i, j, bonusType);
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

        public void loadIcons(string path, ref Bitmap[] icons) //ToDo
        {
            Bitmap bmp = new Bitmap(path);
            this.fieldSize = bmp.Height;
            int count = bmp.Width / fieldSize; // predpokladam, ze to jsou kosticky v rade
            icons = new Bitmap[count];
            for (int i = 0; i < count; i++)
            {
                Rectangle rect = new Rectangle(i * fieldSize, 0, fieldSize, fieldSize);
                icons[i] = bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.DontCare);
            }
        }
        
        enum PressedKey {down, up, left, right, fire, pause, none}

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
                return PressedKey.down;
            }
            else if (up.IsPressed)
            {
                return PressedKey.up;
            }
            else if (left.IsPressed)
            {
                return PressedKey.left;
            }
            else if (right.IsPressed)
            {
                return PressedKey.right;
            }
            else if (fire.IsPressed)
            {   
                return PressedKey.fire; 
            }
            else if (pause.IsPressed)
            {
                return PressedKey.pause;
            }

            else return PressedKey.none;
        }

        private bool isNotCollision(int x, int y) //ToDo
        {
            int mapRow = (x  / fieldSize) ;
            int mapCol = (y  / fieldSize) ;

            if (hero.StandingOnBomb)
            {
                if (hero.StandingOnBomb)
                {
                    if (mapRow == hero.BombWhereStanding.X && mapCol == hero.BombWhereStanding.Y)
                    {
                        return true;
                    }

                    else if ((mapRow != hero.BombWhereStanding.X || mapCol != hero.BombWhereStanding.Y) && (hero.row != hero.BombWhereStanding.X || hero.col != hero.BombWhereStanding.Y) )
                    {
                        hero.StandingOnBomb = false;
                    }
                        
                }
            }

            if (IsBonus(mapRow, mapCol) && bonus.State == BonusState.discovered)
            {
                hero.TakeBonus(bonus.Type);
                bonus.State = BonusState.destroyed;
                map[mapRow, mapCol] = 'O';
            }

            else if (IsExit(mapRow, mapCol) && exit.State == ExitState.closed)
            {
                return true;
            }
            else if (IsExit(mapRow, mapCol) && exit.State == ExitState.opened)
            {
                if (hero.x >= mapRow*fieldSize 
                    && hero.x+hero.Image.Height <= (mapRow+1)*fieldSize
                    && hero.y >= mapCol*fieldSize
                    && hero.y+hero.Image.Width <= (mapCol+1)*fieldSize)
                {
                    this.state = MapState.Win;
                }
                return true;
            }

            return ( isFree(mapRow, mapCol) );
        }

        private bool IsExit(int row, int col)
        {
            return (map[row, col] == 'E');
        }

        public bool IsBomb(int row, int col)
        {
            return (map[row, col] == 'B');
        }

        private bool IsBonus(int row, int col)
        {
            return (map[row, col] == '?');
        }

        public bool IsFreeForMonster(int row, int col, Monster monster)
        {
            if ((monster is Monster3) && (map[row, col] == 'X'))
                return true;
            return ( (map[row, col] == 'O') || (map[row, col] == 'E' && exit.State != ExitState.covered) 
                || (map[row, col] == '?' && bonus.State != BonusState.covered) );
        }

        private bool isFree(int row, int col)
        {
            return (map[row, col] == 'O');
        }

        private void moveHero()
        {
            if (KeyboardInfo.GetKeyState(Keys.Space).IsPressed)
                placeBomb();
            
            PressedKey pressedKey = findPressedKey();

            switch (pressedKey)
            {
                
                case PressedKey.down:
                    if (isNotCollision(hero.x + hero.heroState.Speed + hero.Image.Height, hero.y + 5) &&
                        isNotCollision(hero.x + hero.heroState.Speed + hero.Image.Height, hero.y + hero.Image.Width - 5))
                        hero.x = hero.x + hero.heroState.Speed;
                    break;
                case PressedKey.up:
                    if (isNotCollision(hero.x - hero.heroState.Speed, hero.y + 5) &&
                        isNotCollision(hero.x - hero.heroState.Speed, hero.y + hero.Image.Width - 5))
                        hero.x = hero.x - hero.heroState.Speed;
                    break;
                case PressedKey.left:
                    if (isNotCollision(hero.x + 10, hero.y - hero.heroState.Speed) &&
                        isNotCollision(hero.x + hero.Image.Height, hero.y - hero.heroState.Speed))
                        hero.y = hero.y - hero.heroState.Speed;
                    break;
                case PressedKey.right:
                    if (isNotCollision(hero.x + 10, hero.y + hero.heroState.Speed + hero.Image.Width) && 
                        isNotCollision(hero.x + hero.Image.Height, hero.y + hero.heroState.Speed + hero.Image.Width))
                            hero.y = hero.y + hero.heroState.Speed;
                    break;
                case PressedKey.none:
                    break;
                default:
                    break;
            }

            hero.row = ( (hero.x + hero.Image.Height / 2) / fieldSize);
            hero.col = ( (hero.y + hero.Image.Width / 2) / fieldSize);
        }

        static bool Matches(int row, int col, Bomb bomb)
        {
            return (row == bomb.Row && col == bomb.Col);
        }

        static bool Matches(int row, int col, Monster monster)
        {
            return (row == monster.Row && col == monster.Col);
        }

        private bool destroy(int row, int col, Bitmap bmp)
        {
            if (hero.col == col && hero.row == row)
            {
                killHero();
                heroDeath = 0;
            }
            
            if (map[row, col] == 'W')
            {
                return false;
            }
            else if (map[row, col] == 'O')
            {
                fireFields.Add(new FireTile(row, col, 0, bmp)); //ToDo asi nebude potreba ten typ 0/1
                return true;
            }
            else if (map[row, col] == 'X')
            {
                tilesRemaining--;
                fireFields.Add(new FireTile(row, col, 1, bmpFireWall));
                if (FinalLevel)
                {
                    Random rnd = new Random();
                    int a = rnd.Next(1, 3);
                    if (a == 2)
                    {
                        PlaceMonster(rnd.Next(0, 3), row, col);
                    }
                }
                return false;
            }
            else if (map[row, col] == 'B')
            {
                Bomb bombToDestroy = activeBombs.Find( x => Matches(row, col, x));
                bombToDestroy.ActualTime = bombFireTime - 10;
                return false;
            }
            else if (map[row, col] == '?' && bonus.State == BonusState.covered)
            {
                bonus.State = BonusState.discovered;
                return false;
            }
            else if (map[row, col] == '?' && bonus.State == BonusState.discovered)
            {
                fireFields.Add(new FireTile(row, col, 0, bmp));
                map[row, col] = 'O';
                bonus.State = BonusState.destroyed;
                return true;
            }
            else if (map[row, col] == 'E' && exit.State == ExitState.covered)
            {
                exit.State = ExitState.closed;
                return false;
            }
            else if (map[row, col] == 'E')
            {
                fireFields.Add(new FireTile(row, col, 0, bmp));
                return true;
            }
            else return true;
        }

        private void destroyMonster(Monster monster)
        {
            if (monster.Imunity == 0)
            {
                Monsters.Remove(monster);
                deadMonsters.Add(monster);
                monster.TimeDead = 0;
                ActualScore = ActualScore + monster.Score;
            }
        }

        private void fire(Bomb bomb)
        {
            bool down = true;
            bool up = true;
            bool left = true;
            bool right = true;

            fireFields.Add(new FireTile(bomb.Row, bomb.Col, 0, bmpFireCenter));
            if (hero.col == bomb.Col && hero.row == bomb.Row)
            {
                killHero();
                heroDeath = 0;
            }

            for (int i = 1; i < hero.heroState.FireRange; i++)
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

            
            int j = hero.heroState.FireRange;
            {
                if (down == true)
                {
                    down = destroy(bomb.Row + j, bomb.Col, bmpFireDown); // destroy vraci bool, jestli ma ohen pokracovat dal
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

        private void checkBombs()
        {
            List<Bomb> toRemove = new List<Bomb>();
            foreach (Bomb bomb in activeBombs)
            {
                bomb.ActualTime++;
                if (bomb.ActualTime >= bombFireTime)
                {
                    fire(bomb);
                    toRemove.Add(bomb);
                    map[bomb.Row, bomb.Col] = 'O';
                    if (hero.BombsAvailable < hero.heroState.BombsMaximum)
                    {
                        hero.BombsAvailable++;
                    }
                }
            }
            foreach (Bomb bomb in toRemove)
                activeBombs.Remove(bomb);
        }
       
        private void tryOpenExit()
        {
            if (FinalLevel)
            {
                if (exit.State == ExitState.closed && tilesRemaining == 0 && Monsters.Count == 0)
                {
                    exit.State = ExitState.opened;
                    if (bonus.State == BonusState.covered)
                    {
                        bonus.State = BonusState.discovered;
                    }
                }
            }
            else
            if (exit.State == ExitState.closed && Monsters.Count == 0) //ToDo time OK
            {
                exit.State = ExitState.opened;
                if (bonus.State == BonusState.covered)
                {
                    bonus.State = BonusState.discovered;
                }
            }
        }

        private bool isPaused()
        {
            return (findPressedKey() == PressedKey.pause);
        }

        public void MoveObjects()
        {
            this.HeroState = hero.heroState;   
            moveHero();
            moveMonsters();
            checkMonsters();
            checkBombs();
            tryOpenExit();
            checkFireFields();
            Paused = isPaused();
        }

        private void checkMonsters()
        {
            foreach (Monster monster in Monsters)
            {
                if (heroTouches(monster))
                {
                    killHero();
                    if (monster is Monster1)
                        heroDeath = 1;
                    else if (monster is Monster2)
                        heroDeath = 2;
                    else
                        heroDeath = 3;
                }
            }
        }

        private bool heroTouches(Monster monster)
        {
            int tolerance = 5;
            return (
                 ((hero.x + hero.Image.Height - tolerance >= monster.x && hero.x + hero.Image.Height - tolerance <= monster.x + monster.Image.Height)
                  || (hero.x + tolerance >= monster.x && hero.x + tolerance <= monster.x + monster.Image.Height))
                &&
                 ((hero.y + hero.Image.Width - tolerance >= monster.y && hero.y + hero.Image.Width - tolerance <= monster.y + monster.Image.Width)
                  || (hero.y + tolerance >= monster.y && hero.y + tolerance <= monster.y + monster.Image.Width))
               );
        }

        private void killHero()
        {
             HeroIsDead = true;
        }

        private void checkFireFields()
        {
            List<FireTile> toDelete = new List<FireTile>();
            foreach (FireTile fireField in fireFields)
            {
                if (fireField.Time <= fireFieldTime)
                {
                    fireField.Time++;
                }
                else if (fireField.Time > fireFieldTime)
                {
                    toDelete.Add(fireField);
                }
            }
            foreach (FireTile fireField in toDelete)
            {
                if (fireField.Type == 1)
                {
                    map[fireField.Row, fireField.Col] = 'O';
                }
                fireFields.Remove(fireField);
            }
            
            destroyObjectsOnFireFields();
        }

        private void destroyObjectsOnFireFields()
        {
            foreach (FireTile fireField in fireFields)
            {
                if (hero.col == fireField.Col && hero.row == fireField.Row)
                {
                    killHero();
                    heroDeath = 0;
                }

                List<Monster> toDestroy = new List<Monster>();
                foreach (Monster monster in Monsters)
                {
                    if (monsterDiesOn(monster, fireField.Row, fireField.Col))
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

        private bool monsterDiesOn(Monster monster, int row, int col)
        {
            int tol = 7; // tolerance, how many pixels of monster image on the boarder are not affected with firefield

            return (
                (((monster.x + tol >= row * fieldSize) && (monster.x + tol <= (row + 1) * fieldSize))
                    || ((monster.x + monster.Image.Height - tol >= row * fieldSize) && (monster.x + monster.Image.Height - tol <= (row + 1) * fieldSize)))
                    &&
                (((monster.y + tol >= col * fieldSize) && (monster.y + tol <= (col + 1) * fieldSize))
                    || ((monster.y + monster.Image.Width - tol >= col * fieldSize) && (monster.y + monster.Image.Width - tol <= (col + 1) * fieldSize)))
               );
        }

        private void moveMonsters()
        {
            foreach (Monster monster in Monsters)
            {
                monster.Move();
            }
        }

        public void VykresliSe(Graphics g, ref Bitmap bmp) //ToDo
        {
            int sirkaVyrezuPixely = bmp.Width;
            int vyskaVyrezuPixely = bmp.Height;
            
            int sirkaVyrezu = sirkaVyrezuPixely / fieldSize;
            int vyskaVyrezu = vyskaVyrezuPixely / fieldSize;

            if (sirkaVyrezu > Width)
                sirkaVyrezu = Width;

            if (vyskaVyrezu > Height)
                vyskaVyrezu = Height;

            // urcit LHR vyrezu:
            
            int dx = hero.row - sirkaVyrezu / 2;
            if (dx < 0)
                dx = 0;
            if (dx + sirkaVyrezu - 1 >= this.Width)
                dx = this.Width - sirkaVyrezu;

            int dy = hero.col - vyskaVyrezu / 2;
            if (dy < 0)
                dy = 0;
            if (dy + vyskaVyrezu - 1 >= this.Height)
                dy = this.Height - vyskaVyrezu;

            bool drawLast = false;
            Point pointToDrawLast = new Point();
            Image imageToDrawLast = bmpBackground;
            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                for (int x = 0; x < sirkaVyrezu; x++)
                {
                    for (int y = 0; y < vyskaVyrezu; y++)
                    {
                        int mx = dx + x; // index do mapy
                        int my = dy + y; // index do mapy

                        char c = map[my, mx];

                        Image image;
                        if (c == '?')
                        {
                            if (this.bonus.State == BonusState.covered)
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
                             //   imageToDrawLast = bmpExitClosed;
                             //  pointToDrawLast = new Point(x * fieldSize - ( image.Width - fieldSize)/2, y * fieldSize - (image.Height - fieldSize));
                              //  drawLast = true;
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
                        
                        grfx.DrawImage(image, x * fieldSize, y * fieldSize);

                    }
                    
                }

                foreach (FireTile fireField in fireFields)
                {
                    grfx.DrawImage(fireField.Bitmap, fireField.Col * fieldSize, fireField.Row * fieldSize);
                }

                if (drawLast)
                {
                    grfx.DrawImage(imageToDrawLast, pointToDrawLast.X, pointToDrawLast.Y);
                }    

                foreach (Monster monster in Monsters)
                {
                    grfx.DrawImage(monster.Image, monster.y, monster.x);
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
                        grfx.DrawImage(monster.ImageDead, monster.y, monster.x);
                    }
                    else if (monster.TimeDead < deadMonsterTime + scoreTime)
                    {
                        grfx.DrawString(monster.Score.ToString(), new Font("Comic Sans MS", 18), Brushes.Red, monster.y + monster.Image.Width / 4, monster.x + monster.Image.Height/4);
                    }
                }
                foreach (Monster monster in toDestroy)
                {
                    deadMonsters.Remove(monster);
                }

                if (HeroIsDead)
                {
                    hero.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    grfx.DrawImage(hero.Image, hero.y, hero.x);
                }
                else
                {
                    grfx.DrawImage(hero.Image, hero.y, hero.x);
                }
            }
        }

        private void placeBomb()
        {
            int mapRow = ( (hero.x + hero.Image.Height /2 ) / fieldSize);
            int mapCol = ( (hero.y + hero.Image.Width /2 ) / fieldSize );
            if ( isFree(mapRow, mapCol) && hero.BombsAvailable > 0 )
            {
                map[mapRow, mapCol] = 'B';
                activeBombs.Add(new Bomb(mapRow, mapCol));
                hero.BombsAvailable--;
                hero.StandingOnBomb = true;
                hero.BombWhereStanding = new Point(mapRow, mapCol);
            }
        }

        private char[] shuffleTemporaryWalls(int count) //ToDo
        {
            //gets total number of fields that are not Walls
            decimal a = (this.Width-2)/2;
            decimal b = (this.Height-2)/2;
            int walls = (int)(Math.Floor(a) * Math.Floor(b));
            int total = (Width-2) * (Height-2) - walls - 3;

            //array of temporary walls, free fields, bonus and exit
            char[] array = new char[total];
            array[0] = '?'; //bonus
            array[1] = 'E'; //exit
            for (int i = 2; i < count+2; i++)
            {
                array[i] = 'X';
            }
            for (int i = count+2; i < total; i++)
            {
                array[i] = 'O';
            }

            //shuffle array
            Random rnd = new Random();
            for (int i = 0; i < total-1; i++)
            {
                char temp = array[i];
                int j = rnd.Next(i, total-1);
                array[i] = array[j];
                array[j] = temp;
            }
            return array;
         }
    }
    * */
}
