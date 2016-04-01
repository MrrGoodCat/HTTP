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
        static public Task[] tasks;

        static void Main(string[] args)
        {
            tasks = new Task[1];
            //Initialization(tasks);

            TesterXMLSerializer<Task[]> taskSerializer = new TesterXMLSerializer<Task[]>();
            //taskSerializer.Serialize(tasks, @"D:\1\Task.xml");

            tasks = taskSerializer.Deserialize(@"D:\_NICE_\AutomationTraining\springfield\groupOne\Projects\UniTester\UniTester\Task\Task.xml");

            //TesterLinqXML load = new TesterLinqXML();
            //load.ReadTestingXML("Task.xml");
            Console.ReadKey();
        }

        static void Initialization(Task[] tasks)
        {
            tasks[0] = new Task();
            tasks[0].Id = 5;
            tasks[0].Description = "Task Description";
            tasks[0].Name = "Task Name";
        }
    }
}
