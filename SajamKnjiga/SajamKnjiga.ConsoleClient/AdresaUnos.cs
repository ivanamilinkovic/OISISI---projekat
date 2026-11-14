using SajamKnjiga.Core.Models;

namespace SajamKnjiga.ConsoleClient
{
    public static class AdresaUnos
    {
        public static Adresa Nova()
        {
            Console.WriteLine("-- Adresa --");
            Adresa adresa = new Adresa();
            adresa.Ulica = Unos.Tekst("Ulica");
            adresa.Broj = Unos.Tekst("Broj");
            adresa.Grad = Unos.Tekst("Grad");
            adresa.Drzava = Unos.Tekst("Država");
            return adresa;
        }

        public static Adresa Izmena(Adresa stara)
        {
            Console.WriteLine("-- Adresa --");
            Adresa adresa = new Adresa();
            adresa.Ulica = Unos.TekstIzmena("Ulica", stara.Ulica);
            adresa.Broj = Unos.TekstIzmena("Broj", stara.Broj);
            adresa.Grad = Unos.TekstIzmena("Grad", stara.Grad);
            adresa.Drzava = Unos.TekstIzmena("Država", stara.Drzava);
            return adresa;
        }
    }
}