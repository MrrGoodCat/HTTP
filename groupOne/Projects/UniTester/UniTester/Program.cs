using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UniTester.model;

namespace UniTester
{
    class Program
    {
        static void Main(string[] args)
        {
            InputsProcessor ip = new InputsProcessor(@"D:\_NICE_\AutomationTraining\springfield\groupOne\Projects\UniTester\UniTester\Task\Task.xml");
            var studentList = ip.GetStudentsList();
            var FirstStudentFileList = ip.GetStudentFilesList(studentList[0], "*.txt");
            Console.ReadKey();
        }

    }
}
