using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Secure_TDD.ViewModel
{
    public class MethodIOParameterVM : BaseVM
    {
        public MethodIOParameterVM(ParameterInfo p_parameterInfo, bool p_isInput)
        {
            _parameterInfo = p_parameterInfo;
            _isInput = p_isInput;
            _ioTypeName = p_parameterInfo.ParameterType.Name;
            _parameterName = p_parameterInfo.Name;

            if (!p_isInput)
            {
                _parameterName = "ReturnValue";
            }
        }

        private ParameterInfo _parameterInfo;
        public ParameterInfo ParameterInfo
        {
            get { return _parameterInfo; }
        }

        private bool _isInput;
        public bool IsInput
        {
            get { return _isInput; }
            set
            {
                _isInput = value;
                RaisePropertyChanged(() => IsInput);
            }
        }

        private string _ioTypeName;
        public string IOTypeName
        {
            get { return _ioTypeName; }
            set
            {
                _ioTypeName = value;
                RaisePropertyChanged(() => IOTypeName);
            }
        }

        private string _parameterName;
        public string ParameterName
        {
            get { return _parameterName; }
            set
            {
                _parameterName = value;
                RaisePropertyChanged(() => ParameterName);
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }

        /// <summary>
        /// The current IO parameter is enabled to be checked only if it is an output parameter or its type is System.String.
        /// </summary>
        public bool IsEnabled
        {
            get { return !IsInput || _parameterInfo.ParameterType == typeof(string); }
        }
    }
}
