using System.Text;
using Message_Agent.Common;
using Newtonsoft.Json;
using Sender;

Console.WriteLine("Sender");

var senderSocket = new SenderSocket();
senderSocket.Connect(Settings.BROCKER_IP, Settings.BROCKER_PORT);

if (senderSocket.IsConected)
{
    while (true)
    {
        var payLoad = new PayLoad();
        
        Console.WriteLine("Enter topic:");
        payLoad.Topic = Console.ReadLine().ToLower();
        Console.WriteLine("Enter message:");
        payLoad.Message = Console.ReadLine();
        
        var payLoadString = JsonConvert.SerializeObject(payLoad);
        byte[] data = Encoding.UTF8.GetBytes(payLoadString);
        
        senderSocket.Send(data);
    }
}

Console.ReadLine();
