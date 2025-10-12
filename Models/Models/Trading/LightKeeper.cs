namespace Greed.Models.Trading
{
    public class LightKeeper
    {
        public int AccessTime { get; set; } = 10;
        public int LeaveTime { get; set; } = 1;
        public bool EnableLightKeeper { get; set; }
    }
}
