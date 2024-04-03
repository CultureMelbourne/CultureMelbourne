using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TP03MainProj.Models
{
    public class CalenderDate
    {
        
        [Key]
        public int CalenderDateId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CulturalDate { get; set; }

        public string Name { get; set; }


    }
}