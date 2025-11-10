namespace Snake
{
    public static class Snake
    {
        public static char direction;
        private static LinkedList<(int y, int x)> snake = new LinkedList<(int, int)>();
        public static int Move()
        {
            int x = snake.First.Value.x;
            int y = snake.First.Value.y;
            bool ateFood = false;

            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.UpArrow)
                {
                    if (direction != 'd')
                    {
                        Console.Beep(300, 100);
                        direction = 'u';
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {

                    if (direction != 'u')
                    {
                        Console.Beep(300, 100);

                        direction = 'd';
                    }
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    if (direction != 'r')
                    {
                        Console.Beep(300, 100);

                        direction = 'l';
                    }
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    if (direction != 'l')
                    {
                        Console.Beep(300, 100);

                        direction = 'r';
                    }
                }
            }

            switch (direction)
            {
                case 'u': Program.sleepTime = 200; y--; break;
                case 'd': Program.sleepTime = 200; y++; break;
                case 'l': Program.sleepTime = 100; x--; break;
                case 'r': Program.sleepTime = 100; x++; break;
                default:
                    break;
            }
            if (Program.Map[y, x] == 'X')
            {
                snake.AddLast((snake.Last.Value.y, snake.Last.Value.x));
                ateFood = true;
                AddPoint();
            }
            if (CheckIfOutside(y, x) || CheckIfAteItself(y, x))
            {
                return 1;
            }
            if (ateFood)
            {
                snake.AddFirst(new LinkedListNode<(int x, int y)>((y, x)));
                AddSnakeToMap();

                Program.GenerateNewFood();
            }
            else
            {
                snake.AddFirst(new LinkedListNode<(int x, int y)>((y, x)));
                Program.Map[snake.Last.Value.y, snake.Last.Value.x] = ' ';
                Console.SetCursorPosition(snake.Last.Value.x, snake.Last.Value.y);
                Console.Write(" ");
                snake.RemoveLast();
                AddSnakeToMap();

            }
            return 0;
        }

        private static void AddPoint()
        {
            Program.points++;
            Console.SetCursorPosition(Program.Map.GetLength(1) + 4, Program.Map.GetLength(0) / 2);
            Console.Write(Program.points);
            Console.Beep(450, 100);
        }

        private static void AddSnakeToMap()
        {
            foreach (var node in snake)
            {
                Console.SetCursorPosition(node.x, node.y);
                if (node == snake.First.Value)
                {
                    Program.Map[node.y, node.x] = 'O';
                    Console.Write("O");
                }
                else
                {
                    Program.Map[node.y, node.x] = 'o';
                    Console.Write("o");
                }
            }
        }

        private static bool CheckIfAteItself(int y, int x)
        {
            foreach (var node in snake)
            {
                if (node.x == x && node.y == y)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool CheckIfOutside(int y, int x)
        {
            if (Program.Map[y, x] == '█')
            { return true; }
            return false;
        }

        internal static void GenerateSnake(int y, int x)
        {
            snake = new LinkedList<(int y, int x)>();
            Random random = new Random();
            int startingY = random.Next(2, y - 1);
            int startingX = random.Next(3, x - 2);
            snake.AddLast((startingY, startingX));
            snake.AddLast((startingY, startingX - 1));
            AddSnakeToMap();
        }
    }

    internal class Program
    {
        public static char[,] Map = new char[10, 20];
        public static int sleepTime;
        public static int points;
        static void Main(string[] args)
        {
            int y = 10;
            int x = 20;
            sleepTime = 220;
            if (x < 5 || y < 5 || x > 30 || y > 30)
            {
                Console.WriteLine("Invalid map size!");
                return;
            }
            Play(y, x);
        }
        private static char[,] GenerateMap(int y, int x)
        {
            char[,] matrix = new char[y, x];
            for (int i = 0; i < x; i++)
            {
                matrix[0, i] = '█';
            }
            for (int i = 0; i < x; i++)
            {
                matrix[y - 1, i] = '█';
            }
            for (int i = 1; i < y - 1; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (j == 0 || j == x - 1)
                    {
                        matrix[i, j] = '█';
                    }
                    else
                        matrix[i, j] = ' ';
                }
            }
            return matrix;
        }

        private static void Play(int y, int x)
        {
            Console.Clear();
            Map = GenerateMap(y, x);
            Snake.GenerateSnake(y, x);
            GenerateNewFood();
            RenderMap();

            sleepTime = 250;
            Snake.direction = 'r';
            points = 0;
            Console.ReadKey();

            while (true)
            {
                if (Snake.Move() == 1)
                {
                    GameOver();
                    break;
                }
                System.Threading.Thread.Sleep(sleepTime);
            }
        }

        private static void GameOver()
        {
            Console.Beep(1200, 200);
            Console.Beep(1000, 200);
            Console.Beep(800, 200);
            Console.Beep(600, 200);
            Console.Beep(400, 200);
            Console.SetCursorPosition(0, Map.GetLength(0) + 1);
            Console.WriteLine("Game over!");
            Console.WriteLine("Play again? Y/N");
            string input = Console.ReadLine();
            if (input == "Y")
            {
                Play(Map.GetLength(0), Map.GetLength(1));
            }
        }

        private static void RenderMap()
        {
            Console.Clear();
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Console.Write(Map[i, j]);
                }
                if (i == Map.GetLength(0) / 2 - 1)
                {
                    Console.Write("  Points");
                }
                Console.WriteLine();
            }
            

        }
        public static void GenerateNewFood()
        {
            Random random = new Random();
            int x = random.Next(1, Map.GetLength(1) - 1);
            int y = random.Next(1, Map.GetLength(0) - 1);
            while (true)
            {
                if (Map[y, x] != ' ')
                {
                    x = random.Next(1, Map.GetLength(1) - 1);
                    y = random.Next(1, Map.GetLength(0) - 1);
                }
                else
                {
                    break;
                }
            }
            Map[y, x] = 'X';
            Console.SetCursorPosition(x, y);
            Console.Write("X");
            //System.Threading.Thread.Sleep(500);
        }
    }
}

