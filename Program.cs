// See https://aka.ms/new-console-template for more information

using System.Text;
using MichaelSort;

Dictionary<int, int> data = new()
{
    [82] = 2812,
    [6] = 2788,//
    [11] = 2780,
    [81] = 2777,
    [80] = 2734,
    [75] = 2725,
    [83] = 2725,
    [72] = 2712,
    [74] = 2704,
    [55] = 2696,
    [51] = 2687,
    [64] = 2678,
    [49] = 2676,
    [54] = 2672,
    [56] = 2665,
    [66] = 2657,
    [50] = 2656,
    [25] = 2648,
    [47] = 2646,
    [53] = 2645,
    [67] = 2643,
    [73] = 2639,
    [62] = 2635,
    [65] = 2631,
    [36] = 2629,
    [19] = 2626,
    [27] = 2623,
    [22] = 2622,
    [48] = 2621,
    [16] = 2618,
    [42] = 2618,
    [46] = 2617,
    [26] = 2614,
    [20] = 2611,
    [18] = 2606,
    [52] = 2605,
    [59] = 2605,
    [39] = 2603,
    [34] = 2602,
    [58] = 2602,
    [32] = 2601,
    [5] = 2599,//
    [41] = 2599,
    [24] = 2598,
    [30] = 2597,
    [45] = 2596,
    [37] = 2595,
    [35] = 2594,
    [12] = 2591,
    [44] = 2587,
    [23] = 2586,
    [17] = 2581,
    [57] = 2581,
    [61] = 2579,
    [28] = 2577,
    [89] = 2576,
    [10] = 2573,
    [29] = 2572,
    [38] = 2572,
    [9] = 2570,
    [91] = 2570,
    [4] = 2568, //
    [70] = 2567,
    [14] = 2555,
    [21] = 2553,
    [33] = 2553,
    [68] = 2552,
    [63] = 2550,
    [40] = 2548,
    [71] = 2548,
    [13] = 2545,
    [43] = 2542,
    [79] = 2542,
    [8] = 2540,
    [31] = 2535,
    [3] = 2533, //
    [84] = 2532,
    [86] = 2532,
    [60] = 2530,
    [88] = 2529,
    [90] = 2509,
    [2] = 2506, // 
    [87] = 2505,
    [1] = 2504, //
    [69] = 2504,
    [15] = 2492,
    [7] = 2491, //
    [77] = 2481,
    [78] = 2468,
    [85] = 2428,
    [76] = 2417,
    [121] = 1739,
    [113] = 1715,
    [122] = 1705,
    [92] = 1702,
    [110] = 1669,
    [114] = 1606,
    [115] = 1572,
    [111] = 1548,
    [119] = 1477,
    [98] = 1472,
    [95] = 1466,
    [103] = 1461,
    [101] = 1454,
    [102] = 1454,
    [100] = 1450,
    [106] = 1447,
    [96] = 1446,
    [120] = 1444,
    [118] = 1434,
    [107] = 1426,
    [109] = 1421,
    [97] = 1415,
    [116] = 1415,
    [99] = 1404,
    [105] = 1383,
    [112] = 1377,
};

var tempCells = data.Select(pair => new Cell(pair.Key, pair.Value)).ToArray();
tempCells.Shuffle();
data = new Dictionary<int, int>();
foreach (var cell in tempCells)
{
    data[cell.Index] = cell.Capacity;
}


double avg = (double)data.Sum(x => x.Value)/ data.Count;
int idealSum = (int)Math.Round(avg * 9);
Console.WriteLine($"Average assembly capacity: {avg*9:F2} mA*h");

Battery battery = new Battery();

