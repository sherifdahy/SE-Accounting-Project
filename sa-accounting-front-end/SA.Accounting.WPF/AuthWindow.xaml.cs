using SA.Accounting.WPF.Interfaces;
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
/// Interaction logic for AuthWindow.xaml
/// </summary>
public partial class AuthWindow : Window
{
    public AuthWindow(object data)
    {
        InitializeComponent();

        DataContext = data;
    }
}
