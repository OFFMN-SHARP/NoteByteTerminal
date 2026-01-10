using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteByteTerminal
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /*
         
        输出格式(Debug)：

        >[Command](PID: [Process ID])
        Started [Command] in [Working Directory]...
        =========<Output Start>=========
        [输出内容]
        =========<Error Start>==========
        [错误内容]
        =========<End>==================
        Time:[Time]
        Status: [Exit Status]
        Duration: [Duration]
        Add-onProcess: [Add-on Process Info/NULL]
        LogFile: [Log File Path/NULL]
        SystemDiskUsage: [Disk Usage Info]
        Platform: [Operating System Info]
        DirectXVersion: [DirectX Version Info]
        .NETVersion: [.NET Version Info]
        .NETEdition: [.NET Edition Info]
        VC_Redist: [VC Redist Info]
        MemoryStatus: [Memory Status Info]
        CPUStatus: [CPU Status Info]
        NetworkStatus: [Network Status Info]
        UserStatus: [User Status Info]
        PermissionLevel: [Permission Level Info]

        输出格式(Normal)：
        >[Command](PID: [Process ID])
        Started [Command] in [Working Directory]...
        =========<Output Start>=========
        [输出内容]
        =========<Error Start>==========
        [错误内容]
        =========<End>==================
        Exit Status: [Exit Status]

        输出格式(Minimal)：
        >[Command] at [Time] in [Working Directory] ...
        [输出内容]
        [错误内容]
        [Exit Status]

        输出格式(Micro)：
        >[Command]
        [输出内容]
        [错误内容]
        [Exit Status]
        [Working Directory]

        输出格式(Tiny)：
        >[Command]
        [Exit Status]

         */
       

        private readonly object lockObject = new object();
        private Process TerminalCommand;
        private string WorkingDirectoryText = "";

        private string GetIni(string GetSectionName,string GetKey,string DefaultValue)
        {
            var IniReader = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "User"))
                .AddIniFile("Config.ini", optional: false, reloadOnChange: true);
            IConfiguration Configuration = IniReader.Build();
            string GetValue = Configuration[GetSectionName + ":" + GetKey] ?? DefaultValue;
            return GetValue;
        }
        private System.Timers.Timer UpsateTextTimer;
        private async Task TerminalCommand_Run(int OutputMode=0)
        {
            TerminalCommand = new Process();
            UpsateTextTimer = new System.Timers.Timer();
            string Check = "";
            Dispatcher.Invoke(() =>
            {
                Check = CommandText.Text.ToLower();
            });
            if(Check == "nedit")
            {
                TerminalIsRuningTheCommand = false;
                Dispatcher.Invoke(() =>
                {
                    CodeEditorGrid.Visibility = Visibility.Visible;
                    CodeEditorTextBox.Text = ArgsText.Text;
                });
                
                return;
            }
            if(Check == "help" || Check == "h")
            {
                string HelpText = "";
                string core = GetIni("Terminal", "CommandCore", "cmd.exe");
                if(core == "cmd.exe")
                {
                    HelpText = @"ASSOC          显示或修改文件扩展名关联。
ATTRIB         显示或更改文件属性。
BREAK          设置或清除扩展式 CTRL+C 检查。
BCDEDIT        设置启动数据库中的属性以控制启动加载。
CACLS          显示或修改文件的访问控制列表(ACL)。
CALL           从另一个批处理程序调用这一个。
CD             显示当前目录的名称或将其更改。
CHCP           显示或设置活动代码页数。
CHDIR          显示当前目录的名称或将其更改。
CHKDSK         检查磁盘并显示状态报告。
CHKNTFS        显示或修改启动时间磁盘检查。
CLS            清除屏幕。
CMD            打开另一个 Windows 命令解释程序窗口。
COLOR          设置默认控制台前景和背景颜色。
COMP           比较两个或两套文件的内容。
COMPACT        显示或更改 NTFS 分区上文件的压缩。
CONVERT        将 FAT 卷转换成 NTFS。你不能转换
               当前驱动器。
COPY           将至少一个文件复制到另一个位置。
DATE           显示或设置日期。
DEL            删除至少一个文件。
DIR            显示一个目录中的文件和子目录。
DISKPART       显示或配置磁盘分区属性。
DOSKEY         编辑命令行、撤回 Windows 命令并
               创建宏。
DRIVERQUERY    显示当前设备驱动程序状态和属性。
ECHO           显示消息，或将命令回显打开或关闭。
ENDLOCAL       结束批文件中环境更改的本地化。
ERASE          删除一个或多个文件。
EXIT           退出 CMD.EXE 程序(命令解释程序)。
FC             比较两个文件或两个文件集并显示
               它们之间的不同。
FIND           在一个或多个文件中搜索一个文本字符串。
FINDSTR        在多个文件中搜索字符串。
FOR            为一组文件中的每个文件运行一个指定的命令。
FORMAT         格式化磁盘，以便用于 Windows。
FSUTIL         显示或配置文件系统属性。
FTYPE          显示或修改在文件扩展名关联中使用的文件
               类型。
GOTO           将 Windows 命令解释程序定向到批处理程序
               中某个带标签的行。
GPRESULT       显示计算机或用户的组策略信息。
HELP           提供 Windows 命令的帮助信息。
ICACLS         显示、修改、备份或还原文件和
               目录的 ACL。
IF             在批处理程序中执行有条件的处理操作。
LABEL          创建、更改或删除磁盘的卷标。
MD             创建一个目录。
MKDIR          创建一个目录。
MKLINK         创建符号链接和硬链接
MODE           配置系统设备。
MORE           逐屏显示输出。
MOVE           将一个或多个文件从一个目录移动到另一个
               目录。
OPENFILES      显示远程用户为了文件共享而打开的文件。
PATH           为可执行文件显示或设置搜索路径。
PAUSE          暂停批处理文件的处理并显示消息。
POPD           还原通过 PUSHD 保存的当前目录的上一个
               值。
PRINT          打印一个文本文件。
PROMPT         更改 Windows 命令提示。
PUSHD          保存当前目录，然后对其进行更改。
RD             删除目录。
RECOVER        从损坏的或有缺陷的磁盘中恢复可读信息。
REM            记录批处理文件或 CONFIG.SYS 中的注释(批注)。
REN            重命名文件。
RENAME         重命名文件。
REPLACE        替换文件。
RMDIR          删除目录。
ROBOCOPY       复制文件和目录树的高级实用工具
SET            显示、设置或删除 Windows 环境变量。
SETLOCAL       开始本地化批处理文件中的环境更改。
SC             显示或配置服务(后台进程)。
SCHTASKS       安排在一台计算机上运行命令和程序。
SHIFT          调整批处理文件中可替换参数的位置。
SHUTDOWN       允许通过本地或远程方式正确关闭计算机。
SORT           对输入排序。
START          启动单独的窗口以运行指定的程序或命令。
SUBST          将路径与驱动器号关联。
SYSTEMINFO     显示计算机的特定属性和配置。
TASKLIST       显示包括服务在内的所有当前运行的任务。
TASKKILL       中止或停止正在运行的进程或应用程序。
TIME           显示或设置系统时间。
TITLE          设置 CMD.EXE 会话的窗口标题。
TREE           以图形方式显示驱动程序或路径的目录
               结构。
TYPE           显示文本文件的内容。
VER            显示 Windows 的版本。
VERIFY         告诉 Windows 是否进行验证，以确保文件
               正确写入磁盘。
VOL            显示磁盘卷标和序列号。
XCOPY          复制文件和目录树。
WMIC           在交互式命令 shell 中显示 WMI 信息。";
                }
                if (core == "powershell.exe")
                {
                    HelpText = @"UNSUPPORT";
                }
                Dispatcher.Invoke(() =>
                {
                    TerminalOutput.AppendText(
    @"
========= NoteByte Terminal Help =========
"
    +
    "\n"
    +
    $"{HelpText}"
    +
    "\n"
    +
    "========= NoteByte Terminal ========="
    +
    @"
nedit           Open NoteByte Terminal Code Editor
"
    +
    "========== End =========="
                        );
                });
                return;
            }
            TerminalIsRuningTheCommand = true;
            Dispatcher.Invoke(() =>
            {
                string AllText = TerminalOutput.Text;
            });
            string PFileName = "";
            PFileName = GetIni("Terminal", "CommandCore", "cmd.exe");
            Dispatcher.Invoke(() =>
            {
                WorkingDirectoryText = WorkDirText.Text;
            });
            if (!Directory.Exists(WorkingDirectoryText))
            {
                WorkingDirectoryText = "C:\\";
                WorkDirText.Text = "C:\\";
                if (!Directory.Exists(WorkingDirectoryText))
                {
                    WorkingDirectoryText = Directory.GetCurrentDirectory();
                    WorkDirText.Text = Directory.GetCurrentDirectory();
                }
            }
            string Terminal_Args = "";
            Dispatcher.Invoke(() =>
            {
                Terminal_Args = " /c " + CommandText.Text + " " + ArgsText.Text;
            });
            var Terminal_CommandInfo = new ProcessStartInfo
            {
                FileName = PFileName,
                Arguments =Terminal_Args,
                WorkingDirectory = WorkingDirectoryText,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            string ConsoleOutputTextString=String.Empty;
            string ConsoleErrorTextString =String.Empty;
            string ConsoleTextString = String.Empty;
            TerminalCommand.StartInfo = Terminal_CommandInfo;
            TerminalCommand.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    lock (lockObject)
                    {
                        ConsoleOutputTextString += args.Data + "\n";
                    }
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TerminalOutput.AppendText(args.Data + "\n");
                        TerminalOutput.ScrollToEnd();
                    }));
                }
            };
            bool IsOutputError=false;
            bool IsOutErrorEnd=false;
            TerminalCommand.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    ConsoleErrorTextString += args.Data + "\n";
                }
                if (!IsOutputError)
                {
                    IsOutputError = true;
                    Dispatcher.Invoke(() => 
                    {
                        string Text = "=========<Error Start>=========\n";
                        TerminalOutput.AppendText(Text);
                    }
                    );
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TerminalOutput.AppendText(args.Data + "\n");
                    TerminalOutput.ScrollToEnd();
                }));

                
            };
            try
            {
                
                TerminalCommand.Start();
                Dispatcher.Invoke(() =>
                {
                    string startText = $">[{CommandText.Text + " " + ArgsText.Text}](PID: {TerminalCommand.Id})\nStarted {CommandText.Text + " " + ArgsText.Text} in {WorkingDirectoryText}...\n=========<Output Start>=========\n";
                    TerminalOutput.AppendText(startText);
                });
                TerminalCommand.BeginOutputReadLine();
                TerminalCommand.BeginErrorReadLine();
                if (IsOutErrorEnd)
                {
                    Dispatcher.Invoke(() =>
                    {
                        string Text = "=========<End>==================\n";
                        TerminalOutput.AppendText(Text);
                        TerminalOutput.ScrollToEnd();
                    });
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
            
            
            try
            {
                await Task.Run(() => TerminalCommand.WaitForExit());
            }
            finally
            {
                
            }
            string ExitStatus = "";
            Dispatcher.Invoke(() => {
                ExitStatus =TerminalCommand.ExitCode.ToString();
            });
            Dispatcher.Invoke(() =>
            {
            ExitCodeText.Text = "退出代码:" + ExitStatus;
                /*
            TerminalOutput.Document.Blocks.Clear();
            TerminalOutput.Document.Blocks.Add(new Paragraph(new Run(AllText + "\n" + ConsoleTextString)));
                */
                Dispatcher.Invoke(() =>
                {
                    string Text = "=========<End>==================\n";
                    TerminalOutput.AppendText(Text);
                    TerminalOutput.ScrollToEnd();
                });
            });
            await Task.Run(() => {
                TerminalCommand.WaitForExit();
                TerminalCommand.Dispose();
            });
        }

        private System.Windows.Forms.NotifyIcon TerminalNotifyIcon = new System.Windows.Forms.NotifyIcon();
        private void InitializeNotifyIcon()
        {
            using (var iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Main.ico")).Stream)
            {
                TerminalNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
            }
            TerminalNotifyIcon.Visible = true;
            TerminalNotifyIcon.Text = "NoteByte Terminal";

            System.Windows.Forms.ContextMenuStrip menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("Close Terminal", null, (s1, e1) =>
            {
                TerminalCommand?.Dispose();
                process?.Dispose();
                UpsateTextTimer?.Dispose();
                Environment.Exit(0);
            });
            menu.Items.Add("About NoteByte Terminal" , null, (s2, e2) =>
            {
                MessageBox.Show("NoteByte Terminal\nVersion: 1.0.0\n", "About", MessageBoxButton.OK, MessageBoxImage.Information);
            });
            menu.Items.Add("NoteByte Terminal Settings", null, (s3, e3) =>
            {
                MessageBox.Show("Incomplete Function", "NoteByte Terminal Settings", MessageBoxButton.OK, MessageBoxImage.Information);
            });
            TerminalNotifyIcon.Click += (s, e) => ShowMenu();
            void ShowMenu()
            {
                TerminalNotifyIcon.ContextMenuStrip = menu;
            }
        }
        private void CommandText_MouseEnter(object sender, MouseEventArgs e)
        {
            CommandText.ToolTip = "Enter your command here";
            CommandText.IsDropDownOpen = true;
            string searchText = CommandText.Text;
            if (!string.IsNullOrEmpty(searchText))
            {
                // 在命令列表中查找匹配的命令
                foreach (var item in CommandText.Items)
                {
                    if (item.ToString().StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        CommandText.SelectedItem = item;
                        break;
                    }
                }
            }
        }
        private void CommandText_MouseLeave(object sender, MouseEventArgs e)
        {
            CommandText.ToolTip = null;
            CommandText.IsDropDownOpen = false;
        }

        private void TerminalWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeNotifyIcon();
            CodeEditorGrid.Visibility = Visibility.Hidden;
            WorkDirText.Text = GetIni("Terminal", "WorkingDirectory", "C:\\");
            TerminalWindow.Left = 0;
            TerminalWindow.Top = 0;
            WindowPositionEventCount = 0;
            EventWindowPositionUpdate();
            CommandText.MouseEnter += CommandText_MouseEnter;
            CommandText.MouseLeave += CommandText_MouseLeave;
            string core = GetIni("Terminal", "CommandCore", "cmd.exe");
            if (core == "powershell.exe")
            {
                CommandText.ItemsSource = new List<string>
                {
                    "help",
                    "cd",
                    "Get-help",
                    "Get-Process",
                    "Get-Service",
                    "Get-Command",
                    "Set-Location",
                    "Get-ChildItem",
                    "Copy-Item",
                    "Move-Item",
                    "Remove-Item",
                    "New-Item",
                    "Get-Content",
                    "Set-Content",
                    "Add-Content",
                    "Clear-Host",
                    "Start-Process",
                    "Stop-Process",
                    "Get-History",
                    "Invoke-History",
                    "Import-Module",
                    "Export-ModuleMember"
                };
            }
            if (core == "cmd.exe")
            {
                CommandText.ItemsSource = new List<string>
                {
                    "help",
                    "assoc",
                    "attrib",
                    "break",
                    "cacls",
                    "call",
                    "cd",
                    "chcp",
                    "chdir",
                    "chkdsk",
                    "cls",
                    "cmd",
                    "color",
                    "comp",
                    "compact",
                    "convert",
                    "copy",
                    "date",
                    "del",
                    "dir",
                    "diskpart",
                    "doskey",
                    "driverquery",
                    "echo",
                    "endlocal",
                    "erase",
                    "exit",
                    "fc",
                    "find",
                    "findstr",
                    "for",
                    "format",
                    "fsutil",
                    "ftype"
                };
            }
            if (!File.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "User", "NOEASTER.txt")))
            {
                string egg_str = GetIni("EasterEgg", "NotSupportLinux", "Yes");
                if (egg_str != "Yes")
                {
                    MessageBox.Show("Hei! Bro. Your are joking,isn't right?");
                    File.Create(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "User", "NOEASTER.txt"));
                }
            }

            TerminalWindow.Width = SystemParameters.WorkArea.Width;
            LoadWindow loadWindow = new LoadWindow();
            loadWindow.ShowDialog();
            this.PreviewKeyDown += (s, e) =>
            {
                if ((e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl) && e.Key == Key.C)
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                    }
                    if (TerminalCommand != null && !TerminalCommand.HasExited)
                    {
                        TerminalCommand.Kill();
                    }

                    e.Handled = true;
                }
            };
            this.PreviewKeyDown += async (s, e) =>
            {
                if(e.Key == Key.Enter)
                {
                    if (CodeEditorGrid.Visibility == Visibility.Visible)
                    {
                        return;
                    }
                    if(string.IsNullOrEmpty(CommandText.Text))
                    {
                        return;
                    }
                    RunCommandButton.IsEnabled = false;
                    await Task.Run(() => TerminalCommand_Run());
                    RunCommandButton.IsEnabled = true;
                    if (UpsateTextTimer != null) { UpsateTextTimer.Stop(); UpsateTextTimer.Dispose(); }
                    if ((TerminalCommand != null || TerminalCommand.HasExited) && !TerminalIsRuningTheCommand)
                    {
                        if (TerminalIsRuningTheCommand != true)
                        {
                            return;
                        }
                        TerminalCommand.Kill();
                        TerminalCommand.Dispose();
                    }
                }
            };
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private Process process = new Process();

        private async void TerminalHttpServer_Python(string Mode, string Directory, int Port, string InternetProtocol)
        {

            if (Mode == "FileServer")
            {
                string HttpURL = $"http://{InternetProtocol}:{Port}/";
            }
            if (Mode == "DirectoryServer")
            {
                string HttpURL = $"http://{InternetProtocol}:{Port}/{Directory}/";
            }
            if (Mode == "WebServer")
            {
                string HttpURL = $"http://{InternetProtocol}:{Port}/index.html";
            }
            ProcessStartInfo PythonServer = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"-m http.server {Port} -d \"./{Directory}\" --bind={InternetProtocol}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            process.StartInfo = PythonServer;
            process.Start();
            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Dispatcher.Invoke(() =>
                    {

                    });
                }
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Dispatcher.Invoke(() =>
                    {

                    });
                }
            };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await Task.Run(() => process.WaitForExit());
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }
        }

        private void EventWindowDownAndUpButton_Click(object sender, RoutedEventArgs e)
        {
            EventWindowPositionUpdate();
        }
        private int WindowPositionEventCount = 0;//切换状态
        private async void EventWindowPositionUpdate()
        {
            int getEventWindowDownAndUpButtonHight = (int)EventWindowDownAndUpButton.Height;
            int getWindowTop = (int)TerminalWindow.Height;
            if (WindowPositionEventCount == 0)
            {
                EventWindowDownAndUpButton.IsEnabled = false;
                if (TerminalWindow.Top > 0 - getWindowTop)//软纠正
                {
                    int getTop = (int)TerminalWindow.Top;
                    for (int i = 0; i <= getEventWindowDownAndUpButtonHight; i++)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TerminalWindow.Top = getTop - i;
                        });
                        await Task.Delay(1);
                    }
                    if (TerminalWindow.Top != 0 - getWindowTop)//硬纠正
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TerminalWindow.Top = 0 - getWindowTop;
                        });
                    }
                    for (int i = 0; i <= getWindowTop; i+=10)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TerminalWindow.Top = 0 - getWindowTop + i;
                        });
                        await Task.Delay(1);
                    }
                    EventWindowDownAndUpButton.Content = "▲";
                    WindowPositionEventCount = 1;
                }
                EventWindowDownAndUpButton.IsEnabled=true;
            }
            else if (WindowPositionEventCount == 1)
            {
                EventWindowDownAndUpButton.IsEnabled = false;
                TerminalWindow.Top = 0;
                for (int i = 0; i <= getWindowTop; i+=10)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TerminalWindow.Top = 0 - i;
                    });
                    await Task.Delay(1);
                }
                EventWindowDownAndUpButton.Content = "▼";
                WindowPositionEventCount = 0;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TerminalWindow.Top = 0 - getWindowTop + getEventWindowDownAndUpButtonHight-1;
                });
                EventWindowDownAndUpButton.IsEnabled = true;
            }            
        }
        private bool TerminalIsRuningTheCommand;
        private async void RunCommandButton_Click(object sender, RoutedEventArgs e)
        {
            RunCommandButton.IsEnabled = false;
            await Task.Run(() => TerminalCommand_Run());
            RunCommandButton.IsEnabled = true;
            if(UpsateTextTimer != null) { UpsateTextTimer.Stop(); UpsateTextTimer.Dispose(); }
            if ((TerminalCommand != null || TerminalCommand.HasExited) && !TerminalIsRuningTheCommand)
            {
                if(TerminalIsRuningTheCommand != true) {
                
                    return; }
                TerminalCommand.Kill();
                TerminalCommand.Dispose();
            }
        }

        private void CodeEditorWindowButton_Click(object sender, RoutedEventArgs e)
        {
            CodeEditorWindow codeEditorWindow = new CodeEditorWindow();
            codeEditorWindow.CodeTextGet = CodeEditorTextBox.Text ?? String.Empty;
            codeEditorWindow.Show();
            CodeEditorTextBox.Text =string.Empty;
            CodeEditorGrid.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CodeEditorCancelButton_Click(object sender, RoutedEventArgs e)
        {
            CodeEditorGrid.Visibility = Visibility.Hidden;
            CodeEditorTextBox.Text = string.Empty;
        }

        private void CodeEditorSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string FileType = String.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All File|*.*";
            saveFileDialog.Title = $"Save {FileType} File";
            if (saveFileDialog.ShowDialog() == true)
            {
                // 获取用户选择的文件路径
                string filePath = saveFileDialog.FileName;

                // 将数据保存到文件
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(CodeEditorTextBox.Text);
                }
            }
        }
    }
}
