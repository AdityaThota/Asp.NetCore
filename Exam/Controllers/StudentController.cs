using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Exam.Models;
using Exam.Repository.Repositories;
using Exam.Repository.Repositories.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Exam.Controllers
{
    public class StudentController : Controller
    {
        //StudentContext context;

        //public StudentController(StudentContext context)
        //{
        //    this.context = context;
        //}
        IGenericRepository<Student> studentRep;
        IHostingEnvironment env;

        public StudentController(IGenericRepository<Student> studentRep,
                                 IHostingEnvironment env)
        {
            this.studentRep = studentRep;
            this.env = env;
        }
        public IActionResult FileUpload(int Id) => View(); //Загрузка одного файла

        [HttpPost]
        public async Task<IActionResult> FileUpload(int Id, IFormFile fileUpload) //fileUpload повинно співпадати з іменем на input
        {
            if (fileUpload != null)
            {
                string path = "/Photos/" + fileUpload.FileName.Split(new char[] { '\\', '/' }).Last();
                Student student = studentRep.Get(Id);
                //Зберігаємо в папку Photos
                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await fileUpload.CopyToAsync(fileStream);
                }
                student.PhotoPath = @"..\..\Photos\" + fileUpload.FileName.Split(new char[] { '\\', '/' }).Last(); ;
                studentRep.Save();
            }
            return RedirectToAction("Edit", new { Id = Id });
        }


        public IActionResult Index()
        {
            var model = studentRep.GetAll();
            if (studentRep.GetAll().Count() > 0)
            {
                Dictionary<int, double> marks = new Dictionary<int, double>();
                foreach (var item in model)
                {
                    marks.Add(item.Id, item.Mark);
                }
                var maxMarkId = marks.First(x => x.Value == marks.Values.Max()).Key;
                Student studentWithMaxMark = studentRep.Get(maxMarkId);
                ViewBag.Student = studentWithMaxMark.FirstName + " " + studentWithMaxMark.LastName;
                ViewBag.Photo = studentWithMaxMark.PhotoPath;
                ViewBag.Mark = studentWithMaxMark.Mark;
            }
            return View(studentRep.GetAll());
        }


        public IActionResult Edit(int id = 0)
        {
            var model = (id == 0) ? new Student() { PhotoPath = @"..\..\Photos\default.jpg" } : studentRep.Get(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Student obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    studentRep.Add(obj);
                }
                else
                    studentRep.Update(obj, obj.Id);
                studentRep.Save();
                //return Json("OK");
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public ActionResult Delete(int id)
        {
            Student student = studentRep.Get(id);

            string path = env.WebRootPath + "/Photos/" + student.PhotoPath.Split(new char[] { '\\', '/' }).Last();

            if (System.IO.File.Exists(path) && path != env.WebRootPath + "/Photos/default.jpg")
            {
                System.IO.File.Delete(path);
            }

            studentRep.Delete(student);
            return Json("OK");
        }
        public IActionResult Details(int id)
        {
            return View(studentRep.Get(id));
        }

        public async Task<IActionResult> IndexSort(string sortOrder, string currentFilter, string searchString, int? page)
        {
            IQueryable<Student> model = studentRep.GetAll();
            if (studentRep.GetAll().Count() > 0)
            {
                Dictionary<int, double> marks = new Dictionary<int, double>();
                foreach (var item in model)
                {
                    marks.Add(item.Id, item.Mark);
                }
                var maxMarkId = marks.First(x => x.Value == marks.Values.Max()).Key;
                Student studentWithMaxMark = studentRep.Get(maxMarkId);
                ViewBag.Student = studentWithMaxMark.FirstName + " " + studentWithMaxMark.LastName;
                ViewBag.Photo = studentWithMaxMark.PhotoPath;
                ViewBag.Mark = studentWithMaxMark.Mark;
            }
            ////////////
            {
                ViewData["CurrentSort"] = sortOrder;

                ViewData["LastNameSortParm"] = sortOrder == "Last_name" ? "last_name_desc" : "Last_name";
                ViewData["FirstNameSortParm"] = sortOrder == "First_name" ? "first_name_desc" : "First_name";
                ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
                ViewData["MarkSortParm"] = sortOrder == "Mark" ? "mark_desc" : "Mark";


                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                switch (sortOrder)
                {
                    case "Last_name":
                        model = model.OrderBy(s => s.LastName);
                        break;
                    case "last_name_desc":
                        model = model.OrderByDescending(s => s.LastName);
                        break;
                    case "First_name":
                        model = model.OrderBy(s => s.FirstName);
                        break;
                    case "first_name_desc":
                        model = model.OrderByDescending(s => s.FirstName);
                        break;
                    case "Date":
                        model = model.OrderBy(s => s.Birthday);
                        break;
                    case "date_desc":
                        model = model.OrderByDescending(s => s.Birthday);
                        break;
                    case "Mark":
                        model = model.OrderBy(s => s.Mark);
                        break;
                    case "mark_desc":
                        model = model.OrderByDescending(s => s.Mark);
                        break;
                    default:
                        model = model.OrderBy(s => s.LastName);
                        break;
                }

                int pageSize = 8;
                return View(await PaginatedList<Student>.CreateAsync(model.AsNoTracking(), page ?? 1, pageSize));
            }
        }
    }
}