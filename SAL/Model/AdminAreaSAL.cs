
namespace SAL
{
    public class AdminAreaSAL
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string GeonameId { get; set; }

        public CountrySAL Country { get; set; }

        #endregion

        #region Constructor
        public AdminAreaSAL()
        {
        }
        #endregion

    }
}
