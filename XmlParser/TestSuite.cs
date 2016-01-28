using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlParser
{
    public class TestSuite : TestBase
    {
        public string Type { get; set; }

        public List<TestSuite> TestSuites;
        public List<TestCase> TestCases;

        public TestSuite()
        {
            TestSuites = new List<TestSuite>();
            TestCases = new List<TestCase>();
        }

        public static TestSuite InitFromXElement(XElement testSuite)
        {
            TestSuite tt = new TestSuite()
            {
                Type = testSuite.Attribute("type")?.Value,
                Name = testSuite.Attribute("name")?.Value,
                Executed = testSuite.Attribute("executed")?.Value,
                Result = testSuite.Attribute("result")?.Value,
                Success = testSuite.Attribute("success")?.Value,
                Time = testSuite.Attribute("time")?.Value,
                Asserts = testSuite.Attribute("asserts")?.Value
            };

            return tt;
        }

        public static TestSuite ParseReportTestSuite(XElement testSuite)
        {
            TestSuite tt = InitFromXElement(testSuite);

            string reason = testSuite.Element("reason")?.Element("message")?.Value;
            tt.Reason = reason;

            var results = testSuite.Element("results");
            var subTS = results.Elements("test-suite");
            foreach (XElement _testSuite in subTS)
            {
                TestSuite ttt = TestSuite.ParseReportTestSuite(_testSuite);
                tt.TestSuites.Add(ttt);
            }

            var testCases = results.Elements("test-case");
            foreach (XElement testCase in testCases)
            {
                TestCase test = TestCase.ParseTestCase(testCase);
                tt.TestCases.Add(test);
            }

            return tt;
        }

        public override XElement GenerateHtml()
        {
            XElement html = new XElement("div");

            XElement trMessage = new XElement("tr");
            XElement tdMessage = new XElement("td");


            if (!string.IsNullOrEmpty(Reason))
            {
                XElement reasonMessage = new XElement("pre", Reason);
                tdMessage.Add(reasonMessage);
                trMessage.Add(tdMessage);
            }

            html.Add(trMessage);

            trMessage.SetAttributeValue("style", "font-size:11px; text-align:left;");
            trMessage.SetAttributeValue("bgcolor", "lightgrey");
            tdMessage.SetAttributeValue("colspan", "3");

            foreach (var testSuite in TestSuites)
            {
                XElement _html = testSuite.GenerateHtml();
                html.Add(_html);
            }
            foreach (var testCase in TestCases)
            {
                testCase.HtmlGenerator = new Generator2();
                XElement _html = testCase.GenerateHtml();
                html.Add(_html);
            }
            return html;
        }
    }
}
