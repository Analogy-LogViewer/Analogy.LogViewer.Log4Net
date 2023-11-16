using Analogy.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Log4Net.UnitTests
{
    [TestClass]
    public class RegexParserShould
    {
        [TestMethod]
        public void ParseToLogMessage()
        {
            //var regEx = @"(?<Date>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},\d{3}) \[(?<Thread>\d+)\] (?<Level>.*)   (?<Source>.*) - (?<Text>.*)";
            var regEx = @"(?<Date>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3})\s\[(?<Thread>\d+)\]\s(?<Level>.*)\s(?<Source>.*)\s-\s(?<Text>.*)";
            var testString = "2021-05-06 15:00:00,555 [14] INFO   MyClass.Something - Resolving IDiscoverable";
            var dateTimeFormat = "yyyy-MM-dd HH:mm:ss,fff";

            RegexPattern p = new RegexPattern(regEx, dateTimeFormat, "");
            bool valid = RegexParser.CheckRegex(testString, p, out AnalogyLogMessage m);

            Assert.IsTrue(valid);
        }
    }
}