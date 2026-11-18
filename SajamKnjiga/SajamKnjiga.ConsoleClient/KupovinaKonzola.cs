
using SajamKnjiga.Core.DAO;
using SajamKnjiga.Core.Models;

namespace SajamKnjiga.ConsoleClient
{
    public class KupovinaKonzola
    {
        private readonly KupovinaRepository _kupovine;
        private readonly PosetilacRepository _posetioci;
        private readonly KnjigaRepository _knjige;

        public KupovinaKonzola(KupovinaRepository kupovine, PosetilacRepository posetioci, KnjigaRepository knjige)
        {
            _kupovine = kupovine;
            _posetioci = posetioci;
            _knjige = knjige;
        }

        public void Meni()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== KUPOVINE =====");
                Console.WriteLine("1. Prikaz svih kupovina");
                Console.WriteLine("2. Dodavanje kupovine");
                Console.WriteLine("3. Izmena kupovine");
                Console.WriteLine("4. Brisanje kupovine");
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
            List<Kupovina> sve = _kupovine.GetAll();
            if (sve.Count == 0)
            {
                Console.WriteLine("Nema unetih kupovina.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(string.Format("{0,-4} {1,-25} {2,-30} {3,-12} {4,-6} {5}",
                "Id", "Posetilac", "Knjiga", "Datum", "Ocena", "Komentar"));
            foreach (Kupovina k in sve)
            {
                Console.WriteLine(string.Format("{0,-4} {1,-25} {2,-30} {3,-12} {4,-6} {5}",
                    k.Id, OpisPosetioca(k.PosetilacId), OpisKnjige(k.KnjigaId),
                    k.DatumKupovine.ToString("dd.MM.yyyy."), k.Ocena, k.Komentar));
            }
        }

        private void Dodaj()
        {
            Posetilac? posetilac = IzaberiPosetioca();
            if (posetilac == null)
                return;

            Knjiga? knjiga = IzaberiKnjigu();
            if (knjiga == null)
                return;

            if (_kupovine.GetAll().Any(k => k.PosetilacId == posetilac.Id && k.KnjigaId == knjiga.Id))
            {
                Console.WriteLine("Ovaj posetilac je već kupio ovu knjigu.");
                return;
            }

            Kupovina kupovina = new Kupovina();
            kupovina.PosetilacId = posetilac.Id;
            kupovina.KnjigaId = knjiga.Id;
            kupovina.DatumKupovine = Unos.Datum("Datum kupovine");
            kupovina.Ocena = Unos.CeoBroj("Ocena (1-5)", 1, 5);
            kupovina.Komentar = UnesiKomentar("");
            _kupovine.Add(kupovina);

            // Kupljena knjiga prelazi kod posetioca u kupljene i skida se sa liste želja.
            posetilac.KupovineIds.Add(kupovina.Id);
            posetilac.ListaZeljaIds.Remove(knjiga.Id);
            knjiga.KupciIds.Add(posetilac.Id);
            knjiga.ListaZeljaIds.Remove(posetilac.Id);
            _knjige.Update(knjiga);
            OsveziProsecnuOcenu(posetilac);

            Console.WriteLine("Kupovina je dodata (Id = " + kupovina.Id + ").");
        }

        private void Izmeni()
        {
            Kupovina? k = IzaberiKupovinu();
            if (k == null)
                return;

            Console.WriteLine("Pritisnite Enter da zadržite staru vrednost.");
            k.DatumKupovine = Unos.DatumIzmena("Datum kupovine", k.DatumKupovine);
            k.Ocena = Unos.CeoBrojIzmena("Ocena (1-5)", k.Ocena, 1, 5);
            k.Komentar = UnesiKomentar(k.Komentar);
            _kupovine.Update(k);

            Posetilac? p = _posetioci.GetById(k.PosetilacId);
            if (p != null)
                OsveziProsecnuOcenu(p);

            Console.WriteLine("Kupovina je izmenjena.");
        }

