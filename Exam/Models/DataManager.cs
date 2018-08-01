using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public static class DataManager
    {
        public static void DataInitialize(StudentContext context)
        {
            if (!context.Students.Any())
            {
                context.Students.AddRange(
                new Student { Birthday = new DateTime(1993, 8, 12), FirstName = "Vasja", LastName = "Kozlenko", Mark = 9.1, PhotoPath= @"..\..\Photos\1.jpg" },
                new Student { Birthday = new DateTime(1990, 4, 21), FirstName = "Petro", LastName = "Petrenko", Mark = 11.2, PhotoPath = @"..\..\Photos\2.jpg" },
                new Student { Birthday = new DateTime(1989, 3, 2), FirstName = "Vanja", LastName = "Lomtuk", Mark = 11.3, PhotoPath = @"..\..\Photos\3.jpg" },
                new Student { Birthday = new DateTime(1988, 1, 23), FirstName = "Vanja", LastName = "Ivaniyk", Mark = 8.1, PhotoPath = @"..\..\Photos\4.jpg" },
                new Student { Birthday = new DateTime(1994, 11, 13), FirstName = "Dasha", LastName = "Dovgopala", Mark = 10.1, PhotoPath = @"..\..\Photos\5.jpg" },
                new Student { Birthday = new DateTime(1993, 12, 17), FirstName = "Luba", LastName = "Lokon", Mark = 10.3, PhotoPath = @"..\..\Photos\6.jpg" },
                new Student { Birthday = new DateTime(1991, 4, 1), FirstName = "Luda", LastName = "Pityhova", Mark = 11.5, PhotoPath = @"..\..\Photos\7.jpg" },
                new Student { Birthday = new DateTime(1992, 2, 23), FirstName = "Vanja", LastName = "Kolokol", Mark = 8.5, PhotoPath = @"..\..\Photos\8.jpg" },
                new Student { Birthday = new DateTime(2000, 11, 01), FirstName = "Dasha", LastName = "Koza", Mark = 9.8, PhotoPath = @"..\..\Photos\9.jpg" },
                new Student { Birthday = new DateTime(1998, 12, 10), FirstName = "Ira", LastName = "Lvovich", Mark = 10.0, PhotoPath = @"..\..\Photos\10.jpg" }
                );

                context.SaveChanges();
            }
        }
    }
}
