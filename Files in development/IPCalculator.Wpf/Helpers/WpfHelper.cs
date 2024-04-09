#nullable enable
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using IpCalculator.Wpf;
using IpCalculator.Wpf.IPv4;

namespace PE1.Wpf.Helpers;

public static class WpfHelper
{
    #region Get Controls (variables and methods)
    /// <summary>
    /// Dictionary containing the prefix convention value used for each control key
    /// </summary>
    private static readonly Dictionary<Type, string> Prefixes = new()
    {
        { typeof(TextBox), "txt" },
        { typeof(ComboBox), "cmb" },
        { typeof(Button), "btn" },
        { typeof(ListBox), "lst" },
        { typeof(Label), "lbl" },
        { typeof(CheckBox), "chk" }
    };
    /// <summary>
    /// Gets Controls of the given type that have names corresponding to the Enum values.
    /// prefixes for control type get automatically added
    /// </summary>
    /// <param name="window">The parent window of the controls</param>
    /// <typeparam name="TControl">The Type of the controls you want to get</typeparam>
    /// <typeparam name="TEnum">The enum containing the control names minus prefixes</typeparam>
    /// <returns>An IEnumerable containing all found controls</returns>
    public static IEnumerable<TControl> GetControlsByEnumSuffix<TControl, TEnum>(Window window)
    {
        IEnumerable names = GetEnumsAsIEnumerable<TEnum>();
        IEnumerable<TControl> controls = GetControlsBySuffix<TControl>(names,window);
        return controls;
    }
    /// <summary>
    /// Gets Controls by their name without the added prefix. names should be listed in an IEnumerable
    /// </summary>
    /// <param name="suffixList">IEnumerable of string? containing the names of the controls without prefix</param>
    /// <param name="window">The parent window of the controls</param>
    /// <typeparam name="TControl">The type of control to look for</typeparam>
    /// <returns>An IEnumerable containing all found controls</returns>
    public static IEnumerable<TControl> GetControlsBySuffix<TControl>(IEnumerable suffixList, Window window)
    {

        Prefixes.TryGetValue(typeof(TControl), out string? prefix);
        List<TControl> controls = new();
        foreach (object? suffix in suffixList)
        {
            TControl? possibleControl = GetPossibleControlBySuffix<TControl>(suffix.ToString(), window);
            if (possibleControl != null)
                controls.Add(possibleControl);
        }
        return controls;
    }
    /// <summary>
    /// Gets a control of the given type whose name consists of prefix+suffix, if none are found returns null
    /// </summary>
    /// <param name="prefix">The first part of the control name</param>
    /// <param name="suffix">The second part of the control name</param>
    /// <param name="window">The parent window of the controls</param>
    /// <typeparam name="TControl">The type of control</typeparam>
    /// <returns>The control with that name, null if none are found</returns>
    public static TControl? GetPossibleControlBySuffix<TControl>(string? suffix, Window window)
    {
        Prefixes.TryGetValue(typeof(TControl), out string? prefix);
        return (TControl)window.FindName($"{prefix}{suffix}");
    }
    #endregion Get Controls (variables and methods)
    
    #region Check Inputs (variables and methods)
    private const string NoInputEntered = "Please fill in a valid ";
    
    public static bool IsTextBoxEmpty(TextBox textBox)
    {
        return string.IsNullOrWhiteSpace(textBox.Text);
    }
    public static bool IsNothingSelected(Selector selector)
    {
        return selector.SelectedIndex == -1;
    }
    public static void CheckForEmptyTextBoxes(IEnumerable<TextBox> textBoxCollection)
    {
        foreach (TextBox textBox in textBoxCollection)
        {
            if (IsTextBoxEmpty(textBox)) 
                throw new ArgumentException(NoInputEntered + textBox.Name.Substring(3), textBox.Name);
        }
    }
    public static void CheckForEmptyComboBoxes(IEnumerable<ComboBox> comboBoxCollection)
    {
        foreach (ComboBox comboBox in comboBoxCollection)
        {
            if (IsNothingSelected(comboBox)) 
                throw new ArgumentException(NoInputEntered + comboBox.Name.Substring(3), comboBox.Name);
        }
    }
    #endregion Check Inputs (variables and methods)
    
    #region Check TextBox for invalid input overloads (variables and methods)
    
    private const string NoValidNumberEntered = "Please enter a valid number for";
    
