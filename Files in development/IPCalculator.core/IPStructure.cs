namespace IpCalculator.core;

public static class IpStructure
{
    #region Fields and properties
    public static Dictionary<byte, List<byte>?> IpFirstHalf { get; } = new()
    {
        { 10, GetByteList(0, 255) },
        { 172, GetByteList(16, 31) },
        { 192, new List<byte>(){168} }
    };
    public static List<byte> IpThird { get; } =GetByteList(0, 255);
    public static List<byte> IpLast { get; } = GetByteList(1, 254);
    public static Dictionary<byte, List<byte>?> CidrDictionary { get; } = new()
    {
        { 10, GetByteList(8, 30) },
        { 172, GetByteList(12, 30) },
        { 192, GetByteList(16, 30) }
    };

    #endregion Fields and properties
    #region Methods
    /// <summary>
    /// Returns a list of bytes starting from the first parameter and ending in the second parameter (both included)
    /// </summary>
    /// <param name="first">First number of the list</param>
    /// <param name="last">Last number of the list</param>
    /// <returns>A List of bytes within the range</returns>
    private static List<byte> GetByteList(int first, int last)
    {
        last = last > 255 ? 255 : last;
        first = first < 0 ? 0 : first;
        List<byte> bytes = new();
        for (int i = first; i <= last; i++)
        {
            bytes.Add((byte)i);
        }

        return bytes;
    }
    #endregion Methods
}