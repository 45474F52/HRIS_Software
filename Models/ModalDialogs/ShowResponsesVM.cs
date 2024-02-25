using HRIS_Software.Models.Database;

namespace HRIS_Software.Models.ModalDialogs
{
    internal sealed class ShowResponsesVM : ModalDialogVM<Vacancy>
    {
        public ShowResponsesVM(Vacancy vacancy) : base(vacancy)
        {
            Title = vacancy.Position.PositionName;
        }
    }
}
