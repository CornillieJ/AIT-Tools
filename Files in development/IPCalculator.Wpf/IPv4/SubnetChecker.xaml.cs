using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IpCalculator.core;
using IpCalculator.Wpf.IPv6;
using PE1.Wpf.Helpers;

namespace IpCalculator.Wpf.IPv4;

public partial class SubnetChecker : Window
{
    public SubnetChecker()
    {
        InitializeComponent();
    }
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        string subnet = txtSubnet.Text;
        if (string.IsNullOrWhiteSpace(subnet)) return;
        if (!subnet.Contains('.'))
        {
            for (int i = 3; i < 12; i += 4)
            {
                if (subnet.Length > i)
                    subnet = subnet.Substring(0, i) + '.' + subnet.Substring(i);
            }
        }
        try
        {
            string subnetBitString = ByteConversion.ConvertDottedDecimalToIpBitString(subnet);
            txtResult.Text = subnetBitString;
            if (subnetBitString.Contains("01") || subnetBitString.Contains("0.1") )
            {
                lblAnswer.Content = "No";
                bdrColor.BorderBrush = Brushes.Red;
            }
            else
            {
                lblAnswer.Content = "Yes";
                bdrColor.BorderBrush = Brushes.Green;
            }
        }
        catch
        {
            lblAnswer.Content = "Invalid format";
            bdrColor.BorderBrush = Brushes.Red;
        }
    }

    private void TxtSubnet_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        bdrColor.BorderBrush = Brushes.Gray;
        lblAnswer.Content = "";
        txtResult.Text = "";
    }

    private void SubnetChecker_OnClosing(object sender, CancelEventArgs e)
    {
        if (!WpfHelper.AreAllWindowsOfTypeClosed<SubnetChecker>()) return; 
        StartupWindow startupWindow = Application.Current.Windows.OfType<StartupWindow>().First();
        startupWindow.btnIPv6Calc.Background = Brushes.Coral;
    }
}