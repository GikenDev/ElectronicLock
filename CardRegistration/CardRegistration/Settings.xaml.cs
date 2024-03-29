﻿using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardRegistration
{
    public partial class Settings : Window
    {
        private bool _numberStatus0 = false;
        private bool _numberStatus2 = false;
        private bool _numberStatus3 = false;
        private bool _numberStatus4 = false;
        private string _ipaddress;
        private string _login_pass;
        private string _login_name;
        private Dictionary<string, string> _SettingParam;
        public Settings()
        {
            InitializeComponent();
        }

        private void Window2_Load(object sender, EventArgs e)
        {
            if (ipParam != null)
            {
                string[] num = ipParam.Split('.');
                string num0 = num[0];
                string num2 = num[1];
                string num3 = num[2];
                string num4 = num[3];
                textBox0.Text = num0;
                textBox2.Text = num2;
                textBox3.Text = num3;
                textBox4.Text = num4;
            }
            if(LoginName != null)
            {
                login_name.Text = LoginName;
            }
        }

        public string ipParam
        {
            get
            {
                return _ipaddress;
            }
            set
            {
                _ipaddress = value;
            }
        }

        public string LoginName
        {
            get
            {
                return _login_name;
            }
            set
            {
                _login_name = value;
            }
        }

        public string LoginPass
        {
            get
            {
                return _login_pass;
            }
            set
            {
                _login_pass = value;
            }
        }

        public Dictionary<string, string> SettingParm
        {
            get
            {
                return _SettingParam;
            }
            set
            {
                _SettingParam = value;
            }
        }

        private void textBoxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        private void Button_apply(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> UsrDetails = new Dictionary<string, string>();
            UsrDetails.Add("IpAddress", textBox0.Text + "." + textBox2.Text + "." + textBox3.Text + "." + textBox4.Text);
            UsrDetails.Add("LoginName", login_name.Text);
            if(login_pass.Text != "")
            {
                UsrDetails.Add("LoginPass", login_pass.Text);
            }
            this.SettingParm = UsrDetails;

            this.Close();
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
