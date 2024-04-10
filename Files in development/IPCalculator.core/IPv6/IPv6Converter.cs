using System.Text;
using System.Xml.Serialization;

namespace IpCalculator.core.IPv6;

public static class IPv6Converter
{
    public static string[] GetShortenIPv6Steps(string fullIP)
    {
        
        List<string> results = new();
        string shortIP = ReduceFullZeroesInFullIP(fullIP);
        string shorterIP = ReduceLeadingZeroesInFullIP(shortIP);
        string shortestIP = RemoveDuplicateZeroes(shorterIP);
        if (shortestIP[^1] == ':') shortestIP += ':';
        results.Add(shortIP);
        results.Add(shorterIP);
        results.Add(shortestIP);
        return results.ToArray();
    }

    private static string ReduceLeadingZeroesInFullIP(string shortIP)
    {
        List<string> segments = shortIP.Split(':').ToList();
        for (int i = 0; i < segments.Count; i++)
        {
            if (segments[i].Length == 4) segments[i] = segments[i].TrimStart('0');
        }
        return string.Join(':',segments);
    }
    private static string RemoveDuplicateZeroes(string shortIP)
    {
        List<string> octets = shortIP.Split(':').ToList();
        int bestStartPosition = -1;
        int currentStartPosition = 0;
        int bestZeroLength = 0;
        int currentZeroLength = 0;
        for (int i = 0; i < octets.Count; i++)
        {
            if (octets[i] != "0")
            {
                currentZeroLength = 0;
            }
            else
            {
                currentZeroLength++;
                if (currentZeroLength == 1)
                    currentStartPosition = i;
                if (currentZeroLength <= bestZeroLength) continue;
                bestZeroLength = currentZeroLength;
                bestStartPosition = currentStartPosition;
            }
        }
        if (bestStartPosition == -1) return string.Join(':',octets);
        octets.RemoveRange(bestStartPosition,bestZeroLength);
        octets.Insert(bestStartPosition,"");
        return string.Join(":",octets);
    }

    private static string ReduceFullZeroesInFullIP(string fullIP)
    {
        List<string> segments = fullIP.Split(':').ToList();
        for (int i = 0; i < segments.Count; i++)
        {
            if (segments[i] == "0000") segments[i] = "0";
        }
        return string.Join(':',segments);
    }

    public static string[] GetFullIPv6Steps(string shortIp)
    {
        List<string> steps = new();
        string IPWithAllHextets = AddMissingHextets(shortIp);
        string IPWithAllZeroesAdded = AddMissingZeroes(IPWithAllHextets);
        steps.Add(IPWithAllHextets);
        steps.Add(IPWithAllZeroesAdded);
        return steps.ToArray();
    }

    private static string AddMissingZeroes(string ipWithAllHextets)
    {
        List<string> segments = ipWithAllHextets.Split(':').ToList();
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i] = AddZeroesTillLengthFour(segments[i]);
        }

        return string.Join(':', segments);
    }

    private static string AddZeroesTillLengthFour(string segment)
    {
        while (segment.Length < 4)
        {
            segment = "0" + segment;
        }
        return segment;
    }

    private static string AddMissingHextets(string shortIp)
    {
        List<string> segments = shortIp.Split(':').ToList();
        if (!segments.Contains("")) return shortIp;
        int indexToInsertInto = segments.IndexOf("");
        segments.Remove("");
        int amountOfHextetsToAdd = 8 - segments.Count;
        List<string> hextetsToAdd = new();
        for (int i = 0; i < amountOfHextetsToAdd; i++)
        {
            hextetsToAdd.Add("0000");
        }
        segments.InsertRange(indexToInsertInto,hextetsToAdd);
        return string.Join(':',segments);
    }
    public static string GetSubnetId(int cidr, IEnumerable<string> firstHalf)
    {
        int amountOfRoutingSegments = cidr / 16;
        int amountOfSubnetSegments = 4 - amountOfRoutingSegments-1;
        int amountOfLastBits = cidr % 16;
        if (amountOfLastBits == 0) return string.Join(':', firstHalf.TakeLast(amountOfSubnetSegments));
        IEnumerable<string> fullSegments = firstHalf.TakeLast(amountOfSubnetSegments);
        string firstSegmentAsBits = GetLastSegmentAsBitString(firstHalf, amountOfRoutingSegments);
        string lastBits = GetCorrectBitStringFromFirstSegment(firstSegmentAsBits, amountOfLastBits);
        byte firstByte = ByteConversion.ConvertBitStringToByte(lastBits.Substring(0,8));
        byte lastByte = ByteConversion.ConvertBitStringToByte(lastBits.Substring(8));
        string byteAsHex = Convert.ToHexString(new[] { firstByte, lastByte });
        return byteAsHex + ":" + string.Join(':', fullSegments);
    }
    public static string GetRoutingPrefix(int cidr, IEnumerable<string> firstHalf)
    {
        int amountOfRoutingSegments = cidr / 16;
        int amountOfLastBits = cidr % 16;
        IEnumerable<string> fullSegments = firstHalf.Take(amountOfRoutingSegments);
        if (amountOfLastBits == 0) return string.Join(':', fullSegments);
        string lastSegmentAsBits = GetLastSegmentAsBitString(firstHalf, amountOfRoutingSegments);
        string lastBits = GetCorrectBitStringFromLastSegment(lastSegmentAsBits, amountOfLastBits);
        byte firstByte = ByteConversion.ConvertBitStringToByte(lastBits.Substring(0,8));
        byte lastByte = ByteConversion.ConvertBitStringToByte(lastBits.Substring(8));
        string byteAsHex = Convert.ToHexString(new[] { firstByte, lastByte });
        return string.Join(':', fullSegments) + ":" + byteAsHex;
    }
    private static string GetCorrectBitStringFromLastSegment(string lastSegmentAsBits, int amountOfLastBits)
    {
        string lastBits = lastSegmentAsBits.Substring(0,amountOfLastBits);
        while (lastBits.Length < 16)
        {
            lastBits += "0";
        }

        return lastBits;
    }
    private static string GetCorrectBitStringFromFirstSegment(string lastSegmentAsBits, int amountOfLastBits)
    {
        string lastBits = lastSegmentAsBits.Substring(amountOfLastBits);
        while (lastBits.Length < 16)
        {
            lastBits = "0" + lastBits;
        }

        return lastBits;
    }

    private static string GetLastSegmentAsBitString(IEnumerable<string> firstHalf, int amountOfRoutingSegments)
    {
        string finalSegment = firstHalf.ElementAt(amountOfRoutingSegments);
        byte[] bytes = Convert.FromHexString(finalSegment);
        string lastSegmentAsBitString = "";
        foreach (byte part in bytes)
        {
            string bitString = ByteConversion.ConvertByteToBitString(part);
            lastSegmentAsBitString += bitString;
        }

        return lastSegmentAsBitString;
    }

}