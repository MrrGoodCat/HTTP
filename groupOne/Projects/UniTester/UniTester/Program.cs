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

            TestMethodExecution<int> test = new TestMethodExecution<int>(@"E:\GitFolder\springfield\groupOne\Projects\UniTester\UniTester\Task\Student1\Mult.dll");
           
            
            for(int i = 0; i < ip.Tasks[0].MethodToTest.TestSet.Length; i++)
            {
                object Testing = test.RunMethod(ip.Tasks[0].MethodToTest, ip.Tasks[0].MethodToTest.TestSet[i].Inputs, 
                    ip.Tasks[0].MethodToTest.MethodSignature.Return);
                Console.WriteLine(Testing.ToString());
                Console.ReadKey();
            }
           
        }

    }
}
