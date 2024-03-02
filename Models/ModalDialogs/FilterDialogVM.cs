using HRIS_Software.Core;
using System;
using System.Collections.Generic;

namespace HRIS_Software.Models.ModalDialogs
{
    internal sealed class FilterDialogVM : ModalDialogVM<IEnumerable<DynamicItem>>
    {
        public event Action<bool> OnApplyFilters = delegate { };

        public FilterDialogVM(string title, DataForFilters data) : base(data.filters)
        {
            Title = title;

            CancelCommand = new RelayCommand(() => DialogResult = false);
            ApplyCommand = new RelayCommand(() => Apply(true));
            DropFiltersCommand = new RelayCommand(() => Apply(false));
        }

        public RelayCommand ApplyCommand { get; }
        public RelayCommand DropFiltersCommand { get; }

        private void Apply(bool flag)
        {
            DialogResult = true;
            OnApplyFilters(flag);
        }
    }
}
