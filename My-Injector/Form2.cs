using CefSharp;
using CefSharp.WinForms;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
// ... (остальные using) ...

namespace My_Injector
{
    public partial class Scripts : Form
    {
        private Form1 _mainForm;

        // Константы для первого скрипта
        private const string FILE_DIRECTORY_SCRIPT1 = "LuaScripts";
        private const string FILE_NAME_SCRIPT1 = "InfiniteYield.lua";

        // Константы для второго скрипта
        private const string FILE_DIRECTORY_SCRIPT2 = "LuaScripts";
        private const string FILE_NAME_SCRIPT2 = "OrcaHub.lua";

        private const string FILE_DIRECTORY_SCRIPT3 = "LuaScripts";
        private const string FILE_NAME_SCRIPT3 = "AimbotV2.lua";

        private const string TARGET_ELEMENT_ID = "editor"; // ID элемента Monaco Editor

        // ... (твои другие константы, если они остались и используются) ...

        public Scripts(Form1 mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
        }

        public Scripts() // Твой второй конструктор
        {
            InitializeComponent();
        }

        private void guna2Button6_Click(object sender, EventArgs e) // Твой метод "скрыть"
        {
            this.Hide();
        }

        // Обработчик для первой кнопки (или твоей основной кнопки загрузки)
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Здесь мы вызываем вспомогательный метод
            LoadAndInsertScript(FILE_DIRECTORY_SCRIPT1, FILE_NAME_SCRIPT1);
        }


        // =====================================================================
        // ВОТ ЗДЕСЬ ДОЛЖЕН НАХОДИТЬСЯ МЕТОД LoadAndInsertScript (как отдельный метод класса)
        // =====================================================================
        private void LoadAndInsertScript(string directory, string fileName)
        {
            if (_mainForm == null)
            {
                MessageBox.Show("Ошибка: Ссылка на Form1 не установлена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory, fileName);

            if (!File.Exists(fullFilePath))
            {
                MessageBox.Show($"Ошибка: Файл '{fullFilePath}' не найден. Убедитесь, что файл существует и путь указан верно.", "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string textToInsert = string.Empty;
            try
            {
                textToInsert = File.ReadAllText(fullFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла '{fileName}': {ex.Message}", "Ошибка чтения файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _mainForm.InsertScriptIntoEditor(textToInsert, TARGET_ELEMENT_ID);
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {

            LoadAndInsertScript(FILE_DIRECTORY_SCRIPT2, FILE_NAME_SCRIPT2);

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoadAndInsertScript(FILE_DIRECTORY_SCRIPT3, FILE_NAME_SCRIPT3);
        }
        // =====================================================================
    }
}