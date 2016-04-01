using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UniTester.Controller;

namespace UniTester.View
{
    class TesterUI
    {
        OperationsProcessor ControllerOperation = new OperationsProcessor();

        public void Start()
        {
            string Path = @"D:\DLLs\";
            foreach (var item in ControllerOperation.GetListOfDll(Path))
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        private void PathVerification(string message)
        {
            Console.WriteLine(message);
        }

        public string[] GetListOfStudents(string TaskFolder)
        {
            string[] ListOfStudents = null;
            ListOfStudents = Directory.GetDirectories(TaskFolder);
            return ListOfStudents;
        }

    }
}
