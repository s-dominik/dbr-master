
namespace DBR
{
    partial class BuildBuilding
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
            this.labelOnlyPrice = new System.Windows.Forms.Label();
            this.labelOnlyArea = new System.Windows.Forms.Label();
            this.labelOnlyCapacity = new System.Windows.Forms.Label();
            this.labelPrice = new System.Windows.Forms.Label();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelCapacity = new System.Windows.Forms.Label();
            this.labelTicketPrice = new System.Windows.Forms.Label();
            this.labelMinPeople = new System.Windows.Forms.Label();
            this.buttonBuild = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.trackBarPrice = new System.Windows.Forms.TrackBar();
            this.trackBarPeople = new System.Windows.Forms.TrackBar();
            this.labelTicketPriceValue = new System.Windows.Forms.Label();
            this.labelMinPeopleValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPeople)).BeginInit();
            this.SuspendLayout();
            // 
            // labelOnlyPrice
            // 
            this.labelOnlyPrice.AutoSize = true;
            this.labelOnlyPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelOnlyPrice.Location = new System.Drawing.Point(49, 10);
            this.labelOnlyPrice.Name = "labelOnlyPrice";
            this.labelOnlyPrice.Size = new System.Drawing.Size(47, 21);
            this.labelOnlyPrice.TabIndex = 0;
            this.labelOnlyPrice.Text = "Price:";
            // 
            // labelOnlyArea
            // 
            this.labelOnlyArea.AutoSize = true;
            this.labelOnlyArea.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelOnlyArea.Location = new System.Drawing.Point(49, 38);
            this.labelOnlyArea.Name = "labelOnlyArea";
            this.labelOnlyArea.Size = new System.Drawing.Size(45, 21);
            this.labelOnlyArea.TabIndex = 1;
            this.labelOnlyArea.Text = "Area:";
            // 
            // labelOnlyCapacity
            // 
            this.labelOnlyCapacity.AutoSize = true;
            this.labelOnlyCapacity.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelOnlyCapacity.Location = new System.Drawing.Point(18, 69);
            this.labelOnlyCapacity.Name = "labelOnlyCapacity";
            this.labelOnlyCapacity.Size = new System.Drawing.Size(72, 21);
            this.labelOnlyCapacity.TabIndex = 2;
            this.labelOnlyCapacity.Text = "Capacity:";
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPrice.Location = new System.Drawing.Point(176, 10);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(19, 21);
            this.labelPrice.TabIndex = 3;
            this.labelPrice.Text = "0";
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelArea.Location = new System.Drawing.Point(176, 38);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(19, 21);
            this.labelArea.TabIndex = 4;
            this.labelArea.Text = "0";
            // 
            // labelCapacity
            // 
            this.labelCapacity.AutoSize = true;
            this.labelCapacity.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelCapacity.Location = new System.Drawing.Point(176, 69);
            this.labelCapacity.Name = "labelCapacity";
            this.labelCapacity.Size = new System.Drawing.Size(19, 21);
            this.labelCapacity.TabIndex = 5;
            this.labelCapacity.Text = "0";
            // 
            // labelTicketPrice
            // 
            this.labelTicketPrice.AutoSize = true;
            this.labelTicketPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelTicketPrice.Location = new System.Drawing.Point(19, 118);
            this.labelTicketPrice.Name = "labelTicketPrice";
            this.labelTicketPrice.Size = new System.Drawing.Size(91, 21);
            this.labelTicketPrice.TabIndex = 6;
            this.labelTicketPrice.Text = "Ticket Price:";
            // 
            // labelMinPeople
            // 
            this.labelMinPeople.AutoSize = true;
            this.labelMinPeople.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelMinPeople.Location = new System.Drawing.Point(18, 157);
            this.labelMinPeople.Name = "labelMinPeople";
            this.labelMinPeople.Size = new System.Drawing.Size(93, 21);
            this.labelMinPeople.TabIndex = 7;
            this.labelMinPeople.Text = "Min. People:";
            // 
            // buttonBuild
            // 
            this.buttonBuild.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonBuild.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonBuild.Location = new System.Drawing.Point(19, 211);
            this.buttonBuild.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonBuild.Name = "buttonBuild";
            this.buttonBuild.Size = new System.Drawing.Size(82, 30);
            this.buttonBuild.TabIndex = 8;
            this.buttonBuild.Text = "Build";
            this.buttonBuild.UseVisualStyleBackColor = true;
            this.buttonBuild.Click += new System.EventHandler(this.buttonBuild_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonCancel.Location = new System.Drawing.Point(197, 211);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(82, 30);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // trackBarPrice
            // 
            this.trackBarPrice.Location = new System.Drawing.Point(127, 118);
            this.trackBarPrice.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarPrice.Name = "trackBarPrice";
            this.trackBarPrice.Size = new System.Drawing.Size(114, 45);
            this.trackBarPrice.TabIndex = 10;
            this.trackBarPrice.Scroll += new System.EventHandler(this.trackBarPrice_Scroll);
            // 
            // trackBarPeople
            // 
            this.trackBarPeople.Location = new System.Drawing.Point(127, 157);
            this.trackBarPeople.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarPeople.Minimum = 1;
            this.trackBarPeople.Name = "trackBarPeople";
            this.trackBarPeople.Size = new System.Drawing.Size(114, 45);
            this.trackBarPeople.TabIndex = 11;
            this.trackBarPeople.Value = 1;
            this.trackBarPeople.Scroll += new System.EventHandler(this.trackBarPeople_Scroll);
            // 
            // labelTicketPriceValue
            // 
            this.labelTicketPriceValue.AutoSize = true;
            this.labelTicketPriceValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelTicketPriceValue.Location = new System.Drawing.Point(260, 118);
            this.labelTicketPriceValue.Name = "labelTicketPriceValue";
            this.labelTicketPriceValue.Size = new System.Drawing.Size(19, 21);
            this.labelTicketPriceValue.TabIndex = 12;
            this.labelTicketPriceValue.Text = "0";
            // 
            // labelMinPeopleValue
            // 
            this.labelMinPeopleValue.AutoSize = true;
            this.labelMinPeopleValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelMinPeopleValue.Location = new System.Drawing.Point(260, 157);
            this.labelMinPeopleValue.Name = "labelMinPeopleValue";
            this.labelMinPeopleValue.Size = new System.Drawing.Size(19, 21);
            this.labelMinPeopleValue.TabIndex = 13;
            this.labelMinPeopleValue.Text = "1";
            // 
            // BuildBuilding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 252);
            this.Controls.Add(this.labelMinPeopleValue);
            this.Controls.Add(this.labelTicketPriceValue);
            this.Controls.Add(this.trackBarPeople);
            this.Controls.Add(this.trackBarPrice);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonBuild);
            this.Controls.Add(this.labelMinPeople);
            this.Controls.Add(this.labelTicketPrice);
            this.Controls.Add(this.labelCapacity);
            this.Controls.Add(this.labelArea);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelOnlyCapacity);
            this.Controls.Add(this.labelOnlyArea);
            this.Controls.Add(this.labelOnlyPrice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "BuildBuilding";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Building";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPeople)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelOnlyPrice;
        private System.Windows.Forms.Label labelOnlyArea;
        private System.Windows.Forms.Label labelOnlyCapacity;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelCapacity;
        private System.Windows.Forms.Label labelTicketPrice;
        private System.Windows.Forms.Label labelMinPeople;
        private System.Windows.Forms.Button buttonBuild;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TrackBar trackBarPrice;
        private System.Windows.Forms.TrackBar trackBarPeople;
        private System.Windows.Forms.Label labelTicketPriceValue;
        private System.Windows.Forms.Label labelMinPeopleValue;
    }
}