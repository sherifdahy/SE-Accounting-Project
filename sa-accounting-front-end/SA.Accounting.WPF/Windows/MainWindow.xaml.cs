using SA.Accounting.WPF.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SA.Accounting.WPF;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(object data)
    {
        InitializeComponent();

        DataContext = data;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.CloseCommand.Execute(null);
        }
    }
}
