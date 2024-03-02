using System.Windows;
using HRIS_Software.Core;
using HRIS_Software.Models.Database;

namespace HRIS_Software.Models.ModalDialogs
{
    internal sealed class ShowResponsesVM : ModalDialogVM<Vacancy>
    {
        public ShowResponsesVM(Vacancy vacancy) : base(vacancy)
        {
            Title = vacancy.Position.PositionName;

            CopyTextCommand = new RelayCommand<string>(text => Clipboard.SetText(text, TextDataFormat.UnicodeText));
        }

        public RelayCommand<string> CopyTextCommand { get; }
    }
}
