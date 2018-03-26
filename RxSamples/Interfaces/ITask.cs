
using System;
namespace RxSamples.Interfaces
{
    interface ITask<TEventArgs>
        where TEventArgs : EventArgs
    {
        void Perform();
        event EventHandler<TEventArgs> NotifyEvent;
    }
}
