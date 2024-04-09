using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IpCalculator.core;
using static IpCalculator.core.IpStructure;
using static PE1.Wpf.Helpers.HostHelper;
using static PE1.Wpf.Helpers.WpfHelper;

namespace IpCalculator.Wpf.IPv4
{
    public partial class IPv4CalcWindow : Window
    {
        #region Global variables
        private readonly Host host1 = new("Host1");
        private readonly Host host2 = new("Host2");
        #endregion

        #region Constructor
        public IPv4CalcWindow()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Event Handlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillFirstIpComboBoxes();
            ShowCommunicationColor();
        }
        
        private void IPv4CalcWindow_OnClosed(object sender, EventArgs e)
        {
            if (AreAllWindowsOfTypeClosed<IPv4CalcWindow>()) return;
            StartupWindow startupWindow = Application.Current.Windows.OfType<StartupWindow>().First();
            startupWindow.btnIPv4Calc.Background = Brushes.Coral;
        }
        private void CmbIp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbSender = (ComboBox)sender;
            if (IsNothingSelected(cmbSender)) return;
            //read
            byte selectedByte = (byte)cmbSender.SelectedItem;
            //Process
            string baseNameOfCmb = GetBaseNameOfControl(cmbSender,1);
            Host currentHost = GetCurrentHost(cmbSender);
            OctetNumber currentOctetNumber = GetCurrentOctetNumber(cmbSender);
            int selectedCmbNumber = (int)currentOctetNumber;
            List<byte> nextValues = GetListOfValuesForNextCmb(currentOctetNumber, selectedByte);
            //Write
            currentHost.UpdateIpAddressAndResetIfFirstOctet(currentOctetNumber, selectedByte);
            //Update GUI
            if (selectedCmbNumber == 1)
                ClearComboBoxesExceptCmb1(baseNameOfCmb);
            FillNextComboBoxWithValidIpValues(selectedCmbNumber, nextValues, baseNameOfCmb);
            ShowCommunicationColor();
            try
            {
                FillTxtBoxes(currentHost, this);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void CmbCidr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbSender = (ComboBox)sender;
            if (IsNothingSelected(cmbSender)) return;
            //read
            byte selectedByte = (byte)cmbSender.SelectedItem;
            //Process
            Host currentHost = GetCurrentHost(cmbSender);
            //Write
            currentHost.Cidr = selectedByte;
            //Update GUI
            try
            {
                FillTxtBoxes(currentHost,this);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            ShowCommunicationColor();
        }
        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            //Read and Process
            Host host = (((Button)sender).Name == "btnClearHost1") ? host1 : host2;
            string baseComboBoxName = (host == host1) ? "Host1B" : "Host2B";
            string baseTextBoxName = (host == host1) ? "Host1" : "Host2";
            string baseLblName = (host == host1) ? "Host1" : "Host2";
            //Write
            host.ResetIpAndCidr();
            //Update GUI
            ResetGUI(baseComboBoxName, baseTextBoxName, baseLblName);
            ShowCommunicationColor();
        }
        private void BtnManualWindow_OnClick(object sender, RoutedEventArgs e)
        {
            ManualWindow manualWindow = new ManualWindow();
            manualWindow.Show();
        }
        #endregion Event Handlers

