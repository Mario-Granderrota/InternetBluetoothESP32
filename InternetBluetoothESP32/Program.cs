using System;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace InternetBluetoothESP32
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Buscando dispositivos Bluetooth...");

            var client = new BluetoothClient();
            var devices = client.DiscoverDevices();

            foreach (var device in devices)
            {
                Console.WriteLine($"Encontrado: {device.DeviceName} - {device.DeviceAddress}");
            }

            Console.WriteLine("Introduce la dirección del dispositivo para conectar:");
            string address = Console.ReadLine();

            var targetDevice = new BluetoothAddress(Convert.ToUInt64(address, 16));
            var ep = new BluetoothEndPoint(targetDevice, BluetoothService.SerialPort);

            using (var stream = client.Connect(ep))
            {
                Console.WriteLine("Conectado al dispositivo. Escriba un mensaje para enviar:");
                string message = Console.ReadLine();
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);

                await stream.WriteAsync(buffer, 0, buffer.Length);
                Console.WriteLine("Mensaje enviado.");
            }

            Console.WriteLine("Programa finalizado.");
        }
    }
}

