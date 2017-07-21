using Microsoft.VisualStudio.TestTools.UnitTesting;
using csvApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csvApplication;
using System.Configuration;
namespace csvApplication.Tests
{
    [TestClass()]
    public class clsCreateCSVFileTests
    {
        [TestMethod()]
        public void clsCreateCSVFileTest()
        {
            string readPth = ConfigurationManager.AppSettings["Path"];


            clsReadCSV readfile = new clsReadCSV(readPth);

            clsFrequency frequency = new clsFrequency(readfile.readCsvFile());

            string writePath = ConfigurationManager.AppSettings["Path2"];

            //write
            clsCreateCSVFile createFirstFile = new clsCreateCSVFile("frequency", frequency.getFrequencyOfFirstAndLastName(), writePath);
            
            Assert.IsTrue(createFirstFile.writeToCSVfile());
            //sort alphabeticall
            clsOrderByAddress orderBy = new clsOrderByAddress(readfile.readCsvFile());

            clsCreateCSVFile secondFile = new clsCreateCSVFile("alphabetical", orderBy.getAlphabeticalOrder(), writePath);
            secondFile.writeToCSVfile();
            Assert.IsTrue(secondFile.writeToCSVfile());
        }

       
    }
}