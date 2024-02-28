using HRIS_Software.Core;
using HRIS_Software.ViewModels;

namespace HRIS_Software.Models.ModalDialogs
{
    internal abstract class ModalDialogVM<T> : BaseVM
    {
        public ModalDialogVM(T value)
        {
            Parameter = value;
            OnPropertyChanged(nameof(Parameter));

            CancelCommand = new RelayCommand(() => DialogResult = true);
        }

        public virtual RelayCommand CancelCommand { get; protected internal set; }
        
        public virtual T Parameter { get; }

        private bool _dialogResult;
        public bool DialogResult
        {
            get => _dialogResult;
            protected internal set
            {
                _dialogResult = value;
                OnPropertyChanged();
            }
        }
    }
}
