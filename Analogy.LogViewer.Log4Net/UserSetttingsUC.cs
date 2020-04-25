using Analogy.Interfaces;
using Analogy.LogViewer.Log4Net.Managers;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Analogy.LogViewer.Log4Net
{
    public partial class UserSetttingsUC : UserControl
    {

        private UserSettings Settings => UserSettingsManager.UserSettings.Settings;
        public UserSetttingsUC()
        {
            InitializeComponent();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            })
            {
                // Show the FolderBrowserDialog.  
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtLogsLocation.Text = folderDlg.SelectedPath;
                }
            }
        }

        private void UserSetttingsUC_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            txtLogsLocation.Text = Settings.LogsLocation;
            txtbOpenFileFilters.Text = Settings.FileOpenDialogFilters;
            txtbSupportedFiles.Text = string.Join(";", Settings.SupportFormats.ToList());
            txtbRegEx.Text = string.Join(";", Settings.RegexPatterns.Select(r => r.Pattern));
            txtbDateTimeFormat.Text = Settings.RegexPatterns.First().DateTimeFormat;
        }

        private void SaveSettings()
        {
            Settings.LogsLocation = txtLogsLocation.Text;
            Settings.FileOpenDialogFilters = txtbOpenFileFilters.Text;
            Settings.SupportFormats = txtbSupportedFiles.Text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Settings.RegexPatterns = txtbRegEx.Text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => new RegexPattern(s, txtbDateTimeFormat.Text, "")).ToList();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            RegexPattern p = new RegexPattern(txtbRegEx.Text, txtbDateTimeFormat.Text, "");
            bool valid = RegexParser.CheckRegex(txtbTest.Text, p, out AnalogyLogMessage m);
            if (valid)
            {
                lblResult.Text = "Valid Regular Expression";
                lblResult.BackColor = Color.GreenYellow;
                lblResultMessage.Text = m.ToString();
            }
            else
            {
                lblResult.Text = "Non Valid Regular Expression";
                lblResult.BackColor = Color.OrangeRed;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
