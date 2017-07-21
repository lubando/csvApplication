using Microsoft.VisualStudio.TestTools.UnitTesting;
using csvApplication;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Text;

namespace csvApplication.Tests
{
    [TestClass()]
    public class clsReadCSVTests
    {
        [TestMethod()]
        public void clsReadCSVTest()
        {
            String readPth = "C:\\dev\\data.csv";
            clsReadCSV readfile = new clsReadCSV(readPth);
            DataTable dt = readfile.readCsvFile();
            DataTable dt2 = new DataTable();
            Assert.AreNotEqual(dt2, dt);//expected pass
        }

       
    }
}