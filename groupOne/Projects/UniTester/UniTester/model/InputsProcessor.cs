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
        /// Initialize InputProcessor with the given task full name.
        /// </summary>
        /// <param name="taskFullName">Task full name including path.</param>
        public InputsProcessor(string taskFullName)
        {
            this.taskFullName = taskFullName;
        }


        /// <summary>
        /// Get the list of Students(Folder names) that are placed near the Task.xml.
        /// </summary>
        /// <returns>List of Students(Folder names) that are placed near the Task.xml</returns>
        public List<string> GetStudentsList()
        {
            List<string> students = new List<string>();
            foreach (var dir in Directory.GetDirectories(taskFullName))
            {
                students.Add(Path.GetDirectoryName(dir));
            }
            return students;
        }


        public string GetDllNameOfStudent(string studentName)
        {
            Path.GetDirectoryName(dir)
            Directory.GetDirectories(taskFullName)
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
                    public bool IsOut { set; get; }
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
        public Results ActualResults { set; get; }
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
