using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System.Diagnostics;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;
using System.Text.RegularExpressions;
using OpenQA.Selenium.DevTools.V96.Network;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Microsoft.Win32;
using WebDriverManager.Helpers;

namespace MöbiusErgebnisDownload
{
    public partial class MobiusDownladExams : Form
    {
        public int i_sortname, i_mtknr;
        public bool gesamtlisteVorhanden = false, driverOpen = false;
        public string[] lines;
        public IDictionary<string, string> NameClassDict;
        public string reportName = "null";
        public string DriverPath; 
        public WebDriver driver;

        public MobiusDownladExams()
        {
            InitializeComponent();
        }

        private string GetDefaultBrowser()
        {
            const string userChoice = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            string progId;

            RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoice);

            if (userChoiceKey.GetValue("ProgID") == null)
            {
                return null;
            }

            progId = userChoiceKey.GetValue("ProgID").ToString().ToLower();

            switch (progId)
            {
                case var s when progId.Contains("chromehtml"):
                    return "chrome";
                //case var s when progId.Contains("firefox"):
                //    return "firefox";
                default:
                    return null;
            }  
        }

        private void SetupDrivers()
        {
            string browserChoice = GetDefaultBrowser();
            var downloadDirectory = oFD_Teilnehmer.SelectedPath;

            tB_Teilnehmer.Text = "環境設定中...";
            tB_Teilnehmer.Refresh();

            //var oldval = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);

            switch (browserChoice)
            {
                case "chrome":
                    var cconfig = new ChromeConfig();


                    //Thread.Sleep(10000);
                    //new DriverManager().SetUpDriver("www.google.com", )

                    DriverPath = $"{Directory.GetCurrentDirectory()}\\{cconfig.GetName()}\\{cconfig.GetMatchingBrowserVersion()}\\{ArchitectureHelper.GetArchitecture()}\\chromedriver.exe";
                    new DriverManager().SetUpDriver(cconfig, VersionResolveStrategy.MatchingBrowser);
                    
                    //if (File.Exists(FileHelper.GetZipDestination(UrlHelper.BuildUrl((ArchitectureHelper.GetArchitecture().Equals(WebDriverManager.Helpers.Architecture.X32) ? cconfig.GetUrl32 : cconfig.GetUrl64 ), ))))


                    var isExists = File.Exists(DriverPath);
                    if (!isExists){
                        tB_Teilnehmer.Text = "初期化失敗、もう一度やり直してください。";
                        tB_Teilnehmer.Refresh();
                        return; 
                        //Thread.Sleep(10000);
                    }

                    var chromeOptions = new ChromeOptions();

                    chromeOptions.AddArguments("--browser.download.folderList=2");
                    chromeOptions.AddArguments("--browser.helperApps.neverAsk.saveToDisk=text/csv");
                    chromeOptions.AddArguments("--browser.download.dir=" + downloadDirectory);
                    chromeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                    chromeOptions.AddUserProfilePreference("download.prompt_for_download", "false");
                    chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);
                    chromeOptions.AddUserProfilePreference("plugins.plugins_disabled", new[] { "Chrome PDF Viewer" });

                    tB_Teilnehmer.Text = "設定完了。ブラウザー起動中...";
                    tB_Teilnehmer.Refresh();

                    //Thread.Sleep(1000);

                    driver = new ChromeDriver(chromeOptions);

                    break;

                //case "firefox":
                //    new DriverManager().SetUpDriver(new FirefoxConfig());

                //    var firefoxOptions = new FirefoxOptions();

                //    firefoxOptions.AddArguments("--browser.download.folderList=2");
                //    firefoxOptions.AddArguments("--browser.helperApps.neverAsk.saveToDisk=text/csv");
                //    firefoxOptions.AddArguments("browser.download.dir", downloadDirectory);
                //    firefoxOptions.SetPreference("download.prompt_for_download", "false");
                //    driver = new FirefoxDriver(firefoxOptions);

                //    break;

                default:
                    var econfig = new EdgeConfig();
                    new DriverManager().SetUpDriver(econfig, VersionResolveStrategy.MatchingBrowser );
                    //new DriverManager().SetUpDriver("www.google.com", @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe");

                    //DriverPath = $"{Directory.GetCurrentDirectory()}\\{econfig.GetName()}\\{econfig.GetMatchingBrowserVersion()}\\{ArchitectureHelper.GetArchitecture()}";
                    //tB_Teilnehmer.Text = DriverPath;
                    //tB_Teilnehmer.Refresh();

                    //Environment.SetEnvironmentVariable("PATH", $"{oldval}; {DriverPath}", EnvironmentVariableTarget.Machine);

                    var edgeOptions = new EdgeOptions();

                    edgeOptions.AddArguments("--browser.download.folderList=2");
                    edgeOptions.AddArguments("--browser.helperApps.neverAsk.saveToDisk=text/csv");
                    edgeOptions.AddArguments("--browser.download.dir=" + downloadDirectory);
                    edgeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                    edgeOptions.AddUserProfilePreference("download.prompt_for_download", "false");
                    edgeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);

