using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolReport
{
    public abstract class PropertyChangedBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}