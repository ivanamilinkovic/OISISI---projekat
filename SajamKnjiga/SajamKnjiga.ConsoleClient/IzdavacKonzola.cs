
using SajamKnjiga.Core.DAO;
using SajamKnjiga.Core.Models;

namespace SajamKnjiga.ConsoleClient
{
    public class IzdavacKonzola
    {
        private const int MinGodinaIskustvaSefa = 5;

        private readonly IzdavacRepository _izdavaci;
        private readonly AutorRepository _autori;
        private readonly KnjigaRepository _knjige;

        public IzdavacKonzola(IzdavacRepository izdavaci, AutorRepository autori, KnjigaRepository knjige)
        {
            _izdavaci = izdavaci;
            _autori = autori;
            _knjige = knjige;
        }

        public void Meni()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== IZDAVAČI =====");
                Console.WriteLine("1. Prikaz svih izdavača");
                Console.WriteLine("2. Dodavanje izdavača");
                Console.WriteLine("3. Izmena izdavača");
                Console.WriteLine("4. Brisanje izdavača");
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
            List<Izdavac> svi = _izdavaci.GetAll();
            if (svi.Count == 0)
            {
                Console.WriteLine("Nema unetih izdavača.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(string.Format("{0,-4} {1,-10} {2,-30} {3,-25} {4,-7} {5}",
                "Id", "Šifra", "Naziv", "Šef", "Autora", "Knjiga"));
            foreach (Izdavac i in svi)
            {
                Console.WriteLine(string.Format("{0,-4} {1,-10} {2,-30} {3,-25} {4,-7} {5}",
                    i.Id, i.Sifra, i.Naziv, OpisSefa(i.SefId), i.AutoriIds.Count, i.KnjigeIds.Count));
            }
        }

        private void Dodaj()
        {
            Izdavac i = new Izdavac();

            string sifra = Unos.Tekst("Šifra");
            while (SifraZauzeta(sifra, 0))
            {
                Console.WriteLine("Izdavač sa tom šifrom već postoji.");
                sifra = Unos.Tekst("Šifra");
            }
            i.Sifra = sifra;

            i.Naziv = Unos.Tekst("Naziv");

            if (Unos.DaNe("Da li želite da postavite šefa izdavača?"))
                i.SefId = IzaberiSefa();

            _izdavaci.Add(i);
            Console.WriteLine("Izdavač je dodat (Id = " + i.Id + ").");
        }

        private void Izmeni()
        {
            Izdavac? i = IzaberiIzdavaca();
            if (i == null)
                return;

            Console.WriteLine("Pritisnite Enter da zadržite staru vrednost.");

            string sifra = Unos.TekstIzmena("Šifra", i.Sifra);
            while (SifraZauzeta(sifra, i.Id))
            {
                Console.WriteLine("Izdavač sa tom šifrom već postoji.");
                sifra = Unos.TekstIzmena("Šifra", i.Sifra);
            }
            i.Sifra = sifra;

            i.Naziv = Unos.TekstIzmena("Naziv", i.Naziv);

            Console.WriteLine("Trenutni šef: " + OpisSefa(i.SefId));
            if (Unos.DaNe("Da li želite da promenite šefa?"))
                i.SefId = IzaberiSefa();

            _izdavaci.Update(i);
            Console.WriteLine("Izdavač je izmenjen.");
        }

        private void Obrisi()
        {
            Izdavac? i = IzaberiIzdavaca();
            if (i == null)
                return;

            if (!Unos.DaNe("Da li ste sigurni da želite da obrišete izdavača " + i.Naziv + "?"))
            {
                Console.WriteLine("Brisanje je otkazano.");
                return;
            }

            _izdavaci.Delete(i.Id);

            // Knjige ovog izdavača ostaju bez izdavača.
            foreach (Knjiga k in _knjige.GetAll().Where(k => k.IzdavacId == i.Id).ToList())
            {
                k.IzdavacId = null;
                _knjige.Update(k);
            }

            Console.WriteLine("Izdavač je obrisan.");
        }

        private int? IzaberiSefa()
        {
            List<Autor> autori = _autori.GetAll();
            if (autori.Count == 0)
            {
                Console.WriteLine("Nema unetih autora. Izdavač ostaje bez šefa.");
                return null;
            }

            Console.WriteLine();
            Console.WriteLine("Autori:");
            foreach (Autor a in autori)
                Console.WriteLine("  " + a.Id + ". " + a.Ime + " " + a.Prezime + " (" + a.GodineIskustva + " god. iskustva)");

            while (true)
            {
                int id = Unos.CeoBroj("Id autora (0 = bez šefa)", 0, int.MaxValue);
                if (id == 0)
                    return null;

                Autor? a = _autori.GetById(id);
                if (a == null)
                {
                    Console.WriteLine("Autor sa tim Id-jem ne postoji.");
                    continue;
                }
                if (a.GodineIskustva < MinGodinaIskustvaSefa)
                {
                    Console.WriteLine("Šef mora imati najmanje " + MinGodinaIskustvaSefa + " godina iskustva. Izaberite drugog autora.");
                    continue;
                }
                return a.Id;
            }
        }

        private Izdavac? IzaberiIzdavaca()
        {
            Prikazi();
            if (_izdavaci.GetAll().Count == 0)
                return null;

            int id = Unos.CeoBroj("Id izdavača (0 = odustani)", 0, int.MaxValue);
            if (id == 0)
                return null;

            Izdavac? i = _izdavaci.GetById(id);
            if (i == null)
                Console.WriteLine("Izdavač sa tim Id-jem ne postoji.");
            return i;
        }

        private bool SifraZauzeta(string sifra, int izuzetiId)
        {
            return _izdavaci.GetAll().Any(i => i.Id != izuzetiId
                && i.Sifra.Equals(sifra, StringComparison.OrdinalIgnoreCase));
        }

        private string OpisSefa(int? sefId)
        {
            if (sefId == null)
                return "-";
            Autor? a = _autori.GetById(sefId.Value);
            return a == null ? "(obrisan)" : a.Ime + " " + a.Prezime;
        }
    }
}