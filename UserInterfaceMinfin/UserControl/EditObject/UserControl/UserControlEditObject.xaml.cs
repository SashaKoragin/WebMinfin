using System.Windows;
using System.Windows.Controls;
using UserInterfaceMinfin.UserControl.EditObject.DataContext;

namespace UserInterfaceMinfin.UserControl.EditObject.UserControl
{
    /// <summary>
    /// Логика взаимодействия для UserControlEditObject.xaml
    /// </summary>
    public partial class UserControlEditObject : System.Windows.Controls.UserControl
    {

        private readonly DataContextEditObject _dataContext = new DataContextEditObject();
        public UserControlEditObject()
        {
            InitializeComponent();
            DataContext = _dataContext;
        }

        private void ListView_Drop(object sender, DragEventArgs arg)
        {
            if (arg.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])arg.Data.GetData(DataFormats.FileDrop);
                _dataContext.CollectionTemplateXlsx.AddModelFile(files);
            }
        }
    }
}
