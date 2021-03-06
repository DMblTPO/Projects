﻿using System;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MultiTaskApp.Models;

namespace MultiTaskApp
{
    class MultiTask
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new UnivDbContext())
            {
                //var university = new University
                //{
                //    City = "Kiev",
                //    Name = "NAU",
                //    Rating = 756
                //};
                
                //var student = new Student
                //{
                //    FirstName = "Dmytro",
                //    LastName = "Boboshko",
                //    City = "Kiev",
                //    Course = 1,
                //    Birthday = DateTime.ParseExact("19750906", "yyyyMMdd", null),
                //    Email = "xepomaht@gmail.com",
                //    Stipend = 1234.99m,
                //    University = university
                //};
                
                //dbContext
                //    .Students
                //    .Add(student);
                
                //dbContext.SaveChanges();

                string input = "0V2uClXay+3oDdXWRMsFRg==";
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                    var guid = new Guid(hash).ToString("N");
                    Console.WriteLine(guid);
                }
                
                Console.WriteLine("Find a studnet:");

                var students = dbContext.Students;

                var student = 
                    Task.Run(() => students.Include(x => x.University).FirstAsync()).Result;

                Console.WriteLine($"{student.FirstName} {student.LastName} study at {student.University.Name}");
                
            }
        }
    }
}
