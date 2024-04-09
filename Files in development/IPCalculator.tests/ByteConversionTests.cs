using System.Collections;
using System.Diagnostics;

namespace IpCalculator.tests;

public class ByteConversionTests
{
    [Fact]
    public void GetBitArray_WithValidInput_ReturnsCorrectBitArray()
    {
        //Arrange
        byte input = 170;
        BitArray expectedOutput = new BitArray(new [] {false, true, false, true, false, true, false, true,});
        //Act
        BitArray result = core.ByteConversion.ConvertByteToBitArray(input);
        //Assert
        Assert.Equal(expectedOutput,result);
    }
    [Fact]
    public void ConvertCIDRToSubnetMaskBitString_WithValidInput_ReturnsCorrectValue()
    {
        //Arrange
        byte input = 24; 
        string expectedResult = "11111111.11111111.11111111.00000000";
        //Act
        string result = core.ByteConversion.ConvertCidrToSubnetMaskBitString(input);
        //Assert
        Assert.Equal(expectedResult,result);
    }
    [Fact]
    public void ConvertByteToBitString_WithValidInput_ReturnsCorrectValue()
    {
        //Arrange
        byte input = 253;
        string expectedOutput = "11111101";
        //Act
        string result =core.ByteConversion.ConvertByteToBitString(input);
        //Assert
        Assert.Equal(expectedOutput,result);
    }
    [Fact]
    public void ConvertBitStringToByte_WithValidInput_ReturnsCorrectValue()
    {
        //Arrange
        string input = "11111101";
        byte expectedOutput = 253;
        //Act
        byte result =core.ByteConversion.ConvertBitStringToByte(input);
        //Assert
        Assert.Equal(expectedOutput,result);
    }
    [Fact]
    public void ConvertBitStringToByte_WithTooLongString_ThrowsArgumentException()
    {
        //Arrange
        string input = "111111010"; //length=9
        //Assert on Act
        Assert.Throws<ArgumentException>(()=> core.ByteConversion.ConvertBitStringToByte(input));
    }
    [Theory]
    [InlineData("11121101")]
    [InlineData("111A1101")]
    [InlineData("/*=--]'.")]
    public void ConvertBitStringToByte_WithStringNotBinary_ThrowsArgumentException(string invalidInput)
    {
        //Arrange
        string input = invalidInput;
        //Assert on Act
        Assert.Throws<ArgumentException>(()=> core.ByteConversion.ConvertBitStringToByte(input));
    }
    [Fact]
    public void ConvertDottedDecimalToIpBitString_WithValidInput_ReturnsCorrectValue()
    {
        //Arrange
        string input = "192.168.25.200";
        string expectedOutput = "11000000.10101000.00011001.11001000";
        //Act
        string result = core.ByteConversion.ConvertDottedDecimalToIpBitString(input);
        //Assert
        Assert.Equal(expectedOutput,result);
    }
    [Theory]
    [InlineData("192.168.25.2000")]
    [InlineData("192.168.2.5.200")]
    [InlineData("192.168.A.200")]
    [InlineData("192.168.256.200")]
    public void ConvertDottedDecimalToIpBitString_WithInvalidInput_ThrowsArgumentException(string invalidInput)
    {
        //Arrange
        string input = invalidInput;
        //Assert on Act
        Assert.Throws<ArgumentException>(()=>core.ByteConversion.ConvertDottedDecimalToIpBitString(input));
    }
    [Fact]
    public void ConvertIpBitStringToDottedDecimal_WithValidInput_ReturnsCorrectValue()
    {
        //Arrange
        string input = "11000000.10101000.00011001.11001000";
        string expectedOutput = "192.168.25.200";
        //Act
        string result = core.ByteConversion.ConvertIpBitStringToDottedDecimal(input);
        //Assert
        Assert.Equal(expectedOutput,result);
    }
    [Theory]
    [InlineData("11000000.10101000.00011001.11001000.11100010")]
    [InlineData("11000000.10101000.00011001")]
    [InlineData("11000000.1010A000.00011001.11001000")]
    [InlineData("11000000.10201000.00011001.11001000")]
    public void ConvertIPBitStringToDottedDecimal_WithInvalidInput_ThrowsArgumentException(string invalidInput)
    {
        //Arrange
        string input = invalidInput;
        //Assert on Act
        Assert.Throws<ArgumentException>(()=>core.ByteConversion.ConvertIpBitStringToDottedDecimal(input));
    }
}