using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using ScintillaNET;

namespace QuickEdit
{
    public partial class Form1 : Form
    {
        private string currentFile = string.Empty;
        private bool isDarkMode = false;

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
            // Set up the default style
            scintilla1.StyleResetDefault();
            scintilla1.Styles[Style.Default].Font = "Consolas";
            scintilla1.Styles[Style.Default].Size = 10;
            scintilla1.StyleClearAll();

            // Line numbers
            scintilla1.Margins[0].Width = 16;

            // Code folding
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

            // Syntax highlighting for C#
            scintilla1.Lexer = Lexer.Cpp;

            scintilla1.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            scintilla1.Styles[Style.Cpp.Comment].ForeColor = Color.Green;
            scintilla1.Styles[Style.Cpp.CommentLine].ForeColor = Color.Green;
            scintilla1.Styles[Style.Cpp.CommentDoc].ForeColor = Color.Gray;
            scintilla1.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla1.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            scintilla1.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            scintilla1.Styles[Style.Cpp.String].ForeColor = Color.Red;
            scintilla1.Styles[Style.Cpp.Character].ForeColor = Color.Red;
            scintilla1.Styles[Style.Cpp.Verbatim].ForeColor = Color.Red;
            scintilla1.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla1.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            scintilla1.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;

            scintilla1.SetKeywords(0, "abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while");

            // Set long lines
            scintilla1.EdgeMode = EdgeMode.Background;
            scintilla1.EdgeColumn = 80;
            scintilla1.EdgeColor = Color.LightGray;

            // Drag and drop
            scintilla1.AllowDrop = true;
            scintilla1.DragEnter += scintilla1_DragEnter;
            scintilla1.DragDrop += scintilla1_DragDrop;
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

        private void fontStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                scintilla1.Styles[Style.Default].Font = fontDialog.Font.Name;
                scintilla1.Styles[Style.Default].Size = (int)fontDialog.Font.Size;
                scintilla1.Styles[Style.Default].Italic = fontDialog.Font.Italic;
                scintilla1.Styles[Style.Default].Bold = fontDialog.Font.Bold;
                scintilla1.StyleClearAll();  // Apply the style change to all styles
            }
        }

        private void fontSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                scintilla1.Styles[Style.Default].Size = (int)fontDialog.Font.Size;
                scintilla1.StyleClearAll();  // Apply the style change to all styles
            }
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                scintilla1.Styles[Style.Default].ForeColor = colorDialog.Color;
                scintilla1.StyleClearAll();  // Apply the color change to all styles
            }
        }

        private void darkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isDarkMode)
            {
                scintilla1.Styles[Style.Default].BackColor = Color.White;
                scintilla1.Styles[Style.Default].ForeColor = Color.Black;
                scintilla1.CaretForeColor = Color.Black;
                this.BackColor = Color.White;
                isDarkMode = false;
            }
            else
            {
                scintilla1.Styles[Style.Default].BackColor = Color.Black;
                scintilla1.Styles[Style.Default].ForeColor = Color.White;
                scintilla1.CaretForeColor = Color.White;
                this.BackColor = Color.Black;
                isDarkMode = true;
            }
            scintilla1.StyleClearAll();  // Apply the color change to all styles
        }

        private void findReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FindReplaceForm findReplaceForm = new FindReplaceForm(scintilla1))
            {
                findReplaceForm.ShowDialog(this);
            }
        }

        private void wordCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int wordCount = scintilla1.Text.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            MessageBox.Show($"Word Count: {wordCount}", "Word Count", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("QuickEdit v1.0\nA simple text editor.", "About QuickEdit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void scintilla1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void scintilla1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string fileContent = File.ReadAllText(files[0]);
                    scintilla1.Text = fileContent;
                    currentFile = files[0];
                    UpdateStatus($"Opened {currentFile}");
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                scintilla1.Text += e.Data.GetData(DataFormats.Text).ToString();
            }
        }
    }
}
