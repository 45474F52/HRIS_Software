using System;
using HRIS_Software.ViewModels;

namespace HRIS_Software.Models.Utils
{
    public sealed class CurrentViewService
    {
        private readonly Action<BaseVM> _onViewChanged;

        public CurrentViewService(Action<BaseVM> onViewChanged)
        {
            _onViewChanged = onViewChanged;
        }

        public void SetView(BaseVM view)
        {
            _onViewChanged(view);
        }
    }
}
