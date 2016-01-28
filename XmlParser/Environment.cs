using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlParser
{
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

            XElement header = new XElement("h2", "Envionment");

            XElement table = new XElement("table");
            XElement thead = Program.GetTableHeader(
                new[] { "NUnit version", "CLR version", "OS version", "Platform", "Machine name", "User", "User domain" });
            XElement tbody = new XElement("tbody");

            XElement tr = new XElement("tr");

            List<XElement> tdList = new List<XElement>();
            tdList.Add(new XElement("td", NUnitVersion));
            tdList.Add(new XElement("td", CLRVersion));
            tdList.Add(new XElement("td", OSVersion));
            tdList.Add(new XElement("td", Platform));
            tdList.Add(new XElement("td", MachineName));
            tdList.Add(new XElement("td", User));
            tdList.Add(new XElement("td", UserDomain));

            //XElement tdNUnitVersion = new XElement("td", NUnitVersion);
            //XElement tdCLRVersion = new XElement("td", CLRVersion);
            //XElement tdOSVersion = new XElement("td", OSVersion);
            //XElement tdPlatform = new XElement("td", Platform);
            //XElement tdMachineName = new XElement("td", MachineName);
            //XElement tdUser = new XElement("td", User);
            //XElement tdUserDomain = new XElement("td", UserDomain);

            tdList.ForEach(t => tr.Add(t));
            tbody.Add(tr);
            table.Add(thead);
            table.Add(tbody);
            html.Add(header);
            html.Add(table);

            table.SetAttributeValue("border", "1");
            table.SetAttributeValue("bordercolor", "#666666");
            table.SetAttributeValue("cellpadding", "10");
            table.SetAttributeValue("style", "width:100%;text-align:center;");

            return html;
        }

    }

}
