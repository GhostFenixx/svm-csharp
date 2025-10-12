namespace Greed.Models.CaseSpaceManager
{
    public class SecureContainers
    {
        public Case Alpha { get; set; }
        public Case Beta { get; set; }
        public Case Epsilon { get; set; }
        public Case Gamma { get; set; }
        public Case GammaTUE { get; set; }
        public Case Kappa { get; set; }
        public Case DesecratedKappa { get; set; }
        public Case Dev { get; set; }
        public Case WaistPouch { get; set; }
        public SecureContainers()
        {
            Alpha = new Case()
            {
                Height = 2,
                Width = 2,
            };
            Beta = new Case()
            {
                Height = 2,
                Width = 3,
            };
            Epsilon = new Case()
            {
                Height = 2,
                Width = 4,
            };
            Gamma = new Case()
            {
                Height = 3,
                Width = 3,
            };
            GammaTUE = new Case()
            {
                Height = 3,
                Width = 3,
            };
            Kappa = new Case()
            {
                Height = 4,
                Width = 3,
            };
            DesecratedKappa = new Case()
            {
                Height = 4,
                Width = 3,
            };
            Dev = new Case()
            {
                Height = 3,
                Width = 3,
            };
            WaistPouch = new Case()
            {
                Height = 2,
                Width = 2,
            };
        }
    }
}

