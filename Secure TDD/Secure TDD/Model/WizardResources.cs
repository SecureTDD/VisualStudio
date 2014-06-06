using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Secure_TDD.View;

namespace Secure_TDD.Model
{
    class WizardResources
    {
        public static List<Type> SelectedCheckBoxes = new List<Type>();
        public static CompilerResults compilerResults;
        public static Type[] ReflectedTypes;
        public static string ProjectName;
        public static ScopeForm ScopeWPF;
        public static MatchingForm MatchingWPF;
        public static MainWindow MainWPF;

        public static Type GetTypeByString(string typeName)
        {
            foreach (Type type in ReflectedTypes)
            {
                if (type.Name.Equals(typeName))
                    return type;
            }
            return typeof(string);
        }

        public static void CloseView()
        {
            MainWPF.Close();
        }
    }
}
