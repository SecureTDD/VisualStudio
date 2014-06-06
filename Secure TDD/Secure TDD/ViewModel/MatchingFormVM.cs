using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Secure_TDD.ViewModel
{
    public class MatchingFormVM : BaseVM
    {
        # region Ctor

        public MatchingFormVM()
        {
            CanNavigateBack = true;
            CanNavigateForward = true;
            AvailableMethods = new ObservableCollection<TestingMethodVM>();
        }

        # endregion Ctor

        # region Properties

        private ObservableCollection<TestingMethodVM> _availableMethods;
        public ObservableCollection<TestingMethodVM> AvailableMethods
        {
            get { return _availableMethods; }
            set
            {
                _availableMethods = value;
                RaisePropertyChanged(() => AvailableMethods);
            }
        }

        # endregion Properties

        # region Methods

        public void Load(List<TestingMethodVM> p_testingMethods)
        {
            if (p_testingMethods != null && p_testingMethods.Any())
            {
                var orderedMethods = p_testingMethods.OrderBy(method => method.FullName);
                AvailableMethods = new ObservableCollection<TestingMethodVM>(orderedMethods);
            }
        }

        # endregion Methods
    }
}
