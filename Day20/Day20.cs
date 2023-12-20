namespace AdventOfCode2023.Day20;

public static class Day20
{
    private const string inputFileName = "Day20.txt";
    private static readonly List<string> input = ReadFile($"/Day20/{inputFileName}");
    private static List<(bool, Module, string)> newPulsesSent = new List<(bool, Module, string)> ();
    private static Dictionary<string, Module> nodeMap = new Dictionary<string, Module>();
    private static long highPulses = 0;
    private static long lowPulses = 0;
    private const bool HighPulse = true;
    private const bool LowPulse = false;

    public static void PartOne()
    {
        Console.WriteLine("======    Day20    ======");

        // create the noduleMap
        foreach (var line in input)
        {
            var input = line.Split("->")[0].Trim();
            var outputs = line.Split("->")[1].Replace(" ", "").Split(",").ToList();

            Module newNode;
            if (input.Contains('%'))
            {
                input = input[1..];
                newNode = new FlipFlopModule(input, outputs);
            }
            else if (input.Contains('&'))
            {
                input = input[1..];
                newNode = new ConjunctionModule(input, outputs);
            }
            else
            {
                newNode = new BroadcastModule(input, outputs);
            }
            nodeMap[input] = newNode;
        }

        // add end nodes
        foreach (var line in input)
        {
            var outputs = line.Split("->")[1].Replace(" ", "").Split(",").ToList();
            foreach (var output in outputs)
            {
                if (!nodeMap.ContainsKey(output))
                {
                    nodeMap[output] = new EmptyModule(output, new List<string>());
                }
            }
        }

        // add what conjunction modules need to remember
        foreach (var (inputName, module) in nodeMap)
        {
            foreach (var output in module.Outputs)
            {
                if (nodeMap[output] is ConjunctionModule conjModule)
                {
                    conjModule.AddNodeToMemory(inputName);
                }
            }
        }

        // no need for loop checking as it was only 1000
        for (int i = 0; i < 1000; i++)
        {
            newPulsesSent.Add((false, nodeMap["broadcaster"], ""));
            lowPulses += 1;

            while (newPulsesSent.Count > 0)
            {
                var pulsesToProcess = newPulsesSent;
                newPulsesSent = new List<(bool, Module, string)>();
                foreach (var (pulseType, module, origin) in pulsesToProcess)
                {
                    module.ProcessPulse(pulseType, origin);
                }
            }
            //Console.WriteLine($"I:{i} H:{highPulses} L:{lowPulses}");
        }

        var result = highPulses * lowPulses;
        Console.WriteLine($"Part1: {result}");
    }

    private abstract class Module()
    {
        public string Name { get; set; }
        public List<string> Outputs { get; set; }

        // for conjunction is memory
        // for flip flop is the status of the switch
        // broadcast is not relevant
        public List<bool> states = [];
        public abstract void ProcessPulse(bool pulseType, string pulseOrigin);

        public Module(string name, List<string> outputs) : this()
        {
            Name = name;
            Outputs = outputs;
        }
    }

    private class FlipFlopModule : Module
    {
        public FlipFlopModule(string name, List<string> outputs) : base(name, outputs)
        {
            states.Add(false);
        }

        public override void ProcessPulse(bool pulseType, string pulseOrigin)
        {
            if (pulseType == LowPulse)
            {
                if (states[0] == HighPulse)
                {
                    lowPulses += Outputs.Count;
                }
                else
                {
                    highPulses += Outputs.Count;
                }

                var newPulseType = !states[0];
                states[0] = newPulseType;
                foreach (var output in Outputs)
                {
                    newPulsesSent.Add((newPulseType, nodeMap[output], Name));
                }
            }
        }
    }

    private class ConjunctionModule(string name, List<string> outputs) : Module(name, outputs)
    {
        public Dictionary<string, int> inputIndexes = new Dictionary<string, int>();

        public void AddNodeToMemory(string nodeName)
        {
            states.Add(LowPulse);
            inputIndexes.Add(nodeName, states.Count - 1);
        }

        public override void ProcessPulse(bool pulseType, string pulseOrigin)
        {
            states[inputIndexes[pulseOrigin]] = pulseType;

            bool newPulseType = HighPulse;
            if (states.All(pulse => pulse == HighPulse))
            {
                newPulseType = LowPulse;
            }

            if (newPulseType)
            {
                highPulses += Outputs.Count;
            }
            else
            {
                lowPulses += Outputs.Count;
            }

            foreach (var output in Outputs)
            {
                newPulsesSent.Add((newPulseType, nodeMap[output], Name));
            }
        }
    }

    private class BroadcastModule(string name, List<string> outputs) : Module(name, outputs)
    {
        public override void ProcessPulse(bool pulseType, string pulseOrigin)
        {
            if (pulseType)
            {
                highPulses += Outputs.Count;
            }
            else
            {
                lowPulses += Outputs.Count;
            }
            
            foreach (var output in Outputs)
            {
                newPulsesSent.Add((pulseType, nodeMap[output], Name));
            }
        }
    }

    private class EmptyModule(string name, List<string> outputs) : Module(name, outputs)
    {
        public override void ProcessPulse(bool pulseType, string pulseOrigin)
        {
            // do nothing
        }
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
