using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UniTester.View;

namespace UniTester.Controller
{
    public class OperationsProcessor
    {
        public event Action<string> PathVerification;

        public string[] GetListOfDll(string Path)
        {
            string[] ArrDll = new string[0];
            string[] Arr = new string[10];
            string dll = ".dll";
            Arr = Directory.GetFiles(Path);
            for (int i = 0; i < Arr.Length; i++)
            {
                if (Arr[i].Contains(dll))
                {
                    Array.Resize(ref ArrDll, ArrDll.Length + 1);
                    ArrDll[ArrDll.Length - 1] = Arr[i].Remove(0, Path.Length);
                }
            }
            return ArrDll;
        }

        public string GetTaskFolderPath(string TaskFolderPath)
        {
            if (!Directory.Exists(TaskFolderPath))
            {
                PathVerification(string.Format("{0} is not a valid directory."));
            }
            return TaskFolderPath;
        }

    }
}
