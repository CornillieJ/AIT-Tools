using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IpCalculator.core;

using PE1.Wpf.Helpers;
namespace IpCalculator.Wpf.IPv4;

public partial class CidrConverter : Window
{
    public CidrConverter()
    {
        InitializeComponent();
    }

    private void TxtConvert_OnClick(object sender, RoutedEventArgs e)
    {
        if (txtCidr.Text == "")
        {
            string bitString = txtResult.Text.Trim();
            if (bitString.Contains("01") || bitString.Contains("0.1")) return;
            txtResult.Text = "";
            string dd = ByteConversion.ConvertDottedDecimalToIpBitString(bitString);
            txtCidr.Text = dd.Count(b => b == '1').ToString();
        }
        else
        {
            string cidrText = txtCidr.Text.Trim();
            if (string.IsNullOrWhiteSpace(cidrText)) return;
            if (!byte.TryParse(cidrText, out byte cidr)) return;
            txtCidr.Text = "";
            string bitString = ByteConversion.ConvertCidrToSubnetMaskBitString(cidr);
            txtResult.Text = ByteConversion.ConvertIpBitStringToDottedDecimal(bitString);
        }
    }

    private void TxtCidr_OnTextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void CidrConverter_OnClosed(object sender, EventArgs e)
    {
        if (WpfHelper.AreAllWindowsOfTypeClosed<SubnetChecker>())
        {
            StartupWindow startupWindow = Application.Current.Windows.OfType<StartupWindow>().First();
            startupWindow.btnIPv6Calc.Background = Brushes.Coral;
        } 
    }

    private void TxtResult_OnTextChanged(object sender, TextChangedEventArgs e)
    {
    }
}