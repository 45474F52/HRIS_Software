using System.Windows;

namespace HRIS_Software.Views.Windows
{
    public partial class Main
    {
        public Main()
        {
            InitializeComponent();

            StateChanged += (_, __) => CheckState();

            CheckState();
        }

        private Thickness _oldMargin;

        protected void ToggleWindowState()
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void MaximizeBtn_Click(object sender, RoutedEventArgs e) => ToggleWindowState();

        private void RestoreBtn_Click(object sender, RoutedEventArgs e) => ToggleWindowState();

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Close();

        private void CheckState()
        {
            if (WindowState != WindowState.Maximized)
            {
                TitleBar.Margin = _oldMargin;
                RestoreBtn.Visibility = Visibility.Collapsed;
                MaximizeBtn.Visibility = Visibility.Visible;
            }
            else
            {
                _oldMargin = TitleBar.Margin;
                TitleBar.Margin = new Thickness(_oldMargin.Left, _oldMargin.Top + 7d, _oldMargin.Right, _oldMargin.Bottom);
                RestoreBtn.Visibility = Visibility.Visible;
                MaximizeBtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}