                    tB_Teilnehmer.Text = "設定完了。ブラウザー起動中...";
                    tB_Teilnehmer.Refresh();

                    //Thread.Sleep(1000);

                    driver = new EdgeDriver(edgeOptions);

                    break;
            }

            return;
        }



        private void B_StartWebBrowser_Click(object sender, EventArgs e)
        {
            SetupDrivers();

            //driver = new ChromeDriver();

            driver.Manage().Window.Position = new System.Drawing.Point(50, 50);
            driver.Manage().Window.Size = new System.Drawing.Size(1080*4/3, 1 * 1080);
            //driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            driver.Navigate().GoToUrl("https://hc-nozomi.mobius.cloud/login");

            // Zu Testzwecken in folgenden Zeilen Kommentarzeichen entfernen und auf aktuelle Gegebenheiten anpassen

            //driver.Navigate().GoToUrl("https://exam-h-da.mobius.cloud/rest/saml/init-login");
            //IntPtr handle = GetForegroundWindow();
            ////System.Threading.Thread.Sleep(1000);
            //var user_login = driver.FindElement(By.Id("username"));
            //var user_pass = driver.FindElement(By.Id("password"));
            //user_login.SendKeys("");
            //user_pass.SendKeys("");
            //driver.FindElement(By.Name("_eventId_proceed")).Click();
            //driver.Navigate().GoToUrl("https://exam-h-da.mobius.cloud/7/gradebook");


            //if (!driver.FindElement(By.Id("showUid")).Selected)
            //{
            //    driver.FindElement(By.Id("showUid")).Click();
            //}
            //if (driver.FindElement(By.Id("showLastName")).Selected)
            //{
            //    driver.FindElement(By.Id("showLastName")).Click();
            //}
            //if (driver.FindElement(By.Id("showGivenName")).Selected)
            //{
            //    driver.FindElement(By.Id("showGivenName")).Click();
            //}
            //driver.FindElement(By.Name("numberOfRows")).Clear();
            //driver.FindElement(By.Name("numberOfRows")).SendKeys("20000");
            //IWebElement select = driver.FindElement(By.Id("assignmentSelection"));
            //select.SendKeys("Modulprüfung Strömungsmechanik (2021 SS) - Hausaufgabe/Quiz");
            //driver.FindElement(By.LinkText("Suche")).Click();
            if (gesamtlisteVorhanden && b_StartWebBrowser.Enabled)
            {
                b_Download.Enabled = true;
                driverOpen = true;
            }
        }

        private void MobiusDownladExams_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (driverOpen) {

                //var val = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
                //val = val.Replace(DriverPath, "");
                //Environment.SetEnvironmentVariable("PATH", val, EnvironmentVariableTarget.Machine);
                try {
                    driver.Close();
                    driver.Dispose();
                }
                catch
                {
                    return;
                }
            }

            return;
        }

        // Below block not used anymore: Previous rendition of the download click
        private void b_Download_Click(object sender, EventArgs e)
        {
            IList<IWebElement> elements = driver.FindElement(By.CssSelector("table[class*='gradientTable']")).FindElements(By.CssSelector("tr[class*='dataRow']"));
            List<string> userIds = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.5);
            if (!gesamtlisteVorhanden)
            {
                fBD_Downloads.Description = "Download-Verzeichnis wählen";
                if (fBD_Downloads.ShowDialog() == DialogResult.OK)
                {
                    tB_Teilnehmer.Text = fBD_Downloads.SelectedPath;
                    tB_Teilnehmer.Refresh();
                }
            }

            var i = 0;
            int counter = 0;
            foreach (IWebElement element in elements)
            {
                try
                {
                    counter++;
                    tB_Status.Text = "Bearbeite  Datensatz " + counter + " von " + userIds.Count;
                    tB_Status.Refresh();
                    // string text = "https:/evaluation.mobius.cloud/gradebook/Details.do?" +
                    element.FindElement(By.CssSelector("a[class='grade']")).Click();
                    //.GetAttribute("href").Split('\'')[3];
                    // userIds.Add(element.FindElement(By.CssSelector("td[class*='top userInfo']")).Text + ";" + text);
                    Download("dummy");


                    Thread.Sleep(500);
                    Debug.WriteLine("found ", userIds[i]);
                }
                catch {
                    Debug.WriteLine("Element has no grade.");
                }

            }


            //foreach (string id in userIds)
            //{

            //}

        }
        //



        private void Download_Click(object sender, EventArgs e)
        {
            int n =  driver.FindElement(By.CssSelector("table[class*='gradientTable']")).FindElements(By.CssSelector("tr[class*='dataRow']")).Count();
            IDictionary<string, string> usedKeys = new Dictionary<string, string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            reportName = driver.FindElement(By.CssSelector("table[class*='gradientTable']")).FindElement(By.CssSelector("a[class*='asgnHeader']")).GetAttribute("title");

            int counter = 0;

            while (n > 0) {
                n--;
                IList<IWebElement> elements = driver.FindElement(By.CssSelector("table[class*='gradientTable']")).FindElements(By.CssSelector("tr[class*='dataRow']"));

                foreach (IWebElement element in elements)
                {
                    string name = element.FindElement(By.CssSelector("a[class*='block userLink']")).Text;
                    // GetAttribute("href").Split('\'')[3]
                    string className = element.FindElement(By.CssSelector("span[class*='className']")).Text;
                    if (usedKeys.ContainsKey(name)) { continue; }
                    usedKeys.Add(name, className);

                    try
                    {
                        counter++;
                        tB_Status.Text = "ダウンロード中 - " + counter + "/" + elements.Count();
                        tB_Status.Refresh();
                        // string text = "https:/evaluation.mobius.cloud/gradebook/Details.do?" +
                        IWebElement aLink = element.FindElement(By.CssSelector("a[class='grade']"));

                        ClickCycle(aLink);
                        //.GetAttribute("href").Split('\'')[3];
                        // userIds.Add(element.FindElement(By.CssSelector("td[class*='top userInfo']")).Text + ";" + text);

                        driver.FindElement(By.CssSelector("#gradeContent"));
                        Download("dummy");

                        Thread.Sleep(20);
                        Debug.WriteLine("Back");
                        driver.Navigate().Back();
                        break;
                    }
                    catch {
                        n--;

                        Debug.WriteLine("Element has no grade.");
                    }
                }
            }
            tB_Status.Text = "ダウンロード完了。";
            tB_Status.Refresh();

            NameClassDict = usedKeys;
        }

        private void ClickCycle(IWebElement element)
        {
            try
            {
                //Thread.Sleep(100);
                element.Click();
                return;
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (ElementClickInterceptedException e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                IJavaScriptExecutor jsDriver = driver as IJavaScriptExecutor;
                Debug.WriteLine(e.Message);
                jsDriver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                ClickCycle(element);
            }
            catch
            {
                tB_Status.Text = "実行が失敗しました。もう一回やり直してください。";
                tB_Status.Refresh();
                return;

                // throw new ArgumentException("Couldn't click on desired element error. Error at ClickCycle.", nameof(element));
            }
        }

        private void Download(string gradeId)
        {
            // driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

            // string userId = gradeId.Split(';')[0];
            // string gradeUrl = gradeId.Split(';')[1];
            // Thread.Sleep(500);
            // driver.Navigate().GoToUrl(gradeUrl);
            try {
                driver.ExecuteScript("refresh('csv')");
                Debug.WriteLine("downloaded");
            }
            catch{
                Debug.WriteLine("Can't run(?)");
            }
            //driver.ExecuteScript("window.history.go(-2)");

        }

        private void ValidateFileType(object sender, EventArgs e)
        {
            var downloadDirectory = oFD_Teilnehmer.SelectedPath;
            List<string> csvList = new List<string> ();

            try
            {
                csvList.AddRange(Directory.GetFiles(downloadDirectory, "*.csv"));
//                List<string> nonCSVS = new Listy<string>();
//                foreach file in csvList{
//                    if Path.GetExtension(file) != ".csv" { nonCSVS.Add(file); }
//                }
//                nonCSVS.ForEach(x => csvList.Remove(x))
            }
            catch
            {
                Debug.WriteLine("error at ValidateFileType.");
            }

            Debug.WriteLine("Executing file writing.");
            MergeCSV(csvList);
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

        private void MergeCSV(List<string> csvList)
        {
            var downloadDirectory = oFD_Teilnehmer.SelectedPath;
            var resPath = downloadDirectory + @"\result.csv";
            Debug.WriteLine(resPath);

            if (csvList.Count == 0)
            {
                MessageBox.Show("フォルダーにCSVファイルがありません。ファイルパス変更し, やり直してください。");
                return;
            }

            if (!File.Exists(resPath))
            {
                string firstLine = File.ReadLines(csvList[0], Encoding.UTF8).First();
                Debug.WriteLine(firstLine);

                File.WriteAllText(resPath, " ");
                tB_Status.Text = "result.csv にマージする ...";
                tB_Status.Refresh();

                StreamWriter result = new StreamWriter(resPath, false, Encoding.UTF8);
                result.WriteLine(String.Format("レポートID, クラスID, {0}", firstLine.Substring(3)));
                foreach (string item in csvList){
                    Debug.WriteLine(item);
                    lines = File.ReadAllLines(item);

                    lines = lines.Skip(1).ToArray();

                    string className = "null";
                    string studentID = lines[0].Substring(3).Split(',')[0];
                    if (lines.Length > 0 && NameClassDict.ContainsKey(studentID))
                    {
                        className = NameClassDict[lines[0].Split(',')[1]];
                    }

                    foreach (string line in lines){
                        result.WriteLine(String.Format("{0}, {1}, {2}", reportName, className, line.Substring(3)));
                    }
                }
                result.Close();
                tB_Status.Text = "マージ完成。";
                tB_Status.Refresh();
            }
            else {
                Debug.WriteLine("Deleting File.");
                tB_Status.Text = "result.csv もう存在します。例のファイルを削除しています ...";
                tB_Status.Refresh();
                Task t = Task.Run( () => {

                    File.Delete(resPath);

                    });

                t.Wait();
                MergeCSV(Directory.GetFiles(oFD_Teilnehmer.SelectedPath, "*.csv").ToList());

            }
        }



        private void B_Users_Click(object sender, EventArgs e)
        {
            if (oFD_Teilnehmer.ShowDialog() == DialogResult.OK)
            {
                tB_Teilnehmer.Text = oFD_Teilnehmer.SelectedPath;
                gesamtlisteVorhanden = true;
                if (gesamtlisteVorhanden)
                {
                    b_StartWebBrowser.Enabled = true;
                    Merge.Enabled = true;
                    //this.TopMost = true; ;
                }

            }

        }
    }
}
