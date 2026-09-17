using Message_Agent.Common;
using System;
using System.Net;
using System.Net.Sockets;

namespace Broker
{
    class BrokerSocket
    {
        private Socket _socket;

        private const int CONNECTIONS_LIMIT = 8;

        public BrokerSocket()
        {
            _socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Start(string ip, int port)
        {
            _socket.Bind(
                new IPEndPoint(IPAddress.Parse(ip), port)
            );

            _socket.Listen(CONNECTIONS_LIMIT);

            Accept();
        }

        private void Accept()
        {
            _socket.BeginAccept(
                AcceptedCallback,
                null
            );
        }

        private void AcceptedCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = new ConnectionInfo();

            try
            {
                connection.Socket = _socket.EndAccept(asyncResult);

                connection.Address =
                    connection.Socket.RemoteEndPoint?.ToString();

                Logger.Info(
                    $"Client connected: {connection.Address}"
                );

                connection.Socket.BeginReceive(
                    connection.Data,
                    0,
                    connection.Data.Length,
                    SocketFlags.None,
                    ReceiveCallback,
                    connection
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"Can't accept. {e.Message}"
                );

                Logger.Error(
                    $"Can't accept connection: {e.Message}"
                );
            }
            finally
            {
                Accept();
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection =
                asyncResult.AsyncState as ConnectionInfo;

            try
            {
                Socket senderSocket = connection.Socket;

                SocketError response;

                int buffSize =
                    senderSocket.EndReceive(
                        asyncResult,
                        out response
                    );

                if (response == SocketError.Success && buffSize > 0)
                {
                    byte[] payload = new byte[buffSize];

                    Array.Copy(
                        connection.Data,
                        payload,
                        payload.Length
                    );

                    Logger.Info(
                        $"Data received from {connection.Address}. " +
                        $"Size: {buffSize} bytes."
                    );

                    PayloadHandler.Handle(
                        payload,
                        connection
                    );

                    senderSocket.BeginReceive(
                        connection.Data,
                        0,
                        connection.Data.Length,
                        SocketFlags.None,
                        ReceiveCallback,
                        connection
                    );
                }
                else
                {
                    CloseConnection(connection);
                }
            }
            catch (Exception e)
            {
                Logger.Warning(
                    $"Client disconnected: {connection.Address}. " +
                    $"Reason: {e.Message}"
                );

                CloseConnection(connection);
            }
        }

        private void CloseConnection(ConnectionInfo connection)
        {
            try
            {
                if (connection != null)
                {
                    if (!string.IsNullOrEmpty(connection.Address))
                    {
                        ConnectionsStorage.Remove(
                            connection.Address
                        );
                    }

                    connection.Socket?.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(
                    $"Error closing connection: {ex.Message}"
                );
            }
        }
    }
}