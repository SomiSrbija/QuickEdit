using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using ScintillaNET;

namespace QuickEdit
{
    public partial class Form1 : Form
    {
        private string currentFile = string.Empty;

        public Form1()
        {
            InitializeComponent();
            InitializeScintilla();
            UpdateStatus("");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Any initialization code you need when the form loads
        }

        private void InitializeScintilla()
        {
            scintilla1.Margins[0].Width = 16;

            // Code folding settings
            scintilla1.SetProperty("fold", "1");
            scintilla1.SetProperty("fold.compact", "1");

            scintilla1.Margins[2].Type = MarginType.Symbol;
            scintilla1.Margins[2].Mask = Marker.MaskFolders;
            scintilla1.Margins[2].Sensitive = true;
            scintilla1.Margins[2].Width = 20;

            // Configure a margin to display folding symbols
            for (int i = 25; i <= 31; i++)
            {
                scintilla1.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla1.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            scintilla1.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla1.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla1.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla1.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla1.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla1.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla1.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            scintilla1.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        }

        private void UpdateStatus(string message)
        {
            toolStripStatusLabel1.Text = message;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Text = "";
            currentFile = string.Empty;
            UpdateStatus("New file created");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = openFileDialog.FileName;
                scintilla1.Text = File.ReadAllText(currentFile);
                UpdateStatus($"Opened {currentFile}");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFile))
            {
                SaveFileAs();
            }
            else
            {
                SaveFile(currentFile);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void SaveFileAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|Rich Text Files (*.rtf)|*.rtf|HTML Files (*.html)|*.html|All Files (*.*)|*.*",
                DefaultExt = "txt",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = saveFileDialog.FileName;
                SaveFile(currentFile);
                UpdateStatus($"Saved as {currentFile}");
            }
        }

        private void SaveFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".txt":
                    File.WriteAllText(filePath, scintilla1.Text);
                    break;
                case ".rtf":
                    SaveRtf(filePath);
                    break;
                case ".html":
                    SaveHtml(filePath);
                    break;
                default:
                    File.WriteAllText(filePath, scintilla1.Text);
                    break;
            }
        }

        private void SaveRtf(string filePath)
        {
            RichTextBox rtb = new RichTextBox
            {
                Text = scintilla1.Text
            };
            rtb.SaveFile(filePath, RichTextBoxStreamType.RichText);
        }

        private void SaveHtml(string filePath)
        {
            string htmlContent = "<html><body>" + scintilla1.Text.Replace("\n", "<br>") + "</body></html>";
            File.WriteAllText(filePath, htmlContent);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scintilla1.CanUndo)
            {
                scintilla1.Undo();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.SelectAll();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.WrapMode = scintilla1.WrapMode == WrapMode.None ? WrapMode.Word : WrapMode.None;
            wordWrapToolStripMenuItem.Checked = scintilla1.WrapMode == WrapMode.Word;
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                scintilla1.Styles[Style.Default].Font = fontDialog.Font.Name;
                scintilla1.Styles[Style.Default].Size = (int)fontDialog.Font.Size;
                scintilla1.StyleClearAll();  // Apply the style change to all styles
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("QuickEdit v1.0\nA simple text editor.", "About QuickEdit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
