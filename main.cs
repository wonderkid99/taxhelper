using System;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("====================================================");
        Console.WriteLine("   E-GOV TAX HELPER 🇮🇩 (BANTUAN PENENTUAN PAJAK)");
        Console.WriteLine("   Karyawan | Freelance | Pengusaha");
        Console.WriteLine("   (Array 1D + Array 2D + Sorting + Proc + Func)");
        Console.WriteLine("====================================================\n");

        int n = InputIntMin1("Masukkan jumlah data orang: ");

        // ==========================
        // ARRAY 1 DIMENSI (DATA)
        // ==========================
        string[] nama = new string[n];
        string[] pekerjaan = new string[n];   // karyawan/freelance/pengusaha
        double[] incomeBulanan = new double[n];
        string[] hasilPajak = new string[n];

        // PROCEDURE: input
        InputData(nama, pekerjaan, incomeBulanan);

        // FUNCTION: hitung pajak tiap orang
        for (int i = 0; i < n; i++)
        {
            hasilPajak[i] = TentukanPajak(pekerjaan[i], incomeBulanan[i]);
        }

        // PROCEDURE: sorting
        SortingDesc(nama, pekerjaan, incomeBulanan, hasilPajak);

        // PROCEDURE: tampil hasil
        TampilkanData(nama, pekerjaan, incomeBulanan, hasilPajak);

        // PROCEDURE: tampilkan tabel tarif (array 2D)
        TampilkanTarifProgresif();

        Console.WriteLine("\nEnter buat keluar...");
        Console.ReadLine();
    }

    // ======================================================
    // PROCEDURE: INPUT DATA
    // ======================================================
    static void InputData(string[] nama, string[] pekerjaan, double[] income)
    {
        Console.WriteLine("--- FORM INPUT ---");
        Console.WriteLine("Jenis pekerjaan harus: karyawan / freelance / pengusaha\n");

        for (int i = 0; i < nama.Length; i++)
        {
            Console.Write("Nama ke-" + (i + 1) + ": ");
            nama[i] = Console.ReadLine();

            Console.Write("Pekerjaan (" + nama[i] + "): ");
            pekerjaan[i] = Console.ReadLine().ToLower();

            income[i] = InputDoubleMin0("Penghasilan/Omzet per bulan (Rp): ");
            Console.WriteLine();
        }
    }

    // ======================================================
    // FUNCTION: TENTUKAN PAJAK BERDASARKAN PEKERJAAN
    // ======================================================
    static string TentukanPajak(string pekerjaan, double incomeBulanan)
    {
        if (pekerjaan == "karyawan")
        {
            // Karyawan: PTKP + tarif progresif
            int ptkpPil = PilihPTKP();
            double ptkp = GetPTKPTahunan(ptkpPil);

            double brutoSetahun = incomeBulanan * 12;
            double pkp = brutoSetahun - ptkp;
            if (pkp < 0) pkp = 0;

            double pph21 = HitungPPhProgresif(pkp); // pakai ARRAY 2D
            return "KARYAWAN | PTKP " + GetKodePTKP(ptkpPil) +
                   " | PKP Rp" + Math.Round(pkp, 0) +
                   " | PPh21 Rp" + Math.Round(pph21, 0) + "/th";
        }
        else if (pekerjaan == "freelance")
        {
            // Freelance: pilihan dipotong pihak lain (PPh23) atau bayar sendiri (final UMKM)
            Console.WriteLine("Freelance mode untuk income Rp" + Math.Round(incomeBulanan, 0));
            Console.WriteLine("1) Dipotong pemberi kerja (simulasi PPh23 2%)");
            Console.WriteLine("2) Bayar sendiri (simulasi Final UMKM 0,5% omzet)");
            int mode = InputIntRange("Pilih (1-2): ", 1, 2);

            if (mode == 1)
            {
                double pph23 = incomeBulanan * 0.02;
                return "FREELANCE | PPh23 ~Rp" + Math.Round(pph23, 0) + "/bln (2%)";
            }
            else
            {
                double omzetSetahun = incomeBulanan * 12;
                double finalUmkm = omzetSetahun * 0.005;
                return "FREELANCE | Final UMKM ~Rp" + Math.Round(finalUmkm, 0) + "/th (0,5%)";
            }
        }
        else if (pekerjaan == "pengusaha")
        {
            // Pengusaha: UMKM final atau PKP (PPN simulasi)
            Console.WriteLine("Pengusaha mode untuk omzet Rp" + Math.Round(incomeBulanan, 0));
            Console.WriteLine("1) UMKM (Final 0,5%)");
            Console.WriteLine("2) PKP (PPN simulasi 12%)");
            int mode = InputIntRange("Pilih (1-2): ", 1, 2);

            if (mode == 1)
            {
                double omzetSetahun = incomeBulanan * 12;
                double finalUmkm = omzetSetahun * 0.005;
                return "PENGUSAHA UMKM | Final UMKM Rp" + Math.Round(finalUmkm, 0) + "/th";
            }
            else
            {
                double ppn = incomeBulanan * 0.12;
                return "PENGUSAHA PKP | PPN ~Rp" + Math.Round(ppn, 0) + "/bln (12% sim)";
            }
        }
        else
        {
            return "❌ Pekerjaan invalid";
        }
    }

    // ======================================================
    // PROCEDURE: SORTING (DESC)
    // ======================================================
    static void SortingDesc(string[] nama, string[] pekerjaan, double[] income, string[] hasilPajak)
    {
        for (int i = 0; i < income.Length - 1; i++)
        {
            for (int j = 0; j < income.Length - 1 - i; j++)
            {
                if (income[j] < income[j + 1])
                {
                    SwapString(nama, j, j + 1);
                    SwapString(pekerjaan, j, j + 1);
                    SwapDouble(income, j, j + 1);
                    SwapString(hasilPajak, j, j + 1);
                }
            }
        }
    }

    // ======================================================
    // PROCEDURE: OUTPUT DATA
    // ======================================================
    static void TampilkanData(string[] nama, string[] pekerjaan, double[] income, string[] hasilPajak)
    {
        Console.WriteLine("\n====================== HASIL DATA ======================");
        Console.WriteLine("No\tNama\t\tKerja\t\tIncome/Bln\tPajak");
        Console.WriteLine("--------------------------------------------------------");

        for (int i = 0; i < nama.Length; i++)
        {
            Console.WriteLine(
                (i + 1) + "\t" +
                nama[i] + "\t\t" +
                pekerjaan[i] + "\t\t" +
                "Rp" + Math.Round(income[i], 0) + "\t" +
                hasilPajak[i]
            );
        }

        Console.WriteLine("========================================================");
    }

    // ======================================================
    // FUNCTION: PILIH PTKP
    // ======================================================
    static int PilihPTKP()
    {
        Console.WriteLine("\nPilih PTKP (Karyawan):");
        Console.WriteLine("1) TK/0  (54.000.000)");
        Console.WriteLine("2) TK/1  (58.500.000)");
        Console.WriteLine("3) TK/2  (63.000.000)");
        Console.WriteLine("4) TK/3  (67.500.000)");
        Console.WriteLine("5) K/0   (58.500.000)");
        Console.WriteLine("6) K/1   (63.000.000)");
        Console.WriteLine("7) K/2   (67.500.000)");
        Console.WriteLine("8) K/3   (72.000.000)");
        return InputIntRange("Pilih (1-8): ", 1, 8);
    }

    static string GetKodePTKP(int pilihan)
    {
        if (pilihan == 1) return "TK/0";
        if (pilihan == 2) return "TK/1";
        if (pilihan == 3) return "TK/2";
        if (pilihan == 4) return "TK/3";
        if (pilihan == 5) return "K/0";
        if (pilihan == 6) return "K/1";
        if (pilihan == 7) return "K/2";
        return "K/3";
    }

    static double GetPTKPTahunan(int pilihan)
    {
        if (pilihan == 1) return 54000000;
        if (pilihan == 2) return 58500000;
        if (pilihan == 3) return 63000000;
        if (pilihan == 4) return 67500000;
        if (pilihan == 5) return 58500000;
        if (pilihan == 6) return 63000000;
        if (pilihan == 7) return 67500000;
        return 72000000;
    }

    // ======================================================
    // ✅ ARRAY MULTI DIMENSI (2D) — TARIF PROGRESIF PASAL 17
    // ======================================================
    static double HitungPPhProgresif(double pkp)
    {
        // tabelTarif[i,0] = batas bawah
        // tabelTarif[i,1] = batas atas
        // tabelTarif[i,2] = tarif %
        double[,] tabelTarif = new double[,]
        {
            { 0,          60000000,    0.05 },
            { 60000000,   250000000,   0.15 },
            { 250000000,  500000000,   0.25 },
            { 500000000,  5000000000,  0.30 },
            { 5000000000, double.MaxValue, 0.35 }
        };

        double pajak = 0;

        for (int i = 0; i < tabelTarif.GetLength(0); i++)
        {
            double bawah = tabelTarif[i, 0];
            double atas = tabelTarif[i, 1];
            double rate = tabelTarif[i, 2];

            if (pkp > bawah)
            {
                double kena = Math.Min(pkp, atas) - bawah;
                pajak += kena * rate;
            }
        }

        return pajak;
    }

    // ======================================================
    // PROCEDURE: tampil tarif progresif (biar keliatan multi-dimensi kepake)
    // ======================================================
    static void TampilkanTarifProgresif()
    {
        double[,] tabelTarif = new double[,]
        {
            { 0,          60000000,    0.05 },
            { 60000000,   250000000,   0.15 },
            { 250000000,  500000000,   0.25 },
            { 500000000,  5000000000,  0.30 },
            { 5000000000, 999999999999, 0.35 }
        };

        Console.WriteLine("\n=== TABEL TARIF PROGRESIF (ARRAY 2D) ===");
        Console.WriteLine("Bawah\t\tAtas\t\tTarif");

        for (int i = 0; i < tabelTarif.GetLength(0); i++)
        {
            Console.WriteLine(
                "Rp" + Math.Round(tabelTarif[i, 0], 0) + "\t" +
                "Rp" + Math.Round(tabelTarif[i, 1], 0) + "\t" +
                (tabelTarif[i, 2] * 100) + "%"
            );
        }
    }

    // ======================================================
    // INPUT SAFE
    // ======================================================
    static int InputIntMin1(string pesan)
    {
        while (true)
        {
            Console.Write(pesan);
            string input = Console.ReadLine();
            int val;

            if (int.TryParse(input, out val) && val >= 1) return val;
            Console.WriteLine("❌ Harus angka >=1 ya ges.\n");
        }
    }

    static int InputIntRange(string pesan, int min, int max)
    {
        while (true)
        {
            Console.Write(pesan);
            string input = Console.ReadLine();
            int val;

            if (int.TryParse(input, out val) && val >= min && val <= max) return val;
            Console.WriteLine("❌ Harus angka " + min + "-" + max + " ya ges.\n");
        }
    }

    static double InputDoubleMin0(string pesan)
    {
        while (true)
        {
            Console.Write(pesan);
            string input = Console.ReadLine();
            double val;

            if (double.TryParse(input, out val) && val >= 0) return val;
            Console.WriteLine("❌ Harus angka >=0 ya ges.\n");
        }
    }

    // ======================================================
    // SWAP HELPERS
    // ======================================================
    static void SwapString(string[] arr, int a, int b)
    {
        string temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }

    static void SwapDouble(double[] arr, int a, int b)
    {
        double temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp;
    }
}