﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTester
{
    class Program
    {
        static void Main(string[] args)
        {
            TesterLinqXML load = new TesterLinqXML();
            load.ReadTestingXML("Task.xml");
            Console.ReadKey();
        }
    }
}
