using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvHelper.Configuration.Attributes;

namespace TP2_TSV.Actors
{
    public interface IActor
    {
        [Index(0)]
        public int Id { get; set; }
        [Index(1)]
        public string Nom { get; set; }
        [Index(2)]
        public string TypeLibelle { get; set; }
    }
}
