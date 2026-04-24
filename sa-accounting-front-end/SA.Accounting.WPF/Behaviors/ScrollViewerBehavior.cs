using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SA.Accounting.WPF.Behaviors;

/// <summary>
/// Attached behavior for ScrollViewer to detect scroll-to-bottom and trigger pagination commands
/// </summary>
public static class ScrollViewerBehavior
{
    private const double ScrollThreshold = 50.0; // pixels from bottom to trigger load

    public static ICommand GetLoadMoreCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(LoadMoreCommandProperty);
    }

    public static void SetLoadMoreCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(LoadMoreCommandProperty, value);
    }

    public static readonly DependencyProperty LoadMoreCommandProperty =
        DependencyProperty.RegisterAttached(
            "LoadMoreCommand",
            typeof(ICommand),
            typeof(ScrollViewerBehavior),
            new PropertyMetadata(null, OnLoadMoreCommandChanged));

    private static void OnLoadMoreCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ScrollViewer scrollViewer)
        {
            if (e.NewValue != null)
            {
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
            else
            {
                scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
        }
    }

    private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (sender is ScrollViewer scrollViewer)
        {
            var verticalOffset = scrollViewer.VerticalOffset;
            var scrollableHeight = scrollViewer.ScrollableHeight;

            // Check if scrolled to near bottom
            if (scrollableHeight - verticalOffset < ScrollThreshold && scrollableHeight > 0)
            {
                var command = GetLoadMoreCommand(scrollViewer);
                if (command?.CanExecute(null) == true)
                {
                    command.Execute(null);
                }
            }
        }
    }
}