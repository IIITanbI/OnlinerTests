using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
namespace XmlParser
{
    class TestCase {
        public string Name { get; set; }
        public string Desription { get; set; }
        public string Executed { get; set; }
        public string Result { get; set; }
        public string Success { get; set; }
        public string Time { get; set; }
        public string Asserts { get; set; }


        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Reason { get; set; }

        public static TestCase InitFromXElement(XElement testCase)
        {
            TestCase tt = new TestCase()
            {
                Name = testCase.Attribute("name")?.Value,
                Desription = testCase.Attribute("description")?.Value,
                Executed = testCase.Attribute("executed")?.Value,
                Result = testCase.Attribute("result")?.Value,
                Success = testCase.Attribute("success")?.Value,
                Time = testCase.Attribute("time")?.Value,
                Asserts = testCase.Attribute("asserts")?.Value
            };

            return tt;
        }
        public static TestCase ParseTestCase(XElement testCase)
        {
            TestCase tt = InitFromXElement(testCase);
           
            string message = testCase.Element("failure")?.Element("message")?.Value;
            tt.Message = message;
            string stackTrace = testCase.Element("failure")?.Element("stack-trace")?.Value;
            tt.StackTrace = stackTrace;

            string reason = testCase.Element("reason")?.Element("message")?.Value;
            tt.Reason = reason;
            return tt;
        }

        public XElement GenerateHtml()
        {
            XElement main = new XElement("div");

            XElement tr = new XElement("tr");
            XElement tdName = new XElement("td", Desription);
            XElement tdStatus = new XElement("td", Result);
            XElement tdTime = new XElement("td", Time);
                
            XElement trMessage = new XElement("tr");
            XElement tdMessage = new XElement("td");
          

            if (Message != null || StackTrace != null)
            {
                XElement preMessage = new XElement("pre", Message);
                XElement preStackTrace = new XElement("pre", StackTrace);
                tdMessage.SetValue("MESSAGE:\n");

                tdMessage.Add(preMessage);
                tdMessage.Add(preStackTrace);
                trMessage.Add(tdMessage);
            }
            else if (!string.IsNullOrEmpty(Reason))
            {
                XElement reasonMessage = new XElement("pre", Reason);
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

            
            switch (Result)
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
    class TestSuite
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Executed { get; set; }
        public string Result { get; set; }
        public string Success { get; set; }
        public string Time { get; set; }
        public string Asserts { get; set; }

        public string Reason { get; set; }

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
                Success =testSuite.Attribute("success")?.Value,
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

        public XElement GenerateHtml()
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
                XElement _html = testCase.GenerateHtml();
                html.Add(_html);
            }
            return html;
        }
    }
    class Environment
    {
        public string NUnitVersion { get; set; }
        public string CLRVersion { get; set; }
        public string OSVersion { get; set; }
        public string Platform { get; set; }
        public string MachineName { get; set; }
        public string User { get; set; }
        public string UserDomain { get; set; }

        public static Environment InitFromXElement(XElement testSuite)
        {
            Environment en = new Environment()
            {
                NUnitVersion = testSuite.Attribute("nunit-version")?.Value,
                CLRVersion = testSuite.Attribute("clr-version")?.Value,
                OSVersion = testSuite.Attribute("os-version")?.Value,
                Platform = testSuite.Attribute("platform")?.Value,
                MachineName = testSuite.Attribute("machine-name")?.Value,
                User = testSuite.Attribute("user")?.Value,
                UserDomain = testSuite.Attribute("user-domain")?.Value
            };

            return en;
        }

        public XElement GenerateHtml()
        {

            XElement html = new XElement("div");
            XElement header = new XElement("h2");
            
            XElement html1 = new XElement("span");

            header.Add("Environment");
            html1.Add("NUNIT");
            html.Add(header);
            header.AddAfterSelf(html1);

            return html;
        }

    }

  

