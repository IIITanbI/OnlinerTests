using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
namespace XmlParser
{
    public interface IHtml
    {
        XElement Generate(TestCase testSuite);
    }

    public class Generator1 : IHtml
    {
        public XElement Generate(TestCase testCase)
        {
            XElement main = new XElement("div");

            XElement tr = new XElement("tr");
            XElement tdName = new XElement("td", testCase.Desription);
            XElement tdStatus = new XElement("td", testCase.Result);
            XElement tdTime = new XElement("td", testCase.Time);

            XElement trMessage = new XElement("tr");
            XElement tdMessage = new XElement("td");


            if (testCase.Message != null || testCase.StackTrace != null)
            {
                XElement preMessage = new XElement("pre", testCase.Message);
                XElement preStackTrace = new XElement("pre", testCase.StackTrace);
                tdMessage.SetValue("MESSAGE:\n");

                tdMessage.Add(preMessage);
                tdMessage.Add(preStackTrace);
                trMessage.Add(tdMessage);
            }
            else if (!string.IsNullOrEmpty(testCase.Reason))
            {
                XElement reasonMessage = new XElement("pre", testCase.Reason);
                tdMessage.Add(reasonMessage);
                trMessage.Add(tdMessage);
            }

            tr.Add(tdName);
            tr.Add(tdStatus);
            tr.Add(tdTime);

            main.Add(tr);
            main.Add(trMessage);

            tdName.SetAttributeValue("style", "text-align:left;");
            trMessage.SetAttributeValue("style", "font-size:11px; text-align:left;");
            trMessage.SetAttributeValue("bgcolor", "lightgrey");
            tdMessage.SetAttributeValue("colspan", "3");


            switch (testCase.Result)
            {
                case "Success":
                    tdStatus.SetAttributeValue("bgcolor", "green");
                    break;
                case "Ignored":
                    tdStatus.SetAttributeValue("bgcolor", "yellow");
                    break;
                case "Failure":
                    tdStatus.SetAttributeValue("bgcolor", "red");
                    break;
                default:
                    break;
            }

            return main;
        }
    }
    public class Generator2 : IHtml
    {
        public XElement Generate(TestCase testCase)
        {
            XElement main = new XElement("div");

            XElement tr = new XElement("tr");
            XElement tdName = new XElement("td", testCase.Desription);
            XElement tdStatus = new XElement("td", testCase.Result);
            XElement tdTime = new XElement("td", testCase.Time);

            XElement trMessage = new XElement("tr");
            XElement tdMessage = new XElement("td");


            if (testCase.Result != "Ignored")
            {
                if (testCase.Message != null || testCase.StackTrace != null)
                {
                    XElement preMessage = new XElement("pre", testCase.Message);
                    XElement preStackTrace = new XElement("pre", testCase.StackTrace);
                    tdMessage.SetValue("MESSAGE:\n");

                    tdMessage.Add(preMessage);
                    tdMessage.Add(preStackTrace);
                    trMessage.Add(tdMessage);
                }
                else if (!string.IsNullOrEmpty(testCase.Reason))
                {
                    XElement reasonMessage = new XElement("pre", testCase.Reason);
                    tdMessage.Add(reasonMessage);
                    trMessage.Add(tdMessage);
                }
            }

            tr.Add(tdName);
            tr.Add(tdStatus);
            tr.Add(tdTime);

            main.Add(tr);
            main.Add(trMessage);

            tdName.SetAttributeValue("style", "text-align:left;");
            trMessage.SetAttributeValue("style", "font-size:11px; text-align:left;");
            trMessage.SetAttributeValue("bgcolor", "lightgrey");
            tdMessage.SetAttributeValue("colspan", "3");


            switch (testCase.Result)
            {
                case "Success":
                    tdStatus.SetAttributeValue("bgcolor", "green");
                    break;
                case "Ignored":
                    tdStatus.SetAttributeValue("bgcolor", "yellow");
                    break;
                case "Failure":
                    tdStatus.SetAttributeValue("bgcolor", "red");
                    break;
                default:
                    break;
            }

            return main;
        }
    }

