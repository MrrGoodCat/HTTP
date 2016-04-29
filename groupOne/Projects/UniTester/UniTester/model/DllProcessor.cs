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
            int i = 1;
            Type[] testTypes = new Type[i];


            foreach (Type t in types)
            {
                MethodInfo[] methods = t.GetMethods();
                
                foreach (MethodInfo method in methods)
                {
                    if (method.Name.ToLower().Contains(MethodName) || method.Name.ToUpper().Contains(MethodName))
                    {
                        testTypes[i-1] = t;
                        i++;
                    }
                }
            }

            return testTypes;
        }

        private MethodInfo[] GetMethodsInfo(Type type, Task.Method Method)
        {
            
            string MethodName = Method.MethodName;
            MethodInfo[] methods = type.GetMethods();
            int i = 1;
            MethodInfo[] testMethods = new MethodInfo[i];

            foreach (MethodInfo method in methods)
            {
                if (method.Name.ToLower().Contains(MethodName) || method.Name.ToUpper().Contains(MethodName))
                {
                    testMethods[i-1] = method;
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

                    methodToTest = method;
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
                    if (newParameters[i].ParameterType.FullName.ToString().ToLower().Contains(mySignature.Parameters[i].Type))
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

        private Type getParameterType(MethodInfo TestingMethod, Task.Method.Signature.Parameter ParameterToTest)
        {
            Type ParameterType = null;
            int ParametersArrLength = TestingMethod.GetParameters().Length;
            
            foreach(ParameterInfo parameter in TestingMethod.GetParameters())
            {
                if (parameter.ParameterType.FullName.ToLower().Contains(ParameterToTest.Type) &&
                    parameter.Position == ParameterToTest.Id-1)
                {
                    ParameterType = parameter.ParameterType;
                    return ParameterType;
                }
                /// else --exception of incorrect method parameters
            }

       return null;

       }

        private object[] createParametersArr(MethodInfo TestingMethod, Task.Method.Signature.Parameter[] parameters)
        {
            object[] ParametersArr = new object[parameters.Length];
            ParameterInfo[] Param = new ParameterInfo[parameters.Length];

            for (int i = 0; i < ParametersArr.Length; i++)
            {
                Type ParameterType = getParameterType(TestingMethod, parameters[i]);
                ParametersArr[i] = Activator.CreateInstance(ParameterType);
                ParametersArr[i] = parameters[i].Value;
                ParametersArr[i] = Convert.ChangeType(ParametersArr[i], ParameterType);
                
            }

            foreach(ParameterInfo Parameter in TestingMethod.GetParameters())
            {
                for (int i = 0; i < ParametersArr.Length; i++)
                {
                    
                    if (Parameter.IsOut)
                    {
                        Param[i] = Parameter;
                    }
                }
                
            }


            return ParametersArr;
            
        }
       
        public object RunMethod(Task.Method TestMethod, Task.Method.Signature.Parameter[] parameters)
        {
            object instance = new object();
            MethodInfo TestingMethod = null;
            object[] TestParameters = new object[parameters.Length];
            Type TestType = null;

            foreach (Type type in GetTypesByMethod(assembly, TestMethod))
            {
                MethodInfo method = GetMethodBySignature(GetMethodsInfo(type, TestMethod), TestMethod.MethodSignature);
                if (method != null)
                {
                    TestType = type;
                    if (!method.IsGenericMethodDefinition)
                        TestingMethod = method;
                    else
                    {
                        Type[] GenericArguments = TestingMethod.GetGenericArguments();
                        TestingMethod = method.MakeGenericMethod(GenericArguments);

                        //// ====How to get generic type correctly????=====
                        
                    }

                    TestParameters = createParametersArr(TestingMethod, parameters);
                    
                    instance = Activator.CreateInstance(TestType);
                }
                
            }
            
            
            object testmethod = TestingMethod.Invoke(instance, TestParameters);

            return testmethod;
        }

        
    }

     
}
