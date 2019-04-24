namespace BikeDashboard.Models
{
    public class StationIdentity
    {
        public StationIdentity(string name, string id)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
