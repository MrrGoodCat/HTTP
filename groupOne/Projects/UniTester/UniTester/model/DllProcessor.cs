using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UniTester.model
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

        public Type[] GetTypesByMethod(Assembly asm, Task.Method Method)

        {
            string MethodName = Method.MethodName;
            Type[] types = asm.GetTypes();
            Type[] testTypes = null;
            int i = 0;

            foreach (Type t in types)
            {
                MethodInfo[] methods = t.GetMethods();

                foreach (MethodInfo method in methods)
                {
                    if (method.Name.ToLower().Contains(MethodName) || method.Name.ToUpper().Contains(MethodName))
                    {
                        testTypes[i] = t;
                        i++;
                    }
                }
            }

            return testTypes;
        }

        public MethodInfo[] GetMethodsInfo(string MethodName, Type type)
        {
            MethodInfo[] testMethods = null;
            MethodInfo[] methods = type.GetMethods();
            int i = 0;

            foreach (MethodInfo method in methods)
            {
                if (method.Name.ToLower().Contains(MethodName) || method.Name.ToUpper().Contains(MethodName))
                {
                    testMethods[i] = method;
                    i++;
                }
            }

            return testMethods;
        }

        public MethodInfo GetMethodBySignature(MethodInfo[] methods, Task.Method.Signature Signature)
        {

            MethodInfo methodToTest = null;

            foreach (MethodInfo method in methods)
            {
                if (ValidateMethodBySignature(method, Signature))

                    return methodToTest;
            }

            return null;
        }

        private bool ValidateMethodBySignature(MethodInfo method, Task.Method.Signature Signature)
        {
            Task.Method.Signature mySignature = new Task.Method.Signature();

            mySignature = Signature;

            ParameterInfo[] newParameters = method.GetParameters();

            if (method.ReturnType.ToString() == mySignature.Return.Type
                && newParameters.Length == mySignature.Parameters.Length)
            {
                for (int i = 0; i < newParameters.Length; i++)
                {
                    if (newParameters[i].ParameterType.ToString() == mySignature.Parameters[i].Type)
                    {
                        if (newParameters[i].IsOut == mySignature.Parameters[i].IsOut)
                        {
                            return true;

                        }
                    }

                }
            }
            return false;
        }

        public object RunMethod(MethodInfo method, object[] parameters)
        {

            object instance = new object();
            MethodInfo TestingMethod = null;
            assembly.GetTypes();


            if (!method.IsGenericMethodDefinition)
            {
                instance = Activator.CreateInstance(method.GetType());
                TestingMethod = method;
            }
            else
            {
                //MethodInfo GenericMethod = method.GetGenericMethodDefinition();
                //instance = (method.GetType())Activator.CreateInstance(method.GetType());

            }
            /// if static, if generic...

            object testmethod = method.Invoke(instance, parameters);


            return testmethod;
        }

        //public static Type getTypes(Assembly asm, string TestClassName)
        //{
        //    Type[] types = asm.GetTypes();
        //    Type testType = null;

        //    foreach (Type t in types)
        //    {

        //        if (t.Name.ToLower().Contains(TestClassName))
        //        {
        //            testType = t;
        //            TestData.TypeToTest = t;
        //        }
        //    }

        //    return testType;
        //}

        //public static MethodInfo getMethodToTest(Assembly asm, string TestClassName, string TestMethodName)
        //{         
        //    Type myType = TestData.TypeToTest;

        //    MethodInfo TestMethod = null;


        //    var MemberTypes = myType.MemberType;

        //    MethodInfo[] methods = myType.GetMethods();

        //    foreach (MethodInfo method in methods)
        //    {
        //        if (method.Name.ToLower().Contains(TestMethodName))
        //        {
        //            if (method.ContainsGenericParameters)
        //                TestData.GenericArguments = method.GetGenericArguments();

        //            TestMethod = method;
        //            TestData.MethodToTest = method;
        //        }

        //    }

        //    return TestMethod;
        //}

    }
}
