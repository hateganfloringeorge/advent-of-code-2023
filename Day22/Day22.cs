namespace AdventOfCode2023.Day22;


public static class Day22
{
    private const string inputFileName = "Day22.txt";
    private static readonly List<string> input = ReadFile($"/Day22/{inputFileName}");
    private static Dictionary<int, Dictionary<(int, int), Shapes3D>> shapesOnGroud = new Dictionary<int, Dictionary<(int, int), Shapes3D>>();
    public static void PartOne()
    {
        Console.WriteLine("======    Day22    ======");
        var result = 0L;
        var fallingShapes = new List<Shapes3D>();
        // create shapes
        foreach (var line in input)
        {
            var firstPoint = line.Split('~')[0].Split(',').Select(int.Parse).ToList();
            var secondPoint = line.Split('~')[1].Split(',').Select(int.Parse).ToList();
            
            if (firstPoint.SequenceEqual(secondPoint))
            {
                fallingShapes.Add(new Point(firstPoint));
            } 
            else if (firstPoint[0] == secondPoint[0])
            {
                if (firstPoint[1] == secondPoint[1])
                {
                    // different z
                    fallingShapes.Add(new LineZ(firstPoint, secondPoint[2] - firstPoint[2]));
                }
                else
                {
                    // different y
                    fallingShapes.Add(new LineY(firstPoint, secondPoint[1]));
                }
            }
            else 
            {
                // different x
                fallingShapes.Add(new LineX(firstPoint, secondPoint[0]));
            }
        }

        // sort by Z
        fallingShapes.Sort((a,b) => a.Z.CompareTo(b.Z));
        
        // fall to the ground
        for (var i = 0; i < fallingShapes.Count; i++)
        {
            var shape = fallingShapes[i];
            shape.FallToTheGround();
        }

        // check supporting bricks
        for (var i = 0; i < fallingShapes.Count; i++)
        {
            if (fallingShapes[i].CanBeDesintegrated())
            {
                result += 1;
            }
        }
        
        Console.WriteLine($"Part1: {result}");
    }

    public class Shapes3D
    {
        public Shapes3D(List<int> firstPoint)
        {
            Z = firstPoint[2];
        }

        public int Z {get; set;}
        public List<(int, int)> LevelPoints = new List<(int, int)>();
        public HashSet<Shapes3D> ShapesOnTop {get; set;} = new HashSet<Shapes3D>();
        public HashSet<Shapes3D> ShapesUnderneath {get; set;} = new HashSet<Shapes3D>();

        public virtual void FixPoistion()
        {
            if (!shapesOnGroud.ContainsKey(Z))
            {
                shapesOnGroud[Z] = new Dictionary<(int, int), Shapes3D>();
            }
            
            foreach (var (row, col) in LevelPoints)
            {
                shapesOnGroud[Z][(row, col)] = this;
            }

            if (Z != 1)
            {
                var levelUnder = Z - 1;
                foreach (var (row, col) in LevelPoints)
                {
                    if (shapesOnGroud[levelUnder].ContainsKey((row, col)))
                    {
                        var shapeUnder = shapesOnGroud[levelUnder][(row, col)];
                        ShapesUnderneath.Add(shapeUnder);
                        shapeUnder.ShapesOnTop.Add(this);
                    }
                }
            }
        }

        public void FallToTheGround()
        {
            while (Z >= 1)
            {
                if (Z == 1)
                {
                    FixPoistion();
                    break;
                }

                if (IsSomethingUnder())
                {
                    FixPoistion();
                    break;
                }
                else
                {
                    Z -= 1;
                }
            }
        }

        private bool IsSomethingUnder()
        {
            foreach (var (row, col) in LevelPoints)
            {
                var levelUnder = Z - 1;
                if (shapesOnGroud.ContainsKey(levelUnder) && shapesOnGroud[levelUnder].ContainsKey((row, col)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanBeDesintegrated()
        {
            if (ShapesOnTop.Count == 0)
            {
                return true;
            }

            foreach (Shapes3D shapeOnTop in ShapesOnTop)
            {
                if (shapeOnTop.ShapesUnderneath.Count == 1)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class Point : Shapes3D
    {
        public Point(List<int> firstPoint) : base(firstPoint)
        {
            LevelPoints.Add((firstPoint[0], firstPoint[1]));
        }
    }

    public class LineX : Shapes3D
    {
        public LineX(List<int> firstPoint, int lineEnd) : base(firstPoint)
        {
            for (var i = firstPoint[0]; i <= lineEnd; i++)
            {
                LevelPoints.Add((i, firstPoint[1]));
            }
        }
    }

    public class LineY : Shapes3D
    {
        public LineY(List<int> firstPoint, int lineEnd) : base(firstPoint)
        {
            for (var i = firstPoint[1]; i <= lineEnd; i++)
            {
                LevelPoints.Add((firstPoint[0], i));
            }
        }
    }

    public class LineZ : Shapes3D
    {
        public int Height {get; set;}
        public LineZ(List<int> firstPoint, int lineEnd) : base(firstPoint)
        {
            Height = lineEnd;
            LevelPoints.Add((firstPoint[0], firstPoint[1]));
        }

        public override void FixPoistion()
        {
            if (!shapesOnGroud.ContainsKey(Z))
            {
                shapesOnGroud[Z] = new Dictionary<(int, int), Shapes3D>(); 
            }
            
            foreach (var (row, col) in LevelPoints)
            {
                shapesOnGroud[Z][(row, col)] = this;
            }

            if (Z != 1)
            {
                var levelUnder = Z - 1;
                foreach (var (row, col) in LevelPoints)
                {
                    if (shapesOnGroud[levelUnder].ContainsKey((row, col)))
                    {
                        var shapeUnder = shapesOnGroud[levelUnder][(row, col)];
                        ShapesUnderneath.Add(shapeUnder);
                        shapeUnder.ShapesOnTop.Add(this);
                    }
                }
            }

            var topLevel = Height + Z;
            if (!shapesOnGroud.ContainsKey(topLevel))
            {
                shapesOnGroud[topLevel] = new Dictionary<(int, int), Shapes3D>();
            }
            if (!shapesOnGroud[topLevel].ContainsKey((LevelPoints[0])))
            {
                shapesOnGroud[topLevel][LevelPoints[0]] = this;
            }
            else 
            {
                Console.WriteLine("Something went wrong");
            }
        }
    }

    public static void PartTwo()
    {
        var result = 0;
        Console.WriteLine($"Part2: {result}");
    }
}
