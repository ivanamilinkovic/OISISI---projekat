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

            PosetilacKonzola posetilacKonzola = new PosetilacKonzola(posetilacRepository);

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
                    case 2: Console.WriteLine("Još nije implementirano."); break;
                    case 3: Console.WriteLine("Još nije implementirano."); break;
                    case 4: Console.WriteLine("Još nije implementirano."); break;
                    case 5: Console.WriteLine("Još nije implementirano."); break;
                    case 0: return;
                }
            }
        }
    }
}