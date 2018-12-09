using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace day7_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var requirements = File.ReadLines("Input.txt")
                                   .Select(ParseRequirement);

            /* The use of a dictionary is unneccessary in this case
               as it duplicates the Step.Name information. But tbh I
               am too lazy to change it. */
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

            var workedOn = new HashSet<Step>();
            int timeElapsed = 0;

            while (steps.Count > 0)
            {
                var upForRemoval = new HashSet<Step>();

                if (workedOn.Count < 5)
                {
                    foreach (Step step in steps.Values)
                    {
                        if (workedOn.Count == 5)
                        {
                            break;
                        }
                        if (step.Dependencies.Count == 0)
                        {
                            workedOn.Add(step);
                        }
                    }
                }

                foreach (Step step in workedOn)
                {
                    step.timeRemaining -= 1;
                    
                    if (step.timeRemaining == 0)
                    {
                        upForRemoval.Add(step);
                    }
                }

                foreach (Step step in upForRemoval)
                {
                    foreach (Step dependantStep in step.Dependants)
                    {
                        dependantStep.Dependencies.Remove(step);
                    }

                    steps.Remove(step.Name);
                    workedOn.Remove(step);
                }

                timeElapsed += 1;
            }

            Console.WriteLine(timeElapsed);
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
        public int timeRemaining { get; set; }

        public HashSet<Step> Dependencies { get; }
        public HashSet<Step> Dependants { get; }

        public Step(char name)
        {
            this.Name = name;

            this.timeRemaining = 60 + Name - 64;

            this.Dependencies = new HashSet<Step>();
            this.Dependants = new HashSet<Step>();
        }
    }
}
