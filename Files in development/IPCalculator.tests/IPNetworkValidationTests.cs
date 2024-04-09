using IpCalculator.core;

namespace IpCalculator.tests;

public class IpNetworkValidationTests
{
    [Theory]
    [InlineData(192,168,50,100)]
    [InlineData(172,17,210,10)]
    [InlineData(172,31,0,90)]
    [InlineData(10,168,50,100)]
    public void IsIpLocal_WithLocalIp_ReturnsTrue(byte octet1, byte octet2,byte octet3, byte octet4)
    {
        //Arrange
        byte?[] iPAddress = { octet1, octet2, octet3, octet4 };
        //Act
        bool actualOutput = IpNetworkValidation.IsIpLocal(iPAddress);
        //Assert
        Assert.True(actualOutput);
    }
    [Fact]
    public void IsIpLocal_WithPublicIp_ReturnsFalse()
    {
        //Arrange
        byte?[] iPAddress = { 50,100,20,70 };
        //Act
        bool actualOutput = IpNetworkValidation.IsIpLocal(iPAddress);
        //Assert
        Assert.False(actualOutput);
    }
    [Fact]
    public void IsCidrValid_WithValidCidr_ReturnsTrue()
    {
        //Arrange
        byte cidr = 18;
        byte octet1 = 192;
        //Act
        bool actualOutput = IpNetworkValidation.IsCidrValid(cidr, octet1);
        //Assert
        Assert.True(actualOutput);
    }
    [Fact]
    public void IsCidrValid_WithInvalidCidr_ReturnsFalse()
    {
        //Arrange
        byte cidr = 15;
        byte octet1 = 192;
        //Act
        bool actualOutput = IpNetworkValidation.IsCidrValid(cidr, octet1);
        //Assert
        Assert.False(actualOutput);
    }
    [Theory]
    [InlineData(16,192)]
    [InlineData(30,192)]
    [InlineData(12,172)]
    [InlineData(30,172)]
    [InlineData(8,10)]
    [InlineData(30,10)]
    public void IsCidrValid_WithCidrEdgeCases_ReturnsTrue(byte edgeCaseCidr,byte iPOctet1)
    {
        //Arrange
        byte cidr = edgeCaseCidr;
        byte octet1 = iPOctet1;
        //Act
        bool actualOutput = IpNetworkValidation.IsCidrValid(cidr, octet1);
        //Assert
        Assert.True(actualOutput);
    }
    [Fact]
    public void GetCidrIfValidLocalNetwork_WithValidNetwork_ReturnsCIDR()
    {
        //Arrange
        byte cidr = 16;
        bool isValidNetwork = true;
        byte expectedOutput = cidr;
        //Act
        byte actualOutput = IpNetworkValidation.GetCidrOnlyIfValidLocalNetwork(isValidNetwork, cidr);
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetCidrIfValidLocalNetwork_WithInvalidNetwork_ReturnsCIDR()
    {
        //Arrange
        byte cidr = 16;
        bool isValidNetwork = false;
        byte expectedOutput = 0;
        //Act
        byte actualOutput = IpNetworkValidation.GetCidrOnlyIfValidLocalNetwork(isValidNetwork, cidr);
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Theory]
    [InlineData("192.168.0.1")]
    [InlineData("0.0.0.0")]
    [InlineData("255.255.255.255")]
    [InlineData("1.1.1.1")]
    public void IsIpDDValid_WithValidIpDD_ReturnsTrue(string validIpDD)
    {
        //Arrange
        string ipDD = validIpDD;
        //Act
        bool actualOutput = IpNetworkValidation.IsIpDDValid(validIpDD);
        //Assert
        Assert.True(actualOutput);
    }
    [Theory]
    [InlineData("....")]
    [InlineData("192.168.a.1")]
    [InlineData("192.168.0.256")]
    [InlineData("10.5..1")]
    [InlineData("19216801")]
    public void IsIpDDValid_WithInvalidIpDD_ReturnsFalse(string invalidIpDD)
    {
        //Arrange
        string ipDD = invalidIpDD;
        //Act
        bool actualOutput = IpNetworkValidation.IsIpDDValid(invalidIpDD);
        //Assert
        Assert.False(actualOutput);
    }

    [Theory]
    [InlineData("192.168.0.1",HostColors.Green)]
    [InlineData("19.150.2.10",HostColors.Yellow)]
    public void GetIpValidationColor_WithValidIpString_GetsCorrectColor(string validIpString, HostColors hostcolor)
    {
        //Arrange
        string ipString = validIpString;
        int amountOfPeriods = 3;
        HostColors expectedOutput = hostcolor;
        //Act
        HostColors actualOutput = IpNetworkValidation.GetIpValidationColor(amountOfPeriods, ipString);
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetIpValidationColor_WithNotEnoughPeriods_GetsCorrectColor()
    {
        //Arrange
        string ipString = "192.168.0.1";
        int amountOfPeriods = 2;
        HostColors expectedOutput = HostColors.White;
        //Act
        HostColors actualOutput = IpNetworkValidation.GetIpValidationColor(amountOfPeriods, ipString);
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    
    [Fact]
    public void GetIpValidationColor_WithInvalidIpString_GetsCorrectColor()
    {
        //Arrange
        string ipString = "10.2.12.800";
        int amountOfPeriods = 3;
        HostColors expectedOutput = HostColors.Red;
        //Act
        HostColors actualOutput = IpNetworkValidation.GetIpValidationColor(amountOfPeriods, ipString);
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetIpValidationColor_WithIncorrectlyFormattedIpString_GetsCorrectColor()
    {
        //Arrange
        string ipString = "10.a.12.50";
        int amountOfPeriods = 3;
        HostColors expectedOutput = HostColors.Red;
        //Act
        HostColors actualOutput = IpNetworkValidation.GetIpValidationColor(amountOfPeriods, ipString);
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
}