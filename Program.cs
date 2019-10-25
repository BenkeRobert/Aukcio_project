using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AukcioProject
{
    public class Festmeny
    {
        private string cim;
        private string festo;
        private string stilus;
        private int licitekSzama;
        private int legmagasabbLicit;
        private DateTime legmagasabbLicitIdeje;
        private bool elkelt;

        public Festmeny(string cim, string festo, string stilus)
        {
            this.cim = cim;
            this.festo = festo;
            this.stilus = stilus;
            licitekSzama = 0;
            legmagasabbLicit = 0;
            legmagasabbLicitIdeje = DateTime.Now;
            elkelt = false;
        }

        public string getFesto()
        {
            return this.festo;
        }

        public string getStilus()
        {
            return this.stilus;
        }

        public int getLicitekSzama()
        {
            return this.licitekSzama;
        }

        public int getLegmagasabbLicit()
        {
            return this.legmagasabbLicit;
        }

        public bool Elkelt()
        {
            return this.elkelt;
        }

        public void Licit()
        {
            /* Ha már elkelt a festmény... */
            if (Elkelt() == true)
            {
                Console.WriteLine("Az adott festmény már elkelt!"); // Hibaüzenet
                                                                    // Nem történik semmi utána
            }
            /* Elsõ licit alapbeállításai */
            else
            {
                if (licitekSzama == 0)
                {
                    legmagasabbLicit = 100;                     // Az alapérték 100$
                    licitekSzama++;                             // Licit száma 0-ról 1-re növekszik
                    legmagasabbLicitIdeje = DateTime.Now;       // Beállítja az idõt a jelenlegi idõpontra
                }
                /* Más licitek beállításai */
                if (licitekSzama > 0)
                {
                    // 10% növekedés
                    legmagasabbLicit += legmagasabbLicit / 100 * 10;

                    // Utolsó számjegyek 0-ra állítása ('0' vagy '00')
                    if (legmagasabbLicit % 10 != 0)
                    {
                        legmagasabbLicit = (legmagasabbLicit / 10) * 10;
                    }
                    else if (legmagasabbLicit % 100 != 0)
                    {
                        legmagasabbLicit = (legmagasabbLicit / 100) * 100;
                    }

                    licitekSzama++;                             // Licit száma 0-ról 1-re növekszik
                    legmagasabbLicitIdeje = DateTime.Now;       // Beállítja az idõt a jelenlegi idõpontra
                }
                /* Licitálás után 2 perc elteltével elkel a festmény */
                if (DateTime.Now.Minute - legmagasabbLicitIdeje.Minute >= 2)
                {
                    this.elkelt = true;
                }
                Kiir();                                         // Festményadatok kiíratása
            }
        }

        public void Licit(int mertek)
        {
            if (licitekSzama == 0)
            {
                legmagasabbLicit = 100;                             // Az alapérték 100$
            }
            // 10% és 100% közötti növekedés
            legmagasabbLicit += legmagasabbLicit / 100 * mertek;    // Megadott százalékkal növeli
            licitekSzama++;                                         // Licit száma 1-el növekszik
            legmagasabbLicitIdeje = DateTime.Now;                   // Beállítja az idõt a jelenlegi idõpontra
            Licit();                                                // Elindul az fő licit rész
        }

        public void Kiir()
        {
            // Festõ: cim, (stilus)
            Console.WriteLine($"{getFesto()}: {cim} ({getStilus()})");

            // Elkelt / nem kelt el
            if (Elkelt() == true)
            {

                Console.WriteLine($"{Elkelt()}");
            }

            // Licitadatok kiíratása
            Console.WriteLine($"{getLegmagasabbLicit()} $ - {legmagasabbLicitIdeje.ToShortDateString()} (összesen: {getLicitekSzama()} db)");
        }

        /* A festmények teljes listájának kiíratásához szükséges rész*/
        public string Elkelt_e()
        {
            string valasz;
            if (this.elkelt == true)
            {
                valasz = "Elkelt.";
            }
            else
            {
                valasz =  "Nem kelt el.";
            }

            return valasz;
        }

        public override string ToString()
        {
            return this.festo + ": " + this.cim + $" ({this.stilus})" + "\n" + this.legmagasabbLicit + " - " + legmagasabbLicitIdeje.ToShortDateString() + " Összesen" 
                + licitekSzama + "db\n" + Elkelt_e();
        }

        class Program
        {
            static List<Festmeny> festmenyek = new List<Festmeny>();
            static Festmeny festmeny;

            static void Main(string[] args)
            {
                /* Fájlban szereplő adatok beolvasása */
                Beolvas();
                
                Feladat02();
                Feladat03();
                // Console.WriteLine("\nProgram vége!");
                Console.ReadKey();
            }

            static void Beolvas()
            {
                string fajl = @"...\AukcioProject\AukcioProject\festmenyek.csv";
                StreamReader sr = null;

                /*Adatok beolvasása és feldolgozása, hibaellenõrzéssel*/
                try
                {
                    using (sr = new StreamReader(fajl, Encoding.Default))
                    {
                        string[] darabol = null;

                        while (!sr.EndOfStream)
                        {
                            darabol = sr.ReadLine().Split(';');
                            festmenyek.Add(new Festmeny(darabol[0], darabol[1], darabol[2]));
                        }
                        Console.WriteLine("Fájl adatok beolvasva.");
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                        sr.Dispose();
                    }
                }
            }

            static int Ertekadas()
            {
                // Százalék mértékének megadása
                int mertek = 0;
                string ertek = null;
                do
                {
                    Console.WriteLine("\nAdjon meg egy százalékszámot (10 és 100 között)!");
                    ertek = Console.ReadLine();
                    /* Érték megadásának hiányában a Fő licit részt hívja meg */
                    if (ertek == null)
                    {
                        festmeny.Licit();
                    }
                    else
                    {
                        mertek = int.Parse(ertek);
                    }
                    /* Érték ellenőrzése */
                    if (mertek < 10 || mertek > 100)
                    {
                        Console.WriteLine("\tHibás adatot adott meg!");
                    }
                    else
                    {
                        Console.WriteLine("Licit érték emelkedés: {0}%", mertek);
                    }
                } while (mertek < 10 || mertek > 100);

                return mertek;
            }

            static void Feladat02()
            {
                /* Egyedi festmények listába felvétele */
                int db = 0, stilus_szam;
                string plus_cim = "", plus_festo = "", plus_stilus = "";
                festmeny = new Festmeny("Levétel a keresztről", "Rembrandt", "Barokk"); new Festmeny("Az utolsó vacsora", "Leonardo da Vinci", "Reneszánsz") ;
                festmenyek.Add(festmeny);
                
                /* Tetszőleges számú festmény felvétele a listába */
                do
                {
                    Console.WriteLine("\nHány darab új festmény adatait szeretné felvenni? (0 - 5)");
                    db = Convert.ToInt32(Console.ReadLine());

                    if (db < 0)
                    {
                        Console.WriteLine("\tA darabszám nem lehet 0-nál kisebb!");
                    }
                    else if (db > 5)
                    {
                        Console.WriteLine("\tA darabszám a megengedetthez képest nagyobb!");
                    }
                    else
                    {
                        for (int i = 0; i < db; i++)
                        {
                            Console.WriteLine("\nFestmény címe:");
                            plus_cim = Console.ReadLine();
                            Console.WriteLine("\nFestő neve:");
                            plus_festo = Console.ReadLine();
                            Console.WriteLine("\nFestmény stílusa (0 - Barokk, 1 - Reneszánsz, 2 - Expresszionizmus, 3 - Konstruktivizmus, 4 - Futurizmus):");
                            stilus_szam = Convert.ToInt32(Console.ReadLine());

                            switch (stilus_szam)
                            {
                                case 0:
                                    {
                                        plus_stilus = "Barokk";
                                        break;
                                    }
                                case 1:
                                    {
                                        plus_stilus = "Reneszánsz";
                                        break;
                                    }
                                case 2:
                                    {
                                        plus_stilus = "Expresszionizmus";
                                        break;
                                    }
                                case 3:
                                    {
                                        plus_stilus = "Konstruktivizmus";
                                        break;
                                    }
                                case 4:
                                    {
                                        plus_stilus = "Futurizmus";
                                        break;
                                    }
                                default:
                                    plus_stilus = "Ismeretlen";
                                    break;
                            }
                            festmenyek.Add(new Festmeny(plus_cim, plus_festo, plus_stilus));
                        }
                    }
                } while (db < 0);
                

                // Felhasználó általi licitálás
                int sorszam = -1;
                do
                {
                    Console.WriteLine("\nAdja meg a kért festmény sorszámát! ('0' megadása esetén kilép a programból.)");
                    sorszam = Convert.ToInt32(Console.ReadLine());

                    sorszam -= 1;
                    /* 0 megadása esetén*/
                    if (sorszam == -1)
                    {
                        Console.WriteLine("A program kilép.");
                    }
                    /*A listában létező szám esetén*/
                    else if (sorszam >= 0 && sorszam <= festmenyek.Count)
                    {
                        festmeny.Licit(Ertekadas());
                    }
                    /*Helytelen szám esetén*/
                    else
                    {
                        Console.WriteLine("\tNincs ilyen sorszámú festmény!");
                    }
                } while (sorszam != -1);
                /* Összes festmény listázása */
                foreach (var item in festmenyek)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            static void Feladat03()
            {
                /* Legdrágábban elkelt festmény */
                Console.WriteLine("\nA legdrágábban elkelt festmény: {0}", festmenyek.Max(x => x.legmagasabbLicit));
                /* 10-nél több alkalommal licitáltak */
                int licitMax = festmenyek.Max(x => x.licitekSzama);
                if (licitMax > 10)
                {
                    Console.WriteLine("\nVan olyan festmény, amire 10-nél is többen licitáltak.");
                }
                else
                Console.WriteLine("\nNincs olyan festmény, amire 10-nél is többen licitáltak.");
                /* El nem kelt festmények száma */
                Console.WriteLine("\nAz el nem kelt festmények száma: {0} db.", festmenyek.Count(x => x.elkelt.Equals("false")));
                /* Lista rendezése licit szerint csökkenőbe */
                // ??
            }
        }
    }
}
