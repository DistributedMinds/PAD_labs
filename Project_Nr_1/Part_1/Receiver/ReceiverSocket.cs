using System.Text;
using System.Net.Sockets;
using System.Net;
using Message_Agent.Common;
using System.Collections.Generic;

namespace Receiver
{
    class ReceiverSocket
    {
        private Socket _socket;
        private List<string> _topics;

        public ReceiverSocket()
        {
            _topics = new List<string>();

            _socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(
                new IPEndPoint(IPAddress.Parse(ipAddress), port),
                ConnectedCallback,
                null
            );

            Console.WriteLine("Waiting for a connection");
        }

        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Receiver connected to broker");
                StartReceive();
            }
            else
            {
                Console.WriteLine("Error: Receiver could not connect to broker");
            }
        }

        public void Subscribe(string topic)
        {
            if (_topics.Contains(topic))
            {
                Console.WriteLine($"Already subscribed to: {topic}");
                return;
            }

            _topics.Add(topic);

            var data = Encoding.UTF8.GetBytes("subscribe#" + topic);
            Send(data);

            Console.WriteLine($"Subscribed to: {topic}");
        }

        public List<string> GetTopics()
        {
            return _topics;
        }

        private void StartReceive()
        {
            ConnectionInfo connection = new ConnectionInfo();
            connection.Socket = _socket;

            _socket.BeginReceive(
                connection.Data,
                0,
                connection.Data.Length,
                SocketFlags.None,
                ReceiveCallBack,
                connection
            );
        }

        private void ReceiveCallBack(IAsyncResult asyncResult)
        {
            ConnectionInfo connectionInfo =
                asyncResult.AsyncState as ConnectionInfo;

            try
            {
                SocketError response;

                int buffSize =
                    _socket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success && buffSize > 0)
                {
                    byte[] payloadBytes = new byte[buffSize];

                    Array.Copy(
                        connectionInfo.Data,
                        payloadBytes,
                        payloadBytes.Length
                    );

                    PayloadHandler.Handle(payloadBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"Can't receive data from broker. {e.Message}"
                );
            }
            finally
            {
                try
                {
                    connectionInfo.Socket.BeginReceive(
                        connectionInfo.Data,
                        0,
                        connectionInfo.Data.Length,
                        SocketFlags.None,
                        ReceiveCallBack,
                        connectionInfo
                    );
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    connectionInfo.Socket.Close();
                }
            }
        }

        private void Send(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"Could not send data: {e.Message}"
                );
            }
        }
    }
}