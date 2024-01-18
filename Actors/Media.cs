using CsvHelper.Configuration.Attributes;

namespace TP2_TSV.Actors
{
    public class Media : IActor
    {
        [Index(0)]
        public int Id { get; set; }
        [Index(1)]
        public string Nom { get; set; }
        [Index(2)]
        public string TypeLibelle { get; set; }
        [Index(3)]
        public string TypeCode { get; set; }
        [Index(4)]
        public string RangChallenge { get; set; }
        [Index(5)]
        public string MilliardaireFortune { get; set; }
        [Index(6)]
        public string MediaLibelle { get; set; }
        [Index(8)]
        public string MediaPeriodicite { get; set; }


        // To string
        public override string ToString()
        {
            return $"Media: {Id} {Nom} {TypeLibelle} {TypeCode} {RangChallenge} {MilliardaireFortune} {MediaLibelle} {MediaPeriodicite}";
        }
    }
}