        #region Methods
        /// <summary>
        /// Resets the GUI elements whose basename starts with the given baseNames.
        /// </summary>
        /// <param name="baseComboBoxName">base name of the ComboBoxes, specific suffix will be added</param>
        /// <param name="baseTextBoxName">base name of the TextBoxes, specific suffix will be added</param>
        /// <param name="baseLblName">base name of the Labels, specific suffix will be added</param>
        private void ResetGUI(string baseComboBoxName, string baseTextBoxName, string baseLblName)
        {
            ClearSelectionInComboBox(baseComboBoxName+1);
            for (int i = 2; i < 5; i++)
            {
                ClearCmb(baseComboBoxName+i, this);
            }
            ClearCmb(baseComboBoxName+"Cidr", this);
            ClearTextBoxes(baseTextBoxName, this);
            ClearLabel(baseLblName, this);
        }
        /// <summary>
        /// Sets selectedIndex of combobox with given name to -1, Clearing the selection
        /// </summary>
        /// <param name="comboBoxName">The name of the comboBox</param>
        private void ClearSelectionInComboBox(string comboBoxName)
        {
            ComboBox cmbBox = GetPossibleControlBySuffix<ComboBox>(comboBoxName, this);
            cmbBox.SelectedIndex = -1;
        }
        /// <summary>
        /// Gets the list of possible IP byte values to put into the next ComboBox
        /// </summary>
        /// <param name="octetNumber">an enum denoting the octet we just selected</param>
        /// <param name="selectedByte">The value of the selected byte, to determine the possible range of the next byte</param>
        /// <returns>A list containing the values for the next ComboBox</returns>
        private List<byte> GetListOfValuesForNextCmb(OctetNumber octetNumber, byte selectedByte)
        {
            switch (octetNumber)
            {
                case OctetNumber.Octet1:
                    return IpFirstHalf[selectedByte];
                case OctetNumber.Octet2:
                    return IpThird;
                case OctetNumber.Octet3:
                    return IpLast;
                case OctetNumber.Octet4:
                    return new List<byte>();
                default:
                    return new List<byte>();
            }
        }
        /// <summary>
        /// Gets the number of the current Octet by reading the last digit of the combobox
        /// </summary>
        /// <param name="sender">The combobox that triggered the event</param>
        /// <returns>Enum Octetnumber</returns>
        private OctetNumber GetCurrentOctetNumber(ComboBox sender)
        {
            string lastChar = GetLastCharAsString(sender.Name);
            int.TryParse(lastChar, out int numberOfCmb);
            return (OctetNumber)numberOfCmb;
        }
        /// <summary>
        /// Gets the current Host by reading the number at the provided index.
        /// </summary>
        /// <param name="inputElement">The combobox that triggered the event</param>
        /// <param name="hostDigitIndex">An int containing the index of the digit that identifies the host.
        /// if none is entered the index is 7 to work with Ipcalculator</param>
        /// <returns>Host</returns>
        private Host GetCurrentHost(IFrameworkInputElement inputElement,int hostDigitIndex=7)
        {
            string hostIdentifier = inputElement.Name.Substring(hostDigitIndex, 1);
            return hostIdentifier == "1" ? host1 : host2;
        }
        /// <summary>
        /// Fills the combobox for the next octet with all possible byte values to make a local IpV4
        /// </summary>
        /// <param name="numberOfCurrentCmb">The number of the last filled ComboBox</param>
        /// <param name="newItemSource">The List that we will provide as itemssource for the next cmb</param>
        /// <param name="baseNameOfCmb">The common base name of the ComboBoxes, used to add suffix to get next ComboBox</param>
        private void FillNextComboBoxWithValidIpValues(int numberOfCurrentCmb, IEnumerable<byte> newItemSource, string baseNameOfCmb)
        {
            int numberOfNextCmb = numberOfCurrentCmb + 1;
            if (numberOfNextCmb > 4) return;
            ComboBox nextCmb = GetPossibleControlBySuffix<ComboBox>(baseNameOfCmb + numberOfNextCmb, this);
            nextCmb!.ItemsSource = newItemSource;
        }
        /// <summary>
        /// Clears ComboBoxes 2 through 4 and Cidr combobox
        /// </summary>
        /// <param name="baseName">string containing base common name</param>
        private void ClearComboBoxesExceptCmb1(string baseName)
        {
            ClearCmb(baseName + 2, this);
            ClearCmb(baseName + 3, this);
            ClearCmb(baseName + 4, this);
            ClearCmb(baseName+"Cidr", this);
            FillCidrComboBox(baseName);
        }
        /// <summary>
        /// Fills the first Ip comboboxes for each host with Keyvalues of dictionary in IpStructure static class
        /// </summary>
        private void FillFirstIpComboBoxes()
        {
            cmbHost1B1.ItemsSource = IpFirstHalf.Keys;
            cmbHost2B1.ItemsSource = IpFirstHalf.Keys;
        }
        /// <summary>
        /// Fills the Cidr combobox with the values of the CidrDictionary in IpStructure static class,
        /// based on the value of the first octet
        /// </summary>
        /// <param name="baseControlName">The common inputElement name for each combobox of a host
        /// without the specific suffix</param>
        private void FillCidrComboBox(string baseControlName)
        {
            ComboBox cmbFirst = GetPossibleControlBySuffix<ComboBox>(baseControlName + 1, this);
            ComboBox cmbCidr=GetPossibleControlBySuffix<ComboBox>(baseControlName + "Cidr", this);
            byte selectedByte=(byte)cmbFirst!.SelectedItem;
            cmbCidr!.ItemsSource = CidrDictionary[selectedByte];
        }
        #endregion
        
        #region Color Methods
        /// <summary>
        /// Changes image based on provided enum
        /// </summary>
        /// <param name="hostColor">enum to decide the image color</param>
        public static Uri GetColorUri(HostColors hostColor)
        {
            string path = Environment.CurrentDirectory;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            directoryInfo = new DirectoryInfo(directoryInfo.Parent!.Parent!.Parent!.FullName);
            Uri uri;
            switch (hostColor)
            {
                case HostColors.White:
                    // uri = new Uri(directoryInfo.FullName + "/Images/white.png");
                    uri = new Uri("/Images/white.png",UriKind.Relative);
                    break;
                case HostColors.Red:
                    // uri = new Uri(directoryInfo.FullName + "/Images/red.png");
                    uri = new Uri("/Images/red.png",UriKind.Relative);
                    break;
                case HostColors.Green:
                    // uri = new Uri(directoryInfo.FullName + "/Images/green.png");
                    uri = new Uri("/Images/green.png",UriKind.Relative);
                    break;
                default:
                    // uri = new Uri(directoryInfo.FullName + "/Images/white.png");
                    uri = new Uri("/Images/white.png",UriKind.Relative);
                    break;
            }
            return uri;
        }
        /// <summary>
        /// Gets the Image with the correct color depending on HostColor enum
        /// </summary>
        /// <param name="hostColor">Enum that decides the color</param>
        private void MakeColor(HostColors hostColor)
        {
            imgColor.Source = new BitmapImage(GetColorUri(hostColor));
        }
        /// <summary>
        /// Shows green if Hosts can communicate, red if they cannot and white if both hosts have not been entered yet.
        /// </summary>
        private void ShowCommunicationColor()
        {
            if (!host1.IsValidIp() || !host2.IsValidIp() || !host1.IsValidCidr() || !host2.IsValidCidr() )
                MakeColor(HostColors.White);
            else if (host1.GetNetworkBitString() == host2.GetNetworkBitString())
                MakeColor(HostColors.Green);
            else
                MakeColor(HostColors.Red);
        }
        #endregion Color Methods
    }
}
