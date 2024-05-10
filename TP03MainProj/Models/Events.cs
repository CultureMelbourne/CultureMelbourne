using System;
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
        public string Culture { get; set; }

        [Required]
        public string Title { get; set; }

        //[Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage ="Invalid Event Description. Check data entry.")]
        public string Description { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        [Required]
        public string Url { get; set; }

        // Link to CalenderDate
        public int CalenderDateId { get; set; } // Foreign Key

        [ForeignKey("CalenderDateId")]
        public virtual CalenderDate Start_Date { get; set; } // Navigation Property
    }

    public class CultureQuiz
    {
        public string Name { get; set; }
        public List<Questions> Questions { get; set; }
    }

    public class QuizData
    {
        public List<CultureQuiz> Cultures { get; set; }
    }

    public class Questions
    {
        // Check this modification of the questionNum once
        public int QuestionNum { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }



}