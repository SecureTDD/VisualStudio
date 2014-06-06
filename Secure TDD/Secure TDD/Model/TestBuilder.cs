using EnvDTE;
using EnvDTE80;
using Secure_TDD.Model.Entities;
using Secure_TDD.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSLangProj;

namespace Secure_TDD.Model
{
    public class TestBuilder
    {
        /// <summary>
        /// Creates a test file named 'SecuredTests.cs' that contains all the test methods.
        /// </summary>
        /// <param name="p_methods"></param>
        public static void CreateTestFile(Dictionary<TestingMethodVM, List<TestConfiguration>> p_methods)
        {
            DTE2 dte = (DTE2)Secure_TDDPackage.GetGlobalService(typeof(DTE));
            Solution2 soln = (Solution2)dte.Solution;
            Project currentProj = dte.ActiveDocument.ProjectItem.ContainingProject;
            Project proj = GetTestingProject(currentProj);
            ProjectItem pi = GetCSTestFile(proj, soln);
            FileCodeModel2 fcm = (FileCodeModel2)pi.FileCodeModel;

            if (!IsContainImports(fcm))
            {
                fcm.AddImport("System");
                fcm.AddImport("System.Linq");
                fcm.AddImport("System.Text");
                fcm.AddImport("System.Collections.Generic");
                fcm.AddImport("Microsoft.VisualStudio.TestTools.UnitTesting");
            }

            string namespaceName = "Testing";
            CodeNamespace testingNameSpace = GetNamespace(fcm, namespaceName);

            if (testingNameSpace != null)
            {
                string className = "SecuredTestClass";
                CodeClass2 securedClass = GetClass(fcm, testingNameSpace, className);

                foreach (var methodKeyValue in p_methods)
                {
                    var startIndexOfMethodName = methodKeyValue.Key.FullName.LastIndexOf('.');
                    var methodBaseName = methodKeyValue.Key.FullName.Substring(startIndexOfMethodName + 1) + "_{0}Test";

                    foreach (var testConfiguration in methodKeyValue.Value)
                    {
                        // Remove all white-spaces in the test name.
                        var fixedName = testConfiguration.Name.Replace(" ", string.Empty);
                        var methodName = string.Format(methodBaseName, fixedName);
                        var functionBody = GetMethodBody(methodKeyValue.Key, testConfiguration);
                        AddFunction(securedClass, methodName, functionBody);
                    }
                }
            }

            // if the file is not opened in the text editor, open and activate it.
            Window window = pi.Open(Constants.vsViewKindCode);
            if (window != null)
            {
                window.Activate();
                dte.ExecuteCommand("Edit.FormatDocument");
            }

            // Save the file.
            pi.Save();
        }

        private static Project GetTestingProject(Project p_referencedProject)
        {
            Project testingProject = null;
            string projectName = "UnitTesting";
            DTE2 dte = (DTE2)Secure_TDDPackage.GetGlobalService(typeof(DTE));
            Solution2 soln = (Solution2)dte.Solution;

            foreach (Project proj in soln.Projects)
            {
                if (proj.Name == projectName)
                {
                    testingProject = proj;
                    break;
                }
            }

            if (testingProject == null)
            {
                string unitTestProjectTemplatePath;
                unitTestProjectTemplatePath = soln.GetProjectTemplate("UnitTestProject", "CSharp");
                string projectDestinationPath = soln.FullName.Remove(soln.FullName.LastIndexOf('\\'));
                projectDestinationPath += "\\" + projectName;
                soln.AddFromTemplate(unitTestProjectTemplatePath, projectDestinationPath, projectName);

                foreach (Project proj in soln.Projects)
                {
                    if (proj.Name == projectName)
                    {
                        testingProject = proj;
                        break;
                    }
                }

                VSProject vsProj = testingProject.Object as VSProject;
                vsProj.References.AddProject(p_referencedProject);

            }

            return testingProject;
        }


        /// <summary>
        /// Get a c-sharp (*.cs) file named 'SecuredTests.cs' in the specified solution and project. Creates new file if no exists.
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="soln"></param>
        /// <returns></returns>
        private static ProjectItem GetCSTestFile(Project proj, Solution2 soln)
        {
            ProjectItem pi = null;

            foreach (ProjectItem item in proj.ProjectItems)
            {
                if (item.Name == "SecuredTests.cs")
                {
                    pi = item;
                    break;
                }
            }

            if (pi == null)
            {
                string csItemTemplatePath;
                csItemTemplatePath = soln.GetProjectItemTemplate("CodeFile", "CSharp");
                proj.ProjectItems.AddFromTemplate(csItemTemplatePath, "SecuredTests.cs");
            }

            pi = proj.ProjectItems.Item("SecuredTests.cs");
            return pi;
        }

