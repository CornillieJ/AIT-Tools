using IpCalculator.core;

namespace IpCalculator.tests;

public class HostTests
{
    [Fact]
    public void Host_WithValidConstructorInputs_SavesCorrectIpAddress()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        byte?[] expectedOutput = {250,200,150,100};
        Host host = new(octet1, octet2, octet3, octet4);
        //Act
        byte?[] actualOutput = host.IpAddress;
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }

    [Fact]
    public void IsValidIp_WithValidIp_ReturnsTrue()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        //Act
        bool actualOutput = host.IsValidIp();
        //Assert
        Assert.True(actualOutput);
    }
    [Fact]
    public void IsValidIp_WithInvalidIp_ReturnsTrue()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        Host host = new(octet1, octet2, octet3);
        //Act
        bool actualOutput = host.IsValidIp();
        //Assert
        Assert.False(actualOutput);
    }
    [Fact]
    public void GetIpBitString_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetIpBitString());
    }
    [Fact]
    public void GetIpBitString_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 0;
        byte octet4 = 128;
        string expectedOutput = "11111111.11111111.00000000.10000000";
        Host host = new(octet1, octet2, octet3, octet4);
        //Act
        string actualOutput = host.GetIpBitString();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetIpDD_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetIpDD());
    }
    [Fact]
    public void GetIpDD_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        string expectedOutput = "250.200.150.100";
        Host host = new(octet1, octet2, octet3, octet4);
        //Act
        string actualOutput = host.GetIpDD();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetSubnetBitString_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetSubnetBitString());
    }
    [Fact]
    public void GetSubnetBitString_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 22;
        string expectedOutput = "11111111.11111111.11111100.00000000";
        //Act
        string actualOutput = host.GetSubnetBitString();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetSubnetDD_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetSubnetDD());
    }
    [Fact]
    public void GetSubnetDD_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 22;
        string expectedOutput = "255.255.252.0";
        //Act
        string actualOutput = host.GetSubnetDD();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetNetworkBitString_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        host.Cidr = 24;
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetNetworkBitString());
    }
    [Fact]
    public void GetNetworkBitString_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetNetworkBitString());
    }
    [Fact]
    public void GetNetworkBitString_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        string expectedOutput = "11111010.11001000.10010000.00000000";
        //Act
        string actualOutput = host.GetNetworkBitString();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetNetworkDD_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        host.Cidr = 24;
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetNetworkDD());
    }
    [Fact]
    public void GetNetworkDD_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetNetworkDD());
    }
    [Fact]
    public void GetNetworkDD_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        string expectedOutput = "250.200.144.0";
        //Act
        string actualOutput = host.GetNetworkDD();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetFirstHostDD_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        host.Cidr = 24;
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetFirstHostDD());
    }
    [Fact]
    public void GetFirstHostDD_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetFirstHostDD());
    }
    [Fact]
    public void GetFirstHostDD_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        string expectedOutput = "250.200.144.1";
        //Act
        string actualOutput = host.GetFirstHostDD();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetFirstHostBitString_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        host.Cidr = 24;
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetFirstHostBitString());
    }
    [Fact]
    public void GetFirstHostBitString_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetFirstHostBitString());
    }
    [Fact]
    public void GetFirstHostBitString_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        string expectedOutput = "11111010.11001000.10010000.00000001";
        //Act
        string actualOutput = host.GetFirstHostBitString();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetLastHostDD_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        host.Cidr = 24;
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetLastHostDD());
    }
    [Fact]
    public void GetLastHostDD_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetLastHostDD());
    }
    [Fact]
    public void GetLastHostDD_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        string expectedOutput = "250.200.151.254";
        //Act
        string actualOutput = host.GetLastHostDD();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetLastHostBitString_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        host.Cidr = 24;
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetLastHostBitString());
    }
    [Fact]
    public void GetLastHostBitString_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetLastHostBitString());
    }
    [Fact]
    public void GetLastHostBitString_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        string expectedOutput = "11111010.11001000.10010111.11111110";
        //Act
        string actualOutput = host.GetLastHostBitString();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    
    [Fact]
    public void GetBroadcastDD_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        host.Cidr = 24;
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetBroadcastDD());
    }
    [Fact]
    public void GetBroadcastDD_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetBroadcastDD());
    }
    [Fact]
    public void GetBroadcastDD_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        string expectedOutput = "250.200.151.255";
        //Act
        string actualOutput = host.GetBroadcastDD();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetBroadcastBitString_WithInvalidIp_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        Host host = new(octet1, octet2, octet3);
        host.Cidr = 24;
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetBroadcastBitString());
    }
    [Fact]
    public void GetBroadcastBitString_WithInvalidCidr_throwsInvalidOperationException()
    {
        //Arrange
        byte octet1 = 255;
        byte octet2 = 255;
        byte octet3 = 255;
        byte octet4 = 255;
        Host host = new(octet1, octet2, octet3,octet4);
        //Assert on Act
        Assert.Throws<InvalidOperationException>(() => host.GetBroadcastBitString());
    }
    [Fact]
    public void GetBroadcastBitString_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        string expectedOutput = "11111010.11001000.10010111.11111111";
        //Act
        string actualOutput = host.GetBroadcastBitString();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }
    [Fact]
    public void GetAmountOfHostsPossible_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        int expectedOutput = 2046;
        //Act
        int actualOutput = host.GetAmountOfHostsPossible();
        //Assert
        Assert.Equal(expectedOutput,actualOutput);
    }   
    [Fact]
    public void ResetIpAndCidr_WithValidInput_ReturnsCorrectString()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3, octet4);
        host.Cidr = 21;
        byte?[] expectedIpOutput = new byte?[4];
        int expectedCidrOutput = 0;
        //Act
        host.ResetIpAndCidr();
        byte?[] actualIpOutput = host.IpAddress;
        int actualCidrOutput = host.Cidr;
        //Assert
        Assert.Equal(expectedIpOutput,actualIpOutput);
        Assert.Equal(expectedCidrOutput,actualCidrOutput);
    }

    [Fact]
    public void WriteArrayToHostIp_WithValidInputs_WritesCorrectArray()
    {
        //Arrange
        byte?[] ipAddress = {192,168,0,1 };
        Host host = new Host("Host1");
        byte?[] expectedIpOutput = ipAddress;
        host.WriteArrayToHostIp(ipAddress);
        //Act
        byte?[] actualIpOutput = host.IpAddress;
        //Assert
        Assert.Equal(expectedIpOutput,actualIpOutput);
    }

    [Fact]
    public void UpdateIpAddressAndResetIfFirstOctet_WithValidInputs_UpdatesCorrectly()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        Host host = new(octet1, octet2, octet3);
        byte byteToFill = 100;
        OctetNumber octetToFIl = OctetNumber.Octet4;
        byte?[] expectedIp = { 250, 200, 150, 100 };
        //Act
        host.UpdateIpAddressAndResetIfFirstOctet(octetToFIl,byteToFill);
        //Assert
        Assert.Equal(expectedIp,host.IpAddress);
    }
    [Fact]
    public void UpdateIpAddressAndResetIfFirstOctet_WithUpdatingFirstOctet_ResetsOtherOctets()
    {
        //Arrange
        byte octet1 = 250;
        byte octet2 = 200;
        byte octet3 = 150;
        byte octet4 = 100;
        Host host = new(octet1, octet2, octet3,octet4);
        byte byteToFill = 255;
        OctetNumber octetToFIl = OctetNumber.Octet1;
        byte?[] expectedIp = { 255, null, null, null };
        //Act
        host.UpdateIpAddressAndResetIfFirstOctet(octetToFIl,byteToFill);
        //Assert
        Assert.Equal(expectedIp,host.IpAddress);
    }
}