using System.Text;

namespace IpCalculator.core;

public class StringBuilderIpFormatting
{
    
    public static StringBuilder FormatStringBuilderToIpFormat(StringBuilder sb, int amountOfPeriods, int indexOfLastPeriod)
    {
        if (sb.Length == 0)
            return sb;
        sb = AdjustStringBuilder(sb, amountOfPeriods, indexOfLastPeriod);
        return sb;
    }
    private static StringBuilder AdjustStringBuilder(StringBuilder sb, int amountOfPeriods, int indexOfLastPeriod)
    {
        string stringSinceLastPeriod = GetStringSinceLastPeriod(sb, amountOfPeriods, indexOfLastPeriod);
        if (stringSinceLastPeriod.Length == 0) return sb;
        InsertPeriods(sb, amountOfPeriods, indexOfLastPeriod, stringSinceLastPeriod);
        if (amountOfPeriods > 2 && stringSinceLastPeriod.Length > 3) 
            sb.Length -= 2;
        return sb;
    }
    private static string GetStringSinceLastPeriod(StringBuilder sb, int amountOfPeriods, int indexOfLastPeriod)
    {
        string sbWithoutPeriod;
        if (amountOfPeriods == 0)
            sbWithoutPeriod = sb.ToString();
        else
            sbWithoutPeriod = GetSubStringFromIndex(sb, indexOfLastPeriod+1);
        return sbWithoutPeriod;
    }
    static string GetSubStringFromIndex(StringBuilder sb, int index)
    {
        string str = sb.ToString();
        return str.Substring(index);
    }
    private static void InsertPeriods(StringBuilder sb, int amountOfPeriods, int indexOfLastPeriod, string stringWithoutPeriod)
    {
        if (stringWithoutPeriod.Length <= 3 || stringWithoutPeriod[3] == '.') return;
        if (amountOfPeriods == 0)
            sb.Insert(indexOfLastPeriod + 3, '.');
        else 
            sb.Insert(indexOfLastPeriod + 4, '.');
    }
    public static (int,int) GetPeriodInfo(StringBuilder sb)
    {
        int amountOfPeriods = 0;
        int indexOfLastPeriod = 0;
        for (int i = 0; i < sb.Length; i++)
        {
            if (sb[i] == '.')
            {
                amountOfPeriods++;
                indexOfLastPeriod = i;
            }
        }
        return (amountOfPeriods,indexOfLastPeriod);
    }
}