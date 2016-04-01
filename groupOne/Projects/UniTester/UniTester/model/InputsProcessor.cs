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
        private string taskFullName;

        /// <summary>
        /// Initialize Processor that operates with 
        /// </summary>
        /// <param name="taskFullName"></param>
        public InputsProcessor(string taskFullName)
        {
            this.taskFullName = taskFullName;
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


        public string GetDllFullName(string studentName)
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

        public class Method
        {
            [XmlAttribute]
            public string ClassName { set; get; }
            [XmlAttribute]
            public string MethodName { set; get; }
            public Test[] TestSet { set; get; }
            [XmlElement("Signature")]
            public Signature MethodSignature { set; get; }

            public class Signature
            {
                [XmlElement("MethodReturn")]
                public MethodReturn Return { set; get; }
                public Parameter[] Parameters { set; get; }

                public class MethodReturn
                {
                    [XmlAttribute]
                    public string Type { set; get; }
                    [XmlAttribute]
                    public string Value { set; get; }
                }

                
                public class Parameter
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
        public int Id { set; get; }
        [XmlAttribute]
        public string Description { set; get; }
        public Task.Method.Signature.Parameter[] Inputs { set; get; }
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
                
        public class Results
        {
            [XmlElement("MethodReturn")]
            public Task.Method.Signature.MethodReturn Return { set; get; }
        }


    }
}
