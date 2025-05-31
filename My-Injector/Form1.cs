using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using Guna.UI2.WinForms;

namespace My_Injector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Attachwithapi()
        {

            if (RKOAPI_Xeno.RobloxUtils.IsRobloxRunning())
            {
                LogToConsole("Roblox Has been Detected", LogType.INFO);
                RKOAPI_Xeno.Modules.UseCustomnNotifier("IU", "Inject Succeful!");
                RKOAPI_Xeno.Modules.Inject();
                await ExecuteScriptAsync();

                LogToConsole("Roblox INJECTED!", LogType.Success);
            }
            else
            {
                LogToConsole("Roblox not detected! Download Bloxstrap", LogType.ERROR);
            }
        }





        private async Task CleaWhiteitor()
        {
            string script = "editor.setValue('')";
            await chromiumWebBrowser1.EvaluateScriptAsync(script);
        }

        private async Task SaveTextAsLua()
        {
            string scriptsPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Scripts");


            Directory.CreateDirectory(scriptsPath);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = scriptsPath,
                Filter = "Lua files (*.lua)|*.lua",
                DefaultExt = "lua",
                Title = "Save Lua File"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string content = await GetEditorContent();
                File.WriteAllText(saveFileDialog.FileName, content);
            }
        }

        private async Task LoadTextFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Lua files (*.lua)|*.lua",
                Title = "Open Lua File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string content = File.ReadAllText(openFileDialog.FileName);
                await chromiumWebBrowser1.EvaluateScriptAsync($"editor.setValue(`{content}`);");
            }
        }



        private async Task<string> GetEditorContent()
        {
            string script = "editor.getValue()";
            var result = await chromiumWebBrowser1.EvaluateScriptAsync(script);
            return result.Result.ToString();
        }

        private async Task ExecuteScriptAsync()
        {
            string scriptcontent = await GetEditorContent();
            RKOAPI_Xeno.Modules.ExecuteScript(scriptcontent);

        }

        private async void Execute()
        {
            if (RKOAPI_Xeno.RobloxUtils.IsRobloxRunning())
            {


                if (RKOAPI_Xeno.Modules.InjectionCheck() == 1)
                {
                    await ExecuteScriptAsync();
                    LogToConsole("Sent Execution!", LogType.Success);
                }
                else if (RKOAPI_Xeno.Modules.InjectionCheck() == 1)
                {
                    LogToConsole("Not Injected!", LogType.ERROR);
                }
            }
            else
            {
                LogToConsole("Roblox Not Detected!", LogType.ERROR);
            }
        }

        private void Startup()
        {
            chromiumWebBrowser1.Load(System.Windows.Forms.Application.StartupPath + "\\bin\\MonacoWithTabs\\monaco.html");

        }


        private void Minimize()
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Exit()
        {
            System.Windows.Forms.Application.Exit();
        }

        public enum LogType
        {
            INFO,
            ERROR,
            Warning,
            Success
        }

        public static class ConsoleLogger
        {
            public static Color GetLogColor(LogType type)
            {
                if (type == LogType.INFO)
                    return Color.White;
                if (type == LogType.ERROR)
                    return Color.Red;
                if (type == LogType.Warning)
                    return Color.Yellow;
                if (type == LogType.Success)
                    return Color.Green;

                return Color.White;
            }
        }

        private void LogToConsole(string message, LogType type)
        {
            string timestamp = DateTime.Now.ToString("HH:mm");
            Color logColor = ConsoleLogger.GetLogColor(type);

            consoleBox.SelectionStart = consoleBox.TextLength;
            consoleBox.SelectionLength = 0;


            consoleBox.SelectionColor = logColor;
            consoleBox.AppendText($"[ {type} ]");


            consoleBox.SelectionColor = Color.White;
            consoleBox.AppendText($" [{timestamp}] ");


            consoleBox.SelectionColor = logColor;
            consoleBox.AppendText($"{message}\n");

            consoleBox.ScrollToCaret();
        }

        private void Execute_Click(object sender, EventArgs e)
        {
            Attachwithapi();
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            await CleaWhiteitor();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private async void guna2Button4_Click(object sender, EventArgs e)
        {
            await LoadTextFromFile();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Minimize();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Startup();
        }

        private void chromiumWebBrowser1_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {

        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            Exit();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            Execute();
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            Attachwithapi();
        }

        private async void guna2Button3_Click_1(object sender, EventArgs e)
        {
            await LoadTextFromFile();
        }

        private async void guna2Button2_Click_1(object sender, EventArgs e)
        {
            await CleaWhiteitor();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Minimize();
        }

        private void chromiumWebBrowser1_LoadingStateChanged_1(object sender, LoadingStateChangedEventArgs e)
        {

        }

        private void guna2Button5_Click_2(object sender, EventArgs e)
        {
            Form infoDialog = new Form();
            infoDialog.Text = "говна поешь";
            infoDialog.StartPosition = FormStartPosition.CenterParent;
            infoDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            infoDialog.ShowInTaskbar = false;
            infoDialog.Size = new System.Drawing.Size(300, 150);
            Label infoLabel = new Label();
            infoLabel.Text = "ты лох бля обоссаный, это лучший инжектор";
            infoLabel.Dock = DockStyle.Fill;
            infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            infoLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            infoDialog.Controls.Add(infoLabel);
            infoDialog.ShowDialog();
        }

        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            Exit();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Minimize();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void chromiumWebBrowser1_LoadingStateChanged_2(object sender, LoadingStateChangedEventArgs e)
        {

        }

        private async void guna2Button8_Click(object sender, EventArgs e)
        {
            await SaveTextAsLua();
        }
    }
}
