using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Secure_TDD.Model;

namespace Secure_TDD.View
{
    /// <summary>
    /// Interaction logic for MatchingForm.xaml
    /// </summary>
    public partial class MatchingForm : UserControl
    {
        public MatchingForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WizardResources.ScopeWPF.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
