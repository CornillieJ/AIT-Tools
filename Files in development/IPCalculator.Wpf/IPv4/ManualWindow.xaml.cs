using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static PE1.Wpf.Helpers.WpfHelper;
using IpCalculator.core;
using static PE1.Wpf.Helpers.HostHelper;
using static IpCalculator.core.ByteConversion;
using static IpCalculator.core.IpNetworkValidation;
using static IpCalculator.core.StringBuilderIpFormatting;

namespace IpCalculator.Wpf
{
    public partial class ManualWindow : Window
    {
        #region Global variables
        const string BaseName = "Host1";
        private readonly Host host1 = new("Host1");   
        #endregion Global variables
        
        #region Constructor
        public ManualWindow()
        {
            InitializeComponent();
        }
        #endregion Constructor
        
        #region Event Handlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtIp.Text = GetCurrentIpv4Info();
            txtCidr.Text = GetCurrentCidr();
        }
        private void ManualWindow_OnClosed(object sender, EventArgs e)
        {
            if (AreAllWindowsOfTypeClosed<ManualWindow>()) return;
            StartupWindow startupWindow = Application.Current.Windows.OfType<StartupWindow>().First();
            startupWindow.btnIPv4Manual.Background = Brushes.LightCoral;
        }
        private void TxtIp_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder(txtIp.Text);
            (int amountOfPeriods, int indexOfLastPeriod) =GetPeriodInfo(sb);
            sb = FormatStringBuilderToIpFormat(sb, amountOfPeriods, indexOfLastPeriod);
            txtIp.Text = sb.ToString();
            PlaceCursorAtEnd(txtIp);
            txtIp.Background = GetIpValidationSolidColorBrush(amountOfPeriods, sb);
        }
        private void Txt_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            BtnEnter_OnClick(null,null);
        }
        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            //Write
            host1.ResetIpAndCidr();
            //Update GUI
            ResetGUI(BaseName);
        }
        private void BtnEnter_OnClick(object sender, RoutedEventArgs e)
        {
            ResetGUIOutput(BaseName);
            if (!IsIpDDValid(txtIp.Text))
                return;
            //read
            string[] inputStrings =txtIp.Text.Split('.');
            string cidrText = txtCidr.Text;
            //Process
            byte.TryParse(cidrText, out byte cidr);
            byte?[] bytes = ConvertStringArrayToByteArray(inputStrings);
            //Write
            host1.WriteArrayToHostIp(bytes);
            bool isValidLocalNetwork = IsIpLocal(host1.IpAddress) && IsCidrValid(cidr,host1.IpAddress[0]);
            host1.Cidr = GetCidrOnlyIfValidLocalNetwork(isValidLocalNetwork, cidr);
            //Update GUI
            ColorCidrTextBox(isValidLocalNetwork);
            FillTxtBoxes(host1, this);
        }
        #endregion Event Handlers

        #region Methods
        private SolidColorBrush GetIpValidationSolidColorBrush(int amountOfPeriods, StringBuilder sb)
        {
            HostColors hostColors = GetIpValidationColor(amountOfPeriods, sb.ToString());
            switch (hostColors)
            {
                case HostColors.Green:
                    return Brushes.LightGreen;
                case HostColors.Red:
                    return Brushes.LightCoral;
                case HostColors.Yellow:
                    return Brushes.Yellow;
                default:
                    return Brushes.White;
            }
        }
        private void ColorCidrTextBox(bool isBothInputOk)
        {
            txtCidr.Background = isBothInputOk ? Brushes.LightGreen : Brushes.LightCoral;
        }
        /// <summary>
        /// Resets the GUI elements whose basename starts with the given baseNames.
        /// </summary>
        /// <param name="baseName"></param>
        private void ResetGUI(string baseName)
        {
            ClearTextBoxes(baseName, this);
            ClearLabel(baseName, this);
            ClearExtraWindowTextBoxes();
        }
        private void ResetGUIOutput(string baseName)
        {
            ClearTextBoxes(baseName, this);
            ClearLabel(baseName, this);
        }
        private void ClearExtraWindowTextBoxes()
        {
            txtIp.Clear();
            txtCidr.Clear();
            txtCidr.Background=Brushes.White;
        }
        #endregion Methods
    }
}