    class Program
    {
        static Environment env { get; set;}
        static TestSuite TestFixture { get; set; }
        static IHtml HtmlGenerator { get; set; }

        static void Main(string[] args)
        {
            List<TestSuite> testFixtures = Parse("TestResult.xml");

           
            var html = GenerateHtml(TestFixture);


            XDocument doc = new XDocument();
            doc.Add(html);
            doc.Save("data.html");


            
        }

        public static XElement GetTable(string[] headerNames)
        {
            XElement table = new XElement("table");
            XElement thead = GetTableHeader(headerNames);

            table.Add(thead);
            table.SetAttributeValue("border", "1");
            table.SetAttributeValue("bordercolor", "#666666");
            table.SetAttributeValue("cellpadding", "10");
            table.SetAttributeValue("style", "width:100%;text-align:center;");

            return table;
        }
        public static XElement GetTableHeader(string[] headerNames)
        {
            XElement thead = new XElement("thead");
            XElement theadRow = new XElement("tr");

            foreach(string name in headerNames)
            {
                XElement th = new XElement("th", name);
                th.SetAttributeValue("bgcolor", "lightgrey");
                theadRow.Add(th);
            }

            thead.Add(theadRow);
            return thead;
        }

        public static XElement GetSuitesSummaryTable(TestSuite testFixture)
        {
            XElement main = new XElement("div");
            XElement testHeader = new XElement("h2", "Test Suites");

            XElement table = GetTable(new[] { "Name", "Status", "Time" });
            XElement tbody = new XElement("tbody");
            foreach (var testSuite in testFixture.TestSuites)
            {
                XElement tr = new XElement("tr");
                XElement tdName = new XElement("td", testSuite.Name);
                XElement tdStatus = new XElement("td", testSuite.Result);
                XElement tdTime = new XElement("td", testSuite.Time);

                tr.Add(tdName);
                tr.Add(tdStatus);
                tr.Add(tdTime);

                tdName.SetAttributeValue("style", "text-align:left;");

                switch (testSuite.Result)
                {
                    case "Success":
                        tdStatus.SetAttributeValue("bgcolor", "green");
                        break;
                    case "Ignored":
                        tdStatus.SetAttributeValue("bgcolor", "yellow");
                        break;
                    case "Failure":
                        tdStatus.SetAttributeValue("bgcolor", "red");
                        break;
                }

                tbody.Add(tr);
            }

            table.Add(tbody);
            main.Add(testHeader);
            main.Add(table);

            return main;
        }
        public static XElement GenerateHtml(TestSuite testFixture)
        {
            XElement html = new XElement("div");
            
            html.Add(env.GenerateHtml());
            html.Add(GetSuitesSummaryTable(testFixture));
            foreach (var testSuite in testFixture.TestSuites)
            {
                XElement testHeader = new XElement("h2", testSuite.Name);
                XElement table = GetTable(new[] { "Name", "Status", "Time" });

                XElement tbody = new XElement("tbody");
                XElement _html = testSuite.GenerateHtml();
                tbody.Add(_html);
                table.Add(tbody);

                html.Add(testHeader);
                html.Add(table);
            }

            return html;
        }

        public static List<TestSuite> Parse(string path)
        {
            XDocument doc = XDocument.Load(path);

            env = Environment.InitFromXElement(doc.Root.Element("environment"));
            
            List<TestSuite> testFixtures = new List<TestSuite>();

            var globalTestSuites = doc.Root.Element("test-suite").Element("results").Elements("test-suite");
            foreach (XElement testSuite in globalTestSuites)
            {
                Console.WriteLine(testSuite.Attribute("name"));
                var _testSuites = testSuite.Element("results").Elements("test-suite");
                foreach (XElement ts in _testSuites)
                {
                    Console.WriteLine(ts.Attribute("name"));
                    var nTests = ts.Element("results").Elements("test-suite");
                    TestSuite tt = TestSuite.ParseReportTestSuite(ts);
                    testFixtures.Add(tt);
                    Console.WriteLine(tt.Name);
                }
            }

            TestFixture = testFixtures.FirstOrDefault();
            return testFixtures;
        }
    }
}
