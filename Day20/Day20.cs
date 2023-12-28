using System.Xml.XPath;

namespace AdventOfCode2023.Day20;

public static class Day20
{
    private const string inputFileName = "Day20.txt";
    private static readonly List<string> input = ReadFile($"/Day20/{inputFileName}");
    private static List<(bool, Module, string)> newPulsesSent = new List<(bool, Module, string)> ();
    private static Dictionary<string, Module> nodeMap = new Dictionary<string, Module>();
    private static long totalHighPulses = 0;
    private static long totalLowPulses = 0;
    private const bool HighPulse = true;
    private const bool LowPulse = false;
    private const string EndNodeName = "rx";

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
            totalLowPulses += 1;

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

        var result = totalHighPulses * totalLowPulses;
        Console.WriteLine($"Part1: {result}");
    }

    private abstract class Module()
    {
        public string Name { get; set; }
        public long lowPulsesCounter = 0;
        public long highPulsesCounter = 0;
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

        public void ResetToInitialState()
        {
            for (int i = 0; i < states.Count; i++)
            {
                states[i] = false;
                
            }
            ResetCounters();
        }

        public void ResetCounters()
        {
            lowPulsesCounter = 0;
            highPulsesCounter = 0;
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
                    totalLowPulses += Outputs.Count;
                }
                else
                {
                    totalHighPulses += Outputs.Count;
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

        internal bool HasStartedMachine()
        {
            return highPulsesCounter == 1;
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
                highPulsesCounter += 1;
                totalHighPulses += Outputs.Count;
            }
            else
            {
                lowPulsesCounter += 1;
                totalLowPulses += Outputs.Count;
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
                totalHighPulses += Outputs.Count;
            }
            else
            {
                totalLowPulses += Outputs.Count;
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

    // brute force didn't work or at least I didn't have patience to wait for it
    private class FinishModule : Module
    {
        long iteration = 1;
        public Module previousModule;

        public FinishModule(string name, List<string> outputs, string previousName) : base(name, outputs)
        {
            previousModule = nodeMap[previousName];
        }

        public override void ProcessPulse(bool pulseType, string pulseOrigin)
        {
            if (pulseType == HighPulse)
            {
                highPulsesCounter += 1;
            }
            else
            {
                lowPulsesCounter += 1;
            }
        }

        public void PrintCoutner()
        {
            //Console.WriteLine($"H:{highPulsesCounter} L:{lowPulsesCounter}");
        }
        
        public void ResetCounters()
        {
            iteration += 1;
            if (iteration % 1000000 == 0)
            {
                //Console.WriteLine(iteration);
            }
            lowPulsesCounter = 0;
            highPulsesCounter = 0;
        }

        public long GetResult()
        {
            return iteration;
        }

        internal bool HasStartedMachine()
        {
            return lowPulsesCounter == 1;
        }
    }

    public static void PartTwo()
    {
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
            var input = line.Split("->")[0].Trim()[1..];
            var outputs = line.Split("->")[1].Replace(" ", "").Split(",").ToList();
            foreach (var output in outputs)
            {
                if (!nodeMap.ContainsKey(output))
                {
                    if (output == EndNodeName)
                    {
                        nodeMap[output] = new FinishModule(output, new List<string>(), input);
                    }
                    else
                    {
                        nodeMap[output] = new EmptyModule(output, new List<string>());
                    }
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

        // sadly I did not manage a solid general solution so I made one based on input
        // it ended in such a very very messy code, I am hesitant to make a commit
        List<long> numbersToFindLCM = new List<long>();
        var secondToLastNode = ((FinishModule)nodeMap[EndNodeName]).previousModule;
        foreach (var name in ((ConjunctionModule)secondToLastNode).inputIndexes.Keys)
        {
            var cycleLength = 0L;

            // reset to initial state
            for (var i = 0; i < nodeMap.Count; i++)
            {
                nodeMap.ElementAt(i).Value.ResetToInitialState();
            }

            while (true)
            {
                newPulsesSent.Add((false, nodeMap["broadcaster"], ""));

                cycleLength += 1;
                while (newPulsesSent.Count > 0)
                {
                    var pulsesToProcess = newPulsesSent;
                    newPulsesSent = new List<(bool, Module, string)>();
                    foreach (var (pulseType, module, origin) in pulsesToProcess)
                    {
                        module.ProcessPulse(pulseType, origin);
                    }
                }
                
                var finishNode = (ConjunctionModule)nodeMap[name];
                if (finishNode.HasStartedMachine())
                {
                    break;
                }
            }
            numbersToFindLCM.Add(cycleLength);
        }

        foreach (var cycleLength in numbersToFindLCM)
        {
            Console.WriteLine($"CL: {cycleLength}");
        }

        var result = numbersToFindLCM[0];
        for (int i = 1; i < numbersToFindLCM.Count; i++)
        {
            result = CalculateLCM(result, numbersToFindLCM[i]);
        }

        Console.WriteLine($"Part2: {result}");
    }

    static long CalculateGCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long CalculateLCM(long a, long b)
        {
            // Avoid division by zero
            if (a == 0 || b == 0)
                return 0;

            return Math.Abs(a * b) / CalculateGCD(a, b);
        }
}
