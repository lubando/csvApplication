using System;
using System.Collections.Generic;
using System.Data;  
using System.IO;  
using System.Linq;
using System.ComponentModel;
using System.Text;

using System.Configuration;
using System.Reflection;
namespace csvApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string readPth = ConfigurationManager.AppSettings["Path"];
           

            clsReadCSV readfile = new clsReadCSV(readPth);
           
            clsFrequency frequency = new clsFrequency(readfile.readCsvFile());

            string writePath = ConfigurationManager.AppSettings["Path2"];
          
            //write
            clsCreateCSVFile createFirstFile = new clsCreateCSVFile("frequency", frequency.getFrequencyOfFirstAndLastName(), writePath);
            createFirstFile.writeToCSVfile();
            //sort alphabeticall
            clsOrderByAddress orderBy = new clsOrderByAddress(readfile.readCsvFile());
           
            clsCreateCSVFile secondFile = new clsCreateCSVFile("alphabetical", orderBy.getAlphabeticalOrder(), writePath);
            secondFile.writeToCSVfile();
        }
    }

    public class clsReadCSV
    {
        string Path = null;
        public clsReadCSV(string path)
        {

            Path = path;
        }
        public void pathExist(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Console.WriteLine(path+ "does not exist");
                    Console.ReadLine();
                  
                }
            }
            catch (Exception ex)
            {
                // handle them here
            }

        }

        public DataTable readCsvFile()
        {

            DataTable dtCsv = new DataTable();
            string Fulltext;
            if (Path != null)
            {
                // string FileSaveWithPath = Server.MapPath("\\Files\\Import" + System.DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".csv");

                using (StreamReader sr = new StreamReader(Path))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                            {
                                if (i == 0)
                                {
                                    for (int j = 0; j < rowValues.Count(); j++)
                                    {
                                        dtCsv.Columns.Add(rowValues[j]); //add headers                                  }  
                                    }
                                }
                                else
                                {
                                    DataRow dr = dtCsv.NewRow();
                                    for (int k = 0; k < rowValues.Count(); k++)
                                    {
                                        dr[k] = rowValues[k].ToString();
                                    }
                                    dtCsv.Rows.Add(dr); //add other rows  
                                }
                            }
                        }
                    }
                }
            }

            return dtCsv;
        }
    }
    public class clsCreateCSVFile
    {
        string actoinName;
        DataTable table = new DataTable();
        string path=null;
        public clsCreateCSVFile(string Action,DataTable dt,string _path)
        {
            actoinName = Action;
            table = dt;
            this.path = _path;
        }

        public bool writeToCSVfile()
          {
            bool iscreated = true;
            try
            {
                StringBuilder sb = new StringBuilder();
                path += actoinName;
                createPath(path);
                foreach (DataColumn col in table.Columns)
                {
                    sb.Append(col.ColumnName + ',');
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);

                foreach (DataRow row in table.Rows)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        sb.Append(row[i].ToString() + ",");
                    }

                    sb.Append(Environment.NewLine);
                }

                File.WriteAllText(path + "\\" + actoinName + ".csv", sb.ToString());
            }
            catch (Exception e)
            {
                iscreated = false;
            }
            return true;
        }
        public void createPath(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                // handle them here
            }

        }
    }
    
    public class clsFrequency
    {
        DataTable Dt = new DataTable();
        public clsFrequency(DataTable dt)
        {
            Dt = dt;
        }
        
        public DataTable getFrequencyOfFirstAndLastName()
        {
            var firstNameCounts = Dt.AsEnumerable()
             .GroupBy(row => row.Field<string>("FirstName"))
          .Select(d => new
               {
                 names = d.Key,
                Count = d.Count()
               })
               .ToList();

            //convert list to datatbale for firstName column
            DataTable dtfirstNameCounts = ListToDataTable(firstNameCounts);

            var lastNameCounts = Dt.AsEnumerable()
             .GroupBy(row => row.Field<string>("FirstName"))
          .Select(d => new
          {
              names = d.Key,
              Count = d.Count()
          })
               .ToList();
            //convert list to datatbale for firstName column
            DataTable dtlastNameCounts = ListToDataTable(lastNameCounts);
            dtfirstNameCounts.Merge(dtlastNameCounts);

            DataView dv = dtfirstNameCounts.DefaultView;
            dv.Sort = "Count desc";
            dtfirstNameCounts = dv.ToTable();
            return dtfirstNameCounts;
        }
        public static DataTable ListToDataTable<T>(IList<T> lst)
        {
            DataTable table = new DataTable();
            table.Columns.Add("names");
            table.Columns.Add("Count");
            Type entType = typeof(T);

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            foreach (T item in lst)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {

                    if (prop.PropertyType == typeof(Nullable<decimal>) || prop.PropertyType == typeof(Nullable<int>) || prop.PropertyType == typeof(Nullable<Int64>))
                    {
                        if (prop.GetValue(item) == null)
                            row[prop.Name] = 0;
                        else
                            row[prop.Name] = prop.GetValue(item);
                    }
                    else
                        row[prop.Name] = prop.GetValue(item);

                }
                table.Rows.Add(row);
            }

            return table;
        }


    }
    public class clsOrderByAddress
    {
        DataTable table = new DataTable();
        public clsOrderByAddress(DataTable dt)
        {
            table = dt;
        }
        public DataTable getAlphabeticalOrder()
        {
            DataView dv = table.DefaultView;
            dv.Sort = "Address asc";
            table = dv.ToTable();

            var Rows = (from row in table.AsEnumerable()
                        orderby row["Address"] descending
                        select row);
            table = Rows.AsDataView().ToTable();


            return table;

        }
    }
    public class clsfields
    {
        public clsfields()
        { }
        public string names { get; set; }
        public int Count { get; set; }
    }
   


}




