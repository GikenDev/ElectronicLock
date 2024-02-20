using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRegistration
{
    public class SampleData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SampleData(string index, string number, string name, string uid)
        {
            Index = index;
            StudentNumber = number;
            StudentName = name;
            Uid = uid;
            Enabled = true;
            IsChecked = false;
        }
        private string _Index;
        public string Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("index"));
            }
        }
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public string Uid { get; set; }
        public bool Enabled { get; set; }
        public bool IsChecked { get; set; }
    }
}
