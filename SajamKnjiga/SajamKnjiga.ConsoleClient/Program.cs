
using System.Text;
using SajamKnjiga.Core.DAO;

namespace SajamKnjiga.ConsoleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            PosetilacRepository posetilacRepository = new PosetilacRepository();
            AutorRepository autorRepository = new AutorRepository();
            KnjigaRepository knjigaRepository = new KnjigaRepository();
            KupovinaRepository kupovinaRepository = new KupovinaRepository();
            IzdavacRepository izdavacRepository = new IzdavacRepository();

            PosetilacKonzola posetilacKonzola = new PosetilacKonzola(posetilacRepository);
            AutorKonzola autorKonzola = new AutorKonzola(autorRepository);
            KnjigaKonzola knjigaKonzola = new KnjigaKonzola(knjigaRepository);
            KupovinaKonzola kupovinaKonzola = new KupovinaKonzola(kupovinaRepository, posetilacRepository, knjigaRepository);
            IzdavacKonzola izdavacKonzola = new IzdavacKonzola(izdavacRepository, autorRepository, knjigaRepository);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("========== SAJAM KNJIGA ==========");
                Console.WriteLine("1. Posetioci");
                Console.WriteLine("2. Kupovine");
                Console.WriteLine("3. Izdavači");
                Console.WriteLine("4. Autori");
                Console.WriteLine("5. Knjige");
                Console.WriteLine("0. Izlaz");

                int izbor = Unos.CeoBroj("Izbor", 0, 5);
                switch (izbor)
                {
                    case 1: posetilacKonzola.Meni(); break;
                    case 2: kupovinaKonzola.Meni(); break;
                    case 3: izdavacKonzola.Meni(); break;
                    case 4: autorKonzola.Meni(); break;
                    case 5: knjigaKonzola.Meni(); break;
                    case 0: return;
                }
            }
        }
    }
}