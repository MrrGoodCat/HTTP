using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UniTester
{
    class TesterLinqXML
    {
        public void CreateTestingXML()
        {
            XElement xmlDoc =
            new XElement("Tasks",
                new XElement("Task",
                    new XAttribute("id", "1"),
                    new XAttribute("name", "Swapper"),
                    new XAttribute("description", "Check swap method."),
                    new XElement("Method",
                        new XAttribute("className", "SwapWithXML.abSwap"),
                        new XAttribute("methodName", "Swap"),
                        new XElement("Signature",
                            new XElement("ReturnType",
                                new XAttribute("type", "System.String")),
                            new XElement("Parameters",
                                new XElement("Param",
                                    new XAttribute("id", "1"),
                                    new XAttribute("direction", "input"),
                                    new XAttribute("type", "int")),
                                new XElement("Param",
                                    new XAttribute("id", "2"),
                                    new XAttribute("direction", "input"),
                                    new XAttribute("type", "int")))),
                        new XElement("TestSet",
                            new XElement("Test",
                                new XAttribute("id", "1"),
                                new XAttribute("description", "Check swap method with Int32."),
                                new XElement("Input",
                                    new XElement("Parameter", "1",
                                        new XAttribute("id", "1"),
                                        new XAttribute("type", "System.Int32")),
                                    new XElement("Parameter", "2",
                                        new XAttribute("id", "2"),
                                        new XAttribute("type", "System.Int32"))),
                                new XElement("Output",
                                    new XElement("Parameter", "2",
                                        new XAttribute("type", "System.String")))),
                            new XElement("Test",
                                new XAttribute("id", "2"),
                                new XAttribute("description", "Check swap method with String."),
                                new XElement("Input",
                                    new XElement("Parameter", "a",
                                        new XAttribute("type", "System.String")),
                                    new XElement("Parameter", "b",
                                        new XAttribute("type", "System.String"))),
                                new XElement("Output",
                                    new XElement("Parameter", "b",
                                        new XAttribute("type", "System.String"))))))));
            xmlDoc.Save("lib1.xml");
        }

        public void ReadTestingXML(string fileName)
        {
            XDocument xdoc = XDocument.Load(fileName);

            foreach (XElement testInput in xdoc.Descendants("TestSet").Elements("Test"))
            {
                XAttribute idAttribute = testInput.Attribute("id");
                XAttribute descAttribute = testInput.Attribute("description");
                XElement inElement = testInput.Element("Input");
                XElement outElement = testInput.Element("Output");

                if (idAttribute != null && descAttribute != null && inElement != null && outElement !=null)
                {
                    Console.WriteLine("Id: {0}", idAttribute.Value);
                    Console.WriteLine("Description: {0}", descAttribute.Value);
                    Console.WriteLine("In: {0}", inElement.Value);
                    Console.WriteLine("Out: {0}", outElement.Value);
                }
                Console.WriteLine();
            }
        }
    }
}
