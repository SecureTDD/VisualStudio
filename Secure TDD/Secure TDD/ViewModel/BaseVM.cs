using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Secure_TDD.ViewModel
{
    public class BaseVM : INotifyPropertyChanged
    {
        # region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string p_propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p_propertyName));
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> p_propertyExpression)
        {
            var memberExpression = p_propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("p_propertyExpression should represent access to a member.");
            }

            string memberName = memberExpression.Member.Name;
            RaisePropertyChanged(memberName);
        }

        # endregion INotifyPropertyChanged

        # region Navigation Commands

        private bool _canNavigateForward;
        public bool CanNavigateForward
        {
            get { return _canNavigateForward; }
            protected set
            {
                _canNavigateForward = value;
                RaisePropertyChanged(() => CanNavigateForward);
            }
        }

        private bool _canNavigateBack;
        public bool CanNavigateBack
        {
            get { return _canNavigateBack; }
            protected set
            {
                _canNavigateBack = value;
                RaisePropertyChanged(() => CanNavigateBack);
            }
        }

        # endregion Navigation Commands
    }
}
