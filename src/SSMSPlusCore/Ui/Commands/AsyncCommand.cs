using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SSMSPlusCore.Ui.Utils;
using SSMSPlusCore.Utils;

namespace SSMSPlusCore.Ui
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }

    public interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T parameter);
        bool CanExecute(T parameter);
    }

    public class AsyncCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;
        private AsyncLock _asyncLock = new AsyncLock();
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly ErrorHandler _errorHandler;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null, ErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute()
        {
            return (_canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    using (await _asyncLock.LockAsync())
                    {
                        await _execute();
                    }
                }
                catch (Exception ex)
                {
                    _errorHandler?.Invoke(ex);
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync();
        }
        #endregion
    }

    public class AsyncCommand<T> : IAsyncCommand<T>
    {
        public event EventHandler CanExecuteChanged;

        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        private readonly ErrorHandler _errorHandler;

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, ErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute(T parameter)
        {
            return (_canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    await _execute(parameter);
                }
                catch (Exception ex)
                {
                    _errorHandler?.Invoke(ex);
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync((T)parameter).FireAndForgetSafeAsync();
        }
        #endregion
    }
}
