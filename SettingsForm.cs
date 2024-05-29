using System;
using System.Windows.Forms;

namespace QuickEdit
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            textBoxSaveShortcut.Text = Properties.Settings.Default.SaveShortcut;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SaveShortcut = textBoxSaveShortcut.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
