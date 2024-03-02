using HRIS_Software.Core;

namespace HRIS_Software.Models.Database
{
    internal abstract class ObservableEntity<T> : ObservableObject where T : class
    {
        private protected readonly T _entity;

        public ObservableEntity(T entity) => _entity = entity;

        private bool _isDirty;
        public bool IsDirty
        {
            get => _isDirty;
            private protected set
            {
                _isDirty = value;
                OnPropertyChanged();
            }
        }

        private bool _isMarkedAsRemovable;
        public bool IsMarkedAsRemovable
        {
            get => _isMarkedAsRemovable;
            set
            {
                _isMarkedAsRemovable = value;
                OnPropertyChanged();
            }
        }
    }
}
