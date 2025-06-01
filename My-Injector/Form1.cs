using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using Guna.UI2.WinForms;
using System.Security.Cryptography; // Предполагается, что RKOAPI_Xeno использует это

namespace My_Injector
{
    public partial class Form1 : Form
    {
        // Убедитесь, что chromiumWebBrowser1 объявлен в Form1.Designer.cs как public
        // или что у него есть публичное свойство, чтобы к нему можно было получить доступ извне.
        // public ChromiumWebBrowser chromiumWebBrowser1; // Если создаете вручную, раскомментируйте

        // Предполагается, что у вас есть RichTextBox с именем 'consoleBox' для логов
        // public RichTextBox consoleBox; // Если создаете вручную, раскомментируйте
        private bool isMonacoLoaded = false;
        public Form1()
        {
            InitializeComponent();
            // Подписываемся на событие закрытия формы для корректной очистки CefSharp ресурсов
            this.FormClosing += Form1_FormClosing;
            this.chromiumWebBrowser1.FrameLoadEnd += ChromiumWebBrowser1_FrameLoadEnd;
        }

        private void ChromiumWebBrowser1_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) // <--- Изменился тип аргумента
        {
            // Проверяем, что это главный фрейм, который закончил загрузку
            if (e.Frame.IsMain) // <--- В FrameLoadEndEventArgs обычно есть свойство Frame
            {
                LogToConsole("Monaco Editor HTML-страница полностью загружена (FrameLoadEnd).", LogType.INFO);
                isMonacoLoaded = true;
                // Если Monaco Editor инициализируется после полной загрузки DOM,
                // можно здесь попробовать выполнить скрипт инициализации Monaco Editor,
                // если он не инициализируется автоматически из HTML.
                // Например: await chromiumWebBrowser1.EvaluateScriptAsync("if (typeof editor === 'undefined') { /* инициализация editor */ }");
            }
        }


        // --- Ваши существующие методы и логика ---

        private async void Attachwithapi()
        {
            if (RKOAPI_Xeno.RobloxUtils.IsRobloxRunning())
            {
                LogToConsole("Roblox Has been Detected", LogType.INFO);
                RKOAPI_Xeno.Modules.UseCustomnNotifier("IU", "Inject Succeful!");
                RKOAPI_Xeno.Modules.Inject();
                await ExecuteScriptAsync(); // Выполняет скрипт из Monaco editor
                LogToConsole("Roblox INJECTED!", LogType.Success);
            }
            else
            {
                LogToConsole("Roblox not detected! Download Bloxstrap", LogType.ERROR);
            }
        }

