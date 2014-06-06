using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;
using EnvDTE90;
using System.Windows.Forms;
using Secure_TDD.Model;
using System.Reflection;
using System.Collections.Generic;
using Secure_TDD.View;
using Secure_TDD.Model.Contracts;
using Secure_TDD.ViewModel;
using System.IO;
using Secure_TDD.Model.Entities;


namespace Secure_TDD
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidSecure_TDDPkgString)]
    public sealed class Secure_TDDPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public Secure_TDDPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidSecure_TDDCmdSet, (int)PkgCmdIDList.cmdSecureTDD);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }

            CreateConfigFilesDirectory();
        }

        /// <summary>
        /// Create a directory in Program Files (x86) that contains all the configuration files, if not exists.
        /// The new directory path is C:\Program Files (x86)\Secure TDD\Rules
        /// </summary>
        private static void CreateConfigFilesDirectory()
        {
            var programFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolderOption.None);
            if (string.IsNullOrWhiteSpace(programFilePath))
            {
                programFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles, Environment.SpecialFolderOption.None);
            }

            TestConfiguration.ConfigFilesDirectory = Path.Combine(programFilePath, "Secure TDD", "Rules");
            if (!Directory.Exists(TestConfiguration.ConfigFilesDirectory))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var testConfig1 = "Secure_TDD.TestConfigurationFiles.TestConfig1.xml";

                // Read the file that holds all the configuration tests paths.
                using (Stream stream = assembly.GetManifestResourceStream(testConfig1))
                {
                    Directory.CreateDirectory(TestConfiguration.ConfigFilesDirectory);
                    var configFile1 = Path.Combine(TestConfiguration.ConfigFilesDirectory, "ConfigFile1.xml");
                    using (FileStream fs = new FileStream(configFile1, FileMode.CreateNew, FileAccess.Write))
                    {
                        stream.CopyTo(fs);
                    }
                }

                var testConfig2 = "Secure_TDD.TestConfigurationFiles.TestConfig2.xml";

                // Read the file that holds all the configuration tests paths.
                using (Stream stream = assembly.GetManifestResourceStream(testConfig2))
                {
                    Directory.CreateDirectory(TestConfiguration.ConfigFilesDirectory);
                    var configFile1 = Path.Combine(TestConfiguration.ConfigFilesDirectory, "ConfigFile2.xml");
                    using (FileStream fs = new FileStream(configFile1, FileMode.CreateNew, FileAccess.Write))
                    {
                        stream.CopyTo(fs);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            DTE2 dte = (DTE2)GetService(typeof(SDTE));
            ProjectLoader projectLoader = new ProjectLoader(dte);

            IViewBuilder treeNodeBuilder = new TreeNodeBuilder();
            treeNodeBuilder.Build(dte.ActiveDocument.ProjectItem.ContainingProject.Name, WizardResources.ReflectedTypes);
            TreeItemVM tree = treeNodeBuilder.GetTreeVM();

            WizardResources.MainWPF = new MainWindow();
            var scopeVM = new ScopeFormVM(tree);
            WizardResources.MainWPF.DataContext = new MainWindowVM(scopeVM);
            WizardResources.MainWPF.Show();
        }
    }
}
