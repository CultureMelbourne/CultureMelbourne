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

    public class CountryDataViewModel
    {
        public CountryDataViewModel()
        {// Initialise the list properties
            ChinaData = new List<AgeDistribution>();
            JapanData = new List<AgeDistribution>();
            KoreaData = new List<AgeDistribution>();
            PhilippinesData = new List<AgeDistribution>();
            VietnamData = new List<AgeDistribution>();
        }
        public List<AgeDistribution> ChinaData { get; set; }
        public List<AgeDistribution> JapanData { get; set; }
        public List<AgeDistribution> KoreaData { get; set; }
        public List<AgeDistribution> PhilippinesData { get; set; }
        public List<AgeDistribution> VietnamData { get; set; }
    }



}