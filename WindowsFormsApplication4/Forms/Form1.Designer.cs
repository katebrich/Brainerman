namespace WindowsFormsApplication4
{
    partial class MenuWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuWindow));
            this.LMenuWindowText = new System.Windows.Forms.Label();
            this.LChooseCharacter = new System.Windows.Forms.Label();
            this.LMatfyzacka = new System.Windows.Forms.Label();
            this.LMatfyzak = new System.Windows.Forms.Label();
            this.BPlaySound = new System.Windows.Forms.Button();
            this.BBack = new System.Windows.Forms.Button();
            this.BNewGame = new System.Windows.Forms.Button();
            this.BMatfyzak = new System.Windows.Forms.Button();
            this.BMatfyzacka = new System.Windows.Forms.Button();
            this.BRules = new System.Windows.Forms.Button();
            this.BContinue = new System.Windows.Forms.Button();
            this.BQuit = new System.Windows.Forms.Button();
            this.PBMenuWindow = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PBMenuWindow)).BeginInit();
            this.SuspendLayout();
            // 
            // LMenuWindowText
            // 
            this.LMenuWindowText.BackColor = System.Drawing.Color.Transparent;
            this.LMenuWindowText.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LMenuWindowText.Location = new System.Drawing.Point(62, 72);
            this.LMenuWindowText.Name = "LMenuWindowText";
            this.LMenuWindowText.Size = new System.Drawing.Size(658, 286);
            this.LMenuWindowText.TabIndex = 4;
            this.LMenuWindowText.Text = resources.GetString("LMenuWindowText.Text");
            this.LMenuWindowText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LMenuWindowText.Visible = false;
            // 
            // LChooseCharacter
            // 
            this.LChooseCharacter.BackColor = System.Drawing.Color.Transparent;
            this.LChooseCharacter.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LChooseCharacter.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LChooseCharacter.Location = new System.Drawing.Point(156, 123);
            this.LChooseCharacter.Name = "LChooseCharacter";
            this.LChooseCharacter.Size = new System.Drawing.Size(487, 112);
            this.LChooseCharacter.TabIndex = 7;
            this.LChooseCharacter.Text = "Vyber, za koho budeš hrát:";
            this.LChooseCharacter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LChooseCharacter.Visible = false;
            // 
            // LMatfyzacka
            // 
            this.LMatfyzacka.BackColor = System.Drawing.Color.Transparent;
            this.LMatfyzacka.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LMatfyzacka.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LMatfyzacka.Location = new System.Drawing.Point(444, 499);
            this.LMatfyzacka.Name = "LMatfyzacka";
            this.LMatfyzacka.Size = new System.Drawing.Size(264, 84);
            this.LMatfyzacka.TabIndex = 9;
            this.LMatfyzacka.Text = "Matfyzačka je také chytrý.";
            this.LMatfyzacka.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LMatfyzacka.Visible = false;
            // 
            // LMatfyzak
            // 
            this.LMatfyzak.BackColor = System.Drawing.Color.Transparent;
            this.LMatfyzak.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LMatfyzak.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LMatfyzak.Location = new System.Drawing.Point(88, 500);
            this.LMatfyzak.Name = "LMatfyzak";
            this.LMatfyzak.Size = new System.Drawing.Size(264, 84);
            this.LMatfyzak.TabIndex = 10;
            this.LMatfyzak.Text = "Matfyzák je krásný a chytrý.";
            this.LMatfyzak.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LMatfyzak.Visible = false;
            // 
            // BPlaySound
            // 
            this.BPlaySound.Image = global::Bomberman.Properties.Resources.soundOff;
            this.BPlaySound.Location = new System.Drawing.Point(665, 12);
            this.BPlaySound.Name = "BPlaySound";
            this.BPlaySound.Size = new System.Drawing.Size(55, 55);
            this.BPlaySound.TabIndex = 14;
            this.BPlaySound.UseVisualStyleBackColor = true;
            this.BPlaySound.Click += new System.EventHandler(this.BPlaySound_Click);
            // 
            // BBack
            // 
            this.BBack.Image = global::Bomberman.Properties.Resources.buttonClose;
            this.BBack.Location = new System.Drawing.Point(701, 0);
            this.BBack.Name = "BBack";
            this.BBack.Size = new System.Drawing.Size(38, 38);
            this.BBack.TabIndex = 13;
            this.BBack.UseVisualStyleBackColor = true;
            this.BBack.Visible = false;
            this.BBack.Click += new System.EventHandler(this.BBack_Click);
            // 
            // BNewGame
            // 
            this.BNewGame.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BNewGame.Image = global::Bomberman.Properties.Resources.buttonNewGame;
            this.BNewGame.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BNewGame.Location = new System.Drawing.Point(270, 278);
            this.BNewGame.Name = "BNewGame";
            this.BNewGame.Size = new System.Drawing.Size(233, 80);
            this.BNewGame.TabIndex = 0;
            this.BNewGame.Text = "      Nová hra";
            this.BNewGame.UseVisualStyleBackColor = true;
            this.BNewGame.Click += new System.EventHandler(this.BStart_Click);
            // 
            // BMatfyzak
            // 
            this.BMatfyzak.Image = global::Bomberman.Properties.Resources.matfyzakBig;
            this.BMatfyzak.Location = new System.Drawing.Point(81, 227);
            this.BMatfyzak.Name = "BMatfyzak";
            this.BMatfyzak.Size = new System.Drawing.Size(271, 256);
            this.BMatfyzak.TabIndex = 6;
            this.BMatfyzak.UseVisualStyleBackColor = true;
            this.BMatfyzak.Visible = false;
            this.BMatfyzak.Click += new System.EventHandler(this.BMatfyzak_Click);
            // 
            // BMatfyzacka
            // 
            this.BMatfyzacka.Image = global::Bomberman.Properties.Resources.matfyzackaBig;
            this.BMatfyzacka.Location = new System.Drawing.Point(438, 227);
            this.BMatfyzacka.Name = "BMatfyzacka";
            this.BMatfyzacka.Size = new System.Drawing.Size(271, 256);
            this.BMatfyzacka.TabIndex = 5;
            this.BMatfyzacka.UseVisualStyleBackColor = true;
            this.BMatfyzacka.Visible = false;
            this.BMatfyzacka.Click += new System.EventHandler(this.BMatfyzacka_Click);
            // 
            // BRules
            // 
            this.BRules.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BRules.Image = global::Bomberman.Properties.Resources.buttonRules;
            this.BRules.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BRules.Location = new System.Drawing.Point(270, 392);
            this.BRules.Name = "BRules";
            this.BRules.Size = new System.Drawing.Size(233, 80);
            this.BRules.TabIndex = 3;
            this.BRules.Text = "      Nápověda";
            this.BRules.UseVisualStyleBackColor = true;
            this.BRules.Click += new System.EventHandler(this.BRules_Click);
            // 
            // BContinue
            // 
            this.BContinue.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BContinue.Image = global::Bomberman.Properties.Resources.buttonContinue;
            this.BContinue.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BContinue.Location = new System.Drawing.Point(270, 174);
            this.BContinue.Name = "BContinue";
            this.BContinue.Size = new System.Drawing.Size(233, 80);
            this.BContinue.TabIndex = 2;
            this.BContinue.Text = "      Pokračovat";
            this.BContinue.UseVisualStyleBackColor = true;
            this.BContinue.Visible = false;
            this.BContinue.Click += new System.EventHandler(this.BContinue_Click);
            // 
            // BQuit
            // 
            this.BQuit.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BQuit.Image = global::Bomberman.Properties.Resources.buttonQuit;
            this.BQuit.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BQuit.Location = new System.Drawing.Point(270, 505);
            this.BQuit.Name = "BQuit";
            this.BQuit.Size = new System.Drawing.Size(233, 80);
            this.BQuit.TabIndex = 1;
            this.BQuit.Text = "      Konec hry";
            this.BQuit.UseVisualStyleBackColor = true;
            this.BQuit.Click += new System.EventHandler(this.BQuit_Click);
            // 
            // PBMenuWindow
            // 
            this.PBMenuWindow.Image = global::Bomberman.Properties.Resources.MenuWindowBackground;
            this.PBMenuWindow.Location = new System.Drawing.Point(0, 0);
            this.PBMenuWindow.Name = "PBMenuWindow";
            this.PBMenuWindow.Size = new System.Drawing.Size(792, 613);
            this.PBMenuWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PBMenuWindow.TabIndex = 11;
            this.PBMenuWindow.TabStop = false;
            // 
            // MenuWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 611);
            this.Controls.Add(this.BPlaySound);
            this.Controls.Add(this.BBack);
            this.Controls.Add(this.BNewGame);
            this.Controls.Add(this.LMatfyzak);
            this.Controls.Add(this.LMatfyzacka);
            this.Controls.Add(this.BMatfyzak);
            this.Controls.Add(this.BMatfyzacka);
            this.Controls.Add(this.BRules);
            this.Controls.Add(this.BContinue);
            this.Controls.Add(this.BQuit);
            this.Controls.Add(this.LChooseCharacter);
            this.Controls.Add(this.LMenuWindowText);
            this.Controls.Add(this.PBMenuWindow);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MenuWindow";
            this.Text = "Brainerman";
            ((System.ComponentModel.ISupportInitialize)(this.PBMenuWindow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BNewGame;
        private System.Windows.Forms.Button BQuit;
        private System.Windows.Forms.Button BContinue;
        private System.Windows.Forms.Button BRules;
        private System.Windows.Forms.Label LMenuWindowText;
        private System.Windows.Forms.Button BMatfyzacka;
        private System.Windows.Forms.Button BMatfyzak;
        private System.Windows.Forms.Label LChooseCharacter;
        private System.Windows.Forms.Label LMatfyzacka;
        private System.Windows.Forms.Label LMatfyzak;
        private System.Windows.Forms.PictureBox PBMenuWindow;
        private System.Windows.Forms.Button BBack;
        private System.Windows.Forms.Button BPlaySound;
    }
}

