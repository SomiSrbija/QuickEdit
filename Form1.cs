using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using ScintillaNET;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QuickEdit
{
    public partial class Form1 : Form
    {
        private string currentFile = string.Empty;
        private bool isDarkMode = false;
        private Process terminalProcess;

        public Form1()
        {
            InitializeComponent();
            InitializeScintilla(scintilla1);
            InitializeScintilla(scintilla2);
            UpdateStatus("");

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            LoadSettings();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Any initialization code you need when the form loads
        }

        private void InitializeScintilla(Scintilla scintilla)
        {
            // Set up the default style
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll();

            // Line numbers
            scintilla.Margins[0].Width = 16;

            // Code folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");

            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Configure a margin to display folding symbols
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            // Syntax highlighting for C#
            scintilla.Lexer = Lexer.Cpp;

            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.Green;
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.Green;
            scintilla.Styles[Style.Cpp.CommentDoc].ForeColor = Color.Gray;
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.Red;
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.Red;
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.Red;
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;

            scintilla.SetKeywords(0, "abstract as base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void volatile while");

            // Set long lines
            scintilla.EdgeMode = EdgeMode.Background;
            scintilla.EdgeColumn = 80;
            scintilla.EdgeColor = Color.LightGray;

            // Drag and drop
            scintilla.AllowDrop = true;
            scintilla.DragEnter += scintilla1_DragEnter;
            scintilla.DragDrop += scintilla1_DragDrop;

            // Inline Error Detection
            scintilla.UpdateUI += scintilla_UpdateUI;
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

        private void scintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // Example of inline error detection for C#
            var scintilla = sender as Scintilla;
            if (scintilla != null)
            {
                scintilla.IndicatorClearRange(0, scintilla.TextLength);
                scintilla.Indicators[0].Style = IndicatorStyle.Squiggle;
                scintilla.Indicators[0].ForeColor = Color.Red;

                // Simple example of detecting a missing semicolon
                var lines = scintilla.Lines;
                foreach (var line in lines)
                {
                    var text = line.Text.Trim();
                    if (!string.IsNullOrEmpty(text) && !text.EndsWith(";") && !text.StartsWith("//") && !text.StartsWith("using") && !text.Contains("{") && !text.Contains("}"))
                    {
                        scintilla.IndicatorCurrent = 0;
                        scintilla.IndicatorFillRange(line.Position, line.Length);
                    }
                }
            }
        }

        private void splitViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scintilla2.Visible)
            {
                scintilla2.Visible = false;
                scintilla1.Dock = DockStyle.Fill;
            }
            else
            {
                scintilla2.Visible = true;
                scintilla1.Dock = DockStyle.Left;
                scintilla1.Width = this.ClientSize.Width / 2;
                scintilla2.Dock = DockStyle.Fill;
            }
        }

        private void terminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            terminalPanel.Visible = !terminalPanel.Visible;
            if (terminalPanel.Visible)
            {
                StartTerminal();
            }
            else
            {
                StopTerminal();
            }
        }

        private void terminalTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string command = terminalTextBox.Lines.Last();
                ExecuteCommand(command);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private async void StartTerminal()
        {
            terminalProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            terminalProcess.OutputDataReceived += (sender, args) => AppendTerminalOutput(args.Data);
            terminalProcess.ErrorDataReceived += (sender, args) => AppendTerminalOutput(args.Data);

            terminalProcess.Start();
            terminalProcess.BeginOutputReadLine();
            terminalProcess.BeginErrorReadLine();

            await Task.Run(() => terminalProcess.WaitForExit());
        }

        private void StopTerminal()
        {
            if (terminalProcess != null && !terminalProcess.HasExited)
            {
                terminalProcess.Kill();
                terminalProcess = null;
            }
        }

        private void ExecuteCommand(string command)
        {
            if (terminalProcess != null && !terminalProcess.HasExited)
            {
                terminalProcess.StandardInput.WriteLine(command);
                terminalProcess.StandardInput.Flush();
            }
        }

        private void AppendTerminalOutput(string output)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendTerminalOutput), output);
            }
            else
            {
                terminalTextBox.AppendText(output + Environment.NewLine);
            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyStyle(Style.Cpp.Word);
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyStyle(Style.Cpp.Word2);
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyStyle(Style.Cpp.Character);
        }

        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                ApplyColor(colorDialog.Color);
            }
        }

        private void ApplyStyle(int style)
        {
            scintilla1.StartStyling(scintilla1.SelectionStart);
            scintilla1.SetStyling(scintilla1.SelectionEnd - scintilla1.SelectionStart, style);
        }

        private void ApplyColor(Color color)
        {
            scintilla1.Styles[Style.Default].ForeColor = color;
            scintilla1.StyleClearAll();  // Apply the color change to all styles
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog(this);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsShortcutPressed(e, Properties.Settings.Default.SaveShortcut))
            {
                saveToolStripMenuItem_Click(sender, e);
                e.Handled = true;
            }
            // Add more shortcuts as needed
        }

        private bool IsShortcutPressed(KeyEventArgs e, string shortcut)
        {
            if (string.IsNullOrEmpty(shortcut)) return false;

            var keys = shortcut.Split('+');
            if (keys.Length != 2) return false;

            bool ctrl = keys[0] == "Ctrl" && e.Control;
            bool keyMatch = Enum.TryParse(keys[1], out Keys key) && e.KeyCode == key;

            return ctrl && keyMatch;
        }

        private void LoadSettings()
        {
            // Set default shortcuts if they are not set
            if (string.IsNullOrEmpty(Properties.Settings.Default.SaveShortcut))
            {
                Properties.Settings.Default.SaveShortcut = "Ctrl+S";
                Properties.Settings.Default.Save();
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void ApplyTheme(bool darkMode)
        {
            if (darkMode)
            {
                this.BackColor = Color.FromArgb(45, 45, 48);
                menuStrip1.BackColor = Color.FromArgb(28, 28, 28);
                menuStrip1.ForeColor = Color.White;
                toolStrip1.BackColor = Color.FromArgb(28, 28, 28);
                toolStrip1.ForeColor = Color.White;
                statusStrip1.BackColor = Color.FromArgb(28, 28, 28);
                statusStrip1.ForeColor = Color.White;
                scintilla1.Styles[Style.Default].BackColor = Color.FromArgb(30, 30, 30);
                scintilla1.Styles[Style.Default].ForeColor = Color.White;
                scintilla2.Styles[Style.Default].BackColor = Color.FromArgb(30, 30, 30);
                scintilla2.Styles[Style.Default].ForeColor = Color.White;
                terminalTextBox.BackColor = Color.FromArgb(30, 30, 30);
                terminalTextBox.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.White;
                menuStrip1.BackColor = Color.LightGray;
                menuStrip1.ForeColor = Color.Black;
                toolStrip1.BackColor = Color.LightGray;
                toolStrip1.ForeColor = Color.Black;
                statusStrip1.BackColor = Color.LightGray;
                statusStrip1.ForeColor = Color.Black;
                scintilla1.Styles[Style.Default].BackColor = Color.White;
                scintilla1.Styles[Style.Default].ForeColor = Color.Black;
                scintilla2.Styles[Style.Default].BackColor = Color.White;
                scintilla2.Styles[Style.Default].ForeColor = Color.Black;
                terminalTextBox.BackColor = Color.White;
                terminalTextBox.ForeColor = Color.Black;
            }
            scintilla1.StyleClearAll();
            scintilla2.StyleClearAll();
        }


        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            if (scintilla1.CanRedo)
            {
                scintilla1.Redo();
            }
        }

        private void Form1_Load_2(object sender, EventArgs e)
        {

        }

        private void Form1_Load_3(object sender, EventArgs e)
        {

        }
    }
}
