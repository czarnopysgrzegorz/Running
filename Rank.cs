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
        ComparedNormalRun = NormalRunRatingRepresentation(Elo);
        ComparedZone2Run = Zone2RatingRepresentation(Elo);
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
    
    public int GiveNormalGain(Normal normal, Normal normal2)
    {
        int gain = 0;
        gain += DistanceGain(normal, normal2);
        gain += TimeGain(normal, normal2);
        gain += PaceGain(normal, normal2);
        return gain;
    }
    public int GiveZone2Gain(Zone2 zone2, Zone2 zone22)
    {
        int gain = 0;
        gain += DistanceGain(zone2, zone22);
        gain += TimeGain(zone2, zone22);
        gain += PaceGain(zone2, zone22);
        return gain;
    }

    public void UpdateComparedRun()
    {
        ComparedZone2Run = Zone2RatingRepresentation(Elo);
        ComparedNormalRun = NormalRunRatingRepresentation(Elo);
    }

    public void DownloadElo()
    {
        if (Owner.Runs.Count > 0)
        {
            for (int i = 0; i < Owner.Runs.Count; i++)
            {
                UpdateElo(Owner.Runs[i]);
            }
        }
    }
    public int UpdateElo(Run run)
    {
        int gain = 0;
        if (run is Normal normalRun)
        {
            gain = GiveNormalGain(normalRun, ComparedNormalRun);
        }

        if (run is Zone2 zone2Run)
        {
            gain = GiveZone2Gain(zone2Run, ComparedZone2Run);
        }
        Elo = Elo + (gain * 3);
        UpdateComparedRun();
        return gain * 3;
    }

    public Normal NormalRunRatingRepresentation(int elo)
    {
        Normal representation = new Normal();
        
        double B = Math.Log(8.4) / 1000;
        double A = 5 / 8.4;
        A =  A * Math.Exp(B * elo);
        A = Math.Round(A);
        double distance = A;
        
        double C = -Math.Log(4) / 1900;
        double D = 720 * Math.Pow(4, 100.0 / 1900.0);
        double output = D * Math.Exp(C * elo);
        output = Math.Round(output);
        int pace = (int)output;

        double time = distance * pace;
        Math.Round(time);
        int sTime = (int)time;
        representation.Distance = distance;
        representation.Pace = pace;
        representation.Time = sTime;
        return representation;
    }

    public Zone2 Zone2RatingRepresentation(int elo)
    {
        Zone2 representation = new Zone2();
        
        double B = Math.Log(8.4) / 1000;
        double A = 5 / 8.4;
        A =  A * Math.Exp(B * elo);
        A = Math.Round(A);
        double distance = A;
        
        double C = Math.Log(0.5) / 1000;
        double D = 960;
        double output;
        output = D * Math.Exp(C * elo);
        output = Math.Round(output);
        int pace = (int)output;
        
        double time = distance * pace;
        Math.Round(time);
        int sTime = (int)time;
        
        representation.Distance = distance;
        representation.Pace = pace;
        representation.Time = sTime;
        
        return representation;
    }

    public void ShowEloRepresentationOfRuns()
    {
        Console.WriteLine(ComparedNormalRun.ToString());
        Console.WriteLine(ComparedZone2Run.ToString());
    }
}