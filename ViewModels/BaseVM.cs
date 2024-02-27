using HRIS_Software.Core;

namespace HRIS_Software.ViewModels
{
    internal abstract class BaseVM : ObservableObject
    {
        private string _title;
        public string Title
        {
            get => _title;
            private protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
    }
}