        private static bool IsContainImports(FileCodeModel2 p_fcm)
        {
            bool isContains = false;

            foreach (CodeElement2 codeElement in p_fcm.CodeElements)
            {
                if (codeElement.Kind == vsCMElement.vsCMElementImportStmt)
                {
                    isContains = true;
                    break;
                }
            }

            return isContains;
        }

        /// <summary>
        /// Get the specified namespace from the given file code model. Creates new namespace if no exists.
        /// </summary>
        /// <param name="p_fcm"></param>
        /// <param name="p_namespaceName"></param>
        /// <returns></returns>
        private static CodeNamespace GetNamespace(FileCodeModel2 p_fcm, string p_namespaceName)
        {
            CodeNamespace testingNameSpace = null;

            foreach (CodeElement2 codeElement in p_fcm.CodeElements)
            {
                testingNameSpace = FindNamespaceRecursive(codeElement, p_namespaceName);
                if (testingNameSpace != null) break;
            }

            if (testingNameSpace == null)
            {
                testingNameSpace = p_fcm.AddNamespace(p_namespaceName, -1);
            }
            return testingNameSpace;
        }

        /// <summary>
        /// Search for the namespace recursivly (deep search) in the given code element.
        /// </summary>
        /// <param name="p_codeElement"></param>
        /// <param name="p_namespaceName"></param>
        /// <returns></returns>
        private static CodeNamespace FindNamespaceRecursive(CodeElement2 p_codeElement, string p_namespaceName)
        {
            CodeNamespace foundClass = null;

            if (p_codeElement.Kind == vsCMElement.vsCMElementNamespace && p_codeElement.Name == p_namespaceName)
            {
                foundClass = p_codeElement as CodeNamespace;
            }
            else if (p_codeElement.Children != null && p_codeElement.Children.Count > 0)
            {
                foreach (CodeElement2 child in p_codeElement.Children)
                {
                    foundClass = FindNamespaceRecursive(child, p_namespaceName);
                    if (foundClass != null) break;
                }
            }

            return foundClass;
        }

        /// <summary>
        /// Get the specified class from the given namespace. Creates new class if no exists.
        /// </summary>
        /// <param name="p_fcm"></param>
        /// <param name="p_testingNameSpace"></param>
        /// <param name="p_className"></param>
        /// <returns></returns>
        private static CodeClass2 GetClass(FileCodeModel2 p_fcm, CodeNamespace p_testingNameSpace, string p_className)
        {
            CodeClass2 securedClass = null;

            foreach (CodeElement2 codeElement in p_fcm.CodeElements)
            {
                securedClass = FindClassRecursive(codeElement, p_className);
                if (securedClass != null) break;
            }

            if (securedClass == null)
            {
                // Add a class to the namespace.
                securedClass = (CodeClass2)p_testingNameSpace.AddClass(p_className, -1, null, null, vsCMAccess.vsCMAccessPublic);
                securedClass.DataTypeKind = vsCMDataTypeKind.vsCMDataTypeKindPartial;
                securedClass.AddAttribute("TestClass", string.Empty);
            }
            return securedClass;
        }

        /// <summary>
        /// Search for the class recursivly (deep search) in the given code element.
        /// </summary>
        /// <param name="p_codeElement"></param>
        /// <param name="p_className"></param>
        /// <returns></returns>
        private static CodeClass2 FindClassRecursive(CodeElement2 p_codeElement, string p_className)
        {
            CodeClass2 foundClass = null;

            if (p_codeElement.Kind == vsCMElement.vsCMElementClass && p_codeElement.Name == p_className)
            {
                foundClass = p_codeElement as CodeClass2;
            }
            else if (p_codeElement.Children != null && p_codeElement.Children.Count > 0)
            {
                foreach (CodeElement2 child in p_codeElement.Children)
                {
                    foundClass = FindClassRecursive(child, p_className);
                    if (foundClass != null) break;
                }
            }

            return foundClass;
        }

