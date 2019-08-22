using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using AngCore.Models;

namespace AngCore.Controllers
{
    public class HomeController : Controller
    {
        static string connectionString = @"";
        static string dbname = "";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Databases(String dbconnection)
        {
            connectionString = @"" + dbconnection;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            Databases model = new Databases() { };
            List<string> result = new List<string>();
            SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con);
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader["name"].ToString());
            }
            reader.Close();
            model.List = result;
            con.Close();
            return View(model);
        }

        public IActionResult Tables(String name)
        {
            dbname = name;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            TableNamesList model = new TableNamesList() { };
            List<string> result = new List<string>();
            SqlCommand cmd = new SqlCommand(@"SELECT name FROM " + name + ".sys.Tables", con);
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["name"].ToString() != "__RefactorLog")
                {
                    result.Add(reader["name"].ToString());
                }
            }
            reader.Close();
            model.Database_name = name;
            model.Tables = result;
            con.Close();
            return View(model);
        }

        public IActionResult Columns(string tablename)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string requset = "USE[" + dbname + "] SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='" + tablename + "'";
            SqlCommand cmd = new SqlCommand(requset, con);
            TableViewModel model = new TableViewModel();
            List<string> result = new List<string>();
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader["COLUMN_NAME"].ToString());
            }
            model.Name = tablename;
            model.Columns = result;
            string requset2 = "USE[" + dbname + "] EXEC sp_fkeys '" + tablename + "'";
            cmd = new SqlCommand(requset2, con);
            List<String> fkeys = new List<string>();
            reader.Close();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                fkeys.Add("Column: " + reader["PKCOLUMN_NAME"].ToString() + " Table: " + reader["FKTABLE_NAME"].ToString());
            }
            reader.Close();
            ViewData["FKeys"] = fkeys;
            con.Close();
            return View(model);
        }

        public object GetExcel(string tablename)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Enviroment data");
            IRow nameRow = sheet.CreateRow(0);
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string requset = "USE[" + dbname + "] SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='" + tablename + "'";
            SqlCommand cmd = new SqlCommand(requset, con);
            TableViewModel model = new TableViewModel();
            List<string> result = new List<string>();
            System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                nameRow.CreateCell(i).SetCellValue(reader["COLUMN_NAME"].ToString());
                i++;
            }
            reader.Close();
            string request2 = "SELECT * FROM [" + dbname + "].[dbo].[" + tablename + "]";
            cmd = new SqlCommand(request2, con);
            reader = cmd.ExecuteReader();
            i = 1;
            while (reader.Read())
            {
                IRow newRow = sheet.CreateRow(i);
                for (int u = 0; u < nameRow.Cells.Count(); u++)
                {
                    newRow.CreateCell(u).SetCellValue(reader[Convert.ToString(nameRow.Cells[u].RichStringCellValue.String)].ToString());
                }
                i++;

            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            byte[] filearray;
            filearray = ms.ToArray();
            ms.Close();
            return File(filearray, "application/xlsx", tablename + ".xlsx");
        }

    }
}
