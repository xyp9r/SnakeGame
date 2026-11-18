# SnakeGame

Prosta gra Snake w konsoli napisana w C# (.NET). Projekt powstał jako ćwiczenie
z pracy zdalnej w systemie Git i na GitHubie (gałęzie, pull requesty, recenzje, issues).

## Funkcje gry

- tryb dla dwóch graczy na jednej klawiaturze:
  - gracz 1 (czerwona głowa): strzałki,
  - gracz 2 (niebieska głowa): klawisze `W` `A` `S` `D`,
- węże poruszają się same, bez konieczności naciskania klawiszy,
- zakaz zawracania o 180 stopni,
- jedzenie (`*`) jest wspólne: zwiększa punkty gracza, który je zjadł, i wydłuża jego węża,
- gra przyspiesza wraz z wynikiem,
- przegrywa ten, kto uderzy w ścianę, własne ciało lub ciało przeciwnika;
  czołowe zderzenie oznacza remis,
- wyświetlanie punktów obu graczy oraz ekran końca gry z wynikiem.

## Struktura projektu

| Plik | Opis |
|---|---|
| `Program.cs` | pętla gry, rysowanie, sterowanie, kolizje |
| `Pixel.cs` | pozycja, kolor i znak elementu węża |
| `Obstakel.cs` | pozycja, kolor i znak jedzenia |

## Uruchamianie

Wymagany jest [.NET SDK](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/xyp9r/SnakeGame.git
cd SnakeGame
dotnet run
```

Grę należy uruchamiać w prawdziwym terminalu (nie w oknie bez konsoli).
Okno terminala powinno mieć co najmniej 32 kolumny i 16 wierszy.

## Zasady współpracy

1. `main` zawiera działającą wersję gry. Nie commitujemy do niej bezpośrednio.
2. Każde zadanie realizujemy w osobnej gałęzi (`feature/...`, `docs/...`, `fix/...`).
3. Po zakończeniu zadania tworzymy pull request do `main` z krótkim opisem zmian.
4. Pull request przegląda druga osoba z zespołu: komentuje, sugeruje poprawki
   i zatwierdza (approve). Dopiero wtedy następuje scalenie (merge).
5. Po scaleniu wszyscy testują grę (`dotnet run`); błędy zgłaszamy jako issues.
6. Pomysły i problemy zgłaszamy jako issues, przypisujemy do osoby odpowiedzialnej
   i rozwiązujemy w osobnej gałęzi z pull requestem (`Closes #numer`).

## Podział zadań

| Gałąź | Zadanie |
|---|---|
| `feature/input-movement` | sterowanie i ruch węża |
| `feature/food-score` | jedzenie, punkty, wzrost ogona |
| `feature/collision-gameover` | kolizje i koniec gry |
| `feature/second-snake` | drugi wąż (współpraca graczy) |
| `docs/readme` | dokumentacja |
| `fix/speed-up` | przyspieszanie gry (issue #5) |
