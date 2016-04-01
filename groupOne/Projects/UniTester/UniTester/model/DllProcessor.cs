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

        private bool IsMethodInType(Type type, Task.Method Method)
        {
            string methodName = Method.MethodName;
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

        private Type[] GetTypesByMethod(Assembly asm, Task.Method Method)

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

        private MethodInfo[] GetMethodsInfo(Type type, Task.Method Method)
        {
            MethodInfo[] testMethods = null;
            string MethodName = Method.MethodName;
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

        private MethodInfo GetMethodBySignature(MethodInfo[] methods, Task.Method.Signature Signature)
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

        public object RunMethod(MethodInfo method, Task.Method TestMethod, object[] parameters)
        {
            object instance = new object();

            MethodInfo TestingMethod = null;
            Type TestType = null;

            foreach (Type type in GetTypesByMethod(assembly, TestMethod))
            {
                if(GetMethodBySignature(GetMethodsInfo(type, TestMethod), TestMethod.MethodSignature) == method)
                {
                    TestType = type;
                    if (!method.IsGenericMethodDefinition)
                    {
                        instance = Activator.CreateInstance(type);
                        TestingMethod = method;
                    }
                    else
                    {
                        //TestingMethod = method.GetGenericMethodDefinition();
                        //Type[] GenericParams = TestingMethod.GetGenericArguments();

                        //// ====How to get generic type correctly????=====
                        
                        //instance = Activator.CreateInstance(type.GetGenericTypeDefinition());
                    }
                }
                
            }
            
            object testmethod = TestingMethod.Invoke(instance, parameters);


            return testmethod;
        }

        
    }

     
}
