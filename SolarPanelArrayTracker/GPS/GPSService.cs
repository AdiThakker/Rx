using System;

namespace SolarPanelArrayTracker.GPS
{
    internal class GPSService
    {
        #region Private Members

        private Random randomValueGenerator;

        #endregion

        #region Public Properties

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        #endregion

        #region Constructors

        public GPSService()
        {
            randomValueGenerator = new Random();
        }

        #endregion

        #region Public Methods

        public GPSPositionChangedEventArgs GetGPSPosition()
        {
            this.Latitude = 41 + randomValueGenerator.NextDouble();
            this.Longitude = -83 + randomValueGenerator.NextDouble();

            return new GPSPositionChangedEventArgs(this.Latitude, this.Longitude);
        }

        public override string ToString()
        {
            return String.Format("Latitude: {0} Longitude: {1}", this.Latitude, this.Longitude);
        }

        #endregion
    }
}
