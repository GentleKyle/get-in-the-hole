namespace get_in_the_hole;
using System;
using System.Threading;

class Program
{
    const int width = 60;
    const int height = 20;

    static void Main(string[] args)
    {
        
        DisplayBox(width, height);

        var rand = new Random();
        //1 to stated width is inside box - second param is exclusive so + 1
        int xPos = rand.Next(1, width + 1);

        var fallingO = new FallingO(xPos, 0);

        Timer aTimer = new Timer(fallingO.MoveO, null, 0, 1000);

        
        int origRow = Console.CursorTop;
        int origCol = Console.CursorLeft;


        // Console.WriteLine(origRow.ToString());
        // Console.WriteLine(origCol.ToString());
        // Console.WriteLine(origRow.ToString());
        // Console.SetCursorPosition(1, 18);
        // Console.Write("H");
        Console.ReadKey();
        
    }

    class FallingO {
        private int x;
        private int y;
        

        public FallingO(int newX, int newY) {
            x = newX;
            y = newY;
        }

        public void MoveO(Object state) {
            y++;
            if (y > 18) {
                ResetO(); 
            }
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
//Os no dissappear 
        public void ResetO() {
            var rand = new Random();
            //1 to stated width is inside box - second param is exclusive so + 1
            int xPos = rand.Next(1, width + 1);

            x = xPos;
            y = 1;
        }
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

