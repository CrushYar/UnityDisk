using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnityDisk.ViewModel.Accounts;

namespace UnityDisk.View.AccountsManager.Commands
{
    public class AddNewAccountCommand:ICommand
    {
        private AccountManagerViewModel _viewModel;

        public AddNewAccountCommand(AccountManagerViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}
