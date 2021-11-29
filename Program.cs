using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace AlgorytmyZPowracaniem {
    class Graf {
        protected int _rzad;
        protected List<List<int>> _sasiedzi;
        public Graf(int rzad) {
            _rzad = rzad;
            if (_rzad > 0) {
                _sasiedzi = new List<List<int>>();
            }
            for (int i = 0; i < _rzad; i++) {
                _sasiedzi.Add(new List<int>());
            }
        }
        public bool DodajKrawedz(int a, int b) {
            int indexA = _sasiedzi[a].BinarySearch(b);
            int indexB = _sasiedzi[b].BinarySearch(a);
            if (indexA < 0 && indexB < 0) {
                _sasiedzi[a].Insert(~indexA, b);
                if (a != b) {
                    _sasiedzi[b].Insert(~indexB, a);
                }
                return true;
            }
            return false;
        }
        public void Wypisz() {
            for (int i = 0; i < _rzad; i++) {
                Console.Write(String.Format("{0,5}", i.ToString()) + ":");
                foreach (int sasiad in _sasiedzi[i]) {
                    Console.Write(String.Format("{0,5}", sasiad.ToString()) + " ");
                }
                Console.Write("\n");
            }
        }
        public List<int> CyklHamilton() {
            List<bool> odwiedzone = new List<bool>();
            List<int> wynik = new List<int>();
            for (int i = 0; i < _rzad; i++) {
                odwiedzone.Add(false);
            }
            KrokHamilton(0, odwiedzone, 0, wynik, 0);
            return wynik;
        }
        protected bool KrokHamilton(int wierzcholek, List<bool> odwiedzone, int iloscOdwiedzonych, List<int> wynik, int poczatek) {
            odwiedzone[wierzcholek] = true;
            iloscOdwiedzonych++;
            for (int i = 0; i < _sasiedzi[wierzcholek].Count; i++) {
                if (iloscOdwiedzonych == _rzad && _sasiedzi[wierzcholek][i] == poczatek) {
                    wynik.Insert(0, wierzcholek);
                    return true;
                }
                if (!odwiedzone[_sasiedzi[wierzcholek][i]]) {
                    if (KrokHamilton(_sasiedzi[wierzcholek][i], odwiedzone, iloscOdwiedzonych, wynik, poczatek)) {
                        wynik.Insert(0, wierzcholek);
                        return true;
                    }
                }
            }
            odwiedzone[wierzcholek] = false;
            iloscOdwiedzonych--;
            return false;
        }
        public List<int> SciezkaEuler() {
            int poczatek = WierzcholekPoczatkowy();
            if (poczatek < 0) {
                return null;
            }
            List<int> sciezka = new List<int>();
            List<List<int>> sasiedzi = new List<List<int>>();
            for (int i = 0; i < _sasiedzi.Count; i++) {
                sasiedzi.Add(new List<int>());
                for (int j = 0; j < _sasiedzi[i].Count; j++) {
                    sasiedzi[i].Add(_sasiedzi[i][j]);
                }
            }
            DFSEuler(poczatek, sasiedzi, sciezka);
            foreach (List<int> s in sasiedzi) {
                if (s.Count > 0) {
                    return null;
                }
            }
            return sciezka;
        }
        protected void DFSEuler(int wierzcholek, List<List<int>> sasiedzi, List<int> sciezka) {
            while (sasiedzi[wierzcholek].Count > 0) {
                int nastepny = sasiedzi[wierzcholek][0];
                sasiedzi[wierzcholek].RemoveAt(0);
                sasiedzi[nastepny].RemoveAt(sasiedzi[nastepny].BinarySearch(wierzcholek));
                DFSEuler(nastepny, sasiedzi, sciezka);
            }
            sciezka.Insert(0, wierzcholek);
        }
        protected int WierzcholekPoczatkowy() {
            List<int> stopnieWierzcholkow = StopnieWierzcholow();
            int nieparzyste = 0;
            int wierzcholek = -1;
            for (int i = 0; i < stopnieWierzcholkow.Count; i++) {
                if (stopnieWierzcholkow[i] != 0 && wierzcholek < 0) {
                    wierzcholek = i;
                }
                if (stopnieWierzcholkow[i] % 2 != 0) {
                    if (nieparzyste == 0) {
                        wierzcholek = i;
                    }
                    nieparzyste++;
                }
                if (nieparzyste > 2) {
                    return -1;
                }
            }
            return wierzcholek;
        }
        protected List<int> StopnieWierzcholow() {
            List<int> stopnie = new List<int>();
            foreach (List<int> s in _sasiedzi) {
                stopnie.Add(s.Count);
            }
            return stopnie;
        }
    }
    class Program {
        static void Main(string[] args) {
            Graf graf = new Graf(5);
            graf.DodajKrawedz(0, 1);
            graf.DodajKrawedz(0, 2);
            graf.DodajKrawedz(0, 3);
            graf.DodajKrawedz(1, 3);
            graf.DodajKrawedz(2, 3);
            graf.DodajKrawedz(2, 4);
            graf.DodajKrawedz(3, 4);
            List<int> sciezka = graf.SciezkaEuler();
            if (sciezka == null) {
                Console.WriteLine("Brak ścieżki Eulera.");
            }
            else {
                Console.Write("Ścieżka Eulera: ");
                foreach (int w in sciezka) {
                    Console.Write(w.ToString() + " ");
                }
                Console.WriteLine();
            }
            sciezka = graf.CyklHamilton();
            if (sciezka == null || sciezka.Count == 0) {
                Console.WriteLine("Brak ścieżki Eulera.");
            }
            else {
                Console.Write("Ścieżka Eulera: ");
                foreach (int w in sciezka) {
                    Console.Write(w.ToString() + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
