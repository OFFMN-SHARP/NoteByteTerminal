using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoteByteTerminal
{
    /// <summary>
    /// CodeEditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CodeEditorWindow : Window
    {
        public CodeEditorWindow()
        {
            InitializeComponent();
        }

        public string CodeTextGet { get; set; }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private string GetIni(string GetSectionName, string GetKey, string DefaultValue)
        {
            var IniReader = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "User"))
                .AddIniFile("Config.ini", optional: false, reloadOnChange: true);
            IConfiguration Configuration = IniReader.Build();
            string GetValue = Configuration[GetSectionName + ":" + GetKey] ?? DefaultValue;
            return GetValue;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EditorSettingGridBackground.Visibility = Visibility.Hidden;
            EditorSettingGridBackground.Opacity = 0;
            CodeEditorTextBox.Text = CodeTextGet ?? String.Empty;
        }

        private void CodeEditorSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All files (*.*)|*.*|CS files (*.cs)|*.cs";
            saveFileDialog.Title = "Save Code";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.ShowDialog() == true && !string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.Write(CodeEditorTextBox.Text);
                }
            }
        }




        private async void CodeEditorSettingButton_Click(object sender, RoutedEventArgs e)
        {
            EditorSettingGridMain.Margin = new Thickness(EditorSettingGridMain.Margin.Left, 0-EditorSettingGridMain.Height, 0, 0);
            for (int i = 0;i<=25;i++)
            {
                EditorSettingGridBackground.Visibility = Visibility.Visible;
                EditorSettingGridBackground.Opacity = i;
                await Task.Delay(10);
            }
            await Task.Delay(1);
            for (int i = 0; i <= EditorSettingGridMain.Height+75; i+=15)
            {
                EditorSettingGridMain.Margin = new Thickness(EditorSettingGridMain.Margin.Left, 0 - EditorSettingGridMain.Height + i, 0, 0);
                await Task.Delay(1);
            }
        }

        private void SettingGridClose_Click(object sender, RoutedEventArgs e)
        {
            EditorSettingGridBackground.Visibility = Visibility.Hidden;
        }
    }
}
