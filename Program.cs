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

            // Wyświetlanie punktów pod planszą
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, screenheight - 1);
            Console.Write("Punkty: " + score);

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

            // Sterowanie bez blokowania: czytamy wszystkie naciśnięte klawisze,
            // wąż porusza się dalej, nawet gdy nic nie naciskamy.
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);

                // Zakaz zawracania o 180 stopni
                switch (info.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (movement != "DOWN") movement = "UP";
                        break;
                    case ConsoleKey.DownArrow:
                        if (movement != "UP") movement = "DOWN";
                        break;
                    case ConsoleKey.LeftArrow:
                        if (movement != "RIGHT") movement = "LEFT";
                        break;
                    case ConsoleKey.RightArrow:
                        if (movement != "LEFT") movement = "RIGHT";
                        break;
                }
            }

            // Zapamiętaj starą pozycję głowy jako część ciała
            telje.Insert(0, hoofd.xPos);
            telje.Insert(1, hoofd.yPos);

            if (movement == "UP") hoofd.yPos--;
            if (movement == "DOWN") hoofd.yPos++;
            if (movement == "LEFT") hoofd.xPos--;
            if (movement == "RIGHT") hoofd.xPos++;

            // Zjadanie przeszkody: punkt, nowa pozycja jedzenia
            if (hoofd.xPos == obstacle.Xpos && hoofd.yPos == obstacle.Ypos)
            {
                score++;
                obstacle.Xpos = randomnummer.Next(1, screenwidth - 1);
                obstacle.Ypos = randomnummer.Next(1, screenheight - 1);
            }

            // Długość ogona = 1 + liczba punktów (każdy segment to para x, y)
            while (telje.Count > (score + 1) * 2)
            {
                telje.RemoveAt(telje.Count - 1);
            }

            // TODO (feature/collision-gameover): kolizja ze ścianą i z samym sobą
            if (hoofd.xPos <= 0 || hoofd.xPos >= screenwidth - 1 ||
                hoofd.yPos <= 0 || hoofd.yPos >= screenheight - 1)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Game Over");
                Console.WriteLine("Twój wynik: " + score);
                Environment.Exit(0);
            }

            Thread.Sleep(50);
        }
    }
}
