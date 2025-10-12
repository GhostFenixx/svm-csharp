namespace Greed.Models.CaseSpaceManager
{
    public class CSM
    {
        public bool EnableCases { get; set; }
        public bool EnableSecureCases { get; set; }
        public bool CustomPocket { get; set; }
        public Pockets Pockets { get; set; }
        public Cases Cases { get; set; }
        public SecureContainers SecureContainers { get; set; }
        public bool EnableCSM { get; set; }

        public CSM()
        {
            Pockets = new Pockets();
            SecureContainers = new SecureContainers();
            Cases = new Cases();
        }
    }
}
