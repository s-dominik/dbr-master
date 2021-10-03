
namespace DBR.View
{
    partial class View
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
            this.menusor = new System.Windows.Forms.StatusStrip();
            this.toolMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolLoadGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSaveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolBuilding = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusSpacer2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolLabelTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolLabelGuest = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolGuest = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolLabelRepairman = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolRepairman = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolMoneyLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolMoney = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menusor.SuspendLayout();
            this.SuspendLayout();
            // 
            // menusor
            // 
            this.menusor.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.menusor.Dock = System.Windows.Forms.DockStyle.Top;
            this.menusor.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menusor.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menusor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenu,
            this.toolStripStatusSpacer,
            this.toolBuilding,
            this.toolStripStatusSpacer2,
            this.toolLabelTime,
            this.toolTime,
            this.toolLabelGuest,
            this.toolGuest,
            this.toolLabelRepairman,
            this.toolRepairman,
            this.toolMoneyLabel,
            this.toolMoney});
            this.menusor.Location = new System.Drawing.Point(0, 0);
            this.menusor.Name = "menusor";
            this.menusor.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.menusor.Size = new System.Drawing.Size(612, 31);
            this.menusor.TabIndex = 0;
            this.menusor.Text = "statusStrip1";
            // 
            // toolMenu
            // 
            this.toolMenu.AutoToolTip = false;
            this.toolMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNewGame,
            this.toolStripSeparator1,
            this.toolLoadGame,
            this.toolSaveGame,
            this.toolStripSeparator2,
            this.toolExit});
            this.toolMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolMenu.Name = "toolMenu";
            this.toolMenu.ShowDropDownArrow = false;
            this.toolMenu.Size = new System.Drawing.Size(65, 29);
            this.toolMenu.Text = "Menu";
            // 
            // toolNewGame
            // 
            this.toolNewGame.Name = "toolNewGame";
            this.toolNewGame.Size = new System.Drawing.Size(179, 30);
            this.toolNewGame.Text = "New Game";
            this.toolNewGame.Click += new System.EventHandler(this.MenuNewGame_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // toolLoadGame
            // 
            this.toolLoadGame.Name = "toolLoadGame";
            this.toolLoadGame.Size = new System.Drawing.Size(179, 30);
            this.toolLoadGame.Text = "Load Game";
            this.toolLoadGame.Click += new System.EventHandler(this.MenuLoadGame_Click);
            // 
            // toolSaveGame
            // 
            this.toolSaveGame.Name = "toolSaveGame";
            this.toolSaveGame.Size = new System.Drawing.Size(179, 30);
            this.toolSaveGame.Text = "Save Game";
            this.toolSaveGame.Click += new System.EventHandler(this.MenuSaveGame_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
            // 
            // toolExit
            // 
            this.toolExit.Name = "toolExit";
            this.toolExit.Size = new System.Drawing.Size(179, 30);
            this.toolExit.Text = "Exit";
            this.toolExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // toolStripStatusSpacer
            // 
            this.toolStripStatusSpacer.Name = "toolStripStatusSpacer";
            this.toolStripStatusSpacer.Size = new System.Drawing.Size(168, 26);
            this.toolStripStatusSpacer.Spring = true;
            // 
            // toolBuilding
            // 
            this.toolBuilding.Name = "toolBuilding";
            this.toolBuilding.Size = new System.Drawing.Size(0, 26);
            // 
            // toolStripStatusSpacer2
            // 
            this.toolStripStatusSpacer2.Name = "toolStripStatusSpacer2";
            this.toolStripStatusSpacer2.Size = new System.Drawing.Size(168, 26);
            this.toolStripStatusSpacer2.Spring = true;
            // 
            // toolLabelTime
            // 
            this.toolLabelTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolLabelTime.Image = ((System.Drawing.Image)(resources.GetObject("toolLabelTime.Image")));
            this.toolLabelTime.Name = "toolLabelTime";
            this.toolLabelTime.Size = new System.Drawing.Size(20, 26);
            this.toolLabelTime.Text = "Time:";
            // 
            // toolTime
            // 
            this.toolTime.Name = "toolTime";
            this.toolTime.Size = new System.Drawing.Size(22, 26);
            this.toolTime.Text = "0";
            // 
            // toolLabelGuest
            // 
            this.toolLabelGuest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolLabelGuest.Image = ((System.Drawing.Image)(resources.GetObject("toolLabelGuest.Image")));
            this.toolLabelGuest.Name = "toolLabelGuest";
            this.toolLabelGuest.Size = new System.Drawing.Size(20, 26);
            this.toolLabelGuest.Text = "Guests:";
            // 
            // toolGuest
            // 
            this.toolGuest.Name = "toolGuest";
            this.toolGuest.Size = new System.Drawing.Size(22, 26);
            this.toolGuest.Text = "0";
            // 
            // toolLabelRepairman
            // 
            this.toolLabelRepairman.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolLabelRepairman.Image = ((System.Drawing.Image)(resources.GetObject("toolLabelRepairman.Image")));
            this.toolLabelRepairman.Name = "toolLabelRepairman";
            this.toolLabelRepairman.Size = new System.Drawing.Size(20, 26);
            this.toolLabelRepairman.Text = "Repairman:";
            // 
            // toolRepairman
            // 
            this.toolRepairman.Name = "toolRepairman";
            this.toolRepairman.Size = new System.Drawing.Size(22, 26);
            this.toolRepairman.Text = "0";
            // 
            // toolMoneyLabel
            // 
            this.toolMoneyLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolMoneyLabel.Image = ((System.Drawing.Image)(resources.GetObject("toolMoneyLabel.Image")));
            this.toolMoneyLabel.Name = "toolMoneyLabel";
            this.toolMoneyLabel.Size = new System.Drawing.Size(20, 26);
            this.toolMoneyLabel.Text = "Money:";
            // 
            // toolMoney
            // 
            this.toolMoney.Name = "toolMoney";
            this.toolMoney.Size = new System.Drawing.Size(52, 26);
            this.toolMoney.Text = "2000";
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 252);
            this.Controls.Add(this.menusor);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "View";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Theme park game";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Nezet_Load);
            this.menusor.ResumeLayout(false);
            this.menusor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip menusor;
        private System.Windows.Forms.ToolStripStatusLabel toolTime;
        private System.Windows.Forms.ToolStripStatusLabel toolMoneyLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolMoney;
        private System.Windows.Forms.ToolStripDropDownButton toolMenu;
        private System.Windows.Forms.ToolStripStatusLabel toolLabelGuest;
        private System.Windows.Forms.ToolStripStatusLabel toolGuest;
        private System.Windows.Forms.ToolStripStatusLabel toolLabelRepairman;
        private System.Windows.Forms.ToolStripStatusLabel toolRepairman;
        private System.Windows.Forms.ToolStripStatusLabel toolLabelTime;
        private System.Windows.Forms.ToolStripMenuItem toolNewGame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolLoadGame;
        private System.Windows.Forms.ToolStripMenuItem toolSaveGame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolExit;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSpacer;
        private System.Windows.Forms.ToolStripStatusLabel toolBuilding;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSpacer2;
    }
}

