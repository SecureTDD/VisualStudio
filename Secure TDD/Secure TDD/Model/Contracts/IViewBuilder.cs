using Secure_TDD.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secure_TDD.Model.Contracts
{
    interface IViewBuilder
    {
        void Build(string projectName, Type[] types);

        TreeItemVM GetTreeVM();
    }
}
