﻿namespace _2048_v0._3
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayBeginningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayExampleInGameGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayExampleTestGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startNewGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtTuile1 = new System.Windows.Forms.TextBox();
            this.txtTuile2 = new System.Windows.Forms.TextBox();
            this.txtTuile3 = new System.Windows.Forms.TextBox();
            this.txtTuile4 = new System.Windows.Forms.TextBox();
            this.btTasser = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label_changements = new System.Windows.Forms.Label();
            this.timer_game = new System.Windows.Forms.Timer(this.components);
            this.picture_play_pause = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_play_pause)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(765, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayBeginningToolStripMenuItem,
            this.displayExampleInGameGridToolStripMenuItem,
            this.displayExampleTestGridToolStripMenuItem,
            this.startNewGridToolStripMenuItem,
            this.enableTestToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // displayBeginningToolStripMenuItem
            // 
            this.displayBeginningToolStripMenuItem.Name = "displayBeginningToolStripMenuItem";
            this.displayBeginningToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.displayBeginningToolStripMenuItem.Text = "Display Example Beginning Grid";
            this.displayBeginningToolStripMenuItem.Click += new System.EventHandler(this.displayBeginningToolStripMenuItem_Click);
            // 
            // displayExampleInGameGridToolStripMenuItem
            // 
            this.displayExampleInGameGridToolStripMenuItem.Name = "displayExampleInGameGridToolStripMenuItem";
            this.displayExampleInGameGridToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.displayExampleInGameGridToolStripMenuItem.Text = "Display Example In-Game Grid";
            this.displayExampleInGameGridToolStripMenuItem.Click += new System.EventHandler(this.displayExampleInGameGridToolStripMenuItem_Click);
            // 
            // displayExampleTestGridToolStripMenuItem
            // 
            this.displayExampleTestGridToolStripMenuItem.Name = "displayExampleTestGridToolStripMenuItem";
            this.displayExampleTestGridToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.displayExampleTestGridToolStripMenuItem.Text = "Display Example Test (pour gagner/perdre)";
            this.displayExampleTestGridToolStripMenuItem.Click += new System.EventHandler(this.displayExampleTestGridToolStripMenuItem_Click);
            // 
            // startNewGridToolStripMenuItem
            // 
            this.startNewGridToolStripMenuItem.Name = "startNewGridToolStripMenuItem";
            this.startNewGridToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.startNewGridToolStripMenuItem.Text = "Start New Game";
            this.startNewGridToolStripMenuItem.Click += new System.EventHandler(this.startNewGridToolStripMenuItem_Click);
            // 
            // enableTestToolStripMenuItem
            // 
            this.enableTestToolStripMenuItem.Name = "enableTestToolStripMenuItem";
            this.enableTestToolStripMenuItem.Size = new System.Drawing.Size(298, 22);
            this.enableTestToolStripMenuItem.Text = "Enable/Disable Test";
            this.enableTestToolStripMenuItem.Click += new System.EventHandler(this.enableTestToolStripMenuItem_Click);
            // 
            // txtTuile1
            // 
            this.txtTuile1.Location = new System.Drawing.Point(197, 60);
            this.txtTuile1.Name = "txtTuile1";
            this.txtTuile1.Size = new System.Drawing.Size(37, 20);
            this.txtTuile1.TabIndex = 1;
            this.txtTuile1.Visible = false;
            // 
            // txtTuile2
            // 
            this.txtTuile2.Location = new System.Drawing.Point(240, 60);
            this.txtTuile2.Name = "txtTuile2";
            this.txtTuile2.Size = new System.Drawing.Size(37, 20);
            this.txtTuile2.TabIndex = 2;
            this.txtTuile2.Visible = false;
            // 
            // txtTuile3
            // 
            this.txtTuile3.Location = new System.Drawing.Point(283, 60);
            this.txtTuile3.Name = "txtTuile3";
            this.txtTuile3.Size = new System.Drawing.Size(37, 20);
            this.txtTuile3.TabIndex = 3;
            this.txtTuile3.Visible = false;
            // 
            // txtTuile4
            // 
            this.txtTuile4.Location = new System.Drawing.Point(326, 60);
            this.txtTuile4.Name = "txtTuile4";
            this.txtTuile4.Size = new System.Drawing.Size(37, 20);
            this.txtTuile4.TabIndex = 4;
            this.txtTuile4.Visible = false;
            // 
            // btTasser
            // 
            this.btTasser.Location = new System.Drawing.Point(369, 58);
            this.btTasser.Name = "btTasser";
            this.btTasser.Size = new System.Drawing.Size(67, 23);
            this.btTasser.TabIndex = 5;
            this.btTasser.Text = "Tasser";
            this.btTasser.UseVisualStyleBackColor = true;
            this.btTasser.Visible = false;
            this.btTasser.Click += new System.EventHandler(this.btTasser_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(442, 60);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(100, 20);
            this.txtResult.TabIndex = 6;
            this.txtResult.Visible = false;
            // 
            // label_changements
            // 
            this.label_changements.AutoSize = true;
            this.label_changements.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_changements.Location = new System.Drawing.Point(442, 96);
            this.label_changements.Name = "label_changements";
            this.label_changements.Size = new System.Drawing.Size(81, 13);
            this.label_changements.TabIndex = 7;
            this.label_changements.Text = "Changements : ";
            this.label_changements.Visible = false;
            // 
            // timer_game
            // 
            this.timer_game.Interval = 1000;
            this.timer_game.Tick += new System.EventHandler(this.timer_game_Tick);
            // 
            // picture_play_pause
            // 
            this.picture_play_pause.Image = global::_2048_v0._3.Properties.Resources.pause_button;
            this.picture_play_pause.Location = new System.Drawing.Point(355, 123);
            this.picture_play_pause.Name = "picture_play_pause";
            this.picture_play_pause.Size = new System.Drawing.Size(40, 40);
            this.picture_play_pause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picture_play_pause.TabIndex = 8;
            this.picture_play_pause.TabStop = false;
            this.picture_play_pause.Visible = false;
            this.picture_play_pause.Click += new System.EventHandler(this.picture_play_pause_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 636);
            this.Controls.Add(this.picture_play_pause);
            this.Controls.Add(this.label_changements);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btTasser);
            this.Controls.Add(this.txtTuile4);
            this.Controls.Add(this.txtTuile3);
            this.Controls.Add(this.txtTuile2);
            this.Controls.Add(this.txtTuile1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "2048";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_play_pause)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayBeginningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayExampleInGameGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startNewGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableTestToolStripMenuItem;
        private System.Windows.Forms.TextBox txtTuile1;
        private System.Windows.Forms.TextBox txtTuile2;
        private System.Windows.Forms.TextBox txtTuile3;
        private System.Windows.Forms.TextBox txtTuile4;
        private System.Windows.Forms.Button btTasser;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label_changements;
        private System.Windows.Forms.ToolStripMenuItem displayExampleTestGridToolStripMenuItem;
        private System.Windows.Forms.Timer timer_game;
        private System.Windows.Forms.PictureBox picture_play_pause;
    }
}

