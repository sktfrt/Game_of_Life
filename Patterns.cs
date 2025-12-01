using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

using Game_of_Life;

public class Scanner
{
    public Terrain patternTerrain { get; private set; }

    public event Action<Terrain, string> PatternDetected;

    public Scanner(Terrain terrain)
    {
        patternTerrain = terrain;
    }

    public bool SearchForPatterns(Terrain terrain, int x, int y, Dictionary<string, (int px, int py)[]> pattern)
    {
        foreach (var (px, py) in pattern.Values())
        {
            if (x + px >= terrain.Field.Width || x + px < 0 ||
                y + py >= terrain.Field.Height || y + py < 0)
                return false;

            if (terrain.Field[x + px, y + py] == GameField.CellState.Dead) return false;
        }

        return true;
    }

    public void PatternScanner()
    {
        var patterns = new Dictionary<string, (int dx, int dy)[]>
        {
            {"Block" , PatternLibrary.Block },
            {"Beehive",  PatternLibrary.Beehive } ,
            {"BlinkerH", PatternLibrary.BlinkerH },
            {"Blinkerv", PatternLibrary.BlinkerV },
            {"Glider", PatternLibrary.Glider },
            {"Pentadecathlon", PatternLibrary.Pentadecathlon }
        };

        for (int x = 0; x < patternTerrain.Field.Width; x++)
            for (int y = 0; y < patternTerrain.Field.Height; y++)
            {
                foreach (var pattern in patterns.Keys)
                {
                    if (SearchForPatterns(patternTerrain, x, y, patterns))
                    {
                        PatternDetected?.Invoke(patternTerrain, pattern);
                    }
                }

            }
    }
}

public static class PatternLibrary
{
    public static readonly (int dx, int dy)[] Block =
    {
        (0,0), (1,0),
        (0,1), (1,1)
    };

    public static readonly (int dx, int dy)[] Beehive =
    {
        (1,0), (2,0),
        (0,1), (3,1),
        (1,2), (2,2)
    };

    public static readonly (int dx, int dy)[] BlinkerH =
    {
        (0,0), (1,0), (2,0)
    };

    public static readonly (int dx, int dy)[] BlinkerV =
    {
        (0,0), (0,1), (0,2)
    };

    public static readonly (int dx, int dy)[] Glider =
    {
        (1,0),
        (2,1),
        (0,2), (1,2), (2,2)
    };

    public static readonly (int dx, int dy)[] Pentadecathlon =
    {
        (1,0),(2,0),(3,0),(4,0),(5,0),(6,0),(7,0),(8,0),
        (0,1),(9,1),
        (1,2),(2,2),(3,2),(4,2),(5,2),(6,2),(7,2),(8,2)
    };
}