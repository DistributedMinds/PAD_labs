namespace Common.Models;

public class Song : MongoDocument
{
        public string Name { get; set; }
        public List<String> Artists { get; set; }
        public decimal Budget { get; set; }
        public string Description { get; set; }
}