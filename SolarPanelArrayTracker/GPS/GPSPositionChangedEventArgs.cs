using System;

namespace SolarPanelArrayTracker.GPS
{
    internal class GPSPositionChangedEventArgs : EventArgs
    {
        #region Public Properties

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        #endregion

        #region Constructors

        public GPSPositionChangedEventArgs(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        #endregion
    }
}
