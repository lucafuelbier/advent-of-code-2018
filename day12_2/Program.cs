using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

// This computes way too slow for 50 billion generations
// Trying to find a stabilization in the later generations
// as suggested in the day12 subreddit post

namespace day12_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input.txt");

            var initialStateInput = input[0];
            var ruleInput = new ArraySegment<string>(input, 2, input.Length - 2);

            var pots = ParsePots(initialStateInput);
            var rules = ruleInput.Select(ParseRule)
                                 .ToList();

            StringBuilder sb = new StringBuilder();
            int previousPotSum = 0;

            for (int gen = 0; gen < 1000; gen++)
            {                
                var newPots = new List<Pot>();

                foreach (Pot pot in pots)
                {
                    pot.ApplyRules(rules);
                }

                foreach (Pot pot in pots)
                {
                    var createdPots = pot.AdvanceGeneration();
                    newPots.AddRange(createdPots);
                }

                pots.AddRange(newPots);

                var potSum = pots.Where(pot => pot.State == PotState.Plant).Select(pot => pot.Number).Sum();

                sb.Append($"Generation: {gen+1, 4}: {potSum, 5} ({(potSum - previousPotSum).ToString("+0;-#")})\r\n");

                previousPotSum = potSum;
            }

            File.WriteAllText("Output.txt", sb.ToString());

            Console.WriteLine("Done. Check Output.txt!");

            var potSum1000 = pots.Where(pot => pot.State == PotState.Plant).Select(pot => pot.Number).Sum();
            // Stabilization at Generation 135 with +86 (see Output.txt)
            var solution = (50000000000 - 1000) * 86 + potSum1000;

            Console.WriteLine($"Solution: {solution}");
        }

        static List<Pot> ParsePots(string input)
        {
            Regex potRegex = new Regex(@"^initial state: (?<is>[#.]+)$");

            Match match = potRegex.Match(input);

            var initialState = ".." + match.Groups["is"].Value + "..";

            Pot previous = null;
            int potNumber = -2;
            var pots = new List<Pot>();

            foreach (char potStateChar in initialState)
            {
                Pot pot = new Pot();
                pot.State = CharToPotState(potStateChar);
                pot.Number = potNumber;

                pot.LeftNeighbor = previous;

                if (previous != null)
                {
                    previous.RightNeighbor = pot;
                }

                pots.Add(pot);

                previous = pot;
                potNumber++;
            }

            return pots;
        }

        static Rule ParseRule(string input)
        {
            Regex ruleRegex = new Regex(@"^(?<l2>[#.])(?<l1>[#.])(?<m>[#.])(?<r1>[#.])(?<r2>[#.]) => (?<ng>[#.])$");

            Match match = ruleRegex.Match(input);

            var middle = CharToPotState(Convert.ToChar(match.Groups["m"].Value));
            var left1 = CharToPotState(Convert.ToChar(match.Groups["l1"].Value));
            var left2 = CharToPotState(Convert.ToChar(match.Groups["l2"].Value));
            var right1 = CharToPotState(Convert.ToChar(match.Groups["r1"].Value));
            var right2 = CharToPotState(Convert.ToChar(match.Groups["r2"].Value));
            var nextGen = CharToPotState(Convert.ToChar(match.Groups["ng"].Value));

            return new Rule(middle, left1, left2, right1, right2, nextGen);
        }

        static PotState CharToPotState(char potStateChar)
        {
            if (potStateChar == '#')
            {
                return PotState.Plant;
            }
            else
            {
                return PotState.Empty;
            }
        }
    }

    class Pot
    {
        public PotState State { get; set; }
        public int Number { get; set; }

        public Pot LeftNeighbor { get; set; }
        public Pot RightNeighbor { get; set; }

        private PotState NextGenState { get; set; }

        public Pot()
        {
            this.State = PotState.Empty;
            this.Number = 0;

            this.LeftNeighbor = null;
            this.RightNeighbor = null;

            this.NextGenState = PotState.Empty;
        }

        public void ApplyRules(IEnumerable<Rule> rules)
        {
            foreach (Rule rule in rules)
            {
                PotState l1 = this.LeftNeighbor?.State ?? PotState.Empty;
                PotState l2 = this.LeftNeighbor?.LeftNeighbor?.State ?? PotState.Empty;
                PotState r1 = this.RightNeighbor?.State ?? PotState.Empty;
                PotState r2 = this.RightNeighbor?.RightNeighbor?.State ?? PotState.Empty;

                if (rule.Middle == this.State &&
                    rule.Left1 == l1 &&
                    rule.Left2 == l2 &&
                    rule.Right1 == r1 &&
                    rule.Right2 == r2)
                {
                    this.NextGenState = rule.NextGen;
                    return;
                }
            }

            // if no rule matches the old NextGen value remains
        }

        public List<Pot> AdvanceGeneration()
        {
            var createdPots = new List<Pot>();

            this.State = this.NextGenState;

            if (this.State == PotState.Plant)
            {
                if (this.LeftNeighbor == null)
                {
                    Pot newPot = new Pot();
                    newPot.State = PotState.Empty;
                    newPot.Number = this.Number - 1;

                    this.LeftNeighbor = newPot;
                    newPot.RightNeighbor = this;

                    createdPots.Add(newPot);
                }
                if (this.LeftNeighbor.LeftNeighbor == null)
                {
                    Pot newPot = new Pot();
                    newPot.State = PotState.Empty;
                    newPot.Number = this.Number - 2;

                    this.LeftNeighbor.LeftNeighbor = newPot;
                    newPot.RightNeighbor = this.LeftNeighbor;

                    createdPots.Add(newPot);
                }
                if (this.RightNeighbor == null)
                {
                    Pot newPot = new Pot();
                    newPot.State = PotState.Empty;
                    newPot.Number = this.Number + 1;

                    this.RightNeighbor = newPot;
                    newPot.LeftNeighbor = this;

                    createdPots.Add(newPot);
                }
                if (this.RightNeighbor.RightNeighbor == null)
                {
                    Pot newPot = new Pot();
                    newPot.State = PotState.Empty;
                    newPot.Number = this.Number + 2;

                    this.RightNeighbor.RightNeighbor = newPot;
                    newPot.LeftNeighbor = this.RightNeighbor;

                    createdPots.Add(newPot);
                }
            }

            return createdPots;
        }
    }

    class Rule
    {
        public PotState Middle { get; set; }
        public PotState Left1 { get; set; }
        public PotState Left2 { get; set; }
        public PotState Right1 { get; set; }
        public PotState Right2 { get; set; }

        public PotState NextGen { get; set; }

        public Rule(PotState middle, PotState left1, PotState left2, PotState right1, PotState right2, PotState nextGen)
        {
            this.Middle = middle;
            this.Left1 = left1;
            this.Left2 = left2;
            this.Right1 = right1;
            this.Right2 = right2;
            this.NextGen = nextGen;
        }
    }

    enum PotState
    {
        Empty,
        Plant
    }
}