namespace WindowsFormsApplication4
{
    partial class GameWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.LTime = new System.Windows.Forms.Label();
            this.LLife = new System.Windows.Forms.Label();
            this.LScore = new System.Windows.Forms.Label();
            this.timerCountDown = new System.Windows.Forms.Timer(this.components);
            this.BNextLevel = new System.Windows.Forms.Button();
            this.BBackToMenu = new System.Windows.Forms.Button();
            this.BRetry = new System.Windows.Forms.Button();
            this.LGameWindowCountDown = new System.Windows.Forms.Label();
            this.LGameWindowText = new System.Windows.Forms.Label();
            this.PBGame = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PBGame)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 1;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // LTime
            // 
            this.LTime.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LTime.ForeColor = System.Drawing.SystemColors.Window;
            this.LTime.Location = new System.Drawing.Point(546, 9);
            this.LTime.Name = "LTime";
            this.LTime.Size = new System.Drawing.Size(221, 61);
            this.LTime.TabIndex = 2;
            this.LTime.Text = "2:00";
            this.LTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LLife
            // 
            this.LLife.BackColor = System.Drawing.Color.Black;
            this.LLife.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LLife.ForeColor = System.Drawing.SystemColors.Window;
            this.LLife.Location = new System.Drawing.Point(845, 9);
            this.LLife.Name = "LLife";
            this.LLife.Size = new System.Drawing.Size(127, 61);
            this.LLife.TabIndex = 3;
            this.LLife.Text = "3";
            this.LLife.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LScore
            // 
            this.LScore.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LScore.ForeColor = System.Drawing.SystemColors.Window;
            this.LScore.Location = new System.Drawing.Point(318, 9);
            this.LScore.Name = "LScore";
            this.LScore.Size = new System.Drawing.Size(147, 61);
            this.LScore.TabIndex = 4;
            this.LScore.Text = "0";
            this.LScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerCountDown
            // 
            this.timerCountDown.Interval = 1000;
            // 
            // BNextLevel
            // 
            this.BNextLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BNextLevel.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BNextLevel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BNextLevel.Image = global::Bomberman.Properties.Resources.buttonForward;
            this.BNextLevel.Location = new System.Drawing.Point(664, 512);
            this.BNextLevel.Name = "BNextLevel";
            this.BNextLevel.Size = new System.Drawing.Size(377, 110);
            this.BNextLevel.TabIndex = 5;
            this.BNextLevel.Text = "  Další semestr";
            this.BNextLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BNextLevel.UseVisualStyleBackColor = false;
            this.BNextLevel.Click += new System.EventHandler(this.BNextLevel_Click);
            // 
            // BBackToMenu
            // 
            this.BBackToMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BBackToMenu.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BBackToMenu.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BBackToMenu.Image = global::Bomberman.Properties.Resources.buttonBack;
            this.BBackToMenu.Location = new System.Drawing.Point(193, 512);
            this.BBackToMenu.Name = "BBackToMenu";
            this.BBackToMenu.Size = new System.Drawing.Size(376, 110);
            this.BBackToMenu.TabIndex = 6;
            this.BBackToMenu.Text = "       Zpět do menu";
            this.BBackToMenu.UseVisualStyleBackColor = false;
            this.BBackToMenu.Click += new System.EventHandler(this.BBackToMenu_Click);
            // 
            // BRetry
            // 
            this.BRetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BRetry.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BRetry.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BRetry.Image = global::Bomberman.Properties.Resources.buttonForward;
            this.BRetry.Location = new System.Drawing.Point(664, 512);
            this.BRetry.Name = "BRetry";
            this.BRetry.Size = new System.Drawing.Size(373, 110);
            this.BRetry.TabIndex = 7;
            this.BRetry.Text = "  Zkus to znovu";
            this.BRetry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BRetry.UseVisualStyleBackColor = false;
            this.BRetry.Click += new System.EventHandler(this.BRetry_Click);
            // 
            // LGameWindowountDown
            // 
            this.LGameWindowCountDown.BackColor = System.Drawing.Color.Black;
            this.LGameWindowCountDown.Font = new System.Drawing.Font("Comic Sans MS", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LGameWindowCountDown.ForeColor = System.Drawing.SystemColors.Control;
            this.LGameWindowCountDown.Location = new System.Drawing.Point(244, 234);
            this.LGameWindowCountDown.Name = "LGameWindowountDown";
            this.LGameWindowCountDown.Size = new System.Drawing.Size(770, 343);
            this.LGameWindowCountDown.TabIndex = 8;
            this.LGameWindowCountDown.Text = "label1";
            this.LGameWindowCountDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LGameWindowCountDown.Visible = false;
            // 
            // LGameWindowText
            // 
            this.LGameWindowText.Font = new System.Drawing.Font("Comic Sans MS", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LGameWindowText.ForeColor = System.Drawing.Color.OldLace;
            this.LGameWindowText.Location = new System.Drawing.Point(195, 135);
            this.LGameWindowText.Name = "LGameWindowText";
            this.LGameWindowText.Size = new System.Drawing.Size(842, 374);
            this.LGameWindowText.TabIndex = 9;
            this.LGameWindowText.Text = "label2";
            this.LGameWindowText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PBGame
            // 
            this.PBGame.Location = new System.Drawing.Point(55, 78);
            this.PBGame.Name = "PBGame";
            this.PBGame.Size = new System.Drawing.Size(1119, 692);
            this.PBGame.TabIndex = 1;
            this.PBGame.TabStop = false;
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackgroundImage = global::Bomberman.Properties.Resources.GameWindowBackground;
            this.ClientSize = new System.Drawing.Size(1222, 782);
            this.Controls.Add(this.BNextLevel);
            this.Controls.Add(this.BRetry);
            this.Controls.Add(this.LGameWindowText);
            this.Controls.Add(this.LGameWindowCountDown);
            this.Controls.Add(this.BBackToMenu);
            this.Controls.Add(this.LScore);
            this.Controls.Add(this.LLife);
            this.Controls.Add(this.LTime);
            this.Controls.Add(this.PBGame);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GameWindow";
            this.Text = "Brainerman";
            ((System.ComponentModel.ISupportInitialize)(this.PBGame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PictureBox PBGame;
        private System.Windows.Forms.Label LTime;
        private System.Windows.Forms.Label LLife;
        private System.Windows.Forms.Label LScore;
        private System.Windows.Forms.Timer timerCountDown;
        private System.Windows.Forms.Button BNextLevel;
        private System.Windows.Forms.Button BBackToMenu;
        private System.Windows.Forms.Button BRetry;
        private System.Windows.Forms.Label LGameWindowCountDown;
        private System.Windows.Forms.Label LGameWindowText;


    }
}