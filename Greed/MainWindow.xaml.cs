using Greed.Models;
using Greed.UserControls;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Greed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       readonly string currDir = Directory.GetCurrentDirectory();
       readonly string modFolder = Path.Combine(Directory.GetCurrentDirectory(),"SPT", "user", "mods", "[SVM] Server Value Modifier");
       readonly string presetsFolder = Path.Combine(Directory.GetCurrentDirectory(),"SPT", "user", "mods", "[SVM] Server Value Modifier", "Presets");
       readonly string loaderFolder = Path.Combine(Directory.GetCurrentDirectory(), "SPT", "user", "mods", "[SVM] Server Value Modifier", "Loader");
        public MainWindow()
        {
            InitializeComponent();

                    EventManager.RegisterClassHandler(typeof(FrameworkElement),
            FrameworkElement.MouseEnterEvent,
            new RoutedEventHandler(OnMouseEnter));
            EventManager.RegisterClassHandler(typeof(FrameworkElement),
                FrameworkElement.MouseLeaveEvent,
                new RoutedEventHandler(OnMouseLeave));
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject),
                new FrameworkPropertyMetadata(int.MaxValue));
            LangSwitch(Thread.CurrentThread.CurrentCulture.Name);
            if (!File.Exists(Path.Combine(modFolder, "ServerValueModifier.dll")))
            {
                Popup message = new((string)Application.Current.FindResource("SVMWrongInstallation"));
                message.ShowDialog();
                if (!message.Confirm)
                {
                    Close();
                }
            }
            else//If there is proper folder structure - this will yield in a exception, we can handle it here so app at least would be browsable
            {
                ToList();
            }
            MainClass.MainConfig mainConfig = new();
            DataContext = mainConfig;
        }

        private void LangSwitchClick(object sender, RoutedEventArgs e)
        {
            LangSwitch(((MenuItem)sender).Tag.ToString());
        }

        private static void LoadRTF(RichTextBox RT, Stream Stream)
        {
            RT.Document.Blocks.Clear();
            RT.Selection.Load(Stream, DataFormats.Rtf);
            RT.Selection.Select(RT.Selection.Start, RT.Selection.Start);
        }
        private void ToList()
        {
            
            if (!Directory.Exists(presetsFolder))
            {
                throw new NullReferenceException($"{presetsFolder} does not exist.");
            }

            object tempfield = Presets.SelectedItem;
            Presets.Items.Clear();
            DirectoryInfo dir = new(presetsFolder);
            List<FileInfo> fileList = [.. dir.GetFiles("*.*", SearchOption.AllDirectories)
                .Where(x => x.Extension == ".json")
                .OrderBy(x => x.Name)];

            if (fileList.Count > 0)
            {
                foreach (FileInfo fileInfo in fileList)
                {
                    Presets.Items.Add(fileInfo.Name.Replace(".json", ""));
                }
            }

            Presets.SelectedItem = tempfield;
        }

        public void LangSwitch(string lang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            //Application.Current.Resources.MergedDictionaries.Clear();
            switch (lang)
            {
                case "ru-RU":
                case "ru-UA":
                    LoadLanguage($"Dictionary-ru-RU.xaml", "Russian", "HelloRU.rtf", "faqRU.rtf");
                    break;
                case "ko-KR":
                    LoadLanguage($"Dictionary-ko-KR.xaml", "Korean", "HelloKR.rtf", "faqKR.rtf");
                    break;
                case "uk-UA":
                    LoadLanguage($"Dictionary-uk-UA.xaml", "Ukrainian", "HelloUA.rtf", "faqUA.rtf"); ;
                    break;
                case "zh-CHS":
                case "zh-CN":
                    LoadLanguage($"Dictionary-zh-CN.xaml", "Chinese", "HelloCN.rtf", "faqCN.rtf");
                    break;
                case "es-ES":
                    LoadLanguage($"Dictionary-es-ES.xaml", "Spanish", "HelloES.rtf", "faqES.rtf");
                    break;
                default:
                    LoadDefaultLanguage($"Languages/English/Dictionary-en-US.xaml", "Greed.Languages.English.HelloEN.rtf", "Greed.Languages.English.faqEN.rtf");
                    break;
            }
        }
        private void LoadLanguage(string dict, string lang, string hello, string faq)
        {
            FileStream dictfs = new(Path.Combine(modFolder, "Languages", lang, dict), FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream hellofs = new(Path.Combine(modFolder, "Languages", lang, hello), FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream faqfs = new(Path.Combine(modFolder, "Languages", lang, faq), FileMode.Open, FileAccess.Read, FileShare.Read);//this is lame
            ResourceDictionary resdict = (ResourceDictionary)XamlReader.Load(dictfs);
            Application.Current.Resources.MergedDictionaries.Add(resdict);
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            LoadRTF(WelcomeMessage, hellofs);
            LoadRTF(RTF1, faqfs);
        }

        private void LoadDefaultLanguage(string dict, string hello, string faq)
        {
            ResourceDictionary resdict = new()
            {
                Source = new Uri(dict, UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(resdict);
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            LoadRTF(WelcomeMessage, Assembly.GetExecutingAssembly().GetManifestResourceStream(hello));
            LoadRTF(RTF1, Assembly.GetExecutingAssembly().GetManifestResourceStream(faq));
        }

        private void SavePreset(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Presets.Text == "null" || Presets.Text == "Null")
                {
                    Popup Message = new((string)Application.Current.FindResource("PresetNamedNullError"));
                    Message.ShowDialog();
                }
                else
                {
                    if (CheckName())
                    {
                        string rawJSON = JsonSerializer.Serialize((MainClass.MainConfig)DataContext, new JsonSerializerOptions() {Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,  WriteIndented = true });
                        string filepath = Path.Combine( presetsFolder, string.IsNullOrEmpty(Presets.Text) ? "Noname.json" : Presets.Text + ".json");
                        File.WriteAllText(filepath, rawJSON, Encoding.UTF8);
                        ToList();
                        if (Presets.Text == "")
                        {
                            Presets.Text = "Noname";
                        }
                        _ = SaveTextAsync();
                    }
                }
            }
            catch
            {
                Popup Message = new((string)Application.Current.FindResource("ApplyFailedUnknown")+e);
                Message.ShowDialog();
            }
        }

        private bool CheckName()
        {
            if (File.Exists(Path.Combine( presetsFolder, Presets.Text + ".json")))
            {
                Popup Message = new((string)Application.Current.FindResource("OverridePreset"));
                Message.ShowDialog();
                return Message.Confirm;
            }
            else if (!PresetNameRegex().IsMatch(Presets.Text))
            {
                return true;
            }
            else
            {
                Popup Message = new((string)Application.Current.FindResource("PresetNameFail"));
                Message.ShowDialog();
                return false;
            }
        }
        private void LoadPreset(object sender, RoutedEventArgs e)
        {
            if (Presets.Text == "")
            {
                if (File.Exists(Path.Combine(presetsFolder,"Noname.json")))
                {
                    Presets.Text = "Noname";
                    Popup Message = new((string)Application.Current.FindResource("NonameLoaded"));
                    Message.ShowDialog();
                    LoadFunc();
                    _ = LoadTextAsync();
                }
                else
                {
                    Popup Message = new((string)Application.Current.FindResource("NothingToLoad"));
                    Message.ShowDialog();
                }
            }
            else
            {
                LoadFunc();
                _ = LoadTextAsync();
            }
        }

        private void LoadJson()
        {
            string rawJSON = File.ReadAllText(Path.Combine(presetsFolder, Presets.Text + ".json"), Encoding.UTF8);
            MainClass.MainConfig loadedConfig = JsonSerializer.Deserialize<MainClass.MainConfig>(rawJSON);
            DataContext = loadedConfig;
        }

        private void LoadFunc()
        {
            try
            {
                LoadJson();//Horrible solution - the issue is: Converters and MVVM maximum/minimums bugging out the parsed values, therefore to properly apply them - i need to reload DataContext twice.
                LoadJson();//Possible solution was nulling DataContext, however it cause application to hang for 3-5 seconds, unacceptable. Previous solution was loading whole LoadFunc, causing messages to show up twice.
                           //May coding dieties mercy me
            }
            catch (FileNotFoundException)
            {
                Popup Message = new((string)Application.Current.FindResource("FileNotFoundException"));
                Message.ShowDialog();
            }
            catch (JsonException e)
            {
                Popup Message = new((string)Application.Current.FindResource("CorruptedJSON") + "\n" + e.Message);
                Message.ShowDialog();
            }
            catch (AccessViolationException)
            {
                Popup Message = new((string)Application.Current.FindResource("NoAccess"));
                Message.ShowDialog();
            }
            catch
            {
                Popup Message = new((string)Application.Current.FindResource("ApplyFailedUnknown"));
                Message.ShowDialog();
            }
        }

        private void RunEverything(object sender, RoutedEventArgs e)
        {
            try
            {
                Process serverProcess = new()
                {

                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(currDir, "SPT", "SPT.Server.exe"),
                        WorkingDirectory = Path.Combine(currDir, "SPT"),
                    }
                };
                serverProcess.Start();
                Process launcherprocess = new()
                {

                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(currDir, "SPT", "SPT.Launcher.exe"),
                        WorkingDirectory = Path.Combine(currDir, "SPT"),
                    }
                };
                launcherprocess.Start();
            }
            catch (Exception)
            {
                Popup Message = new((string)Application.Current.FindResource("RunFailed"));
                Message.ShowDialog();
            }
        }
        private void CloseEverything(object sender, RoutedEventArgs e)
        {
            string exefolder = Path.Combine(Directory.GetCurrentDirectory(), "SPT");

            Process[] serverProcesses = Process.GetProcessesByName("SPT.Server");
            {
                foreach (Process process in serverProcesses)
                {
                    if (process.MainModule.FileName == Path.Combine(exefolder, "SPT.Server.exe"))
                        process.Kill();
                }
            }
            Process[] launcherProcesses = Process.GetProcessesByName("SPT.launcher");
            {
                foreach (Process process in launcherProcesses)
                {
                    if (process.MainModule.FileName == Path.Combine(exefolder, "SPT.Launcher.exe"))
                        process.Kill();
                }
            }
        }

        private void PresetFolder(object sender, EventArgs e)
        {
            if (Directory.Exists(presetsFolder))
            {

                Process.Start("explorer.exe", presetsFolder);
            }
            else
            {
                Popup Message = new((string)Application.Current.FindResource("PresetFolderDeleted"));
                Message.ShowDialog();
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            //if (RoubleRatio.Value + DollarRatio.Value + EuroRatio.Value != 100)
            //{
            //    Popup Message = new((string)Application.Current.FindResource("RatioError"));
            //    Message.ShowDialog();
            //    return;
            //} TODO: Reimplement back OR do the check on SVM side
            try
            {
                string savepath = Path.Combine(loaderFolder, "loader.json");
                File.Delete(savepath);
                if (Presets.Text == "")
                {
                    if (File.Exists( Path.Combine(presetsFolder, "Noname.json")))
                    {
                        File.AppendAllText(savepath, "{\n" + "\"CurrentlySelectedPreset\": \"Noname\"" + "\n}");
                        Popup Message = new((string)Application.Current.FindResource("NonameApplied"));
                        Message.ShowDialog();
                    }
                    else
                    {
                        Popup Message = new((string)Application.Current.FindResource("NothingToApply"));
                        Message.ShowDialog();
                    }
                }
                else
                {
                    File.AppendAllText(savepath, "{\n" + "\"CurrentlySelectedPreset\":\"" + Presets.Text + "\"\n}");
                    _ = ApplyTextAsync();
                }
            }
            catch
            {
                Popup Message = new((string)Application.Current.FindResource("ApplyFailedUnknown"));
                Message.ShowDialog();
            }
        }

        private async Task ApplyTextAsync()
        {
            Apply.Text = new((string)Application.Current.FindResource("Applied"));
            await Task.Delay(1500);
            Apply.Text = new((string)Application.Current.FindResource("ApplyButton"));
        }
        private async Task LoadTextAsync()
        {
            Load.Text = new((string)Application.Current.FindResource("Loaded"));
            await Task.Delay(1500);
            Load.Text = new((string)Application.Current.FindResource("LoadButton"));
        }
        private async Task SaveTextAsync()
        {
            Save.Text = new((string)Application.Current.FindResource("Saved"));
            await Task.Delay(1500);
            Save.Text = new((string)Application.Current.FindResource("SaveButton"));
        }

        private void RunURL(string Resource, string URL)
        {
            Popup Message = new((string)Application.Current.FindResource(Resource));
            Message.ShowDialog();
            if (Message.Confirm == true)
            {
                string browserPath = ("C:/Windows/explorer.exe");
                string argUrl = "\"" + URL + "\"";
                Process.Start(browserPath, argUrl);
            }
        }

        private void ItemFinder(object sender, EventArgs e)//TODO Route to tarkynator maybe? Although Item Finder is SPT domain, best to leave as is.
        {
            Popup Message = new((string)Application.Current.FindResource("IDFinder"));
            Message.ShowDialog();
            if (Message.Confirm == true)
            {
                string browserPath = ("C:/Windows/explorer.exe");
                string argUrl = "https://db.sp-tarkov.com/search";
                Process.Start(browserPath, argUrl);
            }
        }

        private void SPTDiscord(object sender, EventArgs e)//TODO: Maybe in future rework as one method and do a switch? Not like it matters really.
        {
            RunURL("SPTLink", "https://discord.gg/Xn9msqQZan");
        }

        private void SPTWEB(object sender, EventArgs e)
        {
            RunURL("SPTWeb", "https://hub.sp-tarkov.com");
        }

        private void KofiLink(object sender, EventArgs e)
        {
            RunURL("KofiLink", "https://ko-fi.com/ghostfenixx");
        }

        private void GitHubLink(object sender, EventArgs e)
        {
            RunURL("GitHubLink", "https://github.com/GhostFenixx?tab=repositories");
        }

        private void ModLink(object sender, EventArgs e)
        {
            RunURL("ModPage", "https://hub.sp-tarkov.com/files/file/379-kmc-server-value-modifier");
        }

        private void FikaDiscord(object sender, EventArgs e)
        {
            RunURL("FikaLink", "https://discord.gg/HknsaAjGvX");
        }

        private void WikiPage(object sender, EventArgs e)
        {
            RunURL("WikiPage_Link", "https://escapefromtarkov.fandom.com/wiki/Escape_from_Tarkov_Wiki");
        }


        private void Disclaimer(object sender, RoutedEventArgs e)
        {
            Popup Message = new((string)Application.Current.FindResource("DisclaimerText"));
            Message.ShowDialog();
        }

        private void ThanksTo(object sender, RoutedEventArgs e)
        {
            Popup Message = new((string)Application.Current.FindResource("ThanksToText"));
            Message.ShowDialog();
        }

        private void License(object sender, RoutedEventArgs e)
        {
            Popup Message = new((string)Application.Current.FindResource("LicenseText"));
            Message.ShowDialog();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LoadLast.IsChecked)
            {
                File.WriteAllText("GreedConfig", "true," + Presets.Text, Encoding.UTF8);
            }
        }

        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = TextValidationRegex();
            e.Handled = regex.IsMatch(e.Text);
        }

        private void LoadLast_Click(object sender, RoutedEventArgs e)
        {
            if (LoadLast.IsChecked)
            {
                File.WriteAllText("GreedConfig", "true," + Presets.Text, Encoding.UTF8);
            }
            else
            {
                File.WriteAllText("GreedConfig", "false," + Presets.Text, Encoding.UTF8);
            }
        }
        private void ChangelogOpen(object sender, RoutedEventArgs e)
        {
            Popup Message = new((string)Application.Current.FindResource("ChangelogText"));
            Message.ShowDialog();
        }

        private static void OnMouseEnter(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.ToolTip != null)
                Mouse.OverrideCursor = Cursors.Help;
        }

        private static void OnMouseLeave(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }
        public static void EnableSection(bool? checker, Grid field)
        {
            bool isChecked = checker.Value;

            field.IsEnabled = isChecked;
            field.Opacity = isChecked ? 1 : 0.2;
        }

        public static void EnableSubSection(bool? checker, StackPanel field)
        {
            field.IsEnabled = checker.Value;
        }

        private void Dragger(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void AppMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void AppMaximize(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Maximized:
                    MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                    WindowState = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                    WindowState = WindowState.Maximized;
                    break;
                default:
                    MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        private void AppClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("GreedConfig"))
            {
                string cfg = File.ReadAllText(Path.Combine(currDir, "GreedConfig"), Encoding.UTF8);
                string[] load = cfg.Split(',');
                if (load[0] == "true")
                {
                    LoadLast.IsChecked = true;
                    Presets.Text = load[1];
                    LoadFunc();
                }
            }
        }
        [GeneratedRegex("(?:\\W)")]
        private static partial Regex PresetNameRegex();
        [GeneratedRegex("[^0-9.,]+")]
        public static partial Regex NumberValidationRegex();
        [GeneratedRegex("[`<>^@!?#%*%:&\\]*$]")]
        public static partial Regex TextValidationRegex();
    }
}