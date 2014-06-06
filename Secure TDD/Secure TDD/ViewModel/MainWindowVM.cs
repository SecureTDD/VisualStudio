using Secure_TDD.Model;
using Secure_TDD.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Secure_TDD.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        # region Data Members

        private ScopeFormVM _scopeForm;
        private MatchingFormVM _matchingForm;

        # endregion Data Members

        # region Ctor

        public MainWindowVM(ScopeFormVM p_viewModel)
        {
            _scopeForm = p_viewModel;
            _matchingForm = new MatchingFormVM();

            _scopeForm.PropertyChanged += Form_PropertyChanged;
            _matchingForm.PropertyChanged += Form_PropertyChanged;

            ViewModelToDisplay = p_viewModel;
        }

        # endregion Ctor

        # region Properties

        private BaseVM _viewModelToDisplay;
        public BaseVM ViewModelToDisplay
        {
            get { return _viewModelToDisplay; }
            set
            {
                _viewModelToDisplay = value;
                RaisePropertyChanged(() => ViewModelToDisplay);
                UpdateCanNavigate();
            }
        }

        # endregion Properties

        # region Commands

        private RelayCommand _previousCommand;
        public RelayCommand PreviousCommand
        {
            get
            {
                if (_previousCommand == null)
                {
                    _previousCommand = new RelayCommand(() =>
                    {
                        ViewModelToDisplay = _scopeForm;
                    });
                }

                return _previousCommand;
            }
        }

        private RelayCommand _nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(() =>
                    {
                        if (ViewModelToDisplay is ScopeFormVM)
                        {
                            NextToMatchingForm();
                        }
                        else if (ViewModelToDisplay is MatchingFormVM)
                        {
                            NextToCreateTestingFile();
                        }
                    });
                }

                return _nextCommand;
            }
        }

        # endregion Commands

        # region Methods

        void Form_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateCanNavigate();
        }

        private void UpdateCanNavigate()
        {
            CanNavigateBack = ViewModelToDisplay.CanNavigateBack;
            CanNavigateForward = ViewModelToDisplay.CanNavigateForward;
        }

        private void NextToCreateTestingFile()
        {
            Dictionary<TestingMethodVM, List<TestConfiguration>> methodsWithConfigs = new Dictionary<TestingMethodVM, List<TestConfiguration>>();

            var availableDALMethods = _matchingForm.AvailableMethods.Where(m => m.IsDALChecked || m.IsBLChecked);

            // Fill the dictionary with methods and its matching test configurations
            foreach (var method in availableDALMethods)
            {
                Rule? currentRule = null;

                var methodParams = method.Params.SelectMany(p => p.IOParams).Where(p => p.IsChecked);
                foreach (var param in methodParams)
                {
                    if (param.IsInput)
                    {
                        AddRule(ref currentRule, Rule.ContainsUserInput);
                    }
                    else
                    {
                        AddRule(ref currentRule, Rule.ContainsUserOutput);
                    }
                }

                if (currentRule.HasValue)
                {
                    var configTests = TestSelector.Instance.GetTestConfigurations(currentRule.Value);
                    methodsWithConfigs.Add(method, configTests);
                }
            }

            // If there are methods, create a secured test file.
            if (methodsWithConfigs.Any())
            {
                TestBuilder.CreateTestFile(methodsWithConfigs);
            }

            // Close the view.
            WizardResources.CloseView();
        }

        private void AddRule(ref Rule? p_currentRule, Rule p_ruleToAdd)
        {
            if (!p_currentRule.HasValue)
            {
                p_currentRule = new Rule?(p_ruleToAdd);
            }
            else
            {
                p_currentRule = p_currentRule | p_ruleToAdd;
            }
        }

        private void NextToMatchingForm()
        {
            var selectedMethods = _scopeForm.ExtractSelectedMethods();
            var testingMethods = CreateTestingMethodsVM(selectedMethods);
            _matchingForm.Load(testingMethods);
            ViewModelToDisplay = _matchingForm;
        }

        /// <summary>
        /// Converts tree items to testing methods.
        /// </summary>
        /// <param name="p_selectedMethods"></param>
        /// <returns></returns>
        private List<TestingMethodVM> CreateTestingMethodsVM(List<TreeItemVM> p_selectedMethods)
        {
            var resuts = new List<TestingMethodVM>();

            if (p_selectedMethods != null && p_selectedMethods.Any())
            {
                foreach (var method in p_selectedMethods)
                {
                    var testingMethod = new TestingMethodVM(method.GetFullName(), method.Method);
                    resuts.Add(testingMethod);
                }
            }

            return resuts;
        }

        # endregion Methods
    }
}
