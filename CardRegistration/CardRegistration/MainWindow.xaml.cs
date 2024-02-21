using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Windows;

namespace CardRegistration
{
    public partial class MainWindow : Window
    {
        private string FTPIpAdress;
        private string FTPuserName;
        private string FTPpassword;
        private ObservableCollection<SampleData> SampleDataCollection;
        public MainWindow()
        {
            InitializeComponent();
            SampleDataCollection = new ObservableCollection<SampleData>();

            FTPIpAdress = RegistSetting.Default.IpAddress;
            FTPuserName = RegistSetting.Default.UserName;
            FTPpassword = RegistSetting.Default.UserPassword;
            if (FTPIpAdress == null || FTPIpAdress == "" || FTPuserName == null || FTPuserName == "" || FTPpassword == null || FTPpassword == "")
            {
                ContentRendered += (s, e) => { SettingWindow(); };
            }
        }

        private void SettingWindow()
        {
            var settingWindow = new Settings
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            if (FTPIpAdress != null && FTPIpAdress != "")
            {
                settingWindow.ipParam = FTPIpAdress;
            }
            if (FTPuserName != null && FTPuserName != "")
            {
                settingWindow.LoginName = FTPuserName;
            }
            settingWindow.ShowDialog();
            if (settingWindow.SettingParm != null)
            {
                if (settingWindow.SettingParm.ContainsKey("IpAddress"))
                {
                    FTPIpAdress = settingWindow.SettingParm["IpAddress"];
                }
                if (settingWindow.SettingParm.ContainsKey("LoginName"))
                {
                    FTPuserName = settingWindow.SettingParm["LoginName"];
                }
                if (settingWindow.SettingParm.ContainsKey("LoginPass"))
                {
                    FTPpassword = settingWindow.SettingParm["LoginPass"];
                }
            }
        }

        private void add_btn(object sender, RoutedEventArgs e)
        {
            var sub = new Regist
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            sub.ShowDialog();
            if (sub.strParam != null)
            {
                SampleDataCollection.Add(new SampleData(SampleDataCollection.Count.ToString(), sub.strParam["StudentNumber"], sub.strParam["StudentName"], sub.strParam["Uid"]));

                UsrDataGrid.ItemsSource = SampleDataCollection;
                refresh_btn.IsEnabled = true;
                apply_btn.IsEnabled = true;
            }
        }

        private void RefreshBtn(object sender, RoutedEventArgs e)
        {
            refresh_btn.IsEnabled = false;
            apply_btn.IsEnabled = false;
            Refresh();
        }

        private void Refresh()
        {
            List<int> DelList = new List<int>();
            List<int> DisList = new List<int>();
            List<int> EnaList = new List<int>();
            List<string> str_DelList = new List<string>();
            List<string> str_DisList = new List<string>();
            List<string> str_EnaList = new List<string>();
            ObservableCollection<SampleData> list = this.UsrDataGrid.ItemsSource as ObservableCollection<SampleData>;
            if (list != null)
            {
                foreach (SampleData cat in list)
                {
                    //delete
                    if (cat.IsChecked)
                    {
                        DelList.Add(int.Parse(cat.Index));
                        str_DelList.Add(cat.StudentNumber);
                    }
                }

                if (DelList.Count > 0 || DisList.Count > 0 || EnaList.Count > 0)
                {
                    string str_del = "";
                    foreach (string del in str_DelList)
                    {
                        str_del += del + ", ";
                    }
                    var YesOrNo = MessageBox.Show(str_del + "\nを削除します。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

                    switch (YesOrNo)
                    {
                        case MessageBoxResult.Yes:
                            //削除
                            if (DelList.Count > 0)
                            {
                                DelList.Reverse();
                                foreach (int index in DelList)
                                {
                                    SampleDataCollection.Remove(SampleDataCollection[index]);
                                    if (SampleDataCollection.Count != index)//同じ数字のときはlistの最後なのではじく
                                    {
                                        for (int i = index; i < SampleDataCollection.Count; i++)
                                        {
                                            int old_index = int.Parse(SampleDataCollection[i].Index);
                                            string new_index = (old_index - 1).ToString();
                                            SampleDataCollection[i].Index = new_index;
                                        }
                                    }
                                }

                            }
                            break;

                        case MessageBoxResult.No:
                            break;
                    }
                }
                refresh_btn.IsEnabled = true;
                apply_btn.IsEnabled = true;
                if (SampleDataCollection.Count == 0)
                {
                    refresh_btn.IsEnabled = false;
                    apply_btn.IsEnabled = false;
                }
            }
        }

        private void MeiboApply(object sender, RoutedEventArgs e)
        {
            refresh_btn.IsEnabled = false;
            apply_btn.IsEnabled = false;
            Refresh();

            string HeaderLine = "Index,StudentNumber,StudentName,StudentID,Enable\n";
            ObservableCollection<SampleData> list = this.UsrDataGrid.ItemsSource as ObservableCollection<SampleData>;

            if (list != null)
            {
                foreach (SampleData cat in list)
                {
                    HeaderLine += cat.Index + "," + cat.StudentNumber + "," + cat.StudentName + "," + cat.Uid + "," + cat.Enabled + "\n";
                }
                string DesktopDir = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string FileName = "meibo";
                string Extend = ".csv";
                string FilePath = DesktopDir + "\\" + FileName + Extend;
                using (StreamWriter sw = new StreamWriter(@FilePath, false))
                {
                    sw.Write(HeaderLine);
                }
                string ftpAddress = "ftp://" + FTPIpAdress + "/ElectroKeyMeibo.csv";

                WebClient wwc = new WebClient();
                try
                {
                    wwc.Credentials = new NetworkCredential(FTPuserName, FTPpassword);
                    wwc.UploadFile(ftpAddress, FilePath);

                    MessageBox.Show("送信しました。", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("送信に失敗しました。\nログイン名やパスワードを確認してください。", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RegistSetting.Default.IpAddress = FTPIpAdress;
            RegistSetting.Default.UserName = FTPuserName;
            if(FTPpassword != null || FTPpassword != "")
            {
                RegistSetting.Default.UserPassword = FTPpassword;
            }
            RegistSetting.Default.Save();
        }

        private void OpenSetting(object sender, RoutedEventArgs e)
        {
            SettingWindow();
        }
    }
}