using System.Collections.ObjectModel;
using UserInterfaceMinfin.UserControl.EditObject.UserControl;

namespace UserInterfaceMinfin.Main.AddUserControlFull
{
    public class AllLogicFull
    {
        /// <summary>
        /// Реализация древовидной структуры
        /// </summary>
        /// <returns></returns>
        public FullWindowInterfaceMethod FullWindowAdd()
        { 
            FullWindowInterfaceMethod allModelUserControl = new FullWindowInterfaceMethod();
            ObservableCollection<FullWindowInterface> window = new ObservableCollection<FullWindowInterface>
            {
                new FullWindowInterface
                {
                    NameControl = "Электронный бюджет",
                    CollectionUserControl = new ObservableCollection<FullWindowInterface>()
                    {
                        new FullWindowInterface()
                        {
                            NameControl = "ВП ВПЦТ",
                            UserControl = new UserControlEditObject()
                        }
                    }
                }
            };
            allModelUserControl.CollectionUserControl = window;
            return allModelUserControl;
        }
    }
}
