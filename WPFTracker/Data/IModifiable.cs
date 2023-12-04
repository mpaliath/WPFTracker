using System.ComponentModel;

namespace WPFTracker.Data
{
    public interface IModifiable : INotifyPropertyChanged
    {
        bool HasChanged { get; }
    }
}