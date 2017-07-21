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
    public class clsOrderByAddressTests
    {
        [TestMethod()]
        public void clsOrderByAddressTest()
        {
            String readPth = "C:\\dev\\data.csv";
            clsReadCSV readfile = new clsReadCSV(readPth);
            DataTable dtnotOrdered = readfile.readCsvFile();

            
            clsOrderByAddress orderBy = new clsOrderByAddress(readfile.readCsvFile());
            DataTable dtOrdered = orderBy.getAlphabeticalOrder();
            Assert.Equals(dtOrdered, dtnotOrdered);//must fail if is correct.
        }
    }
}