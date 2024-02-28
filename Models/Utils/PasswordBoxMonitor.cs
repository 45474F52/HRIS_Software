using System.Windows;
using System.Windows.Controls;

namespace HRIS_Software.Models.Utils
{
    internal sealed class PasswordBoxMonitor : DependencyObject
    {
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(
                "IsMonitoring",
                typeof(bool),
                typeof(PasswordBoxMonitor),
                new UIPropertyMetadata(false, OnIsMonitorChanged));

        public static bool GetIsMonitoring(DependencyObject obj) => (bool)obj.GetValue(IsMonitoringProperty);
        public static void SetIsMonitoring(DependencyObject obj, bool value) => obj.SetValue(IsMonitoringProperty, value);

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached(
                "PasswordLength",
                typeof(int),
                typeof(PasswordBoxMonitor),
                new UIPropertyMetadata(default(int)));

        public static int GetPasswordLength(DependencyObject obj) => (int)obj.GetValue(PasswordLengthProperty);
        public static void SetPasswordLength(DependencyObject obj, int value) => obj.SetValue(PasswordLengthProperty, value);

        private static void OnIsMonitorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb)
            {
                if ((bool)e.NewValue)
                {
                    pb.PasswordChanged += PasswordChanged;
                }
                else
                {
                    pb.PasswordChanged -= PasswordChanged;
                }
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pb)
            {
                SetPasswordLength(pb, pb.SecurePassword.Length);
            }
        }
    }
}
