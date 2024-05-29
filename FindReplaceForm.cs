using System;
using System.Windows.Forms;
using ScintillaNET;

namespace QuickEdit
{
    public partial class FindReplaceForm : Form
    {
        private Scintilla _scintilla;

        public FindReplaceForm(Scintilla scintilla)
        {
            InitializeComponent();
            _scintilla = scintilla;
        }

        private void buttonFindNext_Click(object sender, EventArgs e)
        {
            string textToFind = textBoxFind.Text;
            int startPos = _scintilla.CurrentPosition;
            int findPos = _scintilla.Text.IndexOf(textToFind, startPos);

            if (findPos != -1)
            {
                _scintilla.SelectionStart = findPos;
                _scintilla.SelectionEnd = findPos + textToFind.Length;
                _scintilla.ScrollCaret();
            }
            else
            {
                MessageBox.Show("Text not found", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonReplace_Click(object sender, EventArgs e)
        {
            if (_scintilla.SelectedText == textBoxFind.Text)
            {
                _scintilla.ReplaceSelection(textBoxReplace.Text);
            }
            buttonFindNext_Click(sender, e);
        }

        private void buttonReplaceAll_Click(object sender, EventArgs e)
        {
            string textToFind = textBoxFind.Text;
            string textToReplace = textBoxReplace.Text;

            _scintilla.TargetStart = 0;
            _scintilla.TargetEnd = _scintilla.TextLength;
            _scintilla.SearchFlags = SearchFlags.None;

            while (_scintilla.SearchInTarget(textToFind) != -1)
            {
                _scintilla.ReplaceTarget(textToReplace);
                _scintilla.TargetStart = _scintilla.TargetEnd;
                _scintilla.TargetEnd = _scintilla.TextLength;
            }
        }
    }
}
