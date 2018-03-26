using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolarPanelArrayTracker
{
    internal class TaskManager<TType, TEventArgs>
        where TType : class, new()
        where TEventArgs : EventArgs
    {
        #region Private Members

        private TType contextType;
        private int taskInterval;

        #endregion

        #region Public Prpoerties

        public TType Context
        {
            get { return this.contextType; }
        }

        public int Interval
        {
            get { return this.taskInterval; }
            set { this.taskInterval = value; }
        }

        #endregion

        #region Events

        public event EventHandler<TEventArgs> NotifyEvent;

        #endregion

        #region Constructors

        public TaskManager(int interval)
        {
            contextType = new TType();
            this.taskInterval = interval;
        }

        #endregion

        #region Public Methods

        public async Task Start(Func<TEventArgs> taskFunction)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    // TODO : Provide Cancellation
                    while (true)
                    {
                        TEventArgs eventArgs = taskFunction();

                        if (this.NotifyEvent != null)
                        {
                            this.NotifyEvent(contextType, eventArgs);
                        }
                        Thread.Sleep(this.taskInterval);
                    }
                });
        }
        #endregion

    }
}