        /// <summary>
        /// Add function to the specified class with the given body.
        /// </summary>
        /// <param name="p_securedClass"></param>
        /// <param name="p_functionName"></param>
        /// <param name="p_functionBody"></param>
        private static void AddFunction(CodeClass2 p_securedClass, string p_functionName, string p_functionBody)
        {
            // Add a method with a parameter to the class.
            CodeFunction2 testFunction = (CodeFunction2)p_securedClass.AddFunction(p_functionName, vsCMFunction.vsCMFunctionFunction, "void", -1, vsCMAccess.vsCMAccessPublic, null);
            testFunction.AddAttribute("TestMethod", string.Empty);

            var startPoint = testFunction.GetStartPoint(vsCMPart.vsCMPartBody);
            var endPoint = testFunction.GetEndPoint(vsCMPart.vsCMPartBody);
            var editPoint = startPoint.CreateEditPoint() as EditPoint2;

            // Clear the default generated body
            editPoint.Delete(endPoint);
            editPoint.Indent(null, 1);

            // Write the body of the function
            editPoint.Insert(p_functionBody);
            editPoint.InsertNewLine();

            endPoint = testFunction.GetEndPoint(vsCMPart.vsCMPartBody);
            editPoint.SmartFormat(endPoint);
        }

        private static string GetMethodBody(TestingMethodVM p_method, TestConfiguration p_configuration)
        {
            StringBuilder methodBody = new StringBuilder();

            var methodParams =p_method.Params.SelectMany(p => p.IOParams).Where(p => p.IsChecked &&
                p.IsInput == p_configuration.Conditions.Any(r => r.Key == Rule.ContainsUserInput && r.Value));

            for (int index = 0; index < methodParams.Count(); index ++ )
            {
                methodBody.AppendLine();
                string baseCode = p_configuration.TestCode.Code;

                foreach (var param in p_configuration.TestParameters)
                {
                    if (index > 0 && param.ParameterType == ParameterType.TestingParameterIndex)
                    {
                        baseCode = baseCode.Replace(param.Value, index.ToString());
                    }

                    var expectedValue = ExtractValue(p_method.Method, methodParams.ElementAt(index), param.ParameterType);
                    baseCode = baseCode.Replace(param.Value, expectedValue);
                }

                methodBody.AppendLine(baseCode);
            }

            return methodBody.ToString();
        }

        private static string ExtractValue(MethodInfo p_method, MethodIOParameterVM p_parameter, ParameterType p_parameterType)
        {
            string value = string.Empty;

            switch (p_parameterType)
            {
                case ParameterType.Class:
                    value = p_method.DeclaringType.FullName;
                    break;
                case ParameterType.ConstractorParam:
                    var ctors = p_method.DeclaringType.GetConstructors();
                    var minCtorParamsCount = ctors.Min(c => c.GetParameters().Count());
                    if (minCtorParamsCount > 0)
                    {
                        var ctor = ctors.FirstOrDefault(c => c.GetParameters().Count() == minCtorParamsCount);
                        var ctorParams = ctor.GetParameters();

                        var listOfParams = ctorParams.Select(c => GetDefault(c.ParameterType));
                        value = string.Join(", ", listOfParams);
                    }

                    break;
                case ParameterType.Method:
                    value = p_method.Name;
                    break;
                case ParameterType.FirstParameters:
                case ParameterType.SecondParameters:
                    var methodParams = p_method.GetParameters();

                    List<string> firstParams = new List<string>();
                    List<string> secondParams = null;

                    foreach (var param in methodParams)
                    {
                        if (param == p_parameter.ParameterInfo)
                        {
                            secondParams = new List<string>();
                        }
                        else if (secondParams != null)
                        {
                            secondParams.Add(GetDefault(param.ParameterType));
                        }
                        else
                        {
                            firstParams.Add(GetDefault(param.ParameterType));
                        }
                    }

                    if (p_parameterType == ParameterType.FirstParameters && firstParams.Any())
                    {
                        firstParams.Add(" ");
                        value = string.Join(", ", firstParams);
                    }

                    if (p_parameterType == ParameterType.SecondParameters && secondParams != null && secondParams.Any())
                    {
                        secondParams.Insert(0, " ");
                        value = string.Join(", ", secondParams);
                    }

                    break;
                case ParameterType.TestingParameterIndex:
                    break;
                default:
                    break;
            }

            return value;
        }

        public static string GetDefault(Type type)
        {
            string value = string.Empty;

            if (type.IsValueType)
            {
                if (type.IsEnum)
                {
                    value = type.Name + ".";
                }

                value += Activator.CreateInstance(type).ToString();
            }
            else
            {
                value = "null";
            }

            return value;
        }
    }
}
