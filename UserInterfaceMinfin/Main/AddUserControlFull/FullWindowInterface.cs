using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace UserInterfaceMinfin.Main.AddUserControlFull
{
    public class FullWindowInterface : BindableBase
    {
        /// <summary>
        /// Выбор контента Для Автомата
        /// </summary>
        public FullWindowInterface UseContent
        {
            get { return _useContent; }
            set
            {
                _useContent = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Имена коллекций
        /// </summary>
        private string _nameControl;

        private FullWindowInterface _useContent;

        /// <summary>
        /// Коллекция UserControl Элементов
        /// </summary>
        private ObservableCollection<FullWindowInterface> _collectionUserControl;

        /// <summary>
        /// Элементы UserControl
        /// </summary>
        private System.Windows.Controls.UserControl _userControl;
        public string NameControl
        {
            get { return _nameControl; }
            set
            {
                _nameControl = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<FullWindowInterface> CollectionUserControl
        {
            get { return _collectionUserControl; }
            set
            {
                _collectionUserControl = value;
                RaisePropertyChanged();
            }
        }

        public System.Windows.Controls.UserControl UserControl
        {
            get { return _userControl; }
            set
            {
                _userControl = value;
                RaisePropertyChanged();
            }
        }

    }

    public class FullWindowInterfaceMethod : FullWindowInterface
    {
        private bool _isCheck;

        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                _isCheck = value;
                RaisePropertyChanged();
            }
        }

        public void IsCheked(object parametr)
        {
            var o = (FullWindowInterface)parametr;
            if (o.UserControl != null)
            {
                IsCheck = false;
            }
        }
    }
}
