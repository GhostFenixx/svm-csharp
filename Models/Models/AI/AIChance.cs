namespace Greed.Models.AI
{
    public class AIChance : Clamper //Hmm...I'm starting to think it would be smarter to set 'chance' as one field and then just inherit the clamp under interface, shouldn't be much of a difference performance wise?
    {
        public int KolontayStreets { get; set => SetClamped(ref field, value, 0, 100); } = 30;
        public int KolontayGZ { get; set => SetClamped(ref field, value, 0, 100); } = 0;
        public bool ForcePartisan { get; set; }
        public bool DisableWeeklyBoss { get; set; }
        public int PartisanCustoms { get; set => SetClamped(ref field, value, 0, 100); } = 15;
        public int PartisanShoreline { get; set => SetClamped(ref field, value, 0, 100); } = 30;
        public int PartisanWoods { get; set => SetClamped(ref field, value, 0, 100); } = 30;
        public int PartisanLighthouse { get; set => SetClamped(ref field, value, 0, 100); } = 30;
        public int Kaban { get; set => SetClamped(ref field, value, 0, 100); } = 50;
        public int TagillaNight { get; set => SetClamped(ref field, value, 0, 100); } = 30;
        public int TrioLighthouse { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int TrioShoreline { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int TrioWoods { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int Zryachiy { get; set => SetClamped(ref field, value, 0, 100); } = 100;
        public int CultistCustoms { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int CultistShoreline { get; set => SetClamped(ref field, value, 0, 100); } = 15;
        public int Trio { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int RaiderLab { get; set => SetClamped(ref field, value, 0, 100); } = 45;
        public int RaiderReserve { get; set => SetClamped(ref field, value, 0, 100); } = 35;
        public int CultistFactory { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int CultistWoods { get; set => SetClamped(ref field, value, 0, 100); } = 20;
        public int CultistGroundZero { get; set => SetClamped(ref field, value, 0, 100); } = 44;
        public int Rogue { get; set => SetClamped(ref field, value, 0, 100); } = 70;
        public int Tagilla { get; set => SetClamped(ref field, value, 0, 100); } = 30;
        public int Shturman { get; set => SetClamped(ref field, value, 0, 100); } = 50;
        public int Glukhar { get; set => SetClamped(ref field, value, 0, 100); } = 50;
        public int Sanitar { get; set => SetClamped(ref field, value, 0, 100); } = 30;
        public int Reshala { get; set => SetClamped(ref field, value, 0, 100); } = 39;
        public int Killa { get; set => SetClamped(ref field, value, 0, 100); } = 49;
    }
}