            private async Task CleaWhiteitor() // Метод для очистки Monaco Editor
            {
                string script = "editor.setValue('')"; // JavaScript для очистки Monaco Editor
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
                // Вставка содержимого файла в Monaco Editor
                // Используем обратные кавычки (`) для JS-шаблона, чтобы безопасно вставлять многострочный контент.
                // Также экранируем любые обратные кавычки, которые могут быть в самом контенте.
                string escapedContent = content.Replace("`", "\\`");
                await chromiumWebBrowser1.EvaluateScriptAsync($"editor.setValue(`{escapedContent}`);");
            }
        }

        private async Task<string> GetEditorContent()
        {
            string script = "editor.getValue()"; // JavaScript для получения содержимого Monaco Editor
            var result = await chromiumWebBrowser1.EvaluateScriptAsync(script);
            return result.Result.ToString();
        }

        private async Task ExecuteScriptAsync() // Метод, который отправляет скрипт в твой API
        {
            string scriptcontent = await GetEditorContent(); // Получаем содержимое редактора Monaco
            RKOAPI_Xeno.Modules.ExecuteScript(scriptcontent); // Отправляем скрипт в твой Xeno API
        }

        private async void Execute() // Твой основной метод Execute
        {
            if (RKOAPI_Xeno.RobloxUtils.IsRobloxRunning())
            {
                if (RKOAPI_Xeno.Modules.InjectionCheck() == 1)
                {
                    await ExecuteScriptAsync(); // Выполняет скрипт из Monaco editor
                    LogToConsole("Sent Execution!", LogType.Success);
                }
                else if (RKOAPI_Xeno.Modules.InjectionCheck() == 1) // Дублирующее условие, сохраняем как есть
                {
                    LogToConsole("Not Injected!", LogType.ERROR);
                }
            }
            else
            {
                LogToConsole("Roblox Not Detected!", LogType.ERROR);
            }
        }

        private void Startup() // Твой метод Startup
        {
            // Загрузка Monaco Editor. Убедитесь, что путь правильный.
            chromiumWebBrowser1.Load(System.Windows.Forms.Application.StartupPath + "\\bin\\MonacoWithTabs\\monaco.html");
        }

        private void Minimize() // Твой метод Minimize
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Exit() // Твой метод Exit
        {
            System.Windows.Forms.Application.Exit();
        }

        // --- Логирование (сохраняем как есть) ---
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
            // Используем BeginInvoke для потокобезопасного обновления UI
            if (consoleBox != null) // Проверка, что consoleBox существует
            {
                consoleBox.BeginInvoke((MethodInvoker)delegate
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
                });
            }
        }

        // --- Все твои обработчики событий (сохраняем все дублирующиеся) ---

        private void Execute_Click(object sender, EventArgs e)
        {
            Attachwithapi(); // Это похоже на кнопку "Attach"
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            await CleaWhiteitor(); // Кнопка "Clear"
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Пустой обработчик, сохраняем
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Execute(); // Кнопка "Execute"
        }

        private async void guna2Button4_Click(object sender, EventArgs e)
        {
            await LoadTextFromFile(); // Кнопка "Load"
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Exit(); // Кнопка "Exit"
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Minimize(); // Кнопка "Minimize" (один из вариантов)
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            // Пустой обработчик, сохраняем
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {
            // Пустой обработчик, сохраняем
        }

        public void InitializeChromium()
        {
            // Пустой метод, сохраняем
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Startup(); // Загружает Monaco Editor
            InitializeChromium(); // Пустой метод, сохраняем
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 100;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.guna2Button1, "PRESS TO EXECUTE");
            toolTip1.SetToolTip(this.guna2Button2, "PRESS TO CLEAR");
            toolTip1.SetToolTip(this.guna2Button8, "PRESS TO SAVE"); // В твоем коде это было для LOAD, но потом переопределено для Save. Исправь ToolTip соответственно.
            toolTip1.SetToolTip(this.guna2Button3, "PRESS TO LOAD");  // В твоем коде это было для SAVE, но потом переопределено для Exit. Исправь ToolTip соответственно.
            toolTip1.SetToolTip(this.guna2Button4, "PRESS TO ATTACH"); // В твоем коде это было для ATTACH, но потом переопределено для Load. Исправь ToolTip соответственно.
        }

        private void chromiumWebBrowser1_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // Пустой обработчик, сохраняем
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            Exit(); // Дубликат Exit
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            Execute(); // Дубликат Execute
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            Attachwithapi(); // Дубликат Attach
        }

        private async void guna2Button3_Click_1(object sender, EventArgs e)
        {
            await LoadTextFromFile(); // Дубликат Load
        }

        private async void guna2Button2_Click_1(object sender, EventArgs e)
        {
            await CleaWhiteitor(); // Дубликат Clear
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Minimize(); // Дубликат Minimize (другая кнопка)
        }

        private void chromiumWebBrowser1_LoadingStateChanged_1(object sender, LoadingStateChangedEventArgs e)
        {
            // Пустой обработчик, сохраняем
        }

        private void guna2Button5_Click_2(object sender, EventArgs e)
        {
            Form infoDialog = new Form();
            infoDialog.Text = "говна поешь"; // Твой текст
            infoDialog.StartPosition = FormStartPosition.CenterParent;
            infoDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            infoDialog.ShowInTaskbar = false;
            infoDialog.Size = new System.Drawing.Size(300, 150);
            Label infoLabel = new Label();
            infoLabel.Text = "ты лох бля обоссаный, это лучший инжектор"; // Твой текст
            infoLabel.Dock = DockStyle.Fill;
            infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            infoLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            infoDialog.Controls.Add(infoLabel);
            infoDialog.ShowDialog();
        }

        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            Exit(); // Дубликат Exit
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Minimize(); // Дубликат Minimize
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            // Пустой обработчик, сохраняем
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            // Пустой обработчик, сохраняем
        }

        private void chromiumWebBrowser1_LoadingStateChanged_2(object sender, LoadingStateChangedEventArgs e)
        {
            // Пустой обработчик, сохраняем
        }

        private async void guna2Button8_Click(object sender, EventArgs e)
        {
            await SaveTextAsLua(); // Кнопка "Save" (ранее в ToolTip была "Load")
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            // Открываем Form 'Scripts', передавая ей ссылку на текущий экземпляр Form1 (this).
            Scripts form2 = new Scripts(this);
            form2.Show(); // Открываем Form 'Scripts'
        }

        // --- Метод для вставки Lua-скрипта в Monaco Editor ---
        // Этот метод будет вызываться из формы 'Scripts'
        public async void InsertScriptIntoEditor(string scriptToInsert, string tARGET_ELEMENT_ID)
        {
            if (!isMonacoLoaded)
            {
                LogToConsole("Monaco Editor еще не готов для вставки скрипта. Ожидаем...", LogType.Warning);
                return;
            }

            string escapedScript = scriptToInsert.Replace("`", "\\`");
            string jsToExecute = $"editor.setValue(`{escapedScript}`);"; // Это наш текущий JS-код

            // --- ДОБАВЬТЕ ЭТИ СТРОКИ ДЛЯ ОТЛАДКИ ---
            LogToConsole($"Попытка выполнения JS: {jsToExecute}", LogType.INFO);

            
            // --- КОНЕЦ ДОБАВЛЕННЫХ СТРОК ---

            try
            {
                var result = await chromiumWebBrowser1.EvaluateScriptAsync(jsToExecute);
                if (result.Success)
                {
                    LogToConsole("Lua скрипт успешно вставлен в редактор Monaco.", LogType.Success);
                }
                else
                {
                    LogToConsole($"Ошибка вставки Lua скрипта в Monaco: {result.Message}", LogType.ERROR);
                    // УДАЛИТЕ ЭТИ СТРОКИ:
                    // if (result.Exception != null)
                    // {
                    //     LogToConsole($"JS Exception: {result.Exception.Message}", LogType.ERROR);
                    // }
                }
            }
            catch (Exception ex)
            {
                LogToConsole($"Критическая ошибка при вставке Lua скрипта в Monaco: {ex.Message}", LogType.ERROR);
            }

        }

        // Обработчик закрытия формы для корректной очистки CefSharp
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chromiumWebBrowser1 != null && !chromiumWebBrowser1.IsDisposed)
            {
                chromiumWebBrowser1.Dispose();
            }
            // Если Cef.Shutdown() уже вызывается в Program.cs, здесь не нужно.
            // Cef.Shutdown();
        }
    }
}