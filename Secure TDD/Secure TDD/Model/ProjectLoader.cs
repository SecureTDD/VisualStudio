using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.CSharp;
using Secure_TDD.Model.Entities;
using VSLangProj;


namespace Secure_TDD.Model
{
    class ProjectLoader
    {
        public ProjectLoader(DTE2 dte)
        {
            var solution = dte.Solution as Solution2;
            solution.SolutionBuild.Clean(true);

            Project project = dte.ActiveDocument.ProjectItem.ContainingProject;
            WizardResources.ProjectName = project.Name;
            string projectPath = GetProjectPath(project);
            List<string> sourceFiles = GetSourceFiles(projectPath);
            CompilerParameters compilerParamenters = InitCompiler(project);
            WizardResources.compilerResults = CompileAssemblies(compilerParamenters,sourceFiles);
            SetPublicTypes();
        }

        private string GetProjectPath(Project project)
        {
            if (project.Properties != null)
            {
                foreach (Property property in project.Properties)
                    if (property.Name.Equals("FullPath"))
                        return (string)property.Value;
            }
            return "";
        }

        private List<string> GetSourceFiles(string path)
        {
            List<string> sources = new List<string>();
            GetSourceFilesAndDirectoriesRecursively(sources, path);
            return sources;
        }

        private void GetSourceFilesAndDirectoriesRecursively(List<string> listOfItems, string path)
        {
            foreach (string file in Directory.GetFiles(path, "*.cs"))
                listOfItems.Add(file);
            foreach (string directory in Directory.GetDirectories(path))
                GetSourceFilesAndDirectoriesRecursively(listOfItems, directory);
        }

        private CompilerParameters InitCompiler(Project project)
        {
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.TreatWarningsAsErrors = false;
            References references = ((VSProject)(project.Object)).References;
            foreach (Reference reference in references)
                if (!reference.Path.ToLower().Contains("mscorlib.dll"))
                    parameters.ReferencedAssemblies.Add(reference.Path);
            return parameters;
        }

        private CompilerResults CompileAssemblies(CompilerParameters parameters, List<string> sources)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromFile(parameters, sources.ToArray());

            if (results.Errors.HasErrors)
            {
                foreach (var error in results.Errors)
                {
                    Console.WriteLine(error);
                }

                throw new LoadException("Unable to load project. Please make sure it is compilable.");
            }

            return results;
        }

        private void SetPublicTypes()
        {
            Assembly assembly = WizardResources.compilerResults.CompiledAssembly;
            WizardResources.ReflectedTypes =  assembly.GetTypes();
        }
    }
}
