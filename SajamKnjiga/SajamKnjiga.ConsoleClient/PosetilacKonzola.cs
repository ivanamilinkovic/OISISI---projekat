
using SajamKnjiga.Core.DAO;
using SajamKnjiga.Core.Models;

namespace SajamKnjiga.ConsoleClient
{
    public class PosetilacKonzola
    {
        private readonly PosetilacRepository _posetioci;

        public PosetilacKonzola(PosetilacRepository posetioci)
        {
            _posetioci = posetioci;
        }

        public void Meni()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== POSETIOCI =====");
                Console.WriteLine("1. Prikaz svih posetilaca");
                Console.WriteLine("2. Dodavanje posetioca");
                Console.WriteLine("3. Izmena posetioca");
                Console.WriteLine("4. Brisanje posetioca");
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
            List<Posetilac> svi = _posetioci.GetAll();
            if (svi.Count == 0)
            {
                Console.WriteLine("Nema unetih posetilaca.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(string.Format("{0,-4} {1,-12} {2,-15} {3,-15} {4,-40} {5}",
                "Id", "Br. karte", "Ime", "Prezime", "Adresa", "Status"));
            foreach (Posetilac p in svi)
            {
                Console.WriteLine(string.Format("{0,-4} {1,-12} {2,-15} {3,-15} {4,-40} {5}",
                    p.Id, p.BrojClanskeKarte, p.Ime, p.Prezime, p.Adresa, p.Status));
            }
        }

        private void Dodaj()
        {
            Posetilac p = new Posetilac();
            p.Ime = Unos.Tekst("Ime");
            p.Prezime = Unos.Tekst("Prezime");
            p.DatumRodjenja = Unos.Datum("Datum rođenja");
            p.Adresa = AdresaUnos.Nova();
            p.KontaktTelefon = Unos.Tekst("Kontakt telefon");
            p.Email = Unos.Tekst("E-mejl");

            string broj = Unos.Tekst("Broj članske karte (npr. CK-1-2023)");
            while (BrojKarteZauzet(broj, 0))
            {
                Console.WriteLine("Posetilac sa tim brojem članske karte već postoji.");
                broj = Unos.Tekst("Broj članske karte");
            }
            p.BrojClanskeKarte = broj;

            p.TrenutnaGodinaClanstva = Unos.CeoBroj("Trenutna godina članstva", 1, 100);
            p.Status = UnesiStatus(null);

            _posetioci.Add(p);
            Console.WriteLine("Posetilac je dodat (Id = " + p.Id + ").");
        }

        private void Izmeni()
        {
            Posetilac? p = IzaberiPosetioca();
            if (p == null)
                return;

            Console.WriteLine("Pritisnite Enter da zadržite staru vrednost.");
            p.Ime = Unos.TekstIzmena("Ime", p.Ime);
            p.Prezime = Unos.TekstIzmena("Prezime", p.Prezime);
            p.DatumRodjenja = Unos.DatumIzmena("Datum rođenja", p.DatumRodjenja);
            p.Adresa = AdresaUnos.Izmena(p.Adresa);
            p.KontaktTelefon = Unos.TekstIzmena("Kontakt telefon", p.KontaktTelefon);
            p.Email = Unos.TekstIzmena("E-mejl", p.Email);

            string broj = Unos.TekstIzmena("Broj članske karte", p.BrojClanskeKarte);
            while (BrojKarteZauzet(broj, p.Id))
            {
                Console.WriteLine("Posetilac sa tim brojem članske karte već postoji.");
                broj = Unos.TekstIzmena("Broj članske karte", p.BrojClanskeKarte);
            }
            p.BrojClanskeKarte = broj;

            p.TrenutnaGodinaClanstva = Unos.CeoBrojIzmena("Trenutna godina članstva", p.TrenutnaGodinaClanstva, 1, 100);
            p.Status = UnesiStatus(p.Status);

            _posetioci.Update(p);
            Console.WriteLine("Posetilac je izmenjen.");
        }

        private void Obrisi()
        {
            Posetilac? p = IzaberiPosetioca();
            if (p == null)
                return;

            if (!Unos.DaNe("Da li ste sigurni da želite da obrišete posetioca " + p.Ime + " " + p.Prezime + "?"))
            {
                Console.WriteLine("Brisanje je otkazano.");
                return;
            }

            _posetioci.Delete(p.Id);
            Console.WriteLine("Posetilac je obrisan.");
        }

        private Posetilac? IzaberiPosetioca()
        {
            Prikazi();
            if (_posetioci.GetAll().Count == 0)
                return null;

            int id = Unos.CeoBroj("Id posetioca (0 = odustani)", 0, int.MaxValue);
            if (id == 0)
                return null;

            Posetilac? p = _posetioci.GetById(id);
            if (p == null)
                Console.WriteLine("Posetilac sa tim Id-jem ne postoji.");
            return p;
        }

        private bool BrojKarteZauzet(string broj, int izuzetiId)
        {
            return _posetioci.GetAll().Any(p => p.Id != izuzetiId
                && p.BrojClanskeKarte.Equals(broj, StringComparison.OrdinalIgnoreCase));
        }

        private static StatusPosetioca UnesiStatus(StatusPosetioca? staro)
        {
            while (true)
            {
                string uglaste = staro == null ? "" : " [" + staro + "]";
                Console.Write("Status (R = redovan, V = VIP)" + uglaste + ": ");
                string unos = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (unos == "" && staro != null)
                    return staro.Value;
                if (unos == "R")
                    return StatusPosetioca.R;
                if (unos == "V")
                    return StatusPosetioca.V;

                Console.WriteLine("Unesite R ili V.");
            }
        }
    }
}