using System.IO.Ports;
using NModbus;
using NModbus.Serial;

using var port = new SerialPort("/dev/ttyUSB0");
port.BaudRate = 9600;
port.Parity = Parity.None;
port.DataBits = 8;
port.StopBits = StopBits.One;
port.ReadTimeout = 10000;
port.WriteTimeout = 1000;
port.Open();

var master = new ModbusFactory().CreateRtuMaster(port);

var result = await master.ReadHoldingRegistersAsync(
    slaveAddress: 1,
    startAddress: 0,
    numberOfPoints: 8);

if (result[0] == 0x7FFF || result[1] == 0x7FFF)
{
    Console.WriteLine("Sensor is abnormal (invalid reading)");
    return;
}

var temperatureRaw = unchecked((short)result[0]);
var relativeHumidityRaw = unchecked((short)result[1]);
var slaveAddress = result[4];
            
var temperature = temperatureRaw * 0.1;
var relativeHumidity = relativeHumidityRaw * 0.1;

Console.WriteLine($"Slave address: {slaveAddress}");
Console.WriteLine($"Temperature: {temperature:0.0}°C");
Console.WriteLine($"Relative Humidity: {relativeHumidity:0.0}%");
    