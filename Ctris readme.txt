Michał Rogiński 107892 gr.8

W-rotacja klocka zgodnie ze wskazówkami zegara
S-przyspieszenie spadku klocka
A-przesunięcie klocka w lewo
D-przesunięcie klocka w prawo

po przegranej program zapyta czy zapisać wynik, jeśli jest taka potrzeba należy napisać y i nacisnąć enter

w pliku save.txt znajdują się wszystkie zapisane wyniki poprzedzone nazwami osób które je zdobyły

jeżeli konsola nie zmieniła się automatycznie należy zmienić czcionkę na "Raster Font" o rozmiarze 16x16 lub 12x16

implementacja:

struktura pary jest na wzór pary z C++ z uproszczeniem first do f i second do s
struktura klocek to bloczek którym poruszać może gracz, blok1-blok4 to pozycje części bloku, type to rodzaj klocka, state to stan obrotu klocka(1=nie obrócony,2=obrócony o 90 stopni...)

gettime() zwraca obecny czas
determine() wymaga wprowadzenia liczby, na jej podstawie decyduje jaki klocek jest następny w kolejności 
setklocek() ustawia nowy klocek tuż nad planszą
sidestep() porusza klocek w lewo i w prawo
place() utrwala pozycje klocka(ustawia go w obecnym miejscu)
move() porusza klocek w dół
fil() wypełnia "ekran" dla funkcji draw
draw() rysuje ekran w konsoli
clear() usuwa pełne linie
rotate() obraca klocek o 90 stopni zgodnie ze wskazówkami zegara
gameover() sprawdza czy gracz nie przegrał
endgame() wypisuje wynik

