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

        // Gracz 1: strzałki (czerwona głowa, zielone ciało)
        Pixel hoofd = new Pixel
        {
            xPos = screenwidth / 2,
            yPos = screenheight / 2 + 2,
            schermKleur = ConsoleColor.Red
        };
        string movement = "RIGHT";
        int score = 0;
        List<int> telje = new List<int>();

        // Gracz 2: WASD (niebieska głowa, żółte ciało)
        Pixel hoofd2 = new Pixel
        {
            xPos = screenwidth / 2,
            yPos = screenheight / 2 - 2,
            schermKleur = ConsoleColor.Blue
        };
        string movement2 = "LEFT";
        int score2 = 0;
        List<int> telje2 = new List<int>();

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

            // Wyświetlanie punktów obu graczy
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(1, screenheight - 1);
            Console.Write("G1: " + score + "  G2: " + score2);

            // Rysowanie ciała węży
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i + 1 < telje.Count; i += 2)
            {
                Console.SetCursorPosition(telje[i], telje[i + 1]);
                Console.Write("■");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i + 1 < telje2.Count; i += 2)
            {
                Console.SetCursorPosition(telje2[i], telje2[i + 1]);
                Console.Write("■");
            }

            // Rysowanie głów
            Console.ForegroundColor = hoofd.schermKleur;
            Console.SetCursorPosition(hoofd.xPos, hoofd.yPos);
            Console.Write(hoofd.karacter);
            Console.ForegroundColor = hoofd2.schermKleur;
            Console.SetCursorPosition(hoofd2.xPos, hoofd2.yPos);
            Console.Write(hoofd2.karacter);

            // Sterowanie bez blokowania: gracz 1 strzałki, gracz 2 W/A/S/D
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
                    case ConsoleKey.W:
                        if (movement2 != "DOWN") movement2 = "UP";
                        break;
                    case ConsoleKey.S:
                        if (movement2 != "UP") movement2 = "DOWN";
                        break;
                    case ConsoleKey.A:
                        if (movement2 != "RIGHT") movement2 = "LEFT";
                        break;
                    case ConsoleKey.D:
                        if (movement2 != "LEFT") movement2 = "RIGHT";
                        break;
                }
            }

            // Zapamiętaj stare pozycje głów jako część ciała
            telje.Insert(0, hoofd.xPos);
            telje.Insert(1, hoofd.yPos);
            telje2.Insert(0, hoofd2.xPos);
            telje2.Insert(1, hoofd2.yPos);

            if (movement == "UP") hoofd.yPos--;
            if (movement == "DOWN") hoofd.yPos++;
            if (movement == "LEFT") hoofd.xPos--;
            if (movement == "RIGHT") hoofd.xPos++;

            if (movement2 == "UP") hoofd2.yPos--;
            if (movement2 == "DOWN") hoofd2.yPos++;
            if (movement2 == "LEFT") hoofd2.xPos--;
            if (movement2 == "RIGHT") hoofd2.xPos++;

            // Zjadanie przeszkody (wspólne jedzenie): punkt dla gracza, który je zjadł
            bool eaten = false;
            if (hoofd.xPos == obstacle.Xpos && hoofd.yPos == obstacle.Ypos)
            {
                score++;
                eaten = true;
            }
            else if (hoofd2.xPos == obstacle.Xpos && hoofd2.yPos == obstacle.Ypos)
            {
                score2++;
                eaten = true;
            }
            if (eaten)
            {
                obstacle.Xpos = randomnummer.Next(1, screenwidth - 1);
                obstacle.Ypos = randomnummer.Next(1, screenheight - 1);
            }

            // Długość ogona = 1 + liczba punktów (każdy segment to para x, y)
            while (telje.Count > (score + 1) * 2)
            {
                telje.RemoveAt(telje.Count - 1);
            }
            while (telje2.Count > (score2 + 1) * 2)
            {
                telje2.RemoveAt(telje2.Count - 1);
            }

            // Kolizje: ściana, własne ciało, ciało przeciwnika, czołowe zderzenie
            bool dead1 = Collides(hoofd, telje, telje2, screenwidth, screenheight);
            bool dead2 = Collides(hoofd2, telje2, telje, screenwidth, screenheight);
            if (hoofd.xPos == hoofd2.xPos && hoofd.yPos == hoofd2.yPos)
            {
                dead1 = true;
                dead2 = true;
            }

            if (dead1 || dead2)
            {
                string winner = dead1 && dead2 ? "Remis!" : dead1 ? "Wygrywa gracz 2!" : "Wygrywa gracz 1!";
                GameOver(winner, score, score2, screenwidth, screenheight);
            }

            Thread.Sleep(50);
        }
    }

    // Czy głowa węża uderzyła w ścianę, własne ciało lub ciało przeciwnika
    static bool Collides(Pixel head, List<int> own, List<int> other, int screenwidth, int screenheight)
    {
        if (head.xPos <= 0 || head.xPos >= screenwidth - 1 ||
            head.yPos <= 0 || head.yPos >= screenheight - 1)
        {
            return true;
        }

        for (int i = 0; i + 1 < own.Count; i += 2)
        {
            if (head.xPos == own[i] && head.yPos == own[i + 1]) return true;
        }
        for (int i = 0; i + 1 < other.Count; i += 2)
        {
            if (head.xPos == other[i] && head.yPos == other[i + 1]) return true;
        }
        return false;
    }

    static void GameOver(string result, int score, int score2, int screenwidth, int screenheight)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(screenwidth / 5, screenheight / 2 - 1);
        Console.WriteLine("Koniec gry");
        Console.SetCursorPosition(screenwidth / 5, screenheight / 2);
        Console.WriteLine(result);
        Console.SetCursorPosition(screenwidth / 5, screenheight / 2 + 1);
        Console.WriteLine("G1: " + score + "  G2: " + score2);
        Console.SetCursorPosition(0, screenheight / 2 + 3);
        Console.ResetColor();
        Console.CursorVisible = true;
        Environment.Exit(0);
    }
}
