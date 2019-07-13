using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace WindowsFormsApplication4
{
    /// <summary>
    /// Hlavní okno s menu.
    /// Má své herní okno s vlastní hrou.
    /// Když je viditelné, přehrává zvuk pomocí spMenuWindow.
    /// Zobrazuje nápovědu.
    /// Má dva stavy - Paused (viditelné tlačítko Continue) a Start (nedá se pokračovat v minulé hře).
    /// </summary>
    public partial class MenuWindow : Form
    {
        private GameWindow gameWindow;
        private Graphics graphics;
        private SoundPlayer spMenuWindow = new SoundPlayer(Bomberman.Properties.Resources.menuWindowSound);  //hudba v pozadí okna s menu
        private SoundPlayer spChoice = new SoundPlayer(Bomberman.Properties.Resources.choice);  //zvuk při výběru charakteru

        public bool playSound = true;
        
        /// <summary>
        /// Konstruktor. Začne přehrávat hudbu. Nastaví obrázek pozadí okna a průhlednost komponent.
        /// </summary>
        public MenuWindow()
        {
            InitializeComponent();
            
            if (playSound)
                spMenuWindow.PlayLooping();
            PBMenuWindow.Image = Bomberman.Properties.Resources.MenuWindowBackground;

            //nastavení rodiče, vůči kterému má Label průhledné pozadí
            var pos = this.PointToScreen(LMenuWindowText.Location);
            pos = PBMenuWindow.PointToClient(pos);
            LMenuWindowText.Parent = PBMenuWindow;
            LMenuWindowText.Location = pos;
            LMenuWindowText.BackColor = Color.Transparent;

            pos = this.PointToScreen(LChooseCharacter.Location);
            pos = PBMenuWindow.PointToClient(pos);
            LChooseCharacter.Parent = PBMenuWindow;
            LChooseCharacter.Location = pos;
            LChooseCharacter.BackColor = Color.Transparent;

            pos = this.PointToScreen(LMatfyzak.Location);
            pos = PBMenuWindow.PointToClient(pos);
            LMatfyzak.Parent = PBMenuWindow;
            LMatfyzak.Location = pos;
            LMatfyzak.BackColor = Color.Transparent;

            pos = this.PointToScreen(LMatfyzacka.Location);
            pos = PBMenuWindow.PointToClient(pos);
            LMatfyzacka.Parent = PBMenuWindow;
            LMatfyzacka.Location = pos;
            LMatfyzacka.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Začátek nové hry. 
        /// Nastavení viditelnosti tlačítkům a textům. 
        /// Překrytí černou plochou, vykreslení pomocí Invalidate a zobrazení výběru charakteru.
        /// </summary>
        private void BStart_Click(object sender, EventArgs e)
        {
            BNewGame.Visible = false;
            BContinue.Visible = false;
            BQuit.Visible = false;
            LMenuWindowText.Visible = false;
            BRules.Visible = false;
            BMatfyzacka.Visible = true;
            BMatfyzak.Visible = true;
            LChooseCharacter.Visible = true;
            LMatfyzacka.Visible = true;
            LMatfyzak.Visible = true;
            BPlaySound.Visible = false;

            graphics = Graphics.FromImage(PBMenuWindow.Image);
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), new Rectangle(0, 0, PBMenuWindow.Image.Width, PBMenuWindow.Image.Height));
            Invalidate();
        }

        /// <summary>
        /// Vytvoří herní okno s vybraným charakterem. Skryje okno s menu. Nastaví viditenost komponent na původní stav.
        /// </summary>
        private void BMatfyzacka_Click(object sender, EventArgs e)
        {
             this.gameWindow = new GameWindow(this, 1);
             gameWindow.FormClosed += new FormClosedEventHandler(MyForm_FormClosed); //Když se herní okno zavře, okno s menu se zviditelní
             
             if (playSound)
                spChoice.Play();

             this.Visible = false;
             
             PBMenuWindow.Image = Bomberman.Properties.Resources.MenuWindowBackground;          
             BMatfyzak.Visible = false;
             BMatfyzacka.Visible = false;
             LChooseCharacter.Visible = false;
             LMatfyzacka.Visible = false;
             LMatfyzak.Visible = false;
             BQuit.Visible = true;
             BRules.Visible = true;
             BPlaySound.Visible = true;
        }

        /// <summary>
        /// Vytvoří herní okno s vybraným charakterem. Skryje okno s menu. Nastaví viditenost komponent na původní stav.
        /// </summary>
        private void BMatfyzak_Click(object sender, EventArgs e)
        {
            this.gameWindow = new GameWindow(this, 0);
            gameWindow.FormClosed += new FormClosedEventHandler(MyForm_FormClosed);

            if (playSound)
                spChoice.Play();

            this.Visible = false;

            PBMenuWindow.Image = Bomberman.Properties.Resources.MenuWindowBackground;
            BMatfyzak.Visible = false;
            BMatfyzacka.Visible = false;
            LChooseCharacter.Visible = false;
            LMatfyzacka.Visible = false;
            LMatfyzak.Visible = false;
            BQuit.Visible = true;
            BRules.Visible = true;
            BPlaySound.Visible = true;
        }

        /// <summary>
        /// Ukončí hru.
        /// </summary>
        private void BQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Skryje okno s menu. Ukončí přehrávání hudby. Pokračuje v začaté hře.
        /// </summary>
        private void BContinue_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            BContinue.Visible = false;

            gameWindow.ContinueGame();

            spMenuWindow.Stop();
        }

        /// <summary>
        /// Paused = viditelné BContinue i BNewGame.
        /// Start = viditelné pouze BNewGame.
        /// Začne přehrávat hudbu.
        /// </summary>
        public void ChangeState(GameWindow.MenuWindowState state)
        {
            if (playSound)
              spMenuWindow.PlayLooping();
            
            switch (state)
            {
                case GameWindow.MenuWindowState.Paused:
                    BContinue.Visible = true;
                    BNewGame.Visible = true;
                    break;
                case GameWindow.MenuWindowState.Start:
                    BContinue.Visible = false;
                    BNewGame.Visible = true;
                    break;
            }
        }

        private bool continueVisible; // pomocná proměnná, krátkodobě si pamatuje viditelnost BContinue   
     
        /// <summary>
        /// Zobrazí nápovědu v PBMenuWindow. 
        /// Nastaví viditelnost komponent.
        /// Než skryje BContinue, zapamatuje si jeho viditelnost pro metodu BBack_Click.
        /// </summary>
        private void BRules_Click(object sender, EventArgs e)
        {
            PBMenuWindow.Image = Bomberman.Properties.Resources.rules;

            BNewGame.Visible = false;
            BQuit.Visible = false;
            BRules.Visible = false;
            BBack.Visible = true;
            BPlaySound.Visible = false;

            if (BContinue.Visible)
            {
                BContinue.Visible = false;
                continueVisible = true;
            }
            else
                continueVisible = false;
        }

        /// <summary>
        /// Tlačítko pro návrat z nápovědy do menu.
        /// </summary>
        private void BBack_Click(object sender, EventArgs e)
        {
            BBack.Visible = false;
            PBMenuWindow.Image = Bomberman.Properties.Resources.MenuWindowBackground;
            BContinue.Visible = continueVisible;
            BNewGame.Visible = true;
            BQuit.Visible = true;
            BRules.Visible = true;
            BPlaySound.Visible = true;
        }        

        /// <summary>
        /// Vykreslení po Invalidate (volá se v BStart_Click)
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            {
                PBMenuWindow.Image = PBMenuWindow.Image;
            }
        }

        /// <summary>
        /// Když se herní okno zavře, zviditelní se okno s menu, začne se přehrávat hudba a nastaví se viditelnost komponent.
        /// </summary>
        void MyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;
            BNewGame.Visible = true;
            if (continueVisible)
                BContinue.Visible = true;
            BRules.Visible = true;
            BQuit.Visible = true;
            LMenuWindowText.Visible = false;
            if (playSound)
               spMenuWindow.PlayLooping();
        }

        private void BPlaySound_Click(object sender, EventArgs e)
        {
            if (playSound == true)
            {
                playSound = false;
                spChoice.Stop();
                spMenuWindow.Stop();
                BPlaySound.Image = Bomberman.Properties.Resources.soundOn;
            }
            else
            {
                playSound = true;
                spMenuWindow.PlayLooping();
                BPlaySound.Image = Bomberman.Properties.Resources.soundOff;
            }
        }
    }
}
