namespace SAL
{
    public class CountrySAL
    {
        public string Name { get; set; }
        public string GeonameId { get; set; }

        public CountrySAL(string name, string geonameId)
        {
            Name = name;
            GeonameId = geonameId;
        }     
    }
}
