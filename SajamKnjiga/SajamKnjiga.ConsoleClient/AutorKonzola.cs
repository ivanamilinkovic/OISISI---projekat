using SajamKnjiga.Core.DAO;
using SajamKnjiga.Core.Models;

namespace SajamKnjiga.ConsoleClient
{
    public class AutorKonzola
    {
        private readonly AutorRepository _autori;

        public AutorKonzola(AutorRepository autori)
        {
            _autori = autori;
        }

        public void Meni()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== AUTORI =====");
                Console.WriteLine("1. Prikaz svih autora");
                Console.WriteLine("2. Dodavanje autora");
                Console.WriteLine("3. Izmena autora");
                Console.WriteLine("4. Brisanje autora");
                Console.WriteLine("0. Nazad");

                int izbor = Unos.CeoBroj("Izbor", 0, 4);
                switch (izbor)
                {
                    case 1: Prikazi(); break;
                    case 2: Dodaj(); break;
                    case 3: Izmeni(); break;
                    case 4: Obrisi(); break;
                    case 0: return;
                }
            }
        }

        private void Prikazi()
        {
            List<Autor> svi = _autori.GetAll();
            if (svi.Count == 0)
            {
                Console.WriteLine("Nema unetih autora.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(string.Format("{0,-4} {1,-15} {2,-15} {3,-12} {4,-14} {5}",
                "Id", "Ime", "Prezime", "Br. LK", "Datum rođenja", "E-mejl"));
            foreach (Autor a in svi)
            {
                Console.WriteLine(string.Format("{0,-4} {1,-15} {2,-15} {3,-12} {4,-14} {5}",
                    a.Id, a.Ime, a.Prezime, a.BrojLicneKarte, a.DatumRodjenja.ToString("dd.MM.yyyy."), a.Email));
            }
        }

        private void Dodaj()
        {
            Autor a = new Autor();
            a.Ime = Unos.Tekst("Ime");
            a.Prezime = Unos.Tekst("Prezime");
            a.DatumRodjenja = Unos.Datum("Datum rođenja");
            a.Adresa = AdresaUnos.Nova();
            a.Telefon = Unos.Tekst("Telefon");
            a.Email = Unos.Tekst("E-mejl");

            string brojLk = Unos.Tekst("Broj lične karte");
            while (BrojLkZauzet(brojLk, 0))
            {
                Console.WriteLine("Autor sa tim brojem lične karte već postoji.");
                brojLk = Unos.Tekst("Broj lične karte");
            }
            a.BrojLicneKarte = brojLk;

            a.GodineIskustva = Unos.CeoBroj("Godine iskustva", 0, 100);

            _autori.Add(a);
            Console.WriteLine("Autor je dodat (Id = " + a.Id + ").");
        }

        private void Izmeni()
        {
            Autor? a = IzaberiAutora();
            if (a == null)
                return;

            Console.WriteLine("Pritisnite Enter da zadržite staru vrednost.");
            a.Ime = Unos.TekstIzmena("Ime", a.Ime);
            a.Prezime = Unos.TekstIzmena("Prezime", a.Prezime);
            a.DatumRodjenja = Unos.DatumIzmena("Datum rođenja", a.DatumRodjenja);
            a.Adresa = AdresaUnos.Izmena(a.Adresa);
            a.Telefon = Unos.TekstIzmena("Telefon", a.Telefon);
            a.Email = Unos.TekstIzmena("E-mejl", a.Email);

            string brojLk = Unos.TekstIzmena("Broj lične karte", a.BrojLicneKarte);
            while (BrojLkZauzet(brojLk, a.Id))
            {
                Console.WriteLine("Autor sa tim brojem lične karte već postoji.");
                brojLk = Unos.TekstIzmena("Broj lične karte", a.BrojLicneKarte);
            }
            a.BrojLicneKarte = brojLk;

            a.GodineIskustva = Unos.CeoBrojIzmena("Godine iskustva", a.GodineIskustva, 0, 100);

            _autori.Update(a);
            Console.WriteLine("Autor je izmenjen.");
        }

        private void Obrisi()
        {
            Autor? a = IzaberiAutora();
            if (a == null)
                return;

            if (!Unos.DaNe("Da li ste sigurni da želite da obrišete autora " + a.Ime + " " + a.Prezime + "?"))
            {
                Console.WriteLine("Brisanje je otkazano.");
                return;
            }

            _autori.Delete(a.Id);
            Console.WriteLine("Autor je obrisan.");
        }

        private Autor? IzaberiAutora()
        {
            Prikazi();
            if (_autori.GetAll().Count == 0)
                return null;

            int id = Unos.CeoBroj("Id autora (0 = odustani)", 0, int.MaxValue);
            if (id == 0)
                return null;

            Autor? a = _autori.GetById(id);
            if (a == null)
                Console.WriteLine("Autor sa tim Id-jem ne postoji.");
            return a;
        }

        private bool BrojLkZauzet(string brojLk, int izuzetiId)
        {
            return _autori.GetAll().Any(a => a.Id != izuzetiId
                && a.BrojLicneKarte.Equals(brojLk, StringComparison.OrdinalIgnoreCase));
        }
    }
}