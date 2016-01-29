using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlParser
{
    public class TestCase : TestBase
    {
        public string Desription { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public IHtmlGenerator HtmlGenerator { get; set; } = new HtmlGenerator1();
        public IParsable Parsable { get; set; } = new Parse1();

        public TestCase()
        {

        }
        public TestCase(XElement testCase)
        {
            this.Parse(testCase);
        }

        public override void InitAttributeFromXElement(XElement testCase)
        {
            this.Name = testCase.Attribute("name")?.Value;
            this.Desription = testCase.Attribute("description")?.Value;
            this.Executed = testCase.Attribute("executed")?.Value;
            this.Result = testCase.Attribute("result")?.Value;
            this.Success = testCase.Attribute("success")?.Value;
            this.Time = testCase.Attribute("time")?.Value;
            this.Asserts = testCase.Attribute("asserts")?.Value;

            //TestCase tt = new TestCase()
            //{
            //    Name = testCase.Attribute("name")?.Value,
            //    Desription = testCase.Attribute("description")?.Value,
            //    Executed = testCase.Attribute("executed")?.Value,
            //    Result = testCase.Attribute("result")?.Value,
            //    Success = testCase.Attribute("success")?.Value,
            //    Time = testCase.Attribute("time")?.Value,
            //    Asserts = testCase.Attribute("asserts")?.Value
            //};

            //return tt;
        }
        public override void Parse(XElement testCase)
        {
            Parsable.Parse(this, testCase);
            

            //this.InitAttributeFromXElement(testCase);
            ////TestCase tt = InitFromXElement(testCase);

            //string message = testCase.Element("failure")?.Element("message")?.Value;
            //this.Message = message;
            //string stackTrace = testCase.Element("failure")?.Element("stack-trace")?.Value;
            //this.StackTrace = stackTrace;

            //string reason = testCase.Element("reason")?.Element("message")?.Value;
            //this.Reason = reason;
        }

        public override XElement GenerateHtml()
        {
            var res = HtmlGenerator.Generate(this);
            return res;

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
}
