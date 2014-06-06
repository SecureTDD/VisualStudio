using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secure_TDD.ViewModel
{
    public class MethodParamsVM : BaseVM
    {
        public MethodParamsVM(ObservableCollection<MethodIOParameterVM> p_params, bool p_isInput)
        {
            _ioParams = p_params;
            _isInput = p_isInput;

            if (p_isInput)
            {
                _name = "Inputs:";
            }
            else
            {
                _name = "Output:";
            }
        }

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

        private ObservableCollection<MethodIOParameterVM> _ioParams;
        public ObservableCollection<MethodIOParameterVM> IOParams
        {
            get { return _ioParams; }
            set
            {
                _ioParams = value;
                RaisePropertyChanged(() => IOParams);
            }
        }
    }
}
