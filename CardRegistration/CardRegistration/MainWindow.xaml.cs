using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardRegistration
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<SampleData> SampleDataCollection;
        public MainWindow()
        {
            InitializeComponent();
            SampleDataCollection = new ObservableCollection<SampleData>();
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

        }
    }
}