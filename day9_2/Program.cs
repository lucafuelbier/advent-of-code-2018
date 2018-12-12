using System;
using System.Linq;

namespace day9_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int playerCount = 459;
            int lastMarble = 71320 * 100;

            Marble startingMarble = new Marble(0);
            startingMarble.Clockwise = startingMarble;
            startingMarble.CounterClockwise = startingMarble;

            Marble currentMarble = startingMarble;
            int currentPlayer = 0;
            long[] playerScore = new long[playerCount];

            for (int turn = 1; turn <= lastMarble; turn++)
            {
                if (turn % 23 == 0)
                {
                    playerScore[currentPlayer] += turn;

                    Marble sevenLeft = currentMarble;

                    for (int i = 0; i < 7; i++)
                    {
                        sevenLeft = sevenLeft.CounterClockwise;
                    }

                    currentMarble = sevenLeft.Clockwise;

                    int sevenLeftValue = sevenLeft.Number;
                    playerScore[currentPlayer] += sevenLeftValue;

                    sevenLeft.Remove();
                }
                else
                {
                    Marble newMarble = new Marble(turn);
                    Marble oneClockwise = currentMarble.Clockwise;
                    oneClockwise.AddClockwise(newMarble);
                    currentMarble = newMarble;
                }

                currentPlayer = (currentPlayer + 1) % playerCount;
            }

            var solution = playerScore.Max();

            Console.WriteLine(solution);
        }
    }

    class Marble
    {
        public int Number { get; }
        public Marble Clockwise { get; set; }
        public Marble CounterClockwise { get; set; }

        public Marble(int number)
        {
            this.Number = number;
        }

        public void AddClockwise(Marble marble)
        {
            Marble clockwiseMarble = this.Clockwise;

            this.Clockwise = marble;
            marble.CounterClockwise = this;

            clockwiseMarble.CounterClockwise = marble;
            marble.Clockwise = clockwiseMarble;
        }

        public void AddCounterClockwise(Marble marble)
        {
            Marble counterClockwiseMarble = this.CounterClockwise;

            this.CounterClockwise = marble;
            marble.Clockwise = this;

            counterClockwiseMarble.Clockwise = marble;
            marble.CounterClockwise = counterClockwiseMarble;
        }

        public void Remove()
        {
            Marble clockwiseMarble = this.Clockwise;
            Marble counterClockwiseMarble = this.CounterClockwise;

            clockwiseMarble.CounterClockwise = counterClockwiseMarble;
            counterClockwiseMarble.Clockwise = clockwiseMarble;
        }
    }
}
