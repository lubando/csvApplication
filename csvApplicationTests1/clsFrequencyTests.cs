using Microsoft.VisualStudio.TestTools.UnitTesting;
using csvApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace csvApplication.Tests
{
    [TestClass()]
    public class clsFrequencyTests
    {
        [TestMethod()]
        public void clsFrequencyTest()
        {
            String readPth = "C:\\dev\\data.csv";
            clsReadCSV readfile = new clsReadCSV(readPth);
            clsFrequency frequency = new clsFrequency(readfile.readCsvFile());
            DataTable dt=frequency.getFrequencyOfFirstAndLastName();
            Assert.AreNotSame(dt, readfile.readCsvFile());//are not the same
        }
    }
}