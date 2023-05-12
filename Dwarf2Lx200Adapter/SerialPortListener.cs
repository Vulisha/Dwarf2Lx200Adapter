using System;
using System.IO.Ports;

namespace Dwarf2Lx200Adapter
{
    public class SerialPortListener
    {
        private readonly SerialPort _serialPort;
        private readonly TelescopeController _telescopeControl;
        
        public SerialPortListener(string comPortName, TelescopeController telescopeControl)
        {
            _telescopeControl = telescopeControl;

            _serialPort = new SerialPort(comPortName)
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };

            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadExisting();
            if (data.ToString() != "#:GR#" && data.ToString() != "#:GD#")
                Console.WriteLine(data.ToString());

            // Process the data...
            _telescopeControl.HandleCommand(data.Replace("#",string.Empty));
        }

        public async void StartAsync()
        {
            _serialPort.Open();
        }

        public void Stop()
        {
            _serialPort.Close();
        }
    }

}
