using System;
using Xamarin.Forms;
using Android.Bluetooth;
using System.Text;

namespace InternetBluetoothESP32.Views
{
    public partial class MainPage : ContentPage
    {
        // Adaptador Bluetooth
        BluetoothAdapter adaptador;
        // Socket Bluetooth
        BluetoothSocket socket;
        // Dispositivo Bluetooth
        BluetoothDevice dispositivo;

        public MainPage()
        {
            InitializeComponent();
            // Inicializar el adaptador Bluetooth
            adaptador = BluetoothAdapter.DefaultAdapter;
        }

        // Evento que se dispara cuando se hace clic en el botón de conectar
        private async void OnConectarButtonClicked(object sender, EventArgs e)
        {
            if (adaptador == null || !adaptador.IsEnabled)
            {
                await DisplayAlert("Error", "Bluetooth no está habilitado en este dispositivo.", "OK");
                return;
            }

            // Dirección MAC del ESP32
            dispositivo = adaptador.GetRemoteDevice("XX:XX:XX:XX:XX:XX");
            socket = dispositivo.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
            await socket.ConnectAsync();

            await DisplayAlert("Conectado", "Conectado a ESP32", "OK");
        }

        // Evento que se dispara cuando se hace clic en el botón de enviar datos
        private async void OnEnviarButtonClicked(object sender, EventArgs e)
        {
            if (socket != null && socket.IsConnected)
            {
                byte[] buffer = Encoding.ASCII.GetBytes("Mensaje de prueba");
                await socket.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                await DisplayAlert("Enviado", "Mensaje enviado a ESP32", "OK");
            }
        }
    }
}
