using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Secure_TDD.ViewModel
{
    public class TestingMethodVM : BaseVM
    {
        # region Ctor

        public TestingMethodVM(string p_fullName, MethodInfo p_method)
        {
            _fullName = p_fullName;
            _method = p_method;

            var outputParams = new ObservableCollection<MethodIOParameterVM> { new MethodIOParameterVM(p_method.ReturnParameter, false) };
            var inputParams = new ObservableCollection<MethodIOParameterVM>();
            var methodParams = p_method.GetParameters();
            if (methodParams != null && methodParams.Any())
            {
                foreach (var param in methodParams)
                {
                    inputParams.Add(new MethodIOParameterVM(param, true));
                }
            }


            Params = new ObservableCollection<MethodParamsVM>();
            Params.Add(new MethodParamsVM(inputParams, true));
            Params.Add(new MethodParamsVM(outputParams, false));
        }

        # endregion Ctor

        # region Properties

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                RaisePropertyChanged(() => FullName);
            }
        }

        private bool _isDALChecked = true;
        public bool IsDALChecked
        {
            get { return _isDALChecked; }
            set
            {
                _isDALChecked = value;
                RaisePropertyChanged(() => IsDALChecked);
            }
        }

        private bool _isBLChecked;
        public bool IsBLChecked
        {
            get { return _isBLChecked; }
            set
            {
                _isBLChecked = value;
                RaisePropertyChanged(() => IsBLChecked);
            }
        }

        private bool _isPLChecked;
        public bool IsPLChecked
        {
            get { return _isPLChecked; }
            set
            {
                _isPLChecked = value;
                RaisePropertyChanged(() => IsPLChecked);
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

        private ObservableCollection<MethodParamsVM> _params;
        public ObservableCollection<MethodParamsVM> Params
        {
            get { return _params; }
            set
            {
                _params = value;
                RaisePropertyChanged(() => Params);
            }
        }

        # endregion Properties
    }
}
