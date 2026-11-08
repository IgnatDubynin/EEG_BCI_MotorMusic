using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PolyHubNet
{
    [DataContract]
    public sealed class PolyHubSettings
    {
        [DataMember(Order = 1)]
        public List<PolyHubServiceSettings> PolyHubServiceSettingsList { get; set; }

        [DataMember(Order = 2)]
        public List<PolyHubRouteSettings> PolyHubRouteSettingsList { get; set; }

        [DataMember(Order = 3)]
        public PolyHubUiSettings PolyHubUiSettings { get; set; }

        [DataMember(Order = 4)]
        public PolyHubViewerSettings PolyHubViewerSettings { get; set; }

        [DataMember(Order = 5)]
        public PolyHubRecorderSettings PolyHubRecorderSettings { get; set; }

        [DataMember(Order = 6)]
        public EogRejectionSettings EogRejectionSettings { get; set; }

        [DataMember(Order = 7)]
        public HeogMarkupSettings HeogMarkupSettings { get; set; }
    }

    [DataContract]
    public sealed class HeogMarkupSettings
    {
        [DataMember(Order = 1)]
        public double RangeThresholdMicrovolts { get; set; } = 20.0;

        [DataMember(Order = 2)]
        public int DetectionWindowMilliseconds { get; set; } = 100;

        [DataMember(Order = 3)]
        public int MinimumStableDurationMilliseconds { get; set; } = 200;

        public void Normalize()
        {
            if (double.IsNaN(RangeThresholdMicrovolts) || double.IsInfinity(RangeThresholdMicrovolts) ||
                RangeThresholdMicrovolts <= 0) RangeThresholdMicrovolts = 20.0;
            RangeThresholdMicrovolts = System.Math.Max(0.1, System.Math.Min(10000, RangeThresholdMicrovolts));
            if (DetectionWindowMilliseconds < 1 || DetectionWindowMilliseconds > 10000)
                DetectionWindowMilliseconds = 100;
            if (MinimumStableDurationMilliseconds < 1 || MinimumStableDurationMilliseconds > 60000)
                MinimumStableDurationMilliseconds = 200;
        }
    }

    [DataContract]
    public sealed class PolyHubServiceSettings
    {
        [DataMember(Order = 1)]
        public string ServiceId { get; set; }

        [DataMember(Order = 2)]
        public bool ServiceIsEnabled { get; set; }

        [DataMember(Order = 3)]
        public bool ServiceUseHostedMode { get; set; }
    }

    [DataContract]
    public sealed class PolyHubRouteSettings
    {
        [DataMember(Order = 1)]
        public string SourceNodeId { get; set; }

        [DataMember(Order = 2)]
        public string SourcePinId { get; set; }

        [DataMember(Order = 3)]
        public string TargetNodeId { get; set; }

        [DataMember(Order = 4)]
        public string TargetPinId { get; set; }

        [DataMember(Order = 5)]
        public string RouteDataType { get; set; }

        [DataMember(Order = 6)]
        public bool RouteIsEnabled { get; set; }
    }

    [DataContract]
    public sealed class PolyHubUiSettings
    {
        [DataMember(Order = 1)]
        public int WindowLeft { get; set; }

        [DataMember(Order = 2)]
        public int WindowTop { get; set; }

        [DataMember(Order = 3)]
        public int WindowWidth { get; set; }

        [DataMember(Order = 4)]
        public int WindowHeight { get; set; }

        [DataMember(Order = 5)]
        public string SelectedServiceId { get; set; }

        [DataMember(Order = 6)]
        public int EogRejectionProfileIndex { get; set; }

        [DataMember(Order = 7)]
        public int SplitContainer1Distance { get; set; }

        [DataMember(Order = 8)]
        public int SplitContainer2Distance { get; set; }
    }

    [DataContract]
    public sealed class PolyHubViewerSettings
    {
        [DataMember(Order = 1)]
        public int ViewerTimeBaseSeconds { get; set; }

        [DataMember(Order = 2)]
        public float ViewerScaleVolts { get; set; }

        [DataMember(Order = 3)]
        public int TimeFrequencyTimeBaseSeconds { get; set; }

        [DataMember(Order = 4)]
        public float TimeFrequencyScaleVolts { get; set; }
    }

    [DataContract]
    public sealed class PolyHubRecorderSettings
    {
        [DataMember(Order = 1)]
        public string RecorderSourceNodeId { get; set; }
    }

    [DataContract]
    public sealed class EogRejectionSettings
    {
        [DataMember(Order = 1)]
        public EogRejectionProfileSettings GazeSelection { get; set; }

        [DataMember(Order = 2)]
        public EogRejectionProfileSettings CovertAttention { get; set; }

        internal EogRejectionSettings Copy()
        {
            return new EogRejectionSettings
            {
                GazeSelection = GazeSelection.Copy(),
                CovertAttention = CovertAttention.Copy()
            };
        }

        public void Normalize()
        {
            if (GazeSelection == null)
                GazeSelection = EogRejectionProfileSettings.CreateDefault(false);
            if (CovertAttention == null)
                CovertAttention = EogRejectionProfileSettings.CreateDefault(true);

            GazeSelection.Normalize();
            CovertAttention.Normalize();
        }

        public static EogRejectionSettings CreateDefault()
        {
            return new EogRejectionSettings
            {
                GazeSelection = EogRejectionProfileSettings.CreateDefault(false),
                CovertAttention = EogRejectionProfileSettings.CreateDefault(true)
            };
        }
    }

    [DataContract]
    public sealed class EogRejectionProfileSettings
    {
        public const double MinimumThresholdMicrovolts = 0.1;
        public const double MaximumThresholdMicrovolts = 10000.0;
        public const double DefaultVeogThresholdMicrovolts = 150.0;
        public const double DefaultHeogThresholdMicrovolts = 100.0;

        [DataMember(Order = 1)]
        public bool VeogEnabled { get; set; }

        [DataMember(Order = 2)]
        public double VeogPeakToPeakThresholdMicrovolts { get; set; }

        [DataMember(Order = 3)]
        public bool HeogEnabled { get; set; }

        [DataMember(Order = 4)]
        public double HeogFixationDeviationThresholdMicrovolts { get; set; }

        internal EogRejectionProfileSettings Copy()
        {
            return (EogRejectionProfileSettings)MemberwiseClone();
        }

        public void Normalize()
        {
            VeogPeakToPeakThresholdMicrovolts = NormalizeThreshold(
                VeogPeakToPeakThresholdMicrovolts, DefaultVeogThresholdMicrovolts);
            HeogFixationDeviationThresholdMicrovolts = NormalizeThreshold(
                HeogFixationDeviationThresholdMicrovolts, DefaultHeogThresholdMicrovolts);
        }

        public static EogRejectionProfileSettings CreateDefault(bool heogEnabled)
        {
            return new EogRejectionProfileSettings
            {
                VeogEnabled = true,
                VeogPeakToPeakThresholdMicrovolts = DefaultVeogThresholdMicrovolts,
                HeogEnabled = heogEnabled,
                HeogFixationDeviationThresholdMicrovolts = DefaultHeogThresholdMicrovolts
            };
        }

        private static double NormalizeThreshold(double value, double defaultValue)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || value <= 0.0)
                return defaultValue;

            return System.Math.Max(MinimumThresholdMicrovolts,
                System.Math.Min(MaximumThresholdMicrovolts, value));
        }
    }
}
