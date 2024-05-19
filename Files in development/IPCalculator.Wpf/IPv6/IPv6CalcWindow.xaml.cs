using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IpCalculator.core;
using IpCalculator.Wpf.IPv4;
using PE1.Wpf.Helpers;
using static IpCalculator.core.IPv6.IPv6Converter;
using static PE1.Wpf.Helpers.WpfHelper;

namespace IpCalculator.Wpf.IPv6;

public partial class IPv6CalcWindow : Window
{
    private const string EmptyIPv6 = "____:____:____:____:____:____:____:____";
    private bool isUpdating= false;
    public IPv6CalcWindow()
    {
        InitializeComponent();
        DataObject.AddPastingHandler(txtFullIp,OnPaste);
        DataObject.AddPastingHandler(txtShortIp,OnPaste);
    }
    private void IPv6CalcWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        string ipv6Info = IpNetworkValidation.GetCurrentIpv6Info();
        txtShortIp.Text = ipv6Info.Split('%')[0];
        if (ipv6Info.Split('%').Length > 1)
        {
            txtCIDR.Text = ipv6Info.Split('%')[1];
        }
    }
    private void IPv6CalcWindow_OnClosing(object sender, CancelEventArgs e)
    {
        if (AreAllWindowsOfTypeClosed<IPv6CalcWindow>()) ;
        StartupWindow startupWindow = Application.Current.Windows.OfType<StartupWindow>().First();
        startupWindow.btnIPv6Calc.Background = Brushes.Coral;
    }

    private void TxtFullIp_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender != txtFullIp || isUpdating) return;
        if(txtFullIp.Text == EmptyIPv6) return;
        if (IsTextBoxEmpty(txtFullIp))
        {
            isUpdating = true;
            txtFullIp.Text = EmptyIPv6;
            isUpdating = false;
        }
        int position = txtFullIp.CaretIndex;
        string fullIPv6 = txtFullIp.Text.Trim();
        string[] segments = fullIPv6.Split(':');
        StringBuilder formatted = GetFormattedIPAsStringBuilder(segments);
        isUpdating = true;
        txtFullIp.Text = formatted.ToString();
        isUpdating = false;
        SetCaretToNewPosition(position, formatted);
        if (formatted.ToString().Contains('_'))
        {
            txtFullIp.Background = Brushes.Transparent;
            return;
        }
        txtFullIp.Background = IsIPv6Valid(formatted.ToString()) ? Brushes.LightGreen : Brushes.MediumVioletRed;
    }
    private void TxtShortIp_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender != txtShortIp) return;
        string shortIpv6 = txtShortIp.Text.Trim();
        string[] segments = shortIpv6.Split(":");
        if (segments.Length < 2)
        {
            txtShortIp.Background = Brushes.Transparent;
            return;
        }
        if (segments.Length > 8 || IsMoreThanOneSegmentEmpty(segments) || IsAnySegmentTooLong(segments))
        {
            txtShortIp.Background = Brushes.MediumVioletRed;
            return;
        }
        txtShortIp.Background = IsIPv6Valid(shortIpv6) ? Brushes.LightGreen : Brushes.MediumVioletRed;
    }

    private bool IsMoreThanOneSegmentEmpty(IEnumerable<string> segments)
    {
        try
        {
            segments.Single(s => s.Length == 0);
            return false;
        }
        catch
        {
            if (IsOnlyLast2SegmentsEmpty(segments) || IsOnlyFirst2SegmentsEmpty(segments))  return false;
            return true;
        }
    }

    private bool IsOnlyLast2SegmentsEmpty(IEnumerable<string> segments)
    {
        return segments.TakeLast(2).All(s => s.Length == 0) // last 2 empty 
               &&
               segments.Take(segments.Count() - 2).All(s => s.Length > 0); //all others not empty
    }    
    private bool IsOnlyFirst2SegmentsEmpty(IEnumerable<string> segments)
    {
        return segments.Take(2).All(s => s.Length == 0) // first 2 empty 
               &&
               segments.TakeLast(segments.Count() - 2).All(s => s.Length > 0); //all others not empty
    }
    private bool IsAnySegmentTooLong(IEnumerable<string> segments)
    {
        return segments.Any(s => s.Length > 4);
    }
    private void BtnClear_OnClick(object sender, RoutedEventArgs e)
    {
        isUpdating = true;
        txtFullIp.Text = EmptyIPv6;
        txtShortIp.Clear();
        tbkResult.Text = "";
        TextBox relevantTextBox = sender == btnEnter ? txtFullIp : txtShortIp;
        relevantTextBox.Focus();
        relevantTextBox.CaretIndex = 0;
        txtRouting.Clear();
        txtNode.Clear();
        txtSubnet.Clear();
        isUpdating = false;
    }
    
    private void BtnEnter_OnClick(object sender, RoutedEventArgs e)
    {
        isUpdating = true;
        string fullIP = txtFullIp.Text.Trim();
        if (fullIP.Contains('_') || txtFullIp.Background == Brushes.MediumVioletRed) return;
        tbkResult.Text = "";
        string[] steps = GetShortenIPv6Steps(fullIP);
        tbkResult.Text += $"Stap 1: volledige nullen inkorten \n";
        tbkResult.Text += "   -> " + steps[0] + "\n";
        tbkResult.Text += $"Stap 2 : Weghalen van voorloop nullen\n";
        tbkResult.Text += "   -> " + steps[1] + "\n";
        tbkResult.Text += $"Stap 3 : Achtereenvolgende nullen weghalen \n" +
                          $"op de plaats met het meeste impact\n";
        tbkResult.Text += "   -> " + steps[2] + "\n";

        txtShortIp.Text = steps[^1];
        isUpdating = false;
    }
    private void BtnEnterShortToLong_OnClick(object sender, RoutedEventArgs e)
    {
        string shortIP = txtShortIp.Text.Trim();
        if (shortIP.Length == 0) return;
        tbkResult.Text = "";
        string[] steps = GetFullIPv6Steps(shortIP);
        tbkResult.Text += $"Stap 1: Verwijderde nullen terug plaatsen\n";
        tbkResult.Text += "   -> " + steps[0] + "\n";
        tbkResult.Text += $"Stap 2 : voorloop nullen terug plaatsen\n";
        tbkResult.Text += "   -> " + steps[1] + "\n";
        txtFullIp.Text = steps[^1];
    }
    private void BtnParts_OnClick(object sender, RoutedEventArgs e)
    {
        if (IsTextBoxEmpty(txtCIDR)) ;
        if (IsTextBoxEmpty(txtFullIp) && IsTextBoxEmpty(txtShortIp) || txtFullIp.Text.Contains("_")) return;
        if (IsTextBoxEmpty(txtFullIp)) BtnEnterShortToLong_OnClick(null,null);
        if(IsTextBoxEmpty(txtShortIp)) BtnEnter_OnClick(null,null);
        bool isValidCidr = int.TryParse(txtCIDR.Text, out int cidr);
        if (!isValidCidr || cidr > 64)
        {
            txtRouting.Clear();
            txtNode.Clear();
            txtSubnet.Clear();
            return;
        }
        string fullIp = txtFullIp.Text.Trim();
        IEnumerable<string> firstHalf = fullIp.Split(":").Take(4);
        IEnumerable<string> secondHalf = fullIp.Split(":").TakeLast(4);
        string routing = GetRoutingPrefix(cidr, firstHalf).Trim(':');
        txtRouting.Text = routing;
        txtNode.Text = string.Join(':', secondHalf).Trim(':');
        txtSubnet.Text = GetSubnetId(cidr,firstHalf).Trim(':');
    }
    private bool IsIPv6Valid(string iPv6)
    {
        try
        {
            string ipv6NoSeparator = iPv6.Replace(":", "");
            if (ipv6NoSeparator.Length % 2 != 0) ipv6NoSeparator += "0";
            Convert.FromHexString(ipv6NoSeparator);
        }
        catch
        {
            return false;
        }
        return true;
    }
    private static StringBuilder GetFormattedIPAsStringBuilder(string[] segments)
    {
        StringBuilder formatted = new ();
        List<string> newSegments = segments.ToList();
        while (newSegments.Count != 8)
        {
            newSegments.Add("____");
        }
        foreach (string hextet in newSegments)
        {
            if (hextet.Length == 4) formatted.Append(hextet + ':');
            else if (hextet.Length > 4) formatted.Append(hextet.Substring(0,4) + ':');
            else
            {
                StringBuilder newHextet = new(hextet);
                while (newHextet.Length < 4)
                {
                    newHextet.Append('_');
                }
                formatted.Append(newHextet + ":");
            }
        }
        formatted.Remove(formatted.Length-1,1);
        return new StringBuilder(formatted.ToString().ToLower());
    }
    private void SetCaretToNewPosition(int position, StringBuilder formatted)
    {
        if (position >= formatted.Length) position = formatted.Length;
        else if (formatted[position] == ':') position++;
        txtFullIp.CaretIndex = position;
    }
    private void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        textBox.SelectAll();
    }
}