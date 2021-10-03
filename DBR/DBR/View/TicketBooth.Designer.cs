
namespace DBR
{
    partial class TicketBooth
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
            this.label1 = new System.Windows.Forms.Label();
            this.numEntranceFee = new System.Windows.Forms.NumericUpDown();
            this.btnOpenPark = new System.Windows.Forms.Button();
            this.btnHireRepairman = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblPerBuildingFee = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblRepairmanFee = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numEntranceFee)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Belépődíj:";
            // 
            // numEntranceFee
            // 
            this.numEntranceFee.Location = new System.Drawing.Point(71, 17);
            this.numEntranceFee.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numEntranceFee.Name = "numEntranceFee";
            this.numEntranceFee.Size = new System.Drawing.Size(120, 23);
            this.numEntranceFee.TabIndex = 1;
            // 
            // btnOpenPark
            // 
            this.btnOpenPark.Location = new System.Drawing.Point(6, 46);
            this.btnOpenPark.Name = "btnOpenPark";
            this.btnOpenPark.Size = new System.Drawing.Size(185, 23);
            this.btnOpenPark.TabIndex = 2;
            this.btnOpenPark.Text = "Megnyitás";
            this.btnOpenPark.UseVisualStyleBackColor = true;
            this.btnOpenPark.Click += new System.EventHandler(this.btnOpenPark_Click);
            // 
            // btnHireRepairman
            // 
            this.btnHireRepairman.Location = new System.Drawing.Point(6, 77);
            this.btnHireRepairman.Name = "btnHireRepairman";
            this.btnHireRepairman.Size = new System.Drawing.Size(185, 23);
            this.btnHireRepairman.TabIndex = 3;
            this.btnHireRepairman.Text = "Karbantartó felvétele";
            this.btnHireRepairman.UseVisualStyleBackColor = true;
            this.btnHireRepairman.Click += new System.EventHandler(this.btnHireRepairman_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ár:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnHireRepairman);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 94);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(199, 106);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Karbantartó";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.lblPerBuildingFee, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(102, 47);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(91, 21);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // lblPerBuildingFee
            // 
            this.lblPerBuildingFee.AutoSize = true;
            this.lblPerBuildingFee.Location = new System.Drawing.Point(45, 0);
            this.lblPerBuildingFee.Name = "lblPerBuildingFee";
            this.lblPerBuildingFee.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPerBuildingFee.Size = new System.Drawing.Size(43, 15);
            this.lblPerBuildingFee.TabIndex = 0;
            this.lblPerBuildingFee.Text = "100000";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblRepairmanFee, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(102, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(91, 25);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // lblRepairmanFee
            // 
            this.lblRepairmanFee.AutoSize = true;
            this.lblRepairmanFee.Location = new System.Drawing.Point(39, 0);
            this.lblRepairmanFee.Name = "lblRepairmanFee";
            this.lblRepairmanFee.Size = new System.Drawing.Size(49, 15);
            this.lblRepairmanFee.TabIndex = 0;
            this.lblRepairmanFee.Text = "1000000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Javítási költség:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numEntranceFee);
            this.groupBox2.Controls.Add(this.btnOpenPark);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(199, 76);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parkigazgatás";
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.Location = new System.Drawing.Point(12, 206);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 7;
            this.btnAccept.Text = "OK";
            this.btnAccept.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(136, 206);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Mégse";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // TicketBooth
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(223, 241);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TicketBooth";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Jegypénztár";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TicketBooth_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numEntranceFee)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numEntranceFee;
        private System.Windows.Forms.Button btnOpenPark;
        private System.Windows.Forms.Button btnHireRepairman;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblPerBuildingFee;
        private System.Windows.Forms.Label lblRepairmanFee;
    }
}