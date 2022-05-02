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
            Class aClass = new Class();
            students = db.FetchAllStudents();
            
            foreach (var s in students)
            {
                Console.WriteLine(s);
            }
            aClass = db.FetchAClass(1687966748);

            Console.WriteLine(aClass);
        }
    }
}
