using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Secure_TDD.Model.Contracts;
using Secure_TDD.ViewModel;

namespace Secure_TDD.Model
{
    class TreeNodeBuilder : IViewBuilder
    {
        private TreeItemVM _treeVM;

        public void Build(string projectName, Type[] types)
        {
            _treeVM = new TreeItemVM(projectName, null);

            foreach (Type referencedType in types)
            {
                TreeItemVM membersTree = GetClassTreeVM(referencedType);
                membersTree.Parent = _treeVM;
                _treeVM.Children.Add(membersTree);
            }
        }

        private static TreeItemVM GetClassTreeVM(Type referencedType)
        {
            TreeItemVM membersTreeNode = new TreeItemVM(referencedType.Name, null);
            if (referencedType.IsClass)
            {
                MethodInfo[] methods = referencedType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (MethodInfo method in methods)
                {
                    membersTreeNode.Children.Add(new TreeItemVM(method.Name, membersTreeNode, method));
                }
            }
            else
            {
                Type[] types = referencedType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (Type type in types)
                {
                    membersTreeNode.Children.Add(new TreeItemVM(type.Name, membersTreeNode));
                }
            }
            return membersTreeNode;
        }

        public TreeItemVM GetTreeVM()
        {
            return _treeVM;
        }
    }
}
