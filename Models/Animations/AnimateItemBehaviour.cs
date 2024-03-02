using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HRIS_Software.Models.Animations
{
    internal sealed class AnimateItemBehaviour
    {
        public static readonly DependencyProperty EditAnimationProperty =
            DependencyProperty.RegisterAttached(
                "EditAnimation",
                typeof(Storyboard),
                typeof(AnimateItemBehaviour));

        public static Storyboard GetEditAnimation(DependencyObject obj) =>
            obj.GetValue(EditAnimationProperty) as Storyboard;
        public static void SetEditAnimation(DependencyObject obj, Storyboard value) =>
            obj.SetValue(EditAnimationProperty, value);

        public static readonly DependencyProperty OnEditAnimationCompletedProperty =
            DependencyProperty.RegisterAttached(
                "OnEditAnimationCompleted",
                typeof(ICommand),
                typeof(AnimateItemBehaviour));

        public static ICommand GetOnEditAnimationCompleted(DependencyObject obj) =>
            obj.GetValue(OnEditAnimationCompletedProperty) as ICommand;
        public static void SetOnEditAnimationCompleted(DependencyObject obj, ICommand value) =>
            obj.SetValue(OnEditAnimationCompletedProperty, value);

        public static readonly DependencyProperty IsMarkedAsDirtyProperty =
            DependencyProperty.RegisterAttached(
                "IsMarkedAsDirty",
                typeof(bool),
                typeof(AnimateItemBehaviour),
                new UIPropertyMetadata(OnMarkedAsEditableChanged));

        private static void OnMarkedAsEditableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            HandleOnMarkChanged(d, e, GetEditAnimation(d), GetOnEditAnimationCompleted(d));

        public static bool GetIsMarkedAsDirty(DependencyObject obj) =>
            (bool)obj.GetValue(IsMarkedAsDirtyProperty);
        public static void SetIsMarkedAsDirty(DependencyObject obj, bool value) =>
            obj.SetValue(IsMarkedAsDirtyProperty, value);

        public static readonly DependencyProperty RemoveAnimationProperty =
            DependencyProperty.RegisterAttached(
                "RemoveAnimation",
                typeof(Storyboard),
                typeof(AnimateItemBehaviour));

        public static Storyboard GetRemoveAnimation(DependencyObject obj) =>
            obj.GetValue(RemoveAnimationProperty) as Storyboard;
        public static void SetRemoveAnimation(DependencyObject obj, Storyboard value) =>
            obj.SetValue(RemoveAnimationProperty, value);

        public static readonly DependencyProperty OnRemoveAnimationCompletedProperty =
            DependencyProperty.RegisterAttached(
                "OnRemoveAnimationCompleted",
                typeof(ICommand),
                typeof(AnimateItemBehaviour));

        public static ICommand GetOnRemoveAnimationCompleted(DependencyObject obj) =>
            obj.GetValue(OnRemoveAnimationCompletedProperty) as ICommand;
        public static void SetOnRemoveAnimationCompleted(DependencyObject obj, ICommand value) =>
            obj.SetValue(OnRemoveAnimationCompletedProperty, value);

        public static readonly DependencyProperty IsMarkedAsRemovableProperty =
            DependencyProperty.RegisterAttached(
                "IsMarkedAsRemovable",
                typeof(bool),
                typeof(AnimateItemBehaviour),
                new UIPropertyMetadata(OnMarkedAsRemovableChanged));

        public static bool GetIsMarkedAsRemovable(DependencyObject obj) =>
            (bool)obj.GetValue(IsMarkedAsRemovableProperty);
        public static void SetIsMarkedAsRemovable(DependencyObject obj, bool value) =>
            obj.SetValue(IsMarkedAsRemovableProperty, value);

        private static void OnMarkedAsRemovableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            HandleOnMarkChanged(d, e, GetRemoveAnimation(d), GetOnRemoveAnimationCompleted(d));

        private static void HandleOnMarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e,
            Storyboard animation, ICommand command)
        {
            if ((bool)e.NewValue == true)
            {
                if (d is FrameworkElement element)
                {
                    if (animation != null)
                    {
                        if (animation.IsSealed || animation.IsFrozen)
                            animation = animation.Clone();

                        Storyboard.SetTarget(animation, element);

                        if (command != null)
                        {
                            animation.Completed += (_, __) =>
                            {
                                object vm = element.DataContext;

                                if (command.CanExecute(vm))
                                    command.Execute(vm);
                            };
                        }

                        animation.Begin();
                    }
                }
            }
        }
    }
}
