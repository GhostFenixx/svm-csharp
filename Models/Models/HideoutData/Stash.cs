namespace Greed.Models.HideoutData
{
    public class Stash : Clamper
    {
        public int StashTUE  { get; set => SetClamped(ref field, value, 4, 380); } = 72;
        public int StashLvl4 { get; set => SetClamped(ref field, value, 4, 380);} = 68;
        public int StashLvl3 { get; set => SetClamped(ref field, value, 4, 380); } = 50;
        public int StashLvl2 { get; set => SetClamped(ref field, value, 4, 380); } = 40;
        public int StashLvl1 { get; set => SetClamped(ref field, value, 4, 380); } = 30;
    }
}
