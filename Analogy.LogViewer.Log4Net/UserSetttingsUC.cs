using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.LogViewer.Log4Net.Managers;
using Newtonsoft.Json.Linq;

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
            txtbSupportedFiles.Text = string.Join(";",Settings.SupportFormats.ToList());
            txtbRegEx.Text = Settings.RegExPattern.Pattern;
            txtbDateTimeFormat.Text = Settings.RegExPattern.DateTimeFormat;
        }

        private void SaveSettings()
        {
            Settings.LogsLocation=txtLogsLocation.Text ;
            Settings.FileOpenDialogFilters = txtbOpenFileFilters.Text;
            Settings.SupportFormats = txtbSupportedFiles.Text.Split(new[]{";"},StringSplitOptions.RemoveEmptyEntries ).ToList();
            Settings.RegExPattern.Pattern= txtbRegEx.Text;
            Settings.RegExPattern.DateTimeFormat=txtbDateTimeFormat.Text ;
        }
    }
}
