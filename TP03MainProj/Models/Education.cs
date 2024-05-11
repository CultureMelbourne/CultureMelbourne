using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP03MainProj.Models
{
    public class Education
    {
    }
    
    // Models for the stream chart
    public class CensusData
    {
        public int CensusYear { get; set; }
        public int TotalPopulation { get; set; }
    }

    public class CountryPopulation
    {
        public string Country { get; set; }
        public List<CensusData> Data { get; set; }
    }

}