    class Program
    {
        Environment env;
        static void Main(string[] args)
        {

            List<TestSuite> testFixtures = new Program().Parse("TestResult.xml");
           
            TestSuite ts = testFixtures.FirstOrDefault();

            var html = new Program().GenerateHtml(ts);

            XDocument doc = new XDocument();
            doc.Add(html);
            doc.Save("data.html");
            //Console.Read();
        }

        public XElement GetTableHeader()
        {
            XElement thead = new XElement("thead");
            XElement theadRow = new XElement("tr");

            XElement thName = new XElement("th", "Name");
            XElement thStatus = new XElement("th", "Status");
            XElement thTime = new XElement("th", "Time");

            thName.SetAttributeValue("bgcolor", "lightgrey");
            thStatus.SetAttributeValue("bgcolor", "lightgrey");
            thTime.SetAttributeValue("bgcolor", "lightgrey");
           
            theadRow.Add(thName);
            theadRow.Add(thStatus);
            theadRow.Add(thTime);
            thead.Add(theadRow);

            return thead;
        }
        public XElement GetSuitesSummaryTable(TestSuite testFixture)
        {
            XElement main = new XElement("div");
            XElement testHeader = new XElement("h2", "Test Suites");

            XElement table = new XElement("table");
            XElement thead = GetTableHeader();

            table.SetAttributeValue("border", "1");
            table.SetAttributeValue("bordercolor", "#666666");
            table.SetAttributeValue("cellpadding", "10");
            table.SetAttributeValue("style", "width:100%;text-align:center;");

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
                    default:
                        break;
                }

                tbody.Add(tr);
            }

            table.Add(thead);
            table.Add(tbody);

            main.Add(testHeader);
            main.Add(table);
            return main;
        }
        public XElement GenerateHtml(TestSuite testFixture)
        {
            XElement html = new XElement("div");
            html.Add(GetSuitesSummaryTable(testFixture));
            foreach (var testSuite in testFixture.TestSuites)
            {
                XElement testHeader = new XElement("h2", testSuite.Name);

                XElement table = new XElement("table");
                //XElement thead = new XElement("thead");
                //XElement theadRow = new XElement("tr");

                //XElement thName = new XElement("th", "Name");
                //XElement thStatus = new XElement("th", "Status");
                //XElement thTime = new XElement("th", "Time");

                //thName.SetAttributeValue("bgcolor", "lightgrey");
                //thStatus.SetAttributeValue("bgcolor", "lightgrey");
                //thTime.SetAttributeValue("bgcolor", "lightgrey");
                XElement thead = GetTableHeader();

                table.SetAttributeValue("border", "1");
                table.SetAttributeValue("bordercolor", "#666666");
                table.SetAttributeValue("cellpadding", "10");
                table.SetAttributeValue("style", "width:100%;text-align:center;");

                XElement tbody = new XElement("tbody");

                //theadRow.Add(thName);
                //theadRow.Add(thStatus);
                //theadRow.Add(thTime);
                //thead.Add(theadRow);
                table.Add(thead);
                table.Add(tbody);

                XElement _html = testSuite.GenerateHtml();
                tbody.Add(_html);

                html.Add(testHeader);
                html.Add(table);
                
            }

            return html;
        }
        public List<TestSuite> Parse(string path)
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
            return testFixtures;
        }

        //public TestSuite ParseReportTestSuite(XElement testSuite)
        //{
        //    TestSuite tt = new TestSuite();


        //    var results = testSuite.Element("results");
        //    var subTS = results.Elements("test-suite");
        //    foreach (XElement _testSuite in subTS)
        //    {
        //        TestSuite ttt = new TestSuite();
        //        tt.TestSuites.Add(ttt);
        //        ParseReportTestSuite(_testSuite);
        //    }

        //    var testCases = results.Elements("test-case");
        //    foreach (XElement testCase in testCases)
        //    {
        //        TestCase test = ParseTestCase(testCase);
        //        tt.TestCases.Add(test);
        //    }
        //    return tt;
        //}

        //public TestCase ParseTestCase(XElement testCase)
        //{

        //    return new TestCase() { Name = "123" };
        //}
    }
}
