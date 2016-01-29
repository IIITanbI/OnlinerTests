using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlParser
{
    public abstract class TestBase
    {
        public string Name { get; set; }
        public string Executed { get; set; }
        public string Result { get; set; }
        public string Success { get; set; }
        public string Time { get; set; }
        public string Asserts { get; set; }

        public string Reason { get; set; }

        public abstract void InitAttributeFromXElement(XElement xElement);
        public abstract void Parse(XElement xElement);
      
        public abstract XElement GenerateHtml();
    }
}
