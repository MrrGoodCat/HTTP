using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Reflaction
{
    public class DllProcessor
    {
        private Assembly assembly;

        public DllProcessor(string dllname)
        {
            assembly = Assembly.LoadFrom(dllname);
        }

        private bool IsMethodInType(Type type, string methodName)
        {
            var MemberTypes = type.MemberType;
            MethodInfo[] methods = type.GetMethods();

            foreach (MethodInfo method in methods)
            {
                if (method.Name == methodName)
                {
                    return true;
                }
            }
            return false;
        }

        public static Type getTypes(Assembly asm, string TestClassName)
        {
            Type[] types = asm.GetTypes();
            Type testType = null;

            foreach (Type t in types)
            {

                if (t.Name.ToLower().Contains(TestClassName))
                {
                    testType = t;
                    TestData.TypeToTest = t;
                }
            }

            return testType;
        }

        public static MethodInfo getMethodToTest(Assembly asm, string TestClassName, string TestMethodName)
        {         
            Type myType = TestData.TypeToTest;

            MethodInfo TestMethod = null;
            

            var MemberTypes = myType.MemberType;

            MethodInfo[] methods = myType.GetMethods();

            foreach (MethodInfo method in methods)
            {
                if (method.Name.ToLower().Contains(TestMethodName))
                {
                    if (method.ContainsGenericParameters)
                        TestData.GenericArguments = method.GetGenericArguments();

                    TestMethod = method;
                    TestData.MethodToTest = method;
                }
                    
            }
            
            return TestMethod;
        }

    }
}
