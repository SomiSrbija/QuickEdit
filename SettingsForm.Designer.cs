namespace QuickEdit
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelSaveShortcut;
        private System.Windows.Forms.TextBox textBoxSaveShortcut;
        private System.Windows.Forms.Button buttonSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelSaveShortcut = new System.Windows.Forms.Label();
            this.textBoxSaveShortcut = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelSaveShortcut
            // 
            this.labelSaveShortcut.AutoSize = true;
            this.labelSaveShortcut.Location = new System.Drawing.Point(12, 15);
            this.labelSaveShortcut.Name = "labelSaveShortcut";
            this.labelSaveShortcut.Size = new System.Drawing.Size(86, 13);
            this.labelSaveShortcut.TabIndex = 0;
            this.labelSaveShortcut.Text = "Shortcut for Save:";
            // 
            // textBoxSaveShortcut
            // 
            this.textBoxSaveShortcut.Location = new System.Drawing.Point(104, 12);
            this.textBoxSaveShortcut.Name = "textBoxSaveShortcut";
            this.textBoxSaveShortcut.Size = new System.Drawing.Size(158, 20);
            this.textBoxSaveShortcut.TabIndex = 1;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(187, 38);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // SettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 73);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxSaveShortcut);
            this.Controls.Add(this.labelSaveShortcut);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