int[] indices;
int[] values;
int optimalSum = 0;
for (int j = 0; j < 12; j++)
{



    indices = data.Keys.ToArray();
    values = data.Values.ToArray();
    int[] bag1 = new int[9];
    int[] bagOptimal = new int[9];
    int optimalDelta = Int32.MaxValue;
    bool isBreak = false;
    
    void RecFind(int indexCell, int indexInBag, int sum)
    {
        if (isBreak)
            return;
        if (indexInBag >= 0)
        {
            bag1[indexInBag] = indices[indexCell];
        }

        if (indexInBag == 8)
        {
            int delta = Math.Abs(sum - idealSum);
            if (Math.Abs(sum - idealSum) < optimalDelta)
            {
                optimalDelta = delta;
                Array.Copy(bag1, bagOptimal, 9);
               
                if (delta == 0)
                { 
                    isBreak = true;
                    

                }
            }
        }
        else
        {
            for (int i = indexCell + 1; i < indices.Length - (8 - indexInBag); i++)
            {
                RecFind(i, indexInBag + 1, sum + values[i]);
            }
        }

    }

    RecFind(-1, -1, 0);


    optimalSum = 0;
    for (int i = 0; i < 9; i++)
    {
        optimalSum += data[bagOptimal[i]];
    }
    Console.Write($"Assembly {j + 1} capacity {optimalSum} mA*h: ");
    for (int i = 0; i < 9; i++)
    {
        Console.Write($"({bagOptimal[i]}; {data[bagOptimal[i]]}) ");

        battery[j][i] = new Cell(bagOptimal[i], data[bagOptimal[i]]);
        data.Remove(bagOptimal[i]);
    }
    
    battery[j].IsOptimal = isBreak;
    

    Console.WriteLine();
}
indices = data.Keys.ToArray();
values = data.Values.ToArray();
optimalSum = 0;
for (int i = 0; i < 9; i++)
{
    optimalSum += values[i];
}
Console.Write($"Assembly 13 capacity {optimalSum} mA*h: ");
for (int i = 0; i < 9; i++)
{
    Console.Write($"({indices[i]}; {values[i]}) ");
    battery[12][i] = new Cell(indices[i], values[i]);
}
Console.WriteLine();
Console.WriteLine();

for (int iter = 0; iter < 2; iter++)
{



    var optimalAssemblies = battery.Assemblies.Where(a => a.IsOptimal).ToArray();
    var thinAssembly = battery.Assemblies.MinBy(a => a.Capacity);
    var fatAssembly = battery.Assemblies.MaxBy(a => a.Capacity);

    void FindAlternatives()
    {
        if (thinAssembly != null && fatAssembly != null && optimalAssemblies != null)
        {
            int thinMaxDelta = idealSum - thinAssembly.Capacity;
            int fatMaxDelta = fatAssembly.Capacity - idealSum;
            for (int i = 0; i < 9; i++)
            {
                int thinCap = thinAssembly[i].Capacity;
                for (int j = 0; j < 9; j++)
                {
                    int fatCap = fatAssembly[j].Capacity;
                    int sum2Cap = thinCap + fatCap;
                    foreach (var optimalAssembly in optimalAssemblies)
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            for (int l = 0; l < 9; l++)
                            {
                                if (k == l) continue;
                                if (optimalAssembly[k].Capacity + optimalAssembly[l].Capacity == sum2Cap)
                                {
                                    int thinDelta = optimalAssembly[k].Capacity - thinCap;
                                    int fatDelta = fatCap - optimalAssembly[l].Capacity;
                                    if (thinDelta > 0 && thinDelta <= thinMaxDelta &&
                                        fatDelta > 0 && fatDelta <= fatMaxDelta)
                                    {
                                        (optimalAssembly[k], thinAssembly[i]) = (thinAssembly[i], optimalAssembly[k]);
                                        (optimalAssembly[l], fatAssembly[j]) = (fatAssembly[j], optimalAssembly[l]);
                                        return;
                                    }

                                    thinDelta = optimalAssembly[l].Capacity - thinCap;
                                    fatDelta = fatCap - optimalAssembly[k].Capacity;
                                    if (thinDelta > 0 && thinDelta <= thinMaxDelta &&
                                        fatDelta > 0 && fatDelta <= fatMaxDelta)
                                    {
                                        (optimalAssembly[l], thinAssembly[i]) = (thinAssembly[i], optimalAssembly[l]);
                                        (optimalAssembly[k], fatAssembly[j]) = (fatAssembly[j], optimalAssembly[k]);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        Console.WriteLine($"Not Answer");

    }

    FindAlternatives();

    Console.WriteLine($"{iter} iteration done!");
}
Console.WriteLine();
Console.WriteLine(battery.ToString());
Console.ReadKey();


public record Cell(int Index, int Capacity);

public class Assembly
{
    private Cell[] _cells = new Cell[9];

    public bool IsOptimal = false;
    public Cell this[int i]
    {
        get => _cells[i];
        set => _cells[i] = value;
    }

    public int Capacity => _cells[0].Capacity + _cells[1].Capacity + _cells[2].Capacity +
                           _cells[3].Capacity + _cells[4].Capacity + _cells[5].Capacity +
                           _cells[6].Capacity + _cells[7].Capacity + _cells[8].Capacity;

 
}

public class Battery
{
    public readonly Assembly[] Assemblies = new Assembly[13];
    public Assembly this[int i] => Assemblies[i];


    public Battery()
    {
        for (int i = 0; i < 13; i++)
            Assemblies[i] = new Assembly();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int j = 0; j < 13; j++)
        {


            sb.Append($"Assembly {j + 1} capacity {this[j].Capacity} mA*h: ");
            for (int i = 0; i < 9; i++)
            {
                sb.Append($"({this[j][i].Index}; {this[j][i].Capacity}) ");

                
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}