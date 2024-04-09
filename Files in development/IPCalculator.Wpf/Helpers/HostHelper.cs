using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IpCalculator.core;
using static PE1.Wpf.Helpers.WpfHelper;

namespace PE1.Wpf.Helpers;

public static class HostHelper
{
    
        /// <summary>
        /// Fills the textboxes with the correct calculated values.
        /// Determines which textboxes by the provided host
        /// Selects text boxes based on a common basename + content specific suffix
        /// </summary>
        /// <param name="host">Host to determine correct textboxes</param>
        public static void FillTxtBoxes(Host host, Window window)
        {
            //Gelieve even extra naar deze methode te kijken tijdens het verbeteren
            //ik ben er erg trots op!
            if (!host.IsValidIp())
                return;
            string baseTxtName = host.HostName == "Host1" ? "Host1" : "Host2";
            string baseLblName = host.HostName == "Host1" ? "Host1" : "Host2";
            Dictionary<string,Func<string>> namesFunctions = new()
            {
                {baseTxtName + "IpDD", host.GetIpDD},
                {baseTxtName + "IpBinary", host.GetIpBitString},
                {baseTxtName + "SubnetDD", host.GetSubnetDD},
                {baseTxtName + "SubnetBinary", host.GetSubnetBitString},
                {baseTxtName + "NetworkDD", host.GetNetworkDD},
                {baseTxtName + "NetworkBinary", host.GetNetworkBitString},
                {baseTxtName + "FirstHostDD", host.GetFirstHostDD},
                {baseTxtName + "FirstHostBinary", host.GetFirstHostBitString},
                {baseTxtName + "LastHostDD", host.GetLastHostDD},
                {baseTxtName + "LastHostBinary", host.GetLastHostBitString},
                {baseTxtName + "BroadcastDD", host.GetBroadcastDD},
                {baseTxtName + "BroadcastBinary", host.GetBroadcastBitString},
            };
            List<TextBox> textBoxes = GetControlsBySuffix<TextBox>(namesFunctions.Keys, window).ToList();
            for (int i = 0; i < namesFunctions.Count; i++)
            {
                textBoxes[i].Text = namesFunctions.ElementAt(i).Value();
                if(i==1 && host.Cidr == 0) return;
                textBoxes[i].Text = namesFunctions.ElementAt(i).Value();
            }
            Label lblHostsAmount = GetPossibleControlBySuffix<Label>(baseLblName + "AmountOfHosts", window);
            lblHostsAmount.Content = $"Amount of hosts: {host.GetAmountOfHostsPossible()}";
        }
        /// <summary>
        /// Clears all textboxes with same base name followed by their specific prefixes
        /// </summary>
        /// <param name="baseTextBoxName"></param>
        public static void ClearTextBoxes(string baseTextBoxName, Window window)
        {
            List<string> possibleNames = new()
            {
                baseTextBoxName + "IpDD",
                baseTextBoxName + "IpBinary",
                baseTextBoxName + "SubnetDD",
                baseTextBoxName + "SubnetBinary",
                baseTextBoxName + "NetworkDD",
                baseTextBoxName + "NetworkBinary",
                baseTextBoxName + "FirstHostDD",
                baseTextBoxName + "FirstHostBinary",
                baseTextBoxName + "LastHostDD",
                baseTextBoxName + "LastHostBinary",
                baseTextBoxName + "BroadcastDD",
                baseTextBoxName + "BroadcastBinary"
            };
            IEnumerable<TextBox> textBoxes = GetControlsBySuffix<TextBox>(possibleNames, window);
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Clear();
            }
        }
        /// <summary>
        /// Clears labels
        /// </summary>
        /// <param name="baseLblName">string containing base common name</param>
        public static void ClearLabel(string baseLblName, Window window)
        {
            Label lblHostsAmount = GetPossibleControlBySuffix<Label>(baseLblName + "AmountOfHosts", window);
            lblHostsAmount.Content = $"Amount of hosts:";
        }
}