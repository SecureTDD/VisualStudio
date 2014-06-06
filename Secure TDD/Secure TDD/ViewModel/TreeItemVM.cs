using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Secure_TDD.ViewModel
{
    public class TreeItemVM : BaseVM
    {
        # region Ctor

        public TreeItemVM(string p_name, TreeItemVM p_parent, MethodInfo p_method = null)
        {
            _name = p_name;
            _parent = p_parent;
            _children = new ObservableCollection<TreeItemVM>();

            _isSelected = false;
            if (p_method != null)
            {
                _isMethod = true;
                _method = p_method;
            }
        }

        # endregion Ctor

        # region Properties

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private bool? _isSelected;
        public bool? IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                UpdateChildren();
                UpdateParent(!value.HasValue);

                RaisePropertyChanged(() => IsSelected);
            }
        }

        private bool _isMethod;
        public bool IsMethod
        {
            get { return _isMethod; }
            private set
            {
                _isMethod = value;
                RaisePropertyChanged(() => IsMethod);
            }
        }

        private MethodInfo _method;
        public MethodInfo Method
        {
            get { return _method; }
            private set
            {
                _method = value;
                RaisePropertyChanged(() => Method);
            }
        }

        private TreeItemVM _parent;
        public TreeItemVM Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                RaisePropertyChanged(() => Parent);
            }
        }

        private ObservableCollection<TreeItemVM> _children;
        public ObservableCollection<TreeItemVM> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                RaisePropertyChanged(() => Children);
            }
        }

        # endregion Properties

        # region Update CheckBoxes Chain Methods

        private void UpdateParent(bool p_isNotDetermind)
        {
            if (Parent != null)
            {
                if (p_isNotDetermind)
                {
                    _parent.SetSelectionByChild(null);
                }
                else
                {
                    var hasChildNotdetermind = Parent.Children.Any(child => !child.IsSelected.HasValue);
                    var hasChildSelected = Parent.Children.Any(child => child.IsSelected.HasValue && child.IsSelected.Value);
                    var hasChildNotSelected = Parent.Children.Any(child => child.IsSelected.HasValue && !child.IsSelected.Value);

                    if (hasChildNotdetermind || (hasChildSelected && hasChildNotSelected))
                    {
                        _parent.SetSelectionByChild(null);
                    }
                    else if (!hasChildNotdetermind)
                    {
                        if (!hasChildSelected)
                        {
                            _parent.SetSelectionByChild(false);
                        }
                        else
                        {
                            _parent.SetSelectionByChild(true);
                        }

                    }
                }

                RaisePropertyChanged(() => IsSelected);
            }
        }

        private void UpdateChildren()
        {
            if (IsSelected.HasValue)
            {
                foreach (var child in Children)
                {
                    child.SetSelectionByParent(IsSelected.Value);
                }
            }
        }

        private void SetSelectionByParent(bool p_isSelected)
        {
            _isSelected = p_isSelected;
            RaisePropertyChanged(() => IsSelected);

            UpdateChildren();
        }

        private void SetSelectionByChild(bool? p_isSelected)
        {
            _isSelected = p_isSelected;
            RaisePropertyChanged(() => IsSelected);

            UpdateParent(!IsSelected.HasValue);
        }

        # endregion Update CheckBoxes Chain Methods

        # region Methods

        /// <summary>
        /// Gets the full name of this item,
        /// e.g: this item name is <methofdName>, returns <namespaceName>.<className>.<mathodName>
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            string fullName = Name;

            if (Parent != null)
            {
                fullName = string.Format("{0}.{1}", Parent.GetFullName(), fullName);
            }

            return fullName;
        }

        # endregion Methods
    }
}
