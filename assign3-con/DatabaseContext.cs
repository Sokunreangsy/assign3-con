﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace assign3_con
{
    class DatabaseContext
    {
        private const string db = "gmis";
        private const string user = "gmis";
        private const string pass = "gmis";
        private const string server = "alacritas.cis.utas.edu.au";
        private static MySqlConnection conn = null;


        public DatabaseContext()
        {
            GetConnection();
        }
        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                //Note: This approach is not thread-safe
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }

        public List<Student> FetchAllStudents()
        {
            List<Student> students = new List<Student>();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select given_name, family_name from student", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //Note that in your assignment you will need to inspect the *type* of the
                    //employee/researcher before deciding which kind of concrete class to create.
                    students.Add(new Student { GivenName = rdr.GetString(0), FamilyName = rdr.GetString(1) });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return students;
        }

        private List<StudentGroup> FetchGroups(int id)
        {
            List<StudentGroup> studentGroups = new List<StudentGroup>();
            MySqlDataReader rdr = null;
            try
            {

                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select * from studentGroup where group_id=?id", conn);
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //Note that in your assignment you will need to inspect the *type* of the
                    //employee/researcher before deciding which kind of concrete class to create.
                    studentGroups.Add(new StudentGroup
                    {
                        
                        GroupId = rdr.GetInt32(0),
                        GroupName = rdr.GetString(1)
                    });
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return studentGroups;
        }
        public Class FetchAClass(int id)
        {
            Class resultClass= new Class();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select * from class where class_id=?id", conn);
                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    //Note that in your assignment you will need to inspect the *type* of the
                    //employee/researcher before deciding which kind of concrete class to create.
                    resultClass = new Class { 
                        ClassId=rdr.GetInt32(0),
                        GroupId=rdr.GetInt32(1),
                        Day=ParseEnum<Days>(rdr.GetString(2)),
                        Start=rdr.GetString(3),
                        End=rdr.GetString(4),
                        Room=rdr.GetString(5)
                    };
                }
                
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error connecting to database: " + e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                resultClass.StudentGroups = FetchGroups(resultClass.GroupId);
            }

            return resultClass;
        }
        /*SELECT class_id,day,class.start,class.end,room FROM class 
        INNER JOIN studentGroup ON class.group_id=studentGroup.group_id
        WHERE class_id=2 */

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
