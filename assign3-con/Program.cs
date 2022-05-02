using System;
using System.Collections.Generic;
namespace assign3_con
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            DatabaseContext db = new DatabaseContext();
            List<Student> students = new List<Student>();

            students = db.FetchAllStudents();
            
            foreach (var s in students)
            {
                Console.WriteLine(s);
            }
        }
    }
}
