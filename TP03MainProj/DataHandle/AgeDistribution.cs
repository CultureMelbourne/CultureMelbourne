using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP03MainProj.DataHandle
{

    public class AgeDistribution
    {

        public int CensusId { get; set; }
        public int CensusYear { get; set; }
        public string CountryName { get; set; }
        public List<int> AgeDistributions { get; set; }


    }




    public class Population {
        public int CensusYear { get; set; }
        public string CountryName { get; set; }
        public int Total_Population { get; set; }
    }

    public class ReligionData
    {
        public string Religion_Name { get; set; }
        public int Total { get; set; }
    }

    public class ReligionReport
    {
        public string culture { get; set; }
        public List<ReligionData> data { get; set; }
    }
    public class OccupationData
    {
        public string Occupation { get; set; }
        public int Total { get; set; }
    }

    public class OccupationReport
    {
        public string culture { get; set; }
        public List<OccupationData> data { get; set; }
    }
    public class DiverseCulturesViewModel { 
        public List<CountryData>  countryDatas { get; set; } = new List<CountryData>();

    }

    public class CountryData {
        public string CountryName { get; set; }
        public List<AgeDistribution> ageDistributions { get; set; } = new List<AgeDistribution>();
        public List<OccupationReport> occupation { get; set; } = new List<OccupationReport>();
        public List<Population> populations { get; set; } = new List<Population>();
        public List<ReligionReport> religions { get; set; } = new List<ReligionReport>();
    }

}