using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bomberman.NullFX.Win32;
using System.Diagnostics;
using System.Media;
using Bomberman.MainClasses;

namespace WindowsFormsApplication4
{
    public partial class GameWindow : Form
    {
        public enum MenuWindowState { Start, Paused }

        private Graphics graphics;
        private Map map;
        private Bitmap bmp; // na ní se vykresluje celý herní plán, zobrazuje ji PBGame
        private MenuWindow menuWindow;
        private DateTime endTime = new DateTime(2016, 1, 1, 0, 3, 0); // 3:00, čas na splnění levelu
        private DateTime runningTime = new DateTime(2016, 1, 1, 0, 0, 0); //Jak dlouho už tento level běží
        private Stopwatch stopWatch = new Stopwatch(); //Stopování času levelu
        private int nextLevel = 1; //Jaký se má otevřít level. Zvýší se při výhře.        
        private int character = 0; //0..matfyzák, 1..matfyzačka. Předá ho okno s menu podle výběru uživatele.
        private int totalScore = 0; //celkové skóre ze všech levelů
        private bool gameOver; //pomocná proměnná, došly životy, konec hry
        private bool finalLevelWin; //pomocná proměnná, výhra posledního kola, konec hry
        private int count = 0; //Pomocná proměnná pro oddalování času timeru
        private int k = 1; //Pomocná proměnná pro počítání uplynulých sekund na stopkách
        
        //texty, které zobrazuje LGameWindowText
        private string msgGameOver;
        private string[] msgLoss = new string[6];
        private string msgWin;
        private string msgBeforeFinalLevel;

        //zvuky
        private SoundPlayer spGameWindow = new SoundPlayer(Bomberman.Properties.Resources.gameWindowSound); //hudba na pozadí herního okna
        private SoundPlayer spWin = new SoundPlayer(Bomberman.Properties.Resources.win); //výhra
        private SoundPlayer spLoss = new SoundPlayer(Bomberman.Properties.Resources.loss); //prohra
        private SoundPlayer spBeep = new SoundPlayer(Bomberman.Properties.Resources.beep); // odpočítávání na začátku kola
        private SoundPlayer spBeepFinal = new SoundPlayer(Bomberman.Properties.Resources.beepFinal); // odpočítávání na začátku kola, poslední pípnutí

        /// <summary>
        /// Konstruktor. 
        /// Vytvoří texty pro LGameWindowText.
        /// Vytvoření grafiky okna.
        /// Nastavení viditelnosti komponent a průhlednosti labelů.
        /// Spuštění hry.
        /// </summary>
        public GameWindow(MenuWindow MenuWindow, int Character)
        {
            InitializeComponent();
            CreateMessages();            
            
            this.character = Character;
            this.menuWindow = MenuWindow;            
            this.Visible = true;
            
            this.bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            graphics = Graphics.FromImage(bmp);

            BBackToMenu.Visible = false;
            BNextLevel.Visible = false;
            BRetry.Visible = false;
            timer.Enabled = true;
            LGameWindowCountDown.Visible = false;
            LGameWindowText.Visible = false;
            
            StartGame(1);
            map.State = MapState.Start;
            
            //nastavení rodiče, vzhledem ke kterému průhledné
            var pos = this.PointToScreen(LGameWindowCountDown.Location);
            pos = PBGame.PointToClient(pos);
            LGameWindowCountDown.Parent = PBGame;
            LGameWindowCountDown.Location = pos;
            LGameWindowCountDown.BackColor = Color.Transparent;

            pos = this.PointToScreen(LGameWindowText.Location);
            pos = PBGame.PointToClient(pos);
            LGameWindowText.Parent = PBGame;
            LGameWindowText.Location = pos;
            LGameWindowText.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Pokračování hry po pauze nebo po vrácení se do menu.
        /// </summary>
        public void ContinueGame()
        {
            this.Visible = true;
            menuWindow.Visible = false;

            if (map.Paused == true)
            {
                map.State = MapState.Start;
                count = 100; // kvůli přeskočení několika kroků v timeru
                map.Paused = false;
            }
        }

        /// <summary>
        /// Vytvoří další level.
        /// </summary>
        /// <param name="Level"></param>
        private void StartGame(int Level)
        {
            switch (nextLevel)
            {
                case 1:
                    try
                    {
                        map = new Map("1.ročník \nZimní semestr", 7, 13, 30, 0, 3, new int[] { 0, 0, 0 }, map.HeroState, character);
                    }
                    catch
                    {
                        map = new Map("1.ročník \nZimní semestr", 7, 13, 30, 0, 3, new int[] { 0, 0, 0 }, new HeroState(), character);
                    }
                    break;
                case 2:
                    map = new Map("1.ročník\n Letní semestr", 7, 13, 30, 1, 4, new int[] {0,0,1,1}, map.HeroState, character);
                    break;
                case 3:
                    map = new Map("2.ročník\n Zimní semestr", 7, 13, 30, 2, 5, new int[] {0,1,1,1,1}, map.HeroState, character);
                    break;
                case 4:
                    map = new Map("2.ročník\n Letní semestr", 7, 13, 30, 3, 2, new int[] {2,2}, map.HeroState, character);
                    break;
                case 5:
                    map = new Map("3.ročník\n Zimní semestr", 7, 13, 30, 0, 4, new int[] {0,1,2,2}, map.HeroState, character);
                    break;
                case 6:
                    map = new Map("3.ročník\n Letní semestr", 7, 13, 30, 1, 6, new int[] {0,0,0,1,2,2}, map.HeroState, character);
                    break;
                case 7:
                    map = new Map("Státnice", 7, 13, 20, 2, 0, new int[] {}, map.HeroState,  character);
                    map.FinalLevel = true;
                    break;
            }
            count = 0;
        }        

        /// <summary>
        /// Vytvoří texty pro LGameWindowText.
        /// </summary>
        private void CreateMessages()
        {
            msgGameOver = "Běž radši na VŠE. Zvýší se průměrné IQ na obou školách. Nebo to snad chceš zkusit znovu?";
            msgLoss[0] = "Zabil tě tvůj vlastní mozek? \nI to se na Matfyzu stává...";
            msgLoss[1] = "Dostalo tě prográmko? \nPořiď si debug-kachničku a zkus to znovu.";
            msgLoss[2] = "Nezvládáš principy počítačů? Pro příště věz, že odpověď na základní otázku života, vesmíru a vůbec je 42.";
            msgLoss[3] = "Nezoufej, analýza už potrápila kdejakého matfyzáka. Příště jí to nandej za nás za všechny!";
            msgLoss[4] = "Time's up, semestr's over, smůlička.";
            msgWin = "Gratuluji, Bc. Matfyzáku! \n Nezvolil sis právě nejlehčí cestu k titulu, ale pro tebe to byla hračka.\nTak si zahraj znovu!";
            msgBeforeFinalLevel = "Za své studium jsi získal 180 kreditů!\nNyní tě čekají státnice.\nStáváš se imunní proti příšerám, ale bude to souboj s časem!\nDokážeš to dotáhnout až do konce?";
        }     

        /// <summary>
        /// Podle map.State se provedou události.
        /// Start
        /// - Načtení levelu.
        /// - Nastavení viditelnosti a vzhledu komponent.
        /// - Vynulování stopek.
        /// - Odpočítávání před začátkem hry.
        /// - Nastavení map.State na Running.
        /// Running
        /// - Pohne objekty a vykreslí herní plán.
        /// - Mění čas na LTime podle stopek.
        /// - Odchytává pauzu, smrt hrdiny a vypršení času.
        /// Loss
        /// - Zastaví stopky a zvuk.
        /// - Vypíše zprávu podle toho, jak hrdina zemřel.
        /// - Ubere život.
        /// - Změní stav na Waiting.
        /// Win
        /// - Zvýší nextLevel, přičte skóre z tohoto kola k celkovému.
        /// - Zobrazí dosažené skóre, případně zprávu o výhře hry.
        /// Waiting
        /// - Neděje se nic, pouze se čeká na stisk jednoho ze zobrazených tlačítek.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            switch (this.map.State)
            {               
                case MapState.Start:
                    if (count == 0)
                    {
                        StartGame(nextLevel);
                        map.State = MapState.Start;
                        
                        LTime.Text = endTime.ToString(@"m\:ss");
                        LGameWindowText.Font = new Font("Comic Sans MS", 28);
                        LGameWindowText.TextAlign = ContentAlignment.MiddleCenter;
                        LScore.Text = 0.ToString();
                        
                        BBackToMenu.Visible = false;
                        LGameWindowText.Visible = false;
                        BNextLevel.Visible = false;
                        BRetry.Visible = false;
                        LGameWindowCountDown.Visible = false;
                        
                        stopWatch.Restart();
                        stopWatch.Stop();
                        runningTime = new DateTime(2016, 1, 1, 0, 0, 0);
                        
                        count++;
                        k = 0;
                    }                   
                    
                    if (count == 1)  //zobrazení názvu levelu
                    {
                        LGameWindowCountDown.Visible = true;
                        LGameWindowCountDown.Text = map.LevelName;
                        map.Portray(graphics, ref bmp);
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), new Rectangle(0, 0, bmp.Width, bmp.Height));
                        this.Invalidate();
                    }
                    //odpočítávání 3..2..1..start + přehrávání zvuků
                    else if (count == 100) // když se pokračuje v začaté hře, count je nastaven na 100 a předchozí kroky se přeskočí
                    {
                        LGameWindowCountDown.Visible = true;
                        map.Portray(graphics, ref bmp);
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), new Rectangle(0, 0, bmp.Width, bmp.Height));
                        this.Invalidate();
                        LGameWindowCountDown.Text = "3";
                        if (menuWindow.playSound)
                          spBeep.Play();
                    }
                    else if (count == 150)
                    {
                        LGameWindowCountDown.Text = "2";
                        if (menuWindow.playSound)
                             spBeep.Play();
                    }
                    else if (count == 200)
                    {
                        LGameWindowCountDown.Text = "1";
                        if (menuWindow.playSound)
                            spBeep.Play();
                    }
                    else if (count == 250)
                    {
                        LGameWindowCountDown.Text = "START!";
                        if (menuWindow.playSound)
                           spBeepFinal.Play();
                    }
                    else if (count == 300)
                    {
                        map.State = MapState.Running;
                        LGameWindowCountDown.Visible = false;
                        count = -1;
                        stopWatch.Start();
                        if (menuWindow.playSound)
                           spGameWindow.PlayLooping();
                    }
                    count++;
                    break;
                
                case MapState.Running:

                    map.MoveObjects();
                    map.Portray(graphics, ref bmp);
                    this.Invalidate();
                    
                    if (stopWatch.ElapsedMilliseconds >= k * 1000 && stopWatch.IsRunning) //když uběhla další sekunda, změní se text na LTime
                    {
                        runningTime = runningTime.AddSeconds(1);
                        LTime.Text = (endTime - runningTime).ToString(@"m\:ss");
                        k++;
                    }

                    BBackToMenu.Visible = false;
                    BNextLevel.Visible = false;
                    BRetry.Visible = false;
                    LLife.Text = map.HeroState.Life.ToString();
                    LScore.Text = map.CurrentScore.ToString();
                    
                    if (map.Paused)
                    {
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), new Rectangle(0, 0, bmp.Width, bmp.Height));
                        this.Invalidate();

                        stopWatch.Stop();
                        spGameWindow.Stop();
                        map.State = MapState.Waiting;

                        this.Visible = false;
                        menuWindow.Visible = true;
                        menuWindow.ChangeState(MenuWindowState.Paused);
                        break;
                    }

                    if (map.HeroIsDead)
                    {
                        map.State = MapState.Loss;
                    }
                    if (LTime.Text == "0:00")
                    {
                        map.State = MapState.Loss;
                        LTime.Text = "Time's up!";
                        stopWatch.Stop();
                        map.HeroDeath = 4;
                    }
                    break;

                case MapState.Loss:
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), new Rectangle(0, 0, bmp.Width, bmp.Height));
                    Invalidate();
                    spGameWindow.Stop();
                    if (menuWindow.playSound)
                       spLoss.Play();
                    stopWatch.Stop();

                    map.State = MapState.Waiting;

                    if (map.HeroState.Life == 1)
                    {
                        BRetry.Enabled = false;
                        LGameWindowText.Text = msgGameOver;
                        gameOver = true;
                    }
                    else
                    {
                        LGameWindowText.Text = msgLoss[map.HeroDeath];
                        gameOver = false;
                    }

                    LGameWindowText.Visible = true;
                    timerCountDown.Enabled = false;
                    BBackToMenu.Visible = true;
                    BNextLevel.Visible = false;
                    BRetry.Visible = true;
                    
                    map.HeroState.FireRange = 1;
                    map.HeroState.Speed = 2;
                    map.HeroState.BombsMaximum = 1;
                    map.HeroState.Life--;
                    
                    LLife.Text = map.HeroState.Life.ToString();
                    map.CurrentScore = 0;
                    break;
                
                case MapState.Win:
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), new Rectangle(0, 0, bmp.Width, bmp.Height));
                    Invalidate();
                    timerCountDown.Enabled = false;
                    stopWatch.Stop();
                    spGameWindow.Stop();
                    if (menuWindow.playSound)
                        spWin.Play();

                    nextLevel++;

                    map.State = MapState.Waiting;

                    BBackToMenu.Visible = true;
                    BNextLevel.Visible = true;
                    BRetry.Visible = false;             
                    LGameWindowText.Visible = true;
                    LGameWindowText.TextAlign = ContentAlignment.MiddleRight;
                    
                    if (nextLevel == 7) //státnice, specialni zpráva
                    {
                        LGameWindowText.Text = msgBeforeFinalLevel;
                        LGameWindowText.TextAlign = ContentAlignment.MiddleCenter;
                        LGameWindowText.Font = new Font("Comic Sans MS", 22);
                    }
                    else if ( nextLevel == 8) //výhra hry, hra končí
                    {
                        LGameWindowText.Text = msgWin;
                        LGameWindowText.TextAlign = ContentAlignment.MiddleCenter;
                        LGameWindowText.Font = new Font("Comic Sans MS", 22);
                        BNextLevel.Visible = false;
                        finalLevelWin = true;

                    }
                    else
                    LGameWindowText.Text = String.Format("Tento semestr: {0} kreditů \nPředchozí studium: {1} kreditů  ____________________ \n Celkem získáno: {2} kreditů ", map.CurrentScore.ToString(), totalScore.ToString(), (map.CurrentScore + totalScore).ToString());
                    totalScore = map.CurrentScore + totalScore;
                    break;
                case MapState.Waiting:
                    
                    break;
            }
        }

        /// <summary>
        /// Začátek dalšího kola.
        /// </summary>
        private void BNextLevel_Click(object sender, EventArgs e)
        {
            if (BNextLevel.Visible)
            {
                map.State = MapState.Start;
            }
        }

        /// <summary>
        /// Zviditelní okno s menu a změní jeho stav, samo sebe skryje. 
        /// </summary>
        private void BBackToMenu_Click(object sender, EventArgs e)
        {
            if (BBackToMenu.Visible)
            {
                menuWindow.Visible = true;
                this.Visible = false;
                if (gameOver || finalLevelWin)
                {
                    menuWindow.ChangeState(MenuWindowState.Start);
                }
                else
                menuWindow.ChangeState(MenuWindowState.Paused);
            }
        }
        
        /// <summary>
        /// Začátek stejného levelu.
        /// </summary>
        private void BRetry_Click(object sender, EventArgs e)
        {
            if (BRetry.Visible)
            {
                map.State = MapState.Start;
            }
        }

        /// <summary>
        /// Při volání Invalidate chci vykreslit bmp.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.bmp != null)
            {
                PBGame.Image = bmp;
            }
        }
    }
}
