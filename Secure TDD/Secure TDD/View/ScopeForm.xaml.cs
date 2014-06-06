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
    /// Interaction logic for ScopeForm.xaml
    /// </summary>
    public partial class ScopeForm : UserControl
    {
        public ScopeForm()
        {
            InitializeComponent();
        }

        //public ScopeForm(System.Windows.Forms.TreeNode treeNode)
        //{
        //    InitializeComponent();
        //    DataTemplate template = GetHeaderTemplate();
        //    ScopeTreeView.Items.Add(ConvertToTreeViewItem(treeNode, template));
        //    ScopeTreeView.Items.Refresh();
        //    ScopeTreeView.UpdateLayout();
        //    // Use code from the following link to add chewckbox and folder/class icons:
        //    // http://www.codeproject.com/Articles/124644/Basic-Understanding-of-Tree-View-in-WPF
        //}

        //private TreeViewItem ConvertToTreeViewItem(System.Windows.Forms.TreeNode node, DataTemplate template)
        //{
        //    TreeViewItem treeViewItem = new TreeViewItem();
        //    treeViewItem.HeaderTemplate = template;
        //    treeViewItem.Header = node.Text;
        //    foreach (System.Windows.Forms.TreeNode childNode in node.Nodes)
        //    {
        //        treeViewItem.Items.Add(ConvertToTreeViewItem(childNode, template));
        //    }
        //    return treeViewItem;
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WizardResources.MatchingWPF = new MatchingForm();
            WizardResources.MatchingWPF.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        //private DataTemplate GetHeaderTemplate()
        //{
        //    DataTemplate dataTemplate = new DataTemplate();
        //    FrameworkElementFactory stackPanel = CreateStackPanel();
        //    CreateCheckBox(stackPanel);
        //    CreateText(stackPanel);
        //    dataTemplate.VisualTree = stackPanel;
        //    return dataTemplate;
        //}

        //private static void CreateText(FrameworkElementFactory stackPanel)
        //{
        //    FrameworkElementFactory label = new FrameworkElementFactory(typeof(TextBlock));
        //    label.SetBinding(TextBlock.TextProperty, new Binding());
        //    label.SetValue(TextBlock.ToolTipProperty, new Binding());
        //    stackPanel.AppendChild(label);
        //}

        //private void CreateCheckBox(FrameworkElementFactory stackPanel)
        //{
        //    FrameworkElementFactory checkBox = new FrameworkElementFactory(typeof(CheckBox));
        //    checkBox.Name = "chk";
        //    checkBox.SetValue(CheckBox.NameProperty, "chk");
        //    checkBox.SetValue(CheckBox.TagProperty, new Binding());
        //    checkBox.SetValue(CheckBox.MarginProperty, new Thickness(2));
        //    checkBox.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(CheckBoxClickEvent), true);
        //    stackPanel.AppendChild(checkBox);
        //}

        //private static FrameworkElementFactory CreateStackPanel()
        //{
        //    FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
        //    stackPanel.Name = "parentStackpanel";
        //    stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
        //    return stackPanel;
        //}

        //private void CheckBoxClickEvent(object sender, RoutedEventArgs e)
        //{
        //    WizardResources.SelectedCheckBoxes = GetSelectedCheckBoxes(ScopeTreeView.Items);
        //    if (WizardResources.SelectedCheckBoxes.Count > 0)
        //        NextButton.IsEnabled = true;
        //    else
        //        NextButton.IsEnabled = false;
        //}

        //private List<Type> GetSelectedCheckBoxes(ItemCollection items)
        //{
        //    List<Type> list = new List<Type>();
        //    foreach (TreeViewItem item in items)
        //    {
        //        UIElement elemnt = GetChildControl(item, "chk");
        //        if (elemnt != null && !item.Header.ToString().Equals(WizardResources.ProjectName))
        //        {
        //            CheckBox chk = (CheckBox)elemnt;
        //            if (chk.IsChecked.HasValue && chk.IsChecked.Value)
        //            {
        //                list.Add(WizardResources.GetTypeByString(item.Header.ToString()));
        //            }
        //        }
        //        List<Type> listOfSelectedCheckBoxes = GetSelectedCheckBoxes(item.Items);
        //        list = list.Concat(listOfSelectedCheckBoxes).ToList();
        //    }

        //    return list;
        //}

        //private UIElement GetChildControl(DependencyObject parentObject, string childName)
        //{
        //    UIElement element = null;
        //    if (parentObject != null)
        //    {
        //        int totalChild = VisualTreeHelper.GetChildrenCount(parentObject);
        //        for (int i = 0; i < totalChild; i++)
        //        {
        //            DependencyObject childObject = VisualTreeHelper.GetChild(parentObject, i);

        //            if (childObject is FrameworkElement && ((FrameworkElement)childObject).Name == childName)
        //            {
        //                element = childObject as UIElement;
        //                break;
        //            }
        //            element = GetChildControl(childObject, childName);
        //            if (element != null) break;
        //        }
        //    }
        //   return element;
        //}


    }
}
