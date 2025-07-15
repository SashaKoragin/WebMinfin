using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using UserInterfaceMinfin.Main.Mvvm;

namespace UserInterfaceMinfin.Main.Window
{
    /// <summary>
    /// Логика взаимодействия для MainWindowFull.xaml
    /// </summary>
    public partial class MainWindowFull : System.Windows.Window
    {
        public MainWindowFull()
        {
            InitializeComponent();
            Window.Title = $"Автоматизация сайта МИНФИН - версия продукта {GetRunningVersion()}";
            Window.DataContext = new DataContextWindowsMvvmAuto();
        }

        private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scr = (ScrollViewer)sender;
            scr.ScrollToVerticalOffset(scr.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        private Version GetRunningVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

    }
}
