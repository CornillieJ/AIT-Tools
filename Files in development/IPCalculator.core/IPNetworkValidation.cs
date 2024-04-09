using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using static IpCalculator.core.ByteConversion;

namespace IpCalculator.core;

public static class IpNetworkValidation
{
    public static bool IsIpLocal(byte?[] bytes)
    {
        byte octet1 = (byte)bytes[0]!;
        byte octet2 = (byte)bytes[1]!;
        if (IpStructure.IpFirstHalf.TryGetValue(octet1, out List<byte>? value))
            if (value!.Contains(octet2))
                return true;
        return false;
    }
    
    public static bool IsCidrValid(byte cidr, byte? octet1)
    {
        IpStructure.CidrDictionary.TryGetValue((byte)octet1!, out List<byte>? validCidrs);
        if (validCidrs!.Contains(cidr))
            return true;
        return false;
    }

    public static byte GetCidrOnlyIfValidLocalNetwork(bool isValidLocalNetwork, byte cidr)
    {
        return isValidLocalNetwork ? cidr : (byte)0;
    }
    
    public static HostColors GetIpValidationColor(int amountOfPeriods, String iPString)
    {
        if (amountOfPeriods != 3)
            return HostColors.White;
        if (!IsIpDDValid(iPString)) 
            return HostColors.Red;
        string[] byteStrings = iPString.Split('.');
        byte?[] bytes = ConvertStringArrayToByteArray(byteStrings);
        if (IsIpLocal(bytes)) 
            return HostColors.Green; 
        return HostColors.Yellow; 
    }
    public static bool IsIpDDValid(string ipDD)
    {
        string[] ipDDArray=ipDD.Split('.');
        foreach (string byteString in ipDDArray)
        {
            bool isByte=byte.TryParse(byteString,out byte result);
            if (!isByte) return false;
        }

        return true;
    }

    public static string GetCurrentIpv4Info()
    {
        NetworkInterface[] allNics = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface nic in allNics)
        {
            if (nic.Description.Contains("Virtual")|| nic.Description.Contains("Loopback"))
                continue;
            if ( nic.OperationalStatus != OperationalStatus.Up) 
                continue;
            UnicastIPAddressInformationCollection iPInterfaceProperties = nic.GetIPProperties().UnicastAddresses;
            foreach (UnicastIPAddressInformation ip in iPInterfaceProperties)
            {
                if(ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    return ip.Address.ToString();
            }
        }
        return "";
    }
    public static string GetCurrentCidr()
    {
        NetworkInterface[] allNics = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface nic in allNics)
        {
            if (nic.Description.Contains("Virtual")|| nic.Description.Contains("Loopback"))
                continue;
            if ( nic.OperationalStatus != OperationalStatus.Up) 
                continue;
            UnicastIPAddressInformationCollection iPInterfaceProperties = nic.GetIPProperties().UnicastAddresses;
            foreach (UnicastIPAddressInformation ip in iPInterfaceProperties)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    var subnetMask = ip.IPv4Mask.ToString();
                    var subnetBitString = ConvertDottedDecimalToIpBitString(subnetMask).Replace(".","");
                    var trimmed = subnetBitString.TrimEnd('0');
                    return trimmed.Length.ToString();
                }
            }
        }
        return "";
    }
    public static string GetCurrentIpv6Info()
    {
        NetworkInterface[] allNics = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface nic in allNics)
        {
            if (nic.Description.Contains("Virtual")|| nic.Description.Contains("Loopback"))
                continue;
            if ( nic.OperationalStatus != OperationalStatus.Up) 
                continue;
            UnicastIPAddressInformationCollection iPInterfaceProperties = nic.GetIPProperties().UnicastAddresses;
            foreach (UnicastIPAddressInformation ip in iPInterfaceProperties)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    return ip.Address.ToString();
                }
            }
        }
        return "";
    }
}