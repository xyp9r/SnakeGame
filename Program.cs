class Program
{
    static void Main()
    {
        int screenwidth = 32;
        int screenheight = 16;
        try
        {
            Console.WindowHeight = screenheight;
            Console.WindowWidth = screenwidth;
        }
        catch (PlatformNotSupportedException)
        {
            // macOS/Linux: rozmiaru okna nie można zmienić z kodu
        }

        Random randomnummer = new Random();

        Pixel hoofd = new Pixel
        {
            xPos = screenwidth / 2,
            yPos = screenheight / 2,
            schermKleur = ConsoleColor.Red
        };

        string movement = "RIGHT";
        int score = 0;

        // Pozycje ciała węża: x0, y0, x1, y1, ...
        List<int> telje = new List<int>();

        Obstakel obstacle = new Obstakel
        {
            Xpos = randomnummer.Next(1, screenwidth - 1),
            Ypos = randomnummer.Next(1, screenheight - 1),
            schermKleur = ConsoleColor.Cyan
        };

        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();

            // Rysowanie przeszkody
            Console.ForegroundColor = obstacle.schermKleur;
            Console.SetCursorPosition(obstacle.Xpos, obstacle.Ypos);
            Console.Write(obstacle.karacter);

            // Rysowanie ramki
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < screenwidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, screenheight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < screenheight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenwidth - 1, i);
                Console.Write("■");
            }

            // TODO (feature/food-score): kolor i wyświetlanie punktów

            // Rysowanie ciała
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i + 1 < telje.Count; i += 2)
            {
                Console.SetCursorPosition(telje[i], telje[i + 1]);
                Console.Write("■");
            }

            // Rysowanie głowy
            Console.ForegroundColor = hoofd.schermKleur;
            Console.SetCursorPosition(hoofd.xPos, hoofd.yPos);
            Console.Write(hoofd.karacter);

            // TODO (feature/input-movement): sterowanie bez blokowania (KeyAvailable)
            ConsoleKeyInfo info = Console.ReadKey(true);

            switch (info.Key)
            {
                case ConsoleKey.UpArrow:
                    movement = "UP";
                    break;
                case ConsoleKey.DownArrow:
                    movement = "DOWN";
                    break;
                case ConsoleKey.LeftArrow:
                    movement = "LEFT";
                    break;
                case ConsoleKey.RightArrow:
                    movement = "RIGHT";
                    break;
            }

            // Zapamiętaj starą pozycję głowy jako część ciała
            telje.Insert(0, hoofd.xPos);
            telje.Insert(1, hoofd.yPos);

            if (movement == "UP") hoofd.yPos--;
            if (movement == "DOWN") hoofd.yPos++;
            if (movement == "LEFT") hoofd.xPos--;
            if (movement == "RIGHT") hoofd.xPos++;

            // TODO (feature/food-score): zjadanie przeszkody, punkty i wzrost ogona
            // Na razie wąż ma stałą długość 1 segmentu (obcinamy ogon).
            while (telje.Count > 2)
            {
                telje.RemoveAt(telje.Count - 1);
            }

            // Kolizja ze ścianą
            if (hoofd.xPos <= 0 || hoofd.xPos >= screenwidth - 1 ||
                hoofd.yPos <= 0 || hoofd.yPos >= screenheight - 1)
            {
                GameOver(score, screenwidth, screenheight);
            }

            // Kolizja z własnym ciałem
            for (int i = 0; i + 1 < telje.Count; i += 2)
            {
                if (hoofd.xPos == telje[i] && hoofd.yPos == telje[i + 1])
                {
                    GameOver(score, screenwidth, screenheight);
                }
            }

            Thread.Sleep(50);
        }
    }

    static void GameOver(int score, int screenwidth, int screenheight)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(screenwidth / 5, screenheight / 2);
        Console.WriteLine("Koniec gry");
        Console.SetCursorPosition(screenwidth / 5, screenheight / 2 + 1);
        Console.WriteLine("Twój wynik: " + score);
        Console.SetCursorPosition(0, screenheight / 2 + 3);
        Console.ResetColor();
        Console.CursorVisible = true;
        Environment.Exit(0);
    }
}
