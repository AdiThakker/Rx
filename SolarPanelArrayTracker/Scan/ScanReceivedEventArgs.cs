using System;

namespace SolarPanelArrayTracker.Scan
{
    internal class ScanReceivedEventArgs : EventArgs
    {
        #region Properties

        public ScanEnumType ScanType { get; private set; }
        public long ScanValue { get; private set; }
        public string Defect { get; private set; }

        #endregion

        #region Constructors

        public ScanReceivedEventArgs(ScanEnumType scanType, long scanValue, string defect)
        {
            this.ScanType = scanType;
            this.ScanValue = scanValue;
            this.Defect = defect;
        }

        #endregion

    }

    internal enum ScanEnumType
    {
        SolarPanel = 0,
        Box = 1,
        Unknown = -1
    }
}
