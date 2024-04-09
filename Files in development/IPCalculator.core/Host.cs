using System.Text;

namespace IpCalculator.core;

public class Host
{
    #region Fields and properties

    private const string InvalidIpErrorText = "Gelieve een geldig Ipadres in te voeren";
    private const string InvalidCidrErrorText = "Gelieve een geldige CIDR in te voeren";
    public byte?[] IpAddress { get;}
    public byte Cidr { get; set; }
    public string HostName { get; set; }
    #endregion Fields and properties
    #region constructors
    public Host(string hostName)
    {
        IpAddress = new byte?[4];
        for (int i = 0; i < IpAddress.Length; i++)
        {
            IpAddress[i] = null;
        }
        Cidr = 0;
        HostName = hostName;
    }
    public Host(byte octet1):this("")
    {
        IpAddress[0] = octet1;
    }
    public Host(byte octet1, byte octet2):this(octet1)
    {
        IpAddress[1] = octet2;
    }
    public Host(byte octet1, byte octet2, byte octet3):this(octet1, octet2)
    {
        IpAddress[2] = octet3;
    }
    public Host(byte octet1, byte octet2, byte octet3, byte octet4):this(octet1,octet2,octet3)
    {
        IpAddress[3] = octet4;
    }
    #endregion constructors
    #region Methods
    /// <summary>
    /// Checks if any of the octets are null
    /// </summary>
    /// <returns>true if all IP octets were entered, otherwise returns false</returns>
    public bool IsValidIp()
    {
        foreach (byte? octet in IpAddress)
        {
            if (octet == null) return false;
        }
        return true;
    }
    /// <summary>
    /// Checks if CIDR is entered 
    /// </summary>
    /// <returns>false if CIDR = 0 otherwise true</returns>
    public bool IsValidCidr()
    {
        return Cidr != 0;
    }
    /// <summary>
    /// Error handling on wether a valid IP was entered. throws invalidOperationException if IP is not valid
    /// </summary>
    /// <exception cref="InvalidOperationException">error message is InvalidIpErrorText</exception>
    private void CheckIpValidity()
    {
        if (!IsValidIp()) 
            throw new InvalidOperationException(InvalidIpErrorText);
    }
    /// <summary>
    /// Error handling on wether a valid CIDR was entered. throws invalidOperationException if CIDR is not valid
    /// </summary>
    /// <exception cref="InvalidOperationException">error message is InvalidCidrErrorText</exception>
    private void CheckCidrValidity()
    {
        if (!IsValidCidr()) 
            throw new InvalidOperationException(InvalidCidrErrorText);
    }
    /// <summary>
    /// Gets the IP address as a string of bits if the IP adress is valid 
    /// </summary>
    /// <returns>string of length 32</returns>
    public string GetIpBitString()
    {
        CheckIpValidity();
        StringBuilder sb = new();
        for (int i = 0; i < IpAddress.Length; i++)
        {
            if (i > 0) 
                sb.Append('.');
            sb.Append(ByteConversion.ConvertByteToBitString((byte)IpAddress[i]!));
        }
        return sb.ToString();
    }
    /// <summary>
    /// Gets the IP address as a string in Dotted Decimal if the IP adress is valid 
    /// </summary>
    /// <returns>string representing the IP as a dotted decimal</returns>
    public string GetIpDD()
    {
        CheckIpValidity();
        return $"{IpAddress[0]}.{IpAddress[1]}.{IpAddress[2]}.{IpAddress[3]}";
    }
    /// <summary>
    /// Gets the subnet mask as a string of bits if the IP adress is valid 
    /// </summary>
    /// <returns>string of length 32</returns>
    public string GetSubnetBitString()
    {
        CheckCidrValidity();
        string bitString = ByteConversion.ConvertCidrToSubnetMaskBitString(Cidr);
        return bitString;
    }
    /// <summary>
    /// Gets the subnet mask as a string in Dotted Decimal if the IP adress is valid 
    /// </summary>
    /// <returns>string representing the subnet mask as a dotted decimal</returns>
    public string GetSubnetDD()
    {
        CheckCidrValidity();
        return ByteConversion.ConvertIpBitStringToDottedDecimal(GetSubnetBitString());
    }
    /// <summary>
    /// Gets the Network address as a string of bits if the IP adress is valid 
    /// </summary>
    /// <returns>string of length 32</returns>
    public string GetNetworkBitString()
    {
        CheckIpValidity();
        CheckCidrValidity();
        string subnetBitString = GetSubnetBitString();
        string iPBitString = GetIpBitString();
        StringBuilder sb = new();
        for (int i = 0; i < subnetBitString.Length; i++)
        {
            if (subnetBitString[i] is '1' or '.' ) 
                sb.Append(iPBitString[i]);
            else 
                sb.Append('0');
        }
        return sb.ToString();
    }
    /// <summary>
    /// Gets the Network address as a string in Dotted Decimal if the IP adress is valid 
    /// </summary>
    /// <returns>string representing the Network address as a dotted decimal</returns>
    public string GetNetworkDD()
    {
        CheckIpValidity();
        CheckCidrValidity();
        return ByteConversion.ConvertIpBitStringToDottedDecimal(GetNetworkBitString());
    }
    /// <summary>
    /// Gets the IP address of the first possible host as a string in Dotted Decimal if the IP adress is valid 
    /// </summary>
    /// <returns>string representing the IP as a dotted decimal</returns>
    public string GetFirstHostDD()
    {
        CheckIpValidity();
        CheckCidrValidity();
        string networkDD = GetNetworkDD();
        string[] networkBytes = networkDD.Split('.');
        byte lastByte = Byte.Parse(networkBytes[^1]);
        lastByte++;
        StringBuilder sb = new();
        for (int i = 0; i < networkBytes.Length-1; i++)
        {
            sb.Append(networkBytes[i]);
            sb.Append('.');
        }
        sb.Append(lastByte);
        return sb.ToString();
    }
    /// <summary>
    /// Gets the IP address of the first possible host as a string of bits if the IP adress is valid 
    /// </summary>
    /// <returns>string of length 32</returns>
    public string GetFirstHostBitString()
    {
        CheckIpValidity();
        CheckCidrValidity();
        return ByteConversion.ConvertDottedDecimalToIpBitString(GetFirstHostDD());
    }
    /// <summary>
    /// Gets the broadcast IP address as a string of bits if the IP adress is valid 
    /// </summary>
    /// <returns>string of length 32</returns>
    public string GetBroadcastBitString()
    {
        CheckIpValidity();
        CheckCidrValidity();
        string networkBitString = GetNetworkBitString();
        string subnetMaskString = GetSubnetBitString();
        StringBuilder sb = new();
        for (int i = 0; i < subnetMaskString.Length; i++)
        {
            if (subnetMaskString[i] is '1' or '.')
                sb.Append(networkBitString[i]);
            else
                sb.Append('1');
        }

        return sb.ToString();
    }    
    /// <summary>
    /// Gets the Broadcast address as a string in Dotted Decimal if the IP adress is valid 
    /// </summary>
    /// <returns>string representing the Broadcast address as a dotted decimal</returns>
    public string GetBroadcastDD()
    {
        CheckIpValidity();
        CheckCidrValidity();
        return ByteConversion.ConvertIpBitStringToDottedDecimal(GetBroadcastBitString());
    }
    public string GetLastHostDD()
    {
        CheckIpValidity();
        CheckCidrValidity();
        string broadcastDD = GetBroadcastDD();
        string[] broadcastBytes = broadcastDD.Split('.');
        byte lastByte = Byte.Parse(broadcastBytes[^1]);
        lastByte--;
        StringBuilder sb = new();
        for (int i = 0; i < broadcastBytes.Length-1; i++)
        {
            sb.Append(broadcastBytes[i]);
            sb.Append('.');
        }
        sb.Append(lastByte);
        return sb.ToString();
    }
    /// <summary>
    /// Gets the IP address of the last possible host as a string of bits if the IP adress is valid 
    /// </summary>
    /// <returns>string of length 32</returns>
    public string GetLastHostBitString()
    {
        CheckIpValidity();
        CheckCidrValidity();
        return ByteConversion.ConvertDottedDecimalToIpBitString(GetLastHostDD());
    }
    /// <summary>
    /// Gets the amount of possible hosts for the current IP and CIDR. only if IP and CIDR are valid
    /// </summary>
    /// <returns>An int containing the amoung of possible hosts in the network</returns>
    public int GetAmountOfHostsPossible()
    {
        CheckIpValidity();
        CheckCidrValidity();
        return (int)(Math.Pow(2, 32 - Cidr)) - 2;
    }
    /// <summary>
    /// Resets the IP array and sets Cidr back to 0
    /// </summary>
    public void ResetIpAndCidr()
    {
        Array.Clear(IpAddress);
        Cidr = 0;
    }
    /// <summary>
    /// Writes array of byte? to the IpAddress array
    /// Extra method of saving the IP address, instead of listing every octet
    /// </summary>
    /// <param name="byteArray"></param>
    public void WriteArrayToHostIp(byte?[] byteArray)
    {
        for (int i = 0; i < 4; i++)
        {
            IpAddress[i]= byteArray[i];
        }
    }
    /// <summary>
    /// Adds the selected byte to the correct octet in the hosts Ipaddress.
    /// Resets the hosts values if the first octet changes
    /// </summary>
    /// <param name="octetNumber">The Octetnumber that we selected</param>
    /// <param name="byteToFill">The selected value</param>
    public void UpdateIpAddressAndResetIfFirstOctet(OctetNumber octetNumber, byte byteToFill)
    {
        int selectedOctet = (int)octetNumber-1; //enum is 1-indexed
        if (selectedOctet == 0) 
        {
            ResetIpAndCidr();
        }
        IpAddress[selectedOctet] = byteToFill;
    }
    #endregion Methods
}