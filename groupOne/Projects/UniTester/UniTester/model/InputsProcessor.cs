using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace UniTester.model
{
    class InputsProcessor
    {
        public InputsProcessor(string taskFullName)
        {

        }


        public List<string> GetStudentsList(string taskPath)
        {
            List<string> students = new List<string>();
            foreach (var dir in Directory.GetDirectories(taskPath))
            {
                students.Add(Path.GetDirectoryName(dir));
            }
            return students;
        }

        public string GetDllFullName()
        {
            return null;
        }
    }


    public class Task
    {
        [XmlAttribute]
        public int Id { set; get; }
        [XmlAttribute]
        public string Name { set; get; }
        [XmlAttribute]
        public string Description { set; get; }
        [XmlElement("Method")]
        public Method MethodToTest { set; get; }

        public struct Method
        {
            [XmlAttribute]
            public string ClassName { set; get; }
            [XmlAttribute]
            public string MethodName { set; get; }
            [XmlElement("testSet")]
            public Test[] TestSet { set; get; }
            [XmlElement("Signature")]
            public Signature MethodSignature { set; get; }

            public struct Signature
            {
                [XmlElement("MethodReturn")]
                public MethodReturn Return { set; get; }
                [XmlElement("Parameters")]
                public Parameter[] Parameters { set; get; }

                public struct MethodReturn
                {
                    [XmlAttribute]
                    public string Type { set; get; }
                    [XmlAttribute]
                    public string Value { set; get; }
                }

                public struct Parameter
                {
                    [XmlAttribute]
                    public int Id { set; get; }
                    [XmlAttribute]
                    public string Direction { set; get; }
                    [XmlAttribute]
                    public string Type { set; get; }
                    [XmlAttribute]
                    public string Value { set; get; }
                }
            }
        }

    }


    public class Test
    {
        [XmlAttribute]
        public string Id { set; get; }
        [XmlAttribute]
        public string Description { set; get; }
        public Input[] Inputs { set; get; }
        public Results ExpectedResults { set; get; }
        [XmlIgnore]
        public Results ActualResults { set; get; }
        [XmlIgnore]
        public State TestState { set; get; }

        public enum State
        {
            NotRun,
            Passed,
            Failed
        }
                
        public struct Input
        {
            public Task.Method.Signature.Parameter[] Parameters { set; get; }
        }

        public struct Results
        {
            public Task.Method.Signature.MethodReturn Return { set; get; }
        }


    }
}
