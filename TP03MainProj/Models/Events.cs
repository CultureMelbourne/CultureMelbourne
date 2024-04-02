﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TP03MainProj.Models;

namespace TP03MainProj.Models
{
    public class Events
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        //[Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage ="Invalid Event Description. Check data entry.")]
        public string Description { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime EndTime { get; set; }

        // Link to CalenderDate
        public int CalenderDateId { get; set; } // Foreign Key

        [ForeignKey("CalenderDateId")]
        public virtual CalenderDate CalenderDate { get; set; } // Navigation Property
    }
}