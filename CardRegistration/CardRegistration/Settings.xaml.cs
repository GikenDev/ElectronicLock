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

        private void button_status()
        {
            if (_numberStatus0 && _numberStatus2 && _numberStatus3 && _numberStatus4)
            {
                button1.IsEnabled = true;
            }
            else
            {
                button1.IsEnabled = false;
            }
        }

        private void textBoxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        private void TextBox_TextChanged1(object sender, TextChangedEventArgs e)
        {
            int strLength = textBox0.Text.Length;
            if (strLength < 4 && strLength > 0)
            {
                _numberStatus0 = true;
            }
            else
            {
                _numberStatus0 = false;
            }

            button_status();
        }

        private void TextBox_TextChanged2(object sender, TextChangedEventArgs e)
        {
            int strLength = textBox2.Text.Length;
            if (strLength < 4 && strLength > 0)
            {
                _numberStatus2 = true;
            }
            else
            {
                _numberStatus2 = false;
            }

            button_status();
        }

        private void TextBox_TextChanged3(object sender, TextChangedEventArgs e)
        {
            int strLength = textBox3.Text.Length;
            if (strLength < 4 && strLength > 0)
            {
                _numberStatus3 = true;
            }
            else
            {
                _numberStatus3 = false;
            }

            button_status();
        }

        private void TextBox_TextChanged4(object sender, TextChangedEventArgs e)
        {
            int strLength = textBox4.Text.Length;
            if (strLength < 4 && strLength > 0)
            {
                _numberStatus4 = true;
            }
            else
            {
                _numberStatus4 = false;
            }

            button_status();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> UsrDetails = new Dictionary<string, string>();
            UsrDetails.Add("IpAddress", textBox0.Text + "." + textBox2.Text + "." + textBox3.Text + "." + textBox4.Text);
            this.SettingParm = UsrDetails;

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
