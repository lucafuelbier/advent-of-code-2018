using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace day7_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var requirements = File.ReadLines("Input.txt")
                                   .Select(ParseRequirement);

            var steps = new SortedDictionary<char, Step>();

            foreach (Requirement req in requirements)
            {
                Step firstStep = null;
                Step secondStep = null;

                if (steps.ContainsKey(req.FirstStep))
                {
                    firstStep = steps[req.FirstStep];
                }
                else
                {
                    Step newStep = new Step(req.FirstStep);
                    steps.Add(req.FirstStep, newStep);
                    firstStep = newStep;
                }

                if (steps.ContainsKey(req.SecondStep))
                {
                    secondStep = steps[req.SecondStep];
                }
                else
                {
                    Step newStep = new Step(req.SecondStep);
                    steps.Add(req.SecondStep, newStep);
                    secondStep = newStep;
                }

                secondStep.Dependencies.Add(firstStep);
                firstStep.Dependants.Add(secondStep);
            }

            string order = "";

            while (steps.Count > 0)
            {
                Step upForRemoval = null;

                foreach (Step step in steps.Values)
                {
                    if (step.Dependencies.Count == 0)
                    {
                        order += step.Name;

                        foreach (Step dependantStep in step.Dependants)
                        {
                            dependantStep.Dependencies.Remove(step);
                        }

                        upForRemoval = step;
                        break;
                    }
                }

                steps.Remove(upForRemoval.Name);
            }

            Console.WriteLine(order);
        }

        static Requirement ParseRequirement(string input)
        {
            Regex requirementRegex = new Regex(@"^Step (?<first>\w) must be finished before step (?<second>\w) can begin.");

            Match match = requirementRegex.Match(input);

            char firstStep = Convert.ToChar(match.Groups["first"].Value);
            char secondStep = Convert.ToChar(match.Groups["second"].Value);

            return new Requirement(firstStep, secondStep);
        }
    }

    class Requirement
    {
        public char FirstStep { get; }
        public char SecondStep { get; }

        public Requirement(char firstStep, char secondStep)
        {
            this.FirstStep = firstStep;
            this.SecondStep = secondStep;
        }
    }

    class Step
    {
        public char Name { get; }
        public HashSet<Step> Dependencies { get; }
        public HashSet<Step> Dependants { get; }

        public Step(char name)
        {
            this.Name = name;
            this.Dependencies = new HashSet<Step>();
            this.Dependants = new HashSet<Step>();
        }
    }
}
