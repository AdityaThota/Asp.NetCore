using Exam.Models;
using Exam.Repository.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exam.Repository.Repositories
{
    public class StudentRepository : GenericRepository<Student>
    {
        public StudentRepository(DbContext context) : base(context)
        {
        }
    }
}
