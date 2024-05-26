using System;
using System.CodeDom;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using IpCalculator.Wpf.IPv4;
using IpCalculator.Wpf.IPv6;

namespace IpCalculator.Wpf;

public partial class StartupWindow : Window
{
    public StartupWindow()
    {
        InitializeComponent();
    }

    private void BtnIPv4Calc_OnClick(object sender, RoutedEventArgs e)
    {
        IPv4CalcWindow ipv4Window = new();
        ipv4Window.Show();
        btnIPv4Calc.Background = Brushes.PaleGreen;
    }

    private void BtnIPv6Calc_OnClick(object sender, RoutedEventArgs e)
    {
        IPv6CalcWindow ipv6Window = new();
        ipv6Window.Show();
        btnIPv6Calc.Background = Brushes.PaleGreen;
    }

    private void BtnIPv4Manual_OnClick(object sender, RoutedEventArgs e)
    {
        ManualWindow manualWindow = new();
        manualWindow.Show();
        btnIPv4Manual.Background = Brushes.PaleGreen;
    }

    private void BtnMyNics_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            string programPath = @"MyNics\MyNICs.exe";
            Process.Start(programPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error launching MyNics: {ex.Message}");
        }
    }

    private void BtnNat_OnClick(object sender, RoutedEventArgs e)
    {
        string programPath; 
        programPath = sender == btnNatBig ? @"NATbig\NAT.exe" : @"NATsmall\NAT.exe";
        try
        {
            Process.Start(programPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error launching NAT tool: {ex.Message}");
        }
    }

    private void BtnSummary_OnClick(object sender, RoutedEventArgs e)
    {
        
        string programPath = @"Samenvatting\samenvatting.html";
        try
        {
            Process.Start(@"cmd.exe ", @"/c " + programPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error launching {ex.Message}");
        }
    }
    private void BtnSubnet_OnClick(object sender, RoutedEventArgs e)
    {
        SubnetChecker subnetWindow = new();
        subnetWindow.Show();
        btnSubnet.Background = Brushes.PaleGreen;
    }
    private void BtnCidr_OnClick(object sender, RoutedEventArgs e)
    {
        CidrConverter cidrConverter = new();
        cidrConverter.Show();
        btnCidr.Background = Brushes.PaleGreen;
    }

    private void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        base.Close();
    }

    private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
    {
        base.WindowState = WindowState.Minimized;
    }
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }
}