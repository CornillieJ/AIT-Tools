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
        string cidrText = txtCidr.Text.Trim();
        if (string.IsNullOrWhiteSpace(cidrText)) return;
        if (!byte.TryParse(cidrText, out byte cidr)) return;
        lblResult.Content = ByteConversion.ConvertCidrToSubnetMaskBitString(cidr);
    }

    private void TxtCidr_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        lblResult.Content = "";
    }

    private void CidrConverter_OnClosed(object sender, EventArgs e)
    {
        if (WpfHelper.AreAllWindowsOfTypeClosed<SubnetChecker>())
        {
            StartupWindow startupWindow = Application.Current.Windows.OfType<StartupWindow>().First();
            startupWindow.btnIPv6Calc.Background = Brushes.Coral;
        } 
    }
}