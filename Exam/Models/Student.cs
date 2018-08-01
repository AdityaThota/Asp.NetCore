using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public class Student
    {
        public int Id { get; set; }
        [DisplayName("Ім'я")]
        public string FirstName { get; set; }

        [DisplayName("Прізвище")]
        public string LastName { get; set; }

        [DisplayName("Дата народження")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy'/'MM'/'dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [DisplayName("Середній бал")]
        [Range(1.0, 12.0, ErrorMessage = "Неправильний бал. Мінімальна оцінка 1, максимальна 12.")]
        public double Mark { get; set; }

        [DisplayName("Фото")]
        public string PhotoPath { get; set; }

    }
}
