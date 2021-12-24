using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bio7
{
    class Program
    {
        static readonly int MAX_GENERATIONS = 50;
        static void Main(string[] args)
        {
            Game game = new Igra(10);
            while (game.CurrentGeneration <= game.MaxGenerations)
            {
                game.Show();
                game.Evolve();
                Thread.Sleep(1000);
            }
            Console.WriteLine("Evaluation ended!\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    public class Cell
    {
        public bool IsAlive { get; set; }
        public bool ShouldLive { get; set; }
        public Cell()
        {
            IsAlive = false;
            ShouldLive = false;
        }

        public override string ToString()
        {
            if (IsAlive.Equals(true)) return " X ";
            else return " _ ";

        }

    }
        public class Grid
        {
            public int Rows { get; set; }
            public int Columns { get; set; }
            public Cell[][] Cells { get; set; }
            public Grid(int rows, int columns)
            {
                Rows = rows;
                Columns = columns;
                Cells = new Cell[rows][];
                for (int i = 0; i < Rows; i++)
                {
                    Cells[i] = new Cell[Columns];
                    for (int j = 0; j < Columns; j++)
                    {
                        Cell kletka = new Cell();
                        Cells[i][j] = kletka;
                    }
                }

             }
            public void ToggleCell(int x, int y, bool isAlive)
            {
                this.Cells[x][y].IsAlive = isAlive;
            }
            public void Evolve()
            {

                int count = 0;

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        count = 0;
                        if ((i - 1) >= 0 && (j - 1) >= 0 && Cells[i - 1][j - 1].IsAlive.Equals(true)) count++;
                        if ((i - 1) >= 0 && Cells[i - 1][j].IsAlive.Equals(true)) count++;
                        if ((i - 1) >= 0 && (j + 1) < Columns && Cells[i - 1][j + 1].IsAlive.Equals(true)) count++;
                        if ((j - 1) >= 0 && Cells[i][j - 1].IsAlive.Equals(true)) count++;
                        if ((j + 1) < Columns && Cells[i][j + 1].IsAlive.Equals(true)) count++;
                        if ((i + 1) < Rows && (j - 1) >= 0 && Cells[i + 1][j - 1].IsAlive.Equals(true)) count++;
                        if ((i + 1) < Rows && Cells[i + 1][j].IsAlive.Equals(true)) count++;
                        if ((i + 1) < Rows && (j + 1) < Columns && Cells[i + 1][j + 1].IsAlive.Equals(true)) count++;

                        if (Cells[i][j].IsAlive.Equals(true) && count < 2) Cells[i][j].ShouldLive = false;
                        if (Cells[i][j].IsAlive.Equals(true) && (count == 2 || count == 3)) Cells[i][j].ShouldLive = true;
                        if (Cells[i][j].IsAlive.Equals(true) && count > 3) Cells[i][j].ShouldLive = false;
                        if (Cells[i][j].IsAlive.Equals(false) && count == 3) Cells[i][j].ShouldLive = true;

                    }
                }

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        Cells[i][j].IsAlive = Cells[i][j].ShouldLive;
                    }
                }
            }


        }

    abstract class Game
    {
        protected Grid grid;

        public int CurrentGeneration { get; set; }

        public int MaxGenerations { get; set; }

        public Game(int maxGenerations)
        {
            this.MaxGenerations = maxGenerations;
            CurrentGeneration = 0;
        }

        public void Evolve()
        {
            grid.Evolve();
            CurrentGeneration++;

        }

        virtual public void Show()
        {
            Console.Clear();

            Console.WriteLine("Current generation: {0}", CurrentGeneration);

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Console.Write(grid.Cells[i][j].ToString());

                }
                Console.WriteLine();
            }


        }
    }

    class Igra : Game
    {

        public Igra(int maxGenerations) : base(maxGenerations)
        {
            grid = new Grid(10, 10);
            grid.ToggleCell(0, 1, true);
            grid.ToggleCell(1, 2, true);
            grid.ToggleCell(2, 3, true);
            grid.ToggleCell(3, 4, true);
            grid.ToggleCell(4, 5, true);
            grid.ToggleCell(5, 5, true);
            grid.ToggleCell(6, 5, true);
            grid.ToggleCell(7, 6, true);
            grid.ToggleCell(8, 7, true);
            grid.ToggleCell(9, 8, true);

        }
        override public void Show()
        {
            Console.Title = String.Format("Still Game of Life");
            base.Show();
        }

    }
    class StillLifeGame : Game
    {
        public enum GameType
        {
            Block,
            Beehive,
            Loaf,
            Boat
        }
        public GameType Type { get; set; }

        public StillLifeGame(GameType gameType, int maxGenerations) : base(maxGenerations)
        {
            Type = gameType;
            if (Type == GameType.Block)
            {
                grid = new Grid(4, 4);
                grid.ToggleCell(1, 1, true);
                grid.ToggleCell(1, 2, true);
                grid.ToggleCell(2, 1, true);
                grid.ToggleCell(2, 2, true);
            }

            if (Type == GameType.Beehive)
            {
                grid = new Grid(5, 6);
                grid.ToggleCell(1, 2, true);
                grid.ToggleCell(1, 3, true);
                grid.ToggleCell(2, 1, true);
                grid.ToggleCell(2, 4, true);
                grid.ToggleCell(3, 2, true);
                grid.ToggleCell(3, 3, true);
            }


        }
        override public void Show()
        {
            Console.Title = String.Format("Still Game of Life: {0}", Type);
            base.Show();
        }


    }

    class OscilatorGame : Game
    {
        public enum GameType
        {
            Blinker,
            Toad,
            Beacon,
            Pulsar
        }

        public GameType Type { get; set; }

        public OscilatorGame(GameType gameType, int maxGenerations) : base(maxGenerations)
        {
            Type = gameType;
            if (gameType == GameType.Blinker)
            {
                grid = new Grid(5, 5);
                grid.ToggleCell(2, 1, true);
                grid.ToggleCell(2, 2, true);
                grid.ToggleCell(2, 3, true);
            }

            if (gameType == GameType.Toad)
            {
                grid = new Grid(6, 6);
                grid.ToggleCell(2, 1, true);
                grid.ToggleCell(3, 1, true);
                grid.ToggleCell(4, 2, true);
                grid.ToggleCell(1, 3, true);
                grid.ToggleCell(2, 4, true);
                grid.ToggleCell(3, 4, true);
            }

            if (gameType == GameType.Beacon)
            {
                grid = new Grid(6, 6);
                grid.ToggleCell(1, 1, true);
                grid.ToggleCell(1, 2, true);
                grid.ToggleCell(2, 1, true);
                grid.ToggleCell(2, 2, true);
                grid.ToggleCell(3, 3, true);
                grid.ToggleCell(3, 4, true);
                grid.ToggleCell(4, 3, true);
                grid.ToggleCell(4, 4, true);
            }

            if (gameType == GameType.Pulsar)
            {
                grid = new Grid(17, 17);
                for (int i = 0; i < 17; i++)
                {
                    for (int j = 0; j < 17; j++)
                    {
                        if (i == 2 || i == 7 || i == 9 || i == 14)
                        {
                            if (j == 4 || j == 5 || j == 6 || j == 10 || j == 11 || j == 12)
                            {
                                grid.ToggleCell(i, j, true);
                            }
                        }
                        if ((i >= 4 && i <= 6) || (i >= 10 && i <= 12))
                        {
                            if (j == 2 || j == 7 || j == 9 || j == 14)
                            {
                                grid.ToggleCell(i, j, true);
                            }

                        }
                    }
                }
            }
        }
        override public void Show()
        {
            Console.Title = String.Format("Oscilator Game of Life: {0}", Type);
            base.Show();
        }
    }
}