        private void Obrisi()
        {
            Kupovina? k = IzaberiKupovinu();
            if (k == null)
                return;

            if (!Unos.DaNe("Da li ste sigurni da želite da obrišete ovu kupovinu?"))
            {
                Console.WriteLine("Brisanje je otkazano.");
                return;
            }

            _kupovine.Delete(k.Id);

            Posetilac? p = _posetioci.GetById(k.PosetilacId);
            if (p != null)
            {
                p.KupovineIds.Remove(k.Id);
                OsveziProsecnuOcenu(p);
            }

            Knjiga? knjiga = _knjige.GetById(k.KnjigaId);
            if (knjiga != null)
            {
                knjiga.KupciIds.Remove(k.PosetilacId);
                _knjige.Update(knjiga);
            }

            Console.WriteLine("Kupovina je obrisana.");
        }

        private void OsveziProsecnuOcenu(Posetilac p)
        {
            List<Kupovina> njegove = _kupovine.GetAll().Where(k => k.PosetilacId == p.Id).ToList();
            p.ProsecnaOcena = njegove.Count == 0 ? 0 : njegove.Average(k => k.Ocena);
            _posetioci.Update(p);
        }

        private Kupovina? IzaberiKupovinu()
        {
            Prikazi();
            if (_kupovine.GetAll().Count == 0)
                return null;

            int id = Unos.CeoBroj("Id kupovine (0 = odustani)", 0, int.MaxValue);
            if (id == 0)
                return null;

            Kupovina? k = _kupovine.GetById(id);
            if (k == null)
                Console.WriteLine("Kupovina sa tim Id-jem ne postoji.");
            return k;
        }

        private Posetilac? IzaberiPosetioca()
        {
            List<Posetilac> svi = _posetioci.GetAll();
            if (svi.Count == 0)
            {
                Console.WriteLine("Nema unetih posetilaca. Prvo dodajte posetioca.");
                return null;
            }

            Console.WriteLine();
            Console.WriteLine("Posetioci:");
            foreach (Posetilac p in svi)
                Console.WriteLine("  " + p.Id + ". " + p.BrojClanskeKarte + " - " + p.Ime + " " + p.Prezime);

            int id = Unos.CeoBroj("Id posetioca (0 = odustani)", 0, int.MaxValue);
            if (id == 0)
                return null;

            Posetilac? izabran = _posetioci.GetById(id);
            if (izabran == null)
                Console.WriteLine("Posetilac sa tim Id-jem ne postoji.");
            return izabran;
        }

        private Knjiga? IzaberiKnjigu()
        {
            List<Knjiga> sve = _knjige.GetAll();
            if (sve.Count == 0)
            {
                Console.WriteLine("Nema unetih knjiga. Prvo dodajte knjigu.");
                return null;
            }

            Console.WriteLine();
            Console.WriteLine("Knjige:");
            foreach (Knjiga k in sve)
                Console.WriteLine("  " + k.Id + ". " + k.Isbn + " - " + k.Naziv);

            int id = Unos.CeoBroj("Id knjige (0 = odustani)", 0, int.MaxValue);
            if (id == 0)
                return null;

            Knjiga? izabrana = _knjige.GetById(id);
            if (izabrana == null)
                Console.WriteLine("Knjiga sa tim Id-jem ne postoji.");
            return izabrana;
        }

        private string OpisPosetioca(int id)
        {
            Posetilac? p = _posetioci.GetById(id);
            return p == null ? "(obrisan)" : p.Ime + " " + p.Prezime;
        }

        private string OpisKnjige(int id)
        {
            Knjiga? k = _knjige.GetById(id);
            return k == null ? "(obrisana)" : k.Naziv;
        }

        private static string UnesiKomentar(string staro)
        {
            string uglaste = staro == "" ? "" : " [" + staro + "]";
            Console.Write("Komentar (nije obavezan)" + uglaste + ": ");
            string? unos = Console.ReadLine();
            return string.IsNullOrWhiteSpace(unos) ? staro : unos.Trim();
        }
    }
}