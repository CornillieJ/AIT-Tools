using System.Collections;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;

namespace IpCalculator.core;

public static class ByteConversion
{
    private const string InvalidBitStringErrorText = "Gelieve een string van 8 bits mee te geven";
    private const string InvalidDDErrorText = "Gelieve een geldige dotted decimal waarde mee te geven";
    private const string InvalidIpBitStringErrorText = "Gelieve een geldig binair Ip adres mee te geven";
    /// <summary>
    /// Returns the given byte to a bitArray where the first value is the LSB and the last the MSB
    /// </summary>
    /// <param name="number">Byte to convert</param>
    /// <returns>BitArray starting at LSB</returns>
    public static BitArray ConvertByteToBitArray(byte number)
    {
        return new BitArray(new byte[] { number });
    }
    /// <summary>
    /// parses every string in the given array to a byte and returns the array of bytes.
    /// invalid strings will be registered as 0
    /// </summary>
    /// <param name="inputStrings">string array containing bytes as text</param>
    /// <returns>an array of bytes</returns>
    public static byte?[] ConvertStringArrayToByteArray(string[] inputStrings)
    {
        byte?[] IpByteArray = new byte?[4];
        for (int i = 0; i < inputStrings.Length; i++)
        {
            byte.TryParse(inputStrings[i],out byte currentByte);
            IpByteArray[i] = currentByte;
        }
        return IpByteArray;
    }
    /// <summary>
    /// Converts the CIDR value into a string containing the binary subnet mask equivalent,
    /// separated by a period every 8 bits
    /// </summary>
    /// <param name="byteCIDR">the value of the CIDR to determine subnet mask</param>
    /// <returns>a string containing the corresponding subnet mask in bits</returns>
    public static string ConvertCidrToSubnetMaskBitString(byte byteCIDR)
    {
        StringBuilder sb = new();
        for (byte i = 0; i < byteCIDR; i++)
        {
            sb.Append(1);
        }

        for (int i = sb.Length; i < 32; i++)
        {
            sb.Append(0);
        }

        sb.Insert(8, ".");
        sb.Insert(17, ".");
        sb.Insert(26, ".");
        return sb.ToString();
    }

    /// <summary>
    /// Converts a byte into a string of 8 bits
    /// </summary>
    /// <param name="part">a byte to convert</param>
    /// <returns>a string containing 8 bits starting from MSB to LSB</returns>
    public static string ConvertByteToBitString(byte part)
    {
        BitArray bitArray = ConvertByteToBitArray(part);
        StringBuilder sb = new();
        for (int i = bitArray.Count - 1; i >= 0; i--)
        {
            char c = bitArray[i] ? '1' : '0';
            sb.Append(c);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Converts a string containing 8 bits into a byte
    /// </summary>
    /// <param name="bitString">a string containing 8 bits starting from MSB to LSB</param>
    /// <returns>the corresponding byte value</returns>
    public static byte ConvertBitStringToByte(string bitString)
    {
        string removedBinaryValues = bitString.Replace("1", "").Replace("0","");
        if (bitString.Length != 8 || removedBinaryValues.Length>0) 
            throw new ArgumentException(InvalidBitStringErrorText);
        double result = 0;
        for (int i = 1; i <= bitString.Length; i++)
        {
            if (bitString[^i] == '1') result += Math.Pow(2, i - 1);
        }

        return (byte)result;
    }

    /// <summary>
    /// Convert a  dotted decimal string into a 32 bit Bit-string equivalent separated by a period every 8 bits.
    /// </summary>
    /// <param name="dottedDecimalString">a string containing 4 bytes separated by a period</param>
    /// <returns>A bit string containing 32 bits separated by a period every 8 bits</returns>
    public static string ConvertDottedDecimalToIpBitString(string dottedDecimalString)
    {
        string[] byteStrings = dottedDecimalString.Split('.');
        if (byteStrings.Length != 4) throw new ArgumentException(InvalidDDErrorText);
        StringBuilder sb = new();
        for (int i = 0; i < byteStrings.Length; i++)
        {
            bool isByte = byte.TryParse(byteStrings[i],out byte currentByte);
            if(!isByte) throw new ArgumentException(InvalidDDErrorText);
            sb.Append(ConvertByteToBitString(currentByte));
            if (i < byteStrings.Length - 1) sb.Append('.');
        }

        return sb.ToString();
    }

    /// <summary>
    /// Convert a 32 bit Bit-string separated by a period every 8 bits, into a dotted decimal equivalent.
    /// </summary>
    /// <param name="bitString">A string containing 32 bits separated by a period every 8 bits</param>
    /// <returns>a dotted decimal string</returns>
    public static string ConvertIpBitStringToDottedDecimal(string bitString)
    {
        string[] byteStrings = bitString.Split('.');
        if (byteStrings.Length != 4) throw new ArgumentException(InvalidIpBitStringErrorText);
        StringBuilder sb = new();
        for (int i = 0; i < byteStrings.Length; i++)
        {
            sb.Append(ConvertBitStringToByte(byteStrings[i]));
            if (i < byteStrings.Length - 1) sb.Append('.');
        }

        return sb.ToString();
    }
}