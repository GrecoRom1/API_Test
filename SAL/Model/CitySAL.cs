
namespace SAL
{
    public class CitySAL
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public int Population { get; private set; }

        public AdminAreaSAL AdminArea { get; set; }
        
        public CitySAL(string id, string name, double latitude, double longitude, int population)
        {
            Id = id;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Population = population;
        }
    }
}
