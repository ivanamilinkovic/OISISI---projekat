using SajamKnjiga.Core.DAO;
using SajamKnjiga.Core.Models;

namespace SajamKnjiga.ConsoleClient
{
    public class KnjigaKonzola
    {
        private readonly KnjigaRepository _knjige;

        public KnjigaKonzola(KnjigaRepository knjige)
        {
            _knjige = knjige;
        }

        public void Meni()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== KNJIGE =====");
                Console.WriteLine("1. Prikaz svih knjiga");
                Console.WriteLine("2. Dodavanje knjige");
                Console.WriteLine("3. Izmena knjige");
                Console.WriteLine("4. Brisanje knjige");
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
            List<Knjiga> sve = _knjige.GetAll();
            if (sve.Count == 0)
            {
                Console.WriteLine("Nema unetih knjiga.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(string.Format("{0,-4} {1,-18} {2,-30} {3,-10} {4,-8} {5}",
                "Id", "ISBN", "Naziv", "Cena", "Godina", "Žanr"));
            foreach (Knjiga k in sve)
            {
                Console.WriteLine(string.Format("{0,-4} {1,-18} {2,-30} {3,-10} {4,-8} {5}",
                    k.Id, k.Isbn, k.Naziv, k.Cena, k.GodinaIzdanja, k.Zanr));
            }
        }

        private void Dodaj()
        {
            Knjiga k = new Knjiga();

            string isbn = Unos.Tekst("ISBN");
            while (IsbnZauzet(isbn, 0))
            {
                Console.WriteLine("Knjiga sa tim ISBN-om već postoji.");
                isbn = Unos.Tekst("ISBN");
            }
            k.Isbn = isbn;

            k.Naziv = Unos.Tekst("Naziv");
            k.Zanr = Unos.Tekst("Žanr");
            k.GodinaIzdanja = Unos.CeoBroj("Godina izdanja", 1450, DateTime.Now.Year);
            k.Cena = Unos.Decimalni("Cena (RSD)", 0);
            k.BrojStrana = Unos.CeoBroj("Broj strana", 1, 10000);

            _knjige.Add(k);
            Console.WriteLine("Knjiga je dodata (Id = " + k.Id + ").");
        }

        private void Izmeni()
        {
            Knjiga? k = IzaberiKnjigu();
            if (k == null)
                return;

            Console.WriteLine("Pritisnite Enter da zadržite staru vrednost.");

            string isbn = Unos.TekstIzmena("ISBN", k.Isbn);
            while (IsbnZauzet(isbn, k.Id))
            {
                Console.WriteLine("Knjiga sa tim ISBN-om već postoji.");
                isbn = Unos.TekstIzmena("ISBN", k.Isbn);
            }
            k.Isbn = isbn;

            k.Naziv = Unos.TekstIzmena("Naziv", k.Naziv);
            k.Zanr = Unos.TekstIzmena("Žanr", k.Zanr);
            k.GodinaIzdanja = Unos.CeoBrojIzmena("Godina izdanja", k.GodinaIzdanja, 1450, DateTime.Now.Year);
            k.Cena = Unos.DecimalniIzmena("Cena (RSD)", k.Cena, 0);
            k.BrojStrana = Unos.CeoBrojIzmena("Broj strana", k.BrojStrana, 1, 10000);

            _knjige.Update(k);
            Console.WriteLine("Knjiga je izmenjena.");
        }

        private void Obrisi()
        {
            Knjiga? k = IzaberiKnjigu();
            if (k == null)
                return;

            if (!Unos.DaNe("Da li ste sigurni da želite da obrišete knjigu \"" + k.Naziv + "\"?"))
            {
                Console.WriteLine("Brisanje je otkazano.");
                return;
            }

            _knjige.Delete(k.Id);
            Console.WriteLine("Knjiga je obrisana.");
        }

        private Knjiga? IzaberiKnjigu()
        {
            Prikazi();
            if (_knjige.GetAll().Count == 0)
                return null;

            int id = Unos.CeoBroj("Id knjige (0 = odustani)", 0, int.MaxValue);
            if (id == 0)
                return null;

            Knjiga? k = _knjige.GetById(id);
            if (k == null)
                Console.WriteLine("Knjiga sa tim Id-jem ne postoji.");
            return k;
        }

        private bool IsbnZauzet(string isbn, int izuzetiId)
        {
            return _knjige.GetAll().Any(k => k.Id != izuzetiId
                && k.Isbn.Equals(isbn, StringComparison.OrdinalIgnoreCase));
        }
    }
}