using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Secure_TDD.ViewModel
{
    public class ScopeFormVM : BaseVM
    {
        # region Ctor

        public ScopeFormVM(TreeItemVM p_root)
        {
            if (p_root != null)
            {
                p_root.PropertyChanged += root_PropertyChanged;
            }

            _tree = new ObservableCollection<TreeItemVM> { p_root };
            CanNavigateBack = false;
        }

        # endregion Ctor

        # region Properties

        private ObservableCollection<TreeItemVM> _tree;
        public ObservableCollection<TreeItemVM> Tree
        {
            get { return _tree; }
            set
            {
                _tree = value;
                RaisePropertyChanged(() => Tree);
            }
        }

        # endregion Properties

        # region Methods

        void root_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var root = sender as TreeItemVM;
            CanNavigateForward = !root.IsSelected.HasValue || root.IsSelected.Value;
        }

        /// <summary>
        /// Extract all the selected methods.
        /// </summary>
        /// <returns>List of selected methods</returns>
        public List<TreeItemVM> ExtractSelectedMethods()
        {
            var results = new List<TreeItemVM>();

            var root = Tree.FirstOrDefault();
            if (root != null)
            {
                results.AddRange(ExtractSelectedMethods(root));
            }

            return results;
        }

        /// <summary>
        /// Extract all the selected methods that under the givet item in the tree-chain (Recursive).
        /// </summary>
        /// <returns>List of selected child methods</returns>
        private List<TreeItemVM> ExtractSelectedMethods(TreeItemVM p_treeItem)
        {
            var results = new List<TreeItemVM>();

            if (!p_treeItem.IsSelected.HasValue || p_treeItem.IsSelected.Value)
            {
                if (!p_treeItem.Children.Any() && p_treeItem.IsMethod)
                {
                    results.Add(p_treeItem);
                }
                else if (p_treeItem.Children.Any())
                {
                    var selectedCildren = p_treeItem.Children.Where(child => !child.IsSelected.HasValue || child.IsSelected.Value);
                    foreach (var child in selectedCildren)
                    {
                        results.AddRange(ExtractSelectedMethods(child));
                    }
                }
            }

            return results;
        }

        # endregion Methods
    }
}
