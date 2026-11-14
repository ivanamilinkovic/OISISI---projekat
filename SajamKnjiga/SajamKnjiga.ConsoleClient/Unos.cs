using System.Globalization;

namespace SajamKnjiga.ConsoleClient
{
    public static class Unos
    {
        private static readonly string[] FormatiDatuma = { "dd.MM.yyyy.", "dd.MM.yyyy", "d.M.yyyy.", "d.M.yyyy" };

        public static string Tekst(string poruka)
        {
            while (true)
            {
                Console.Write(poruka + ": ");
                string? unos = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(unos))
                    return unos.Trim();
                Console.WriteLine("Polje ne sme biti prazno.");
            }
        }

        public static int CeoBroj(string poruka, int min, int max)
        {
            while (true)
            {
                Console.Write(poruka + ": ");
                if (int.TryParse(Console.ReadLine(), out int broj) && broj >= min && broj <= max)
                    return broj;
                Console.WriteLine("Unesite ceo broj od " + min + " do " + max + ".");
            }
        }

        public static decimal Decimalni(string poruka, decimal min)
        {
            while (true)
            {
                Console.Write(poruka + ": ");
                string? unos = Console.ReadLine()?.Replace(',', '.');
                if (decimal.TryParse(unos, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal broj) && broj >= min)
                    return broj;
                Console.WriteLine("Unesite broj veći ili jednak " + min + ".");
            }
        }

        public static DateTime Datum(string poruka)
        {
            while (true)
            {
                Console.Write(poruka + " (dd.MM.yyyy.): ");
                string? unos = Console.ReadLine()?.Trim();
                if (DateTime.TryParseExact(unos, FormatiDatuma, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum))
                    return datum;
                Console.WriteLine("Neispravan datum. Primer: 28.11.2025.");
            }
        }

        public static bool DaNe(string poruka)
        {
            while (true)
            {
                Console.Write(poruka + " (da/ne): ");
                string? unos = Console.ReadLine()?.Trim().ToLower();
                if (unos == "da") return true;
                if (unos == "ne") return false;
                Console.WriteLine("Unesite 'da' ili 'ne'.");
            }
        }

        public static string TekstIzmena(string poruka, string staro)
        {
            Console.Write(poruka + " [" + staro + "]: ");
            string? unos = Console.ReadLine();
            return string.IsNullOrWhiteSpace(unos) ? staro : unos.Trim();
        }

        public static int CeoBrojIzmena(string poruka, int staro, int min, int max)
        {
            while (true)
            {
                Console.Write(poruka + " [" + staro + "]: ");
                string? unos = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(unos))
                    return staro;
                if (int.TryParse(unos, out int broj) && broj >= min && broj <= max)
                    return broj;
                Console.WriteLine("Unesite ceo broj od " + min + " do " + max + ".");
            }
        }

        public static decimal DecimalniIzmena(string poruka, decimal staro, decimal min)
        {
            while (true)
            {
                Console.Write(poruka + " [" + staro + "]: ");
                string? unos = Console.ReadLine()?.Replace(',', '.');
                if (string.IsNullOrWhiteSpace(unos))
                    return staro;
                if (decimal.TryParse(unos, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal broj) && broj >= min)
                    return broj;
                Console.WriteLine("Unesite broj veći ili jednak " + min + ".");
            }
        }

        public static DateTime DatumIzmena(string poruka, DateTime staro)
        {
            while (true)
            {
                Console.Write(poruka + " [" + staro.ToString("dd.MM.yyyy.") + "]: ");
                string? unos = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(unos))
                    return staro;
                if (DateTime.TryParseExact(unos, FormatiDatuma, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum))
                    return datum;
                Console.WriteLine("Neispravan datum. Primer: 28.11.2025.");
            }
        }
    }
}