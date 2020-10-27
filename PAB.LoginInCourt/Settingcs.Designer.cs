namespace PAB.LoginInCourt
{
    partial class Settingcs
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
            this.StartSecond = new System.Windows.Forms.TextBox();
            this.EndSecond = new System.Windows.Forms.TextBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Exitdate = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // StartSecond
            // 
            this.StartSecond.Location = new System.Drawing.Point(258, 49);
            this.StartSecond.Name = "StartSecond";
            this.StartSecond.Size = new System.Drawing.Size(36, 20);
            this.StartSecond.TabIndex = 0;
            this.StartSecond.TabStop = false;
            this.StartSecond.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.StartSecond_KeyPress);
            // 
            // EndSecond
            // 
            this.EndSecond.Location = new System.Drawing.Point(346, 49);
            this.EndSecond.Name = "EndSecond";
            this.EndSecond.Size = new System.Drawing.Size(38, 20);
            this.EndSecond.TabIndex = 1;
            this.EndSecond.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EndSecond_KeyPress);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(202, 145);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 3;
            this.BtnSave.Text = "შენახვა";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "დალოგინების ინტერვალი (წამებში)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(300, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "დან";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "პროგრამიდან გამოსვლის დრო";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(390, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "მდე";
            // 
            // Exitdate
            // 
            this.Exitdate.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.Exitdate.Location = new System.Drawing.Point(258, 94);
            this.Exitdate.Name = "Exitdate";
            this.Exitdate.Size = new System.Drawing.Size(101, 20);
            this.Exitdate.TabIndex = 8;
            // 
            // Settingcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 191);
            this.Controls.Add(this.Exitdate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.EndSecond);
            this.Controls.Add(this.StartSecond);
            this.Name = "Settingcs";
            this.Text = "Settingcs";
            this.Load += new System.EventHandler(this.Settingcs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox StartSecond;
        private System.Windows.Forms.TextBox EndSecond;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker Exitdate;
    }
}