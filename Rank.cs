using System.ComponentModel;
using System.Text.Json.Nodes;

namespace Running;

public class Rank : World
{
    private string division;
    public string Division
    {
        get { return division; }
        set { division = value; }
    }

    private int elo;
    public int Elo
    {
        get { return elo; }
        set { elo = value; }
    }

    private Normal comparedNormalRun;
    public Normal ComparedNormalRun
    {
        get { return comparedNormalRun; }
        set { comparedNormalRun = value; }
    }
    private Zone2 comparedZone2Run;
    public Zone2 ComparedZone2Run
    {
        get { return comparedZone2Run; }
        set { comparedZone2Run = value; }
    }
    private Person owner;
    public Person Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    public Rank()
    {
        Division = "Undefined";
        Elo = 1000;
        Normal normalRun = new Normal();
        Zone2 zone2Run = new Zone2();
        normalRun.Distance = 5;
        normalRun.Time = 1890;
        normalRun.AddPace(normalRun);
        normalRun.MaxBPM = 185;
        normalRun.IsZone2 = false;
        ComparedNormalRun = normalRun;
        zone2Run.Distance = 5;
        zone2Run.Time = 2100;
        zone2Run.AddPace(zone2Run);
        zone2Run.IsZone2 = true;
        ComparedZone2Run = zone2Run;
    }

    public void setOwnerForRank(Person owner)
    {
        Owner = owner;
    }
    public int DistanceGain(Run run, Run run2)
    {
        double disDelta = run.Distance - run2.Distance;
        int dissDelta = (int)Math.Round(disDelta, 2); // Every extra km is 1 gain
        if (dissDelta == 0)
        {
            dissDelta = 1;
        }
        return dissDelta;
    }

    public int TimeGain(Run run, Run run2)
    {
        int timDelta = run.Time - run2.Time;
        timDelta = timDelta / 180; // Every extra 3 minutes is 1 gain
        if (timDelta == 0)
        {
            timDelta = 1;
        }
        if (PaceGain(run, run2) > 0)
        {
            timDelta = 1;
        }
        return timDelta;
    }

    public int PaceGain(Run run, Run run2)
    {
        int pacDelta = run2.Pace - run.Pace;
        pacDelta = pacDelta / 3;// Every 3 seconds less is 1 gain
        if (pacDelta == 0)
        {
            pacDelta = 1;
        }
        return pacDelta;
    }

    public int BPMGain(Normal normal, Normal normal2)
    {
        int bpmDelta = normal.MaxBPM - normal2.MaxBPM; 
        // Every 1 bpm more is 1 gain
        if (bpmDelta == 0)
        {
            bpmDelta = 1;
            return bpmDelta;
        }

        if (bpmDelta < 0)
        {
            bpmDelta = 0;
        }
        return bpmDelta;
    }
    public int CompareNormalGain(Normal normal, Normal normal2)
    {
        int gain = 0;
        gain += DistanceGain(normal, normal2);
        gain += TimeGain(normal, normal2);
        gain += PaceGain(normal, normal2);
        gain += BPMGain(normal, normal2);
        return gain;
    }
    public int CompareZone2Gain(Zone2 zone2, Zone2 zone22)
    {
        int gain = 0;
        gain += DistanceGain(zone2, zone22);
        gain += TimeGain(zone2, zone22);
        gain += PaceGain(zone2, zone22);
        return gain;
    }

    public void UpdateComparedRun()
    {
        if (Owner.Runs[Owner.Runs.Count - 1] is Zone2 zone2Run)
        {
            Zone2 zone2 = new Zone2();
            zone2 = zone2Run;
            ComparedZone2Run = zone2;
        }
        if (Owner.Runs[Owner.Runs.Count - 1] is Normal normalRun)
        {
            Normal normal = new Normal();
            normal = normalRun;
            ComparedNormalRun = normal;
        }
    }
    public void UpdateElo(Run run)
    {
        int gain = 0;
        if (run is Normal normalRun)
        {
            gain = CompareNormalGain(normalRun, ComparedNormalRun);
        }

        if (run is Zone2 zone2Run)
        {
            gain = CompareZone2Gain(zone2Run, ComparedZone2Run);
        }
        Elo = Elo + (gain * 3);
        UpdateComparedRun();
    }
}