    public static void ReadAndValidateNumbers(TextBox textBox,out decimal result)
    {
        if(!decimal.TryParse(textBox.Text, out result))
            throw new ArgumentException(NoValidNumberEntered + textBox.Name.Substring(3), textBox.Name);
    }
    public static void ReadAndValidateNumbers(TextBox textBox,out int result)
    {
        if(!int.TryParse(textBox.Text, out result))
            throw new ArgumentException(NoValidNumberEntered + textBox.Name.Substring(3), textBox.Name);
    }
    public static void ReadAndValidateNumbers(TextBox textBox,out float result)
    {
        if(!float.TryParse(textBox.Text, out result))
            throw new ArgumentException(NoValidNumberEntered + textBox.Name.Substring(3), textBox.Name);
    }
    public static void ReadAndValidateNumbers(TextBox textBox,out double result)
    {
        if(!double.TryParse(textBox.Text, out result))
            throw new ArgumentException(NoValidNumberEntered + textBox.Name.Substring(3), textBox.Name);
    }
    public static void ReadAndValidateNumbers(TextBox textBox,out byte result)
    {
        if(!byte.TryParse(textBox.Text, out result))
            throw new ArgumentException(NoValidNumberEntered + textBox.Name.Substring(3), textBox.Name);
    }
    
    #endregion Check TextBox for invalid input overloads (variables and methods)

    #region Flash control

    /// <summary>
    /// This method flashes the border of the given control the given amount of times
    /// </summary>
    /// <param name="control">The control that needs to flash</param>
    /// <param name="amountOfFlashes">The amount of times you want to flash the button</param>
    public static async void FlashControlBorder(Control control, int amountOfFlashes)
    {
        for (int i = 0; i < amountOfFlashes; i++)
        {
            control.BorderBrush = Brushes.Red;
            await Task.Delay(100);
            control.ClearValue(Border.BorderBrushProperty);
            await Task.Delay(100);
        }
        control.Focus();
    }

    /// <summary>
    /// This method flashes the border of the given control the given amount of times
    /// </summary>
    /// <param name="control">The control that needs to flash</param>
    /// <param name="amountOfFlashes">The amount of times you want to flash the button</param>
    /// <param name="window">The parent window of the control</param>
    public static void FlashControl(Control control, int amountOfFlashes, Window window)
    {
        string prefix = control.Name.Substring(0, 3);
        if (prefix == "cmb")
        {
            FlashBorderAroundControl(control,amountOfFlashes,window);
        }
        else
        {
            FlashControlBorder(control,amountOfFlashes);
        }
    }
    public static async void FlashBorderAroundControl(Control control, int amountOfFlashes, Window window)
    {
        string borderName = $"bdr{control.Name.Substring(3)}";
        Border border = (Border)window.FindName(borderName);
        for (int i = 0; i < amountOfFlashes; i++)
        {
            border.BorderBrush = Brushes.Red;
            border.BorderThickness = new Thickness(1);
            await Task.Delay(100);
            border.ClearValue(Border.BorderBrushProperty);
            border.BorderThickness = new Thickness(0);
            await Task.Delay(100);
        }
        control.Focus();
    }
    #endregion Flash control
    
    #region Other helpers
    /// <summary>
    /// Gets an IEnumerable containing all enumvalues.
    /// </summary>
    /// <typeparam name="TEnum">The type of Enum</typeparam>
    /// <returns>IEnumerable</returns>
    public static IEnumerable GetEnumsAsIEnumerable<TEnum>()
    {
        return Enum.GetValues(typeof(TEnum));
    }
    
    /// <summary>
    /// Gets the last character from a string and returns it as a string value
    /// </summary>
    /// <param name="text">a string that you would like to get the last char of</param>
    /// <returns>a string of length 1 containing the last character</returns>
    public static string GetLastCharAsString(string text)
    {
        return text.Substring(text.Length-1);
    }
    /// <summary>
    /// Get the base common name of controls in project. Returns the name minus given suffixlength.
    /// This baseName includes the control specific prefix
    /// </summary>
    /// <param name="inputElement">The inputElement</param>
    /// <param name="suffixLength">The length of the suffix to remove</param>
    /// <returns></returns>
    public static string GetBaseNameOfControl(IFrameworkInputElement inputElement,int suffixLength)
    {
        if (inputElement == null) return string.Empty;
        string baseName = inputElement.Name.Substring(3, inputElement.Name.Length -3-suffixLength);
        return baseName;
    }
    /// <summary>
    /// Clears the Combobox that has the provided name
    /// </summary>
    /// <param name="name">Name of the Combobox</param>
    public static void ClearCmb(string name, Window window)
    {
        ComboBox? comboBox =GetPossibleControlBySuffix<ComboBox>(name, window);
        comboBox!.ItemsSource=null;
    }

    public static void PlaceCursorAtEnd(TextBox txtBox)
    {
        txtBox.Select(txtBox.Text.Length,0);
    }
    #endregion Other helpers

    public static bool AreAllWindowsOfTypeClosed<T>()
    {
        if (!Application.Current.Windows.OfType<StartupWindow>().Any()) return false;
        if (!Application.Current.Windows.OfType<T>().Any()) return false;
        return true;
    }
}