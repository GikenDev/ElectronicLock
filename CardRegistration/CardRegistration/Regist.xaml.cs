using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PCSC;
using PCSC.Iso7816;

namespace CardRegistration
{
    public partial class Regist : Window
    {
        private Dictionary<string, string> UsrDetails = new Dictionary<string, string>();
        private bool _numberStatus = false;
        private bool _nameStatus = false;
        private bool _handle = false;
        private bool _isChecked = false;
        private Dictionary<string, string> _strParam;
        public Regist()
        {
            InitializeComponent();
        }

        public Dictionary<string, string> strParam
        {
            get
            {
                return _strParam;
            }
            set
            {
                _strParam = value;
            }
        }

        private void Btn_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private static bool NoReaderFound(ICollection<string> readerNames) =>
           readerNames == null || readerNames.Count < 1;

        private void ScanCard(object sender, RoutedEventArgs e)
        {
            using (var context = ContextFactory.Instance.Establish(SCardScope.System))
            {
                var readerNames = context.GetReaders();
                if (NoReaderFound(readerNames))
                {
                    MessageBox.Show("nfcリーダを接続してください", "No Device", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var rfidReader = context.ConnectReader(readerNames[0], SCardShareMode.Shared, SCardProtocol.Any))
                {
                    var apdu = new CommandApdu(IsoCase.Case2Short, rfidReader.Protocol)
                    {
                        CLA = 0xFF,
                        Instruction = InstructionCode.GetData,
                        P1 = 0x00,
                        P2 = 0x00,
                        Le = 0
                    };

                    using (rfidReader.Transaction(SCardReaderDisposition.Leave))
                    {
                        var sendPci = SCardPCI.GetPci(rfidReader.Protocol);
                        var receivePci = new SCardPCI();

                        var receiveBuffer = new byte[256];
                        var command = apdu.ToArray();

                        var byteReceived = rfidReader.Transmit(
                            sendPci,
                            command,
                            command.Length,
                            receivePci,
                            receiveBuffer,
                            receiveBuffer.Length
                        );

                        var responseApdu = new ResponseApdu(receiveBuffer, byteReceived, IsoCase.Case2Short, rfidReader.Protocol);
                        if (responseApdu.HasData)
                        {
                            var result = BitConverter.ToString(responseApdu.GetData());
                            Regex reg = new Regex("-");
                            string uid = reg.Replace(result, "");

                            ComboBoxItem selectedItem = DepartmentChoices.SelectedItem as ComboBoxItem;
                            string Department = selectedItem.Content.ToString();

                            string StudentNumber = Department + input_studentNum.Text;
                            string StudentName = input_name.Text;
                            UsrDetails.Add("StudentNumber", StudentNumber);
                            UsrDetails.Add("StudentName", StudentName);
                            UsrDetails.Add("Uid", uid);

                            this.strParam = UsrDetails;

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("IDの取得に失敗しました。再度読み取りしてください。", "Failure", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private void button_status()
        {
            if (_numberStatus && _nameStatus && _handle && _isChecked)
            {
                scan_card.IsEnabled = true;
            }
            else
            {
                scan_card.IsEnabled = false;
            }
        }

        private void Department_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _handle = true;
            button_status();
        }

        private void inputnumPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }

        private void inputnum_TextChanged(object sender, TextChangedEventArgs e)
        {
            int strLength = input_studentNum.Text.Length;
            if (strLength == 5)
            {
                _numberStatus = true;
            }
            else
            {
                _numberStatus = false;
            }

            button_status();
        }

        private void Name_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int strLength = input_name.Text.Length;
            if (strLength > 0)
            {
                _nameStatus = true;
            }
            else
            {
                _nameStatus = false;
            }
            button_status();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _isChecked = true;
            button_status();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _isChecked = false;
            button_status();
        }
    }
}
