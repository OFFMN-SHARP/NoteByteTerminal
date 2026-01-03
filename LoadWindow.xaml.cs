using Microsoft.Extensions.Configuration;
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
    /// LoadWindow.xaml 的交互逻辑
    /// 
    /// 这个窗口需要进行ini文件处理，读取/写入ini文件，并将读取到的内容放在public变量中
    /// 
    /// 
    /// </summary>
    public partial class LoadWindow : Window
    {
        public LoadWindow()
        {
            InitializeComponent();
        }

        /*
         需要读取的内容：
[Terminal]
CommandCore = powershell.exe
WorkingDirectory = C:\Users\unknown\Desktop
[About]
Version = 1.0.0
User =unknown
ProgramType = Terminal&Workstation
ProgramName = NoteByte Terminal
ProgramDescription = Terminal.But not just a terminal.
[EasterEgg]
NotSupportLinux = Yes
[Server]
Mode = Local
Port = 29963
InternetPost = http://localhost:29963/
[Note]
CommandHistorySavePatch = ./CommandHistory.txt
[Log]
Patch =./Log.txt
[User]
Username = unknown
Permession = unknown
[System]
OSVer=Windows 10
[Themes]
SelectedTheme = white
[Screenshot]
SavePath =./Screenshot/
[Settings]
AutoGetAdminPermission = No
DefaultWorkingDirectory = C:\Users\unknown\Desktop
NotAllowCommand = null
AutoRunPowerShellScriptPath = null
[DownloadManager]
SavePath =./Download/
DownloadProgress = 1
[Language]
LanguageList = English,Chinese
SelectedLanguage = Chinese
[ProgramLanguage]
SupportLanguage = CPP,C,Java,Python,JavaScript
         */

        //====================================
        public string Out_Terminal_CommandCore;
        public string Out_Terminal_WorkingDirectory;
        //====================================
        public string Out_About_Version;
        public string Out_About_User;
        public string Out_About_ProgramType;
        public string Out_About_ProgramName;
        public string Out_About_ProgramDescription;
        //====================================
        public string Out_EasterEgg_NotSupportLinux;
        //====================================
        public string Out_Server_Mode;
        public string Out_Server_Port;
        public string Out_Server_InternetPost;
        //====================================
        public string Out_Note_CommandHistorySavePatch;
        //====================================
        public string Out_Log_Patch;
        //====================================
        public string Out_User_Username;
        public string Out_User_Permession;
        //====================================
        public string Out_System_OSVer;
        //====================================
        public string Out_Themes_SelectedTheme;
        //====================================
        public string Out_Screenshot_SavePath;
        //====================================
        public string Out_Settings_AutoGetAdminPermission;
        public string Out_Settings_DefaultWorkingDirectory;
        public string Out_Settings_NotAllowCommand;
        public string Out_Settings_AutoRunPowerShellScriptPath;
        //====================================
        public string Out_DownloadManager_SavePath;
        public string Out_DownloadManager_DownloadProgress;
        //====================================
        public string Out_Language_LanguageList;
        public string Out_Language_SelectedLanguage;
        //====================================
        public string Out_ProgramLanguage_SupportLanguage;
        //====================================

        private void IfFileNotExistCreate()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + ".\\User"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + ".\\User");
            }
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + ".\\User\\Config.ini"))
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + ".\\User\\Config.ini").Close();
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + ".\\User\\Config.ini", false, Encoding.UTF8))
                {
                    sw.WriteLine("[Terminal]");
                    sw.WriteLine("CommandCore = powershell.exe");
                    sw.WriteLine($"WorkingDirectory = C:\\Users\\{Environment.UserName}\\Desktop");
                    sw.WriteLine("[About]");
                    sw.WriteLine("Version = 1.0.0");
                    sw.WriteLine($"User ={Environment.UserName}");
                    sw.WriteLine("ProgramType = Terminal&Workstation");
                    sw.WriteLine("ProgramName = NoteByte Terminal");
                    sw.WriteLine("ProgramDescription = Terminal.But not just a terminal.");
                    sw.WriteLine("[EasterEgg]");
                    sw.WriteLine("NotSupportLinux = Yes");
                    sw.WriteLine("[Server]");
                    sw.WriteLine("Mode = Local");
                    sw.WriteLine("Port = 29963");
                    sw.WriteLine("InternetPost = http://localhost:29963/");
                    sw.WriteLine("[Note]");
                    sw.WriteLine("CommandHistorySavePatch = ./CommandHistory.txt");
                    sw.WriteLine("[Log]");
                    sw.WriteLine("Patch =./Log.txt");
                    sw.WriteLine("[User]");
                    sw.WriteLine($"Username = {Environment.UserName}");
                    sw.WriteLine("Permession = null");
                    sw.WriteLine("[System]");
                    sw.WriteLine($"OSVer= {PlatformID.Win32NT.ToString()}");
                    sw.WriteLine("[Themes]");
                    sw.WriteLine("SelectedTheme = white");
                    sw.WriteLine("[Screenshot]");
                    sw.WriteLine("SavePath =./Screenshot/");
                    sw.WriteLine("[Settings]");
                    sw.WriteLine("AutoGetAdminPermission = No");
                    sw.WriteLine($"DefaultWorkingDirectory = C:\\Users\\{Environment.UserName}\\Desktop");
                    sw.WriteLine("NotAllowCommand = null");
                    sw.WriteLine("AutoRunPowerShellScriptPath = null");
                    sw.WriteLine("[DownloadManager]");
                    sw.WriteLine("SavePath =./Download/");
                    sw.WriteLine("DownloadProgress = 1");
                    sw.WriteLine("[Language]");
                    sw.WriteLine("LanguageList = English,Chinese");
                    sw.WriteLine("SelectedLanguage = Chinese");
                    sw.WriteLine("[ProgramLanguage]");
                    sw.WriteLine("SupportLanguage = CPP,C,Java,Python,JavaScript");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建配置文件出错！\n" + ex.Message);

            }
        }











        private async void LoadWindows_Loaded(object sender, RoutedEventArgs e)
        {
            if(!File.Exists(AppDomain.CurrentDomain.BaseDirectory + ".\\User\\Config.ini"))
            {
                IfFileNotExistCreate();
            }

            var IniReader = new ConfigurationBuilder()
                        .SetBasePath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "User"))
                        .AddIniFile("Config.ini", optional: false, reloadOnChange: true);
            IConfiguration Configuration = IniReader.Build();
            string ReadDirPath = AppDomain.CurrentDomain.BaseDirectory + ".\\User\\Config.ini";
            ReadDirTextBlock.Text = $"目标:{ReadDirPath}";
            GetConfigAndLoadOthersBar.Value = 0;
            await Task.Delay(1);
            int WaitTime = 10;
            //try { await Task.Run(() => { string ValueGet = Configuration["Null"] ?? "Null"; }); }catch (Exception) { }//占位符
            for (int i = 0; i < 14; i++) 
            {
                try
                {
                    switch (i)
                    {
                        case 0:
                            //TerminalSection
                            Out_Terminal_CommandCore = Configuration["Terminal:CommandCore"] ?? "powershell.exe";
                            Out_Terminal_WorkingDirectory = Configuration["Terminal:WorkingDirectory"] ?? "C:\\Users\\unknown\\Desktop";
                            GetConfigAndLoadOthersBar.Value = 100 * (1 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 1:
                            //AboutSection
                            Out_About_Version = Configuration["About:Version"] ?? "1.0.0";
                            Out_About_User = Configuration["About:User"] ?? "unknown";
                            Out_About_ProgramType = Configuration["About:ProgramType"] ?? "Terminal&Workstation";
                            Out_About_ProgramName = Configuration["About:ProgramName"] ?? "NoteByte Terminal";
                            Out_About_ProgramDescription = Configuration["About:ProgramDescription"] ?? "Terminal.But not just a terminal.";
                            GetConfigAndLoadOthersBar.Value = 100 *(2 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 2:
                            //EasterEggSection
                            Out_EasterEgg_NotSupportLinux = Configuration["EasterEgg:NotSupportLinux"] ?? "Yes";
                            GetConfigAndLoadOthersBar.Value = 100 * (3 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 3:
                            //ServerSection
                            Out_Server_Mode = Configuration["Server:Mode"] ?? "Local";
                            Out_Server_Port = Configuration["Server:Port"] ?? "29963";
                            Out_Server_InternetPost = Configuration["Server:InternetPost"] ?? "http://localhost:29963/";
                            GetConfigAndLoadOthersBar.Value = 100 * (4 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 4:
                            //NoteSection
                            Out_Note_CommandHistorySavePatch = Configuration["Note:CommandHistorySavePatch"] ?? "./CommandHistory.txt";
                            GetConfigAndLoadOthersBar.Value = 100 * (5 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 5:
                            //LogSection
                            Out_Log_Patch = Configuration["Log:Patch"] ?? "./Log.txt";
                            GetConfigAndLoadOthersBar.Value = 100 * (6 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 6:
                            //UserSection
                            Out_User_Username = Configuration["User:Username"] ?? "unknown";
                            Out_User_Permession = Configuration["User:Permession"] ?? "unknown";
                            GetConfigAndLoadOthersBar.Value = 100 * (7 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 7:
                            //SystemSection
                            Out_System_OSVer = Configuration["System:OSVer"] ?? "Windows 10";
                            GetConfigAndLoadOthersBar.Value = 100 * (8 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 8:
                            //ThemesSection
                            Out_Themes_SelectedTheme = Configuration["Themes:SelectedTheme"] ?? "white";
                            GetConfigAndLoadOthersBar.Value = 100 * (9 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 9:
                            //ScreenshotSection
                            Out_Screenshot_SavePath = Configuration["Screenshot:SavePath"] ?? "./Screenshot/";
                            GetConfigAndLoadOthersBar.Value = 100 * (10 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 10:
                            //SettingsSection
                            Out_Settings_AutoGetAdminPermission = Configuration["Settings:AutoGetAdminPermission"] ?? "No";
                            Out_Settings_DefaultWorkingDirectory = Configuration["Settings:DefaultWorkingDirectory"] ?? "C:\\Users\\unknown\\Desktop";
                            Out_Settings_NotAllowCommand = Configuration["Settings:NotAllowCommand"] ?? "null";
                            Out_Settings_AutoRunPowerShellScriptPath = Configuration["Settings:AutoRunPowerShellScriptPath"] ?? "null";
                            GetConfigAndLoadOthersBar.Value = 100 * (11 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 11:
                            //DownloadManagerSection
                            Out_DownloadManager_SavePath = Configuration["DownloadManager:SavePath"] ?? "./Download/";
                            Out_DownloadManager_DownloadProgress = Configuration["DownloadManager:DownloadProgress"] ?? "1";
                            GetConfigAndLoadOthersBar.Value =100 * (12 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 12:
                            //LanguageSection
                            Out_Language_LanguageList = Configuration["Language:LanguageList"] ?? "English,Chinese";
                            Out_Language_SelectedLanguage = Configuration["Language:SelectedLanguage"] ?? "Chinese";
                            GetConfigAndLoadOthersBar.Value = 100 * (13 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        case 13:
                            //ProgramLanguageSection
                            Out_ProgramLanguage_SupportLanguage = Configuration["ProgramLanguage:SupportLanguage"] ?? "CPP,C,Java,Python,JavaScript";
                            GetConfigAndLoadOthersBar.Value = 100 * (14 / 14);
                            await Task.Delay(WaitTime);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取配置文件出错！\n"+ex.Message);
                }
            }
            GetConfigAndLoadOthersBar.Value = 100;
            this.Close();
        }
    }
}
