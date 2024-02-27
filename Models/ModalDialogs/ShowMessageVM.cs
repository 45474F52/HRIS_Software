using System;
using System.Media;
using HRIS_Software.ViewModels;

namespace HRIS_Software.Models.ModalDialogs
{
    internal sealed class ShowMessageVM : BaseVM
    {
        private static Uri GetImageUri(MessageType type)
        {
            switch (type)
            {
                case MessageType.Info:
                    SystemSounds.Beep.Play();
                    return new Uri(@"/Resources/Images/Info.png", UriKind.Relative);
                case MessageType.Warning:
                    SystemSounds.Exclamation.Play();
                    return new Uri(@"/Resources/Images/Warning.png", UriKind.Relative);
                case MessageType.Error:
                    SystemSounds.Hand.Play();
                    return new Uri(@"/Resources/Images/Report.png", UriKind.Relative);
                case MessageType.None:
                default:
                    return null;
            }
        }

        public ShowMessageVM(string message, string title = null, MessageType type = MessageType.None)
            : this(message, title, GetImageUri(type)) { }

        public ShowMessageVM(string message, string title = null, Uri image = null)
        {
            Title = title ?? App.Current.MainWindow.Title;

            Message = message;
            OnPropertyChanged(nameof(Message));

            if (image != null)
            {
                Image = image;
                OnPropertyChanged(nameof(Image));

                HasImage = true;
            }
            else HasImage = false;

            OnPropertyChanged(nameof(HasImage));
        }

        public string Message { get; }
        public Uri Image { get; }
        public bool HasImage { get; }
    }

    internal enum MessageType
    {
        None,
        Info,
        Warning,
        Error
    }
}
