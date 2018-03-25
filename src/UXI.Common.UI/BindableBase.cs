using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.UI
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        protected bool Set<T>(ref T storage, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, newValue))
            {
                return false;
            }

            storage = newValue;
            OnPropertyChanged(propertyName);

            return true;
        }


        protected bool Set<T>(ref T storage, T newValue, out T oldValue, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, newValue))
            {
                oldValue = storage;
                return false;
            }

            oldValue = storage;
            storage = newValue;
            OnPropertyChanged(propertyName);

            return true;
        }


        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

}
