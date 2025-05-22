using System.IO.Ports;
using NModbus;
using NModbus.Serial;

using var port = new SerialPort("/dev/ttyUSB0");
port.BaudRate = 9600;
port.Parity = Parity.None;
port.DataBits = 8;
port.StopBits = StopBits.One;
port.ReadTimeout = 1000;
port.WriteTimeout = 1000;
port.Open();

var master = new ModbusFactory().CreateRtuMaster(port);

master.WriteMultipleRegisters(
    slaveAddress: 1,
    startAddress: 100,
    data: [1, 2, 3]);