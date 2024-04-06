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
        public string Culture { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Start_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime End_Date { get; set; }

        //There might be a festival name for the date or might not be
        public string Title { get; set; }


    }
}