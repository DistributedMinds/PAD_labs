using System.Net;
using System.Net.Sockets;

namespace Sender;
public class SenderSocket
{
    private Socket _socket;
    public bool IsConected;
    
    public SenderSocket() => 
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    public void Connect(string ipAddress, int port)
    {
        _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectedCallback, null);
         
        //could be refactored
        Thread.Sleep(2000);
    }

    private void ConnectedCallback(IAsyncResult asyncResult)
    {
        if (_socket.Connected)
        {
            Console.WriteLine("Sender connected to brocker");
            _socket.EndConnect(asyncResult);
        }
        else 
        {
            Console.WriteLine("Error: Sender could not connect to brocker");
        }

        IsConected = _socket.Connected;
    }
    
    public void Send(byte[] data)
    {
        try
        {
            _socket.Send(data);
        }
        catch (Exception e)
        {
           Console.WriteLine($"Could not send data {e.Message}");
        }
       
    }
        
}