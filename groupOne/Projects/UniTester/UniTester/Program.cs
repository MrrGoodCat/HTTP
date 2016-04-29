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
            InputsProcessor ip = new InputsProcessor(@"E:\GitFolder\springfield\groupOne\Projects\UniTester\UniTester\Task\Task.xml");
            var studentList = ip.GetStudentsList();
            var FirstStudentFileList = ip.GetStudentFilesList(studentList[0], "*.txt");

            DllProcessor test = new DllProcessor(@"E:\GitFolder\springfield\groupOne\Projects\UniTester\UniTester\Task\Student1\Mult.dll");
           
            
            
           object Testing = test.RunMethod(ip.Tasks[0].MethodToTest, ip.Tasks[0].MethodToTest.MethodSignature.Parameters);
            Console.ReadKey();
        }

    }
}
