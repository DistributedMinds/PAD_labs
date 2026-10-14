namespace Broker_gRPC.Models
{
    public class Connection
    {
        public string Address { get; set; }
        public List<string> Topics { get; set; } = new List<string>();

        public Connection(string address)
        {
            Address = address;
        }
    }
}
