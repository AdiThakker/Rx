using System;
using System.Linq;

namespace SolarPanelArrayTracker.Scan
{
    internal class ScanService
    {
        #region Private Members

        Random randomValueGenerator;
        private string[] defects;

        #endregion

        #region Public Properties

        public ScanEnumType ScanType { get; private set; }
        public int ScanValue { get; private set; }
        public string Defect { get; private set; }

        #endregion

        #region Constructors

        public ScanService()
        {
            randomValueGenerator = new Random();
            defects = new string[] { "Chipped", "Cracked", "String Out", "Covered with Dust" };
        }

        #endregion

        #region Public Methods

        public ScanReceivedEventArgs GetScan()
        {
            this.ScanType = ScanEnumType.SolarPanel;
            this.ScanValue = randomValueGenerator.Next(100000000, 999999999);
            this.Defect = defects[randomValueGenerator.Next(0, defects.Count())];
            return new ScanReceivedEventArgs(this.ScanType, this.ScanValue, this.Defect);
        }

        public override string ToString()
        {
            return String.Format("{0} : {1} - {2}", this.ScanType, this.ScanValue, this.Defect);
        }

        #endregion
    }
}
