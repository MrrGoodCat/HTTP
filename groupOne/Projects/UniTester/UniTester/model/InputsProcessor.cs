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
        public string TaskName { get; }
        public string TaskPath { get; }
        public string TaskFullName { get; }
        public List<Task> Tasks { get; }

        /// <summary>
        /// Initialize InputProcessor with the given task full name. Set TaskName and TaskPath. Load CurrentTask from XML.
        /// </summary>
        /// <param name="taskFullName">Task full name including path.</param>
        public InputsProcessor(string taskFullName)
        {
            TaskFullName = taskFullName;
            TaskName = Path.GetFileName(taskFullName);
            TaskPath = Path.GetDirectoryName(taskFullName);
            Tasks = LoadTaskFromXML(taskFullName);
        }

        private List<Task> LoadTaskFromXML(string taskFullName)
        {
            TesterXMLSerializer<List<Task>> xmlSerializer = new TesterXMLSerializer<List<Task>>();
            return xmlSerializer.Deserialize(TaskFullName);
        }

        /// <summary>
        /// Get the list of Students(Folder names) that are placed near the Task.xml.
        /// </summary>
        /// <returns>List of Students(Folder names) that are placed near the Task.xml</returns>
        public List<string> GetStudentsList()
        {
            List<string> students = new List<string>();
            foreach (var dir in Directory.GetDirectories(TaskPath))
            {
                students.Add(new DirectoryInfo(dir).Name);
            }
            return students;
        }

        /// <summary>
        /// Get the list of all files according to file filter (e.g. "*.dll").
        /// </summary>
        /// <param name="studentFolder">Name of the Student folder</param>
        /// <param name="filePattern">String file filter. For example "*.dll".</param>
        /// <returns></returns>
        public List<string> GetStudentFilesList(string studentFolder, string filePattern)
        {
            List<string> files = new List<string>();
            foreach (var file in Directory.GetFiles(String.Format($@"{TaskPath}\{studentFolder}"), filePattern))
            {
                files.Add(file);
            }
            return files;
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
