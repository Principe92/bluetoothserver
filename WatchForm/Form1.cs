using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;
using Timer = System.Threading.Timer;

namespace WatchForm
{
    public partial class Form1 : Form
    {
        private const string Uuid = "{7A51FDC2-FDDF-4c9b-AFFC-98BCD91BF93B}";
        private readonly string _myPin;
        private BluetoothListener _blueListener;
        private BluetoothClient _client;
        private bool _deflating;
        private int _diastolic;
        private int _diastolicValue;
        private bool _done;
        private bool _inflating;
        private Stream _mStream;
        private int _pressure;
        private bool _serverStarted;
        private Thread _serverThread;
        private int _systolic;
        private int _systolicValue;
        private Timer _timer;

        public Form1()
        {
            InitializeComponent();
            _serverStarted = false;
            _myPin = "123ABCD";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var br = BluetoothRadio.PrimaryRadio;
            if (br == null)
            {
                MessageBox.Show(@"No supported Bluetooth radio/stack found.");
            }
            else if (br.Mode != RadioMode.Discoverable)
            {
                var rslt = MessageBox.Show(@"Make BluetoothRadio Discoverable?", @"Bluetooth Remote Listener",
                    MessageBoxButtons.YesNo);
                if (rslt == DialogResult.Yes)
                    br.Mode = RadioMode.Discoverable;
            }

            status.ScrollBars = ScrollBars.Vertical;
            _blueListener = new BluetoothListener(new Guid(Uuid)) {ServiceName = "Health Watch", Authenticate = false};
        }

        private void UpdatePressureValue(object state)
        {
            //while (true)
            //{
            //var mStream = (Stream) state;

            if (_mStream != null && _mStream.CanWrite)
            {
                if (_inflating)
                {
                    if (_pressure < 220)
                    {
                        _pressure += 5;
                        var sent = _pressure.ToBytes();
                        _mStream.Write(sent, 0, sent.Length);

                        UpdateUi($"Pressure: {_pressure} mmHg");
                    }
                    else
                    {
                        _inflating = false;
                        var sent = Phase.Deflating.ToBytes();
                        _mStream.Write(sent, 0, sent.Length);

                        UpdateUi("======== Deflating cuff ============");
                    }
                }
                else
                {
                    if (_pressure > 0)
                    {
                        _pressure -= 5;
                        var sent = _pressure.ToBytes();
                        _mStream.Write(sent, 0, sent.Length);

                        UpdateUi($"Pressure: {_pressure} mmHg");
                    }
                    else
                    {
                        // send done signal
                        var sent = Phase.Systolic.ToBytes();
                        _mStream.Write(sent, 0, sent.Length);
                        UpdateUi("===== About to transmit systolic =======");

                        // send systolic
                        sent = _systolicValue.ToBytes();
                        _mStream.Write(sent, 0, sent.Length);
                        UpdateUi($"Systolic: {_systolicValue}");

                        // send done signal
                        sent = Phase.Diastolic.ToBytes();
                        _mStream.Write(sent, 0, sent.Length);
                        UpdateUi("===== About to transmit diastolic =======");

                        // send diastolic
                        sent = _diastolicValue.ToBytes();
                        _mStream.Write(sent, 0, sent.Length);
                        UpdateUi($"Diastolic: {_diastolicValue}");

                        // send done signal
                        sent = Phase.Done.ToBytes();
                        _mStream.Write(sent, 0, sent.Length);
                        UpdateUi("===== Transmission done =======");

                        //  break;
                        _timer.Dispose();
                        _mStream.Close();
                        _done = true;
                    }
                }
                //}
            }
            else _timer.Dispose();
        }

        private void UpdateUi(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateUi), message);
                return;
            }

            status.AppendText($"\n{message}\n");
        }

        private BluetoothDeviceInfo GetData(BluetoothClient client)
        {
            var devices = client.DiscoverDevices();
            foreach (var device in devices)
                if (device.Connected) return device;

            return null;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void start_Click(object sender, EventArgs e)
        {
            int.TryParse(diastolic.Text, out _diastolicValue);
            int.TryParse(systolic.Text, out _systolicValue);

            if (!_serverStarted)
            {
                _serverStarted = true;

                _blueListener.Start();
                _blueListener.BeginAcceptBluetoothClient(BluetoothListenerAcceptClientCallback, _blueListener);
                status.AppendText("\nStarted server...\n");
                //  PairDevice();
            }
        }

        private void end_Click_1(object sender, EventArgs e)
        {
            if (_serverStarted)
            {
                _serverThread?.Abort();
                _client?.Close();
                _blueListener.Stop();
                _serverStarted = false;

                UpdateUi("Server Stopped");
            }
        }

        private void ConnectThread()
        {
            //var connection = _blueListener.AcceptBluetoothClient();
            //var info = GetData(connection);

            //if (info != null)
            //    UpdateUi($"Connection established to {info.DeviceName}");
            PairDevice();
        }

        private bool PairDevice()
        {
            using (var discoverForm = new SelectBluetoothDeviceDialog())
            {
                if (discoverForm.ShowDialog(this) != DialogResult.OK)
                    return false;

                var deviceInfo = discoverForm.SelectedDevice;

                if (!deviceInfo.Authenticated) // previously paired?
                    if (!BluetoothSecurity.PairRequest(deviceInfo.DeviceAddress, _myPin))
                        return false;

                // device should now be paired with the OS so make a connection to it asynchronously
                var client = new BluetoothClient();
                client.BeginConnect(deviceInfo.DeviceAddress, BluetoothService.SerialPort,
                    BluetoothClientConnectCallback, client);

                return true;
            }
        }

        private void BluetoothClientConnectCallback(IAsyncResult result)
        {
            _client = (BluetoothClient) result.AsyncState;
            _client.EndConnect(result);

            RunTransaction(_client);

            while (!_done)
            {
            }
        }

        private void BluetoothListenerAcceptClientCallback(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                var listener = (BluetoothListener) result.AsyncState;


                // continue listening for other broadcasting devices


                // create a connection to the device that's just been found
                _client = listener.EndAcceptBluetoothClient(result);
                //  _client.Authenticate = true;
                //_blueListener.SetPin(_client.RemoteEndPoint.Address, _myPin);

                listener.BeginAcceptBluetoothClient(BluetoothListenerAcceptClientCallback, listener);


                // the method we're in is already asynchronous and it's already connected to the client (via EndAcceptBluetoothClient) so there's no need to call BeginConnect

                RunTransaction(_client);
            }
        }

        private void RunTransaction(BluetoothClient client)
        {
            if (!client.Connected) return;
            _mStream = client.GetStream();
            try
            {
                while (true)
                {
                    int value;
                    var received = new byte[1024];
                    _mStream.Read(received, 0, received.Length);
                    var success = int.TryParse(Encoding.ASCII.GetString(received), out value);

                    var msg = success ? (Phase) value : Phase.Unknown;

                    if (msg == Phase.Start)
                    {
                        UpdateUi("===== Recieved signal to transmit =======");
                        _inflating = true;
                        break;
                    }
                }

                // Inflating:
                var sent = Phase.Inflating.ToBytes();
                _mStream.Write(sent, 0, sent.Length);
                UpdateUi("===== Informed device we are now Inflating cuff =======");

                _timer = new Timer(UpdatePressureValue, null, 0, 1000);
                // UpdatePressureValue(mStream);
            }
            catch (IOException exception)
            {
                UpdateUi($"Error: {exception.Message}");
            }
        }
    }
}