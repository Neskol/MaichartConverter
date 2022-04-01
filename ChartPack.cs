using System;

namespace MaichartConverter;

/// <summary>
/// Construct a collection to store charts in relate of SD and DX.
/// </summary>
public abstract class ChartPack : IChart
{
    /// <summary>
    /// Stores SD and DX chart
    /// [0] SD [1] DX
    /// </summary>
    private List<Chart>[] sddxCharts;

    /// <summary>
    /// Stores shared information
    /// </summary>
    private TrackInformation? globalInformation;

    /// <summary>
    /// Default constructor
    /// </summary>
    public ChartPack()
    {
        sddxCharts = new List<Chart>[2];
    }

    /// <summary>
    /// Accesses this.sddxCharts
    /// </summary>
    /// <value>this.sddxCharts</value>
    public List<Chart>[] SDDXCharts
    {
        get
        {
            return this.sddxCharts;
        }
        set
        {
            this.sddxCharts=value;
        }
    }

    /// <summary>
    /// Accesses this.globalInformation
    /// </summary>
    /// <value>this.globalInformation</value>
    public TrackInformation? GlobalInformation 
    {
        get
        {
            return this.globalInformation;
        }
        set
        {
            this.globalInformation=value;
        }
    }

    public abstract bool CheckValidity();

    public abstract string Compose();

    public abstract void Update();
}