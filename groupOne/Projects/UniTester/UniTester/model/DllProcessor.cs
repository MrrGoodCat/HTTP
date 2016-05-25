using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UniTester.model
{
    public class DllProcessor : Task
    {
        
        public Assembly assembly;

        public DllProcessor(string dllname)
        {
            assembly = Assembly.LoadFrom(dllname);
        }

        public Type[] GetTypesByMethod(Assembly asm, Method Method)

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
                    if (method.Name.ToLower().Contains(MethodName.ToLower()))
                    {
                        testTypes[i-1] = t;
                        i++;
                    }
                }
            }

            return testTypes;
        }

        public MethodInfo[] GetMethodsInfo(Type type, Method Method)
        {
            
            string MethodName = Method.MethodName;
            MethodInfo[] methods = type.GetMethods();
            int i = 1;
            MethodInfo[] testMethods = new MethodInfo[i];

            foreach (MethodInfo method in methods)
            {
                if (method.Name.ToLower().Contains(MethodName.ToLower()))
                {
                    testMethods[i-1] = method;
                    i++;
                }
            }

            return testMethods;
        }

        public MethodInfo GetMethodBySignature(MethodInfo[] methods, Method.Signature Signature)
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

        public bool ValidateMethodBySignature(MethodInfo method, Task.Method.Signature Signature)
        {
            Method.Signature mySignature = new Method.Signature();

            mySignature = Signature;

            ParameterInfo[] newParameters = method.GetParameters();

            if (method.ReturnType.ToString() == mySignature.Return.Type
                && newParameters.Length == mySignature.Parameters.Length)
            {
                for (int i = 0; i < newParameters.Length; i++)
                {
                    if (!newParameters[i].ParameterType.IsGenericParameter)
                    {
                        if (newParameters[i].ParameterType.FullName.ToString().ToLower().Contains(mySignature.Parameters[i].Type.ToLower()))
                        {
                            if (newParameters[i].IsOut == mySignature.Parameters[i].IsOut)
                            {
                                return true;

                            }
                        }
                    }

                    else
                    {
                        if (newParameters[i].IsOut == mySignature.Parameters[i].IsOut)
                            return true;                        
                    }
                    
                }
            }
            return false;
        }
        
    }

    public class TestMethodExecution<T> : DllProcessor where T : IConvertible, new()
    {

        public TestMethodExecution(string dllname) : base(dllname)
        {
        }

        private Type getParameterType(MethodInfo TestingMethod, Method.Signature.Parameter ParameterToTest)
        {
            Type ParameterType = null;
            int ParametersArrLength = TestingMethod.GetParameters().Length;

            foreach (ParameterInfo parameter in TestingMethod.GetParameters())
            {
                if (!parameter.ParameterType.IsGenericParameter)

                {
                    if (parameter.ParameterType.FullName.ToLower().Contains(ParameterToTest.Type.ToLower()) &&
                    parameter.Position == ParameterToTest.Id - 1)
                    {
                        ParameterType = parameter.ParameterType;
                        return ParameterType;
                    }

                }

                /// int for test
                else { ParameterType = typeof(int); }


                /// else --exception of incorrect method parameters
            }

            return null;

        }

        private object[] createParametersArr(MethodInfo TestingMethod, Method.Signature.Parameter[] parameters)
        {
            object[] ParametersArr = new object[parameters.Length];
            ParameterInfo[] Param = new ParameterInfo[parameters.Length];

            for (int i = 0; i < ParametersArr.Length; i++)
            {
                Type ParameterType = getParameterType(TestingMethod, parameters[i]);

                if (ParameterType != null)
                {
                    if (!ParameterType.ContainsGenericParameters)
                    {
                        ParametersArr[i] = Activator.CreateInstance(ParameterType);
                        ParametersArr[i] = parameters[i].Value;
                        ParametersArr[i] = Convert.ChangeType(ParametersArr[i], ParameterType);
                    }

                    else
                    {

                        ParameterType = ParameterType.MakeGenericType();
                        ParametersArr[i] = (T)Activator.CreateInstance(ParameterType, ParameterType.GetGenericTypeDefinition());
                        ParametersArr[i] = parameters[i].Value;
                        /// define the test type
                    }



                }

            }

            //foreach (ParameterInfo Parameter in TestingMethod.GetParameters())
            //{
            //    for (int i = 0; i < ParametersArr.Length; i++)
            //    {

            //        if (Parameter.IsOut)
            //        {
            //            Param[i] = Parameter;
            //        }
            //    }

            //}
            return ParametersArr;

        }

        public object RunMethod(Method TestMethod, Method.Signature.Parameter[] parameters)
        {
            object instance = new object();
            MethodInfo TestingMethod = null;
            object[] TestParameters = new object[parameters.Length];
            Type TestType = null;
            object testmethod = null;

            foreach (Type type in GetTypesByMethod(assembly, TestMethod))
            {
                MethodInfo method = GetMethodBySignature(GetMethodsInfo(type, TestMethod), TestMethod.MethodSignature);

                if (method != null)
                {
                    TestType = type;
                    TestingMethod = method;

                    if (!method.ContainsGenericParameters)
                    {

                        instance = Activator.CreateInstance(TestType);
                        TestParameters = createParametersArr(TestingMethod, parameters);
                        testmethod = TestingMethod.Invoke(instance, TestParameters);
                    }

                    else
                    {

                        TestType = type.MakeGenericType(typeof(T));

                        /// int type only for testing
                        TestingMethod = TestingMethod.MakeGenericMethod(typeof(int));
                        instance = Activator.CreateInstance(TestType);

                        //// ====How to get generic type correctly????=====
                        testmethod = (int)TestingMethod.Invoke(instance, TestParameters);

                    }

                }

            }

            return testmethod;
        }

    }
}
