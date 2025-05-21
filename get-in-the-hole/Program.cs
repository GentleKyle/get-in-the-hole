namespace get_in_the_hole;
using System;
using System.Threading;

class Program
{
    const int width = 40;
    const int height = 14;

    static void Main(string[] args)
    {
        ConsoleKeyInfo input;
        ConsoleKeyInfo answer;
        do {
            Console.CursorVisible = false;
            DisplayWelcome();
            answer = Console.ReadKey(true);
            if (answer.Key.ToString() != "Enter") {
                break;
            }
            int score = 0;
            DisplayBox(width, height);
            
            var rand = new Random();
            //1 to stated width is inside box - second param is exclusive so + 1
            int xPos = rand.Next(1, width + 1);

            var scoringEvent = new AutoResetEvent(false);
            var fallingO = new FallingO(xPos, 0);

            Timer aTimer = new Timer(fallingO.MoveO, scoringEvent, 500, 250);

            var bucket = new Bucket();
            bucket.DisplayBuck();

            do {
                
                
                do {
                    Console.CursorVisible = false;
                    //true intercepts so it is not displayed in console
                    input = Console.ReadKey(true);
                    bucket.Closed();
                    //RightArrow / LeftArrow --- to String
                    if (input.Key.ToString() == "RightArrow") {
                        bucket.MoveBuck("right");
                    }
                    if (input.Key.ToString() == "LeftArrow") {
                        bucket.MoveBuck("left");
                    }
                } while (input.Key.ToString() != "UpArrow");
                
                bucket.Open();

                scoringEvent.WaitOne();
                if (bucket.isOpen && fallingO.GetOPos() == bucket.GetBuckPos()) {
                    score++;
                }
                else {
                    //game over
                    aTimer.Dispose();
                    GameOver(score);
                    //so key spam before game over does not affect game over screen
                    while(Console.KeyAvailable)
                        Console.ReadKey(true);
                    answer = Console.ReadKey(true);

                }
            } while (answer.Key.ToString() == "Enter");
        } while (answer.Key.ToString() == "Spacebar");
        
        Console.Clear();
        Console.WriteLine("Thanks for stopping by!");
        Console.WriteLine();
        Console.WriteLine("See you again soon! :)");
        Console.WriteLine();
        Console.WriteLine(); 
    }
    class FallingO {
        private int x;
        private int lastX;
        private int y;
        

        public FallingO(int newX, int newY) {
            x = newX;
            lastX = newX;
            y = newY;
        }

        public void MoveO(Object state) {
            AutoResetEvent autoEvent = (AutoResetEvent)state;
            
            if (y == height - 2) {
                lastX = x;
                ResetO();
                autoEvent.Set();
            }
            y++;
            DisplayO();
            
        }
        
        public void DisplayO() {
            if (y > 1) {
                Console.SetCursorPosition(x, y - 1);
                Console.Write(" ");
            }

            Console.SetCursorPosition(x, y);
            Console.Write("O");
        }

        public void ResetO() {
            var rand = new Random();
            //1 to stated width is inside box but edges do not line up with center of bucket
            int xPos = rand.Next(2, width);

            x = xPos;
            y = 1;
        }

        public int GetOPos() {
            return lastX;
        }
    }
    
    class Bucket {
        private static int x = 1;
        readonly static int y = height - 2;
        public bool isOpen = false;

        public void DisplayBuck() {
        //1 to 60 is range inside box but takes 3 spots to display buck so - 1
            if (x > 0 && x < width - 1) {
                Console.SetCursorPosition(x, y);
                Console.Write("\\");
                Console.SetCursorPosition(x + 1, y);
                Console.Write("_");
                Console.SetCursorPosition(x + 1, y - 1);
                Console.Write("_");
                isOpen = false;
                Console.SetCursorPosition(x + 2, y);
                Console.Write("/");
            }
        }

        public static void ClearBuck() {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
            Console.Write(" ");
            Console.Write(" ");
            Console.SetCursorPosition(x + 1, y - 1);
            Console.Write(" ");
            
        }

        public void MoveBuck(string direction) {
            if (direction == "right" && x < width - 2) {
                ClearBuck();
                x++;
                DisplayBuck();
            }

            if (direction == "left" && x > 1) {
                ClearBuck();
                x--;
                DisplayBuck();
            }
        }

        public int GetBuckPos() {
            return x + 1;
        }

        public void Closed() {
            Console.SetCursorPosition(x + 1, y - 1);
            Console.Write("_");
            isOpen = false;
        }
        public void Open() {
            Console.SetCursorPosition(x + 1, y - 1);
            Console.Write(" ");
            isOpen = true;
        }

    }

    static void GameOver(int score) {
        Console.Clear();
        Console.WriteLine("Game Over!");
        Console.WriteLine();
        Console.WriteLine($"Your Score: {score}");
        Console.WriteLine();
        Console.WriteLine("Press 'Spacebar' to try again!");
        Console.WriteLine("Press any other key to quit.");

    }
    static void DisplayWelcome() {
        Console.Clear();
        Console.WriteLine("Get in the Hole!");
        Console.WriteLine();
        Console.WriteLine("Use the left and right arrow keys to move the bucket. Use the up arrow key to open the bucket.");
        Console.WriteLine("Catch the falling O's inside the bucket to score. If you miss an O then you lose!");
        Console.WriteLine();
        Console.WriteLine("Press 'Enter' to start!");
        Console.WriteLine("Press any other key to quit.");
    }
    static void DisplayBox(int width, int height) {

        Console.Clear();

        for (int i = 0; i < height; i++) {
            if (i == 0) {
                Console.Write("╔");
                for (int j = 0; j < width; j++) {
                    Console.Write("═");
                }
                Console.Write("╗");
                Console.WriteLine();
            }
            else if (i == height - 1) {
                Console.Write("╚");
                for (int j = 0; j < width; j++) {
                    Console.Write("═");
                }
                Console.Write("╝");
            }
            else {
                Console.Write("║");
                for (int j = 0; j < width; j++) {
                    Console.Write(" ");
                }
                Console.Write("║");
                Console.WriteLine();
            }
        }
    }
}

