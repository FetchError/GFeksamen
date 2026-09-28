//Eksamensprojekt GF2 - CMIS informationsstander til nyhedsbreve

namespace IntroProgram;

class Program
{
    // ============================================================
    // DATA - gemmes i arrays som alle metoder kan bruge
    // ============================================================

    // Største antal brugere der kan gemmes (single source of truth)
    static int maksBrugere = 100;

    // Antal brugere der er gemt lige nu
    static int antalBrugere = 0;

    // Password til admin-menuen
    static string adminPassword = "admin123";

    // Antal brugere der vises på én side
    static int linjerPrSide = 14;

    // Parallelle arrays - bruger nummer i ligger på plads i i alle arrays
    static string[] telefoner = new string[maksBrugere];
    static string[] fornavne = new string[maksBrugere];
    static string[] efternavne = new string[maksBrugere];
    static int[] aldre = new int[maksBrugere];
    static string[] adresser = new string[maksBrugere];
    static int[] postnumre = new int[maksBrugere];
    static string[] byer = new string[maksBrugere];
    static string[] emails = new string[maksBrugere];
    static int[] frekvenser = new int[maksBrugere];

    // ============================================================
    // HOVEDPROGRAM
    // ============================================================

    static void Main(string[] args)
    {
        // Opretter de 20 brugere der skal findes på forhånd
        LoadStartUsers();

        // Variabel der styrer om programmet skal køre videre
        bool kørProgram = true;

        // Hovedmenuen vises igen og igen indtil brugeren vælger 9
        while (kørProgram)
        {
            ShowHeader("CMIS - Tilmelding til nyhedsbrev");
            Console.WriteLine("1. Opret bruger");
            Console.WriteLine("2. Admin (kræver password)");
            Console.WriteLine("9. Afslut");
            Console.WriteLine(" ");
            Console.Write("Vælg 1, 2 eller 9: ");

            // Læser brugerens valg og sikrer mod null
            string valg = Console.ReadLine() ?? "";

            // Tjekker hvilket menupunkt brugeren har valgt
            switch (valg)
            {
                case "1":
                    CreateUser();
                    break;

                case "2":
                    // Admin-menuen vises kun hvis password er rigtigt
                    if (AdminLogin())
                    {
                        AdminMenu();
                    }
                    break;

                case "9":
                    // Stopper løkken så programmet afsluttes pænt
                    kørProgram = false;
                    break;

                default:
                    ShowError("Ugyldigt valg - tast 1, 2 eller 9");
                    WaitForEnter();
                    break;
            }
        }

        Console.WriteLine(" ");
        Console.WriteLine("Tak for besøget - programmet afsluttes.");
        Console.Write("Tryk på en tast for at lukke...");
        Console.ReadKey();
    }

    // ============================================================
    // OPRET BRUGER
    // ============================================================

    // Guider brugeren igennem tilmeldingen til nyhedsbrevet
    static void CreateUser()
    {
        ShowHeader("Opret bruger");

        // Tjekker om der er plads til flere brugere i arrays
        if (antalBrugere >= maksBrugere)
        {
            ShowError("Der er desværre ikke plads til flere brugere");
        }
        else
        {
            string telefon = InputPhone();

            // Tjekker om telefonnummeret findes i forvejen
            if (FindPhoneIndex(telefon) != -1)
            {
                ShowError("Telefonnummeret " + telefon + " er allerede oprettet");
            }
            else
            {
                Console.WriteLine("Nummeret er ledigt - udfyld resten af oplysningerne:");
                Console.WriteLine(" ");

                // Spørger om alle oplysninger - hver metode tjekker selv inputtet
                string fornavn = InputText("Fornavn: ");
                string efternavn = InputText("Efternavn: ");
                int alder = InputNumber("Alder: ", 1, 120);
                string adresse = InputText("Adresse: ");
                int postnummer = InputNumber("Postnummer (4 cifre): ", 1000, 9999);
                string by = InputText("By: ");
                string email = InputEmail();
                int frekvens = InputFrequency();

                // Gemmer brugeren i arrays
                AddUser(telefon, fornavn, efternavn, alder, adresse, postnummer, by, email, frekvens);

                // Kvittering til brugeren i grøn
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Tak {fornavn}! Du er nu tilmeldt nyhedsbrevet: {FrequencyText(frekvens)}");
                Console.ResetColor();
            }
        }

        WaitForEnter();
    }

    // Gemmer én bruger på den første ledige plads i alle arrays
    static void AddUser(string telefon, string fornavn, string efternavn, int alder, string adresse, int postnummer, string by, string email, int frekvens)
    {
        telefoner[antalBrugere] = telefon;
        fornavne[antalBrugere] = fornavn;
        efternavne[antalBrugere] = efternavn;
        aldre[antalBrugere] = alder;
        adresser[antalBrugere] = adresse;
        postnumre[antalBrugere] = postnummer;
        byer[antalBrugere] = by;
        emails[antalBrugere] = email;
        frekvenser[antalBrugere] = frekvens;

        // Tæller antal brugere 1 op, så næste bruger kommer på næste plads
        antalBrugere++;
    }

    // Finder pladsen i arrayet for et telefonnummer - giver -1 hvis det ikke findes
    static int FindPhoneIndex(string telefon)
    {
        int indeks = -1;

        // Løber alle gemte brugere igennem
        for (int i = 0; i < antalBrugere; i++)
        {
            if (telefoner[i] == telefon)
            {
                indeks = i;
            }
        }

        return indeks;
    }

    // ============================================================
    // INDTASTNING OG KONTROL AF INPUT
    // ============================================================

    // Tjekker om en tekst kun består af cifre (0-9)
    static bool IsOnlyDigits(string tekst)
    {
        string cifre = "0123456789";

        // En tom tekst er ikke et tal
        bool kunTal = tekst.Length > 0;

        // Tjekker tegn for tegn om det er et ciffer
        for (int i = 0; i < tekst.Length; i++)
        {
            if (!cifre.Contains(tekst.Substring(i, 1)))
            {
                kunTal = false;
            }
        }

        return kunTal;
    }

    // Spørger om en tekst indtil brugeren har skrevet noget
    static string InputText(string spørgsmål)
    {
        string svar = "";
        bool gyldig = false;

        while (!gyldig)
        {
            Console.Write(spørgsmål);
            svar = Console.ReadLine() ?? "";

            if (svar == "")
            {
                ShowError("Feltet må ikke være tomt - prøv igen");
            }
            else
            {
                gyldig = true;
            }
        }

        return svar;
    }

    // Spørger om et heltal indtil det er et tal mellem min og max
    static int InputNumber(string spørgsmål, int min, int max)
    {
        int tal = 0;
        bool gyldig = false;

        while (!gyldig)
        {
            Console.Write(spørgsmål);
            string svar = Console.ReadLine() ?? "";

            // Tjekker først at der kun er cifre, så int.Parse ikke crasher
            if (!IsOnlyDigits(svar))
            {
                ShowError("Du skal skrive et tal (kun cifre) - prøv igen");
            }
            // Et tal med mere end 9 cifre er for stort til en int
            else if (svar.Length > 9)
            {
                ShowError("Tallet er for stort - prøv igen");
            }
            else
            {
                tal = int.Parse(svar);

                // Tjekker at tallet er inden for grænserne
                if (tal < min || tal > max)
                {
                    ShowError($"Tallet skal være mellem {min} og {max} - prøv igen");
                }
                else
                {
                    gyldig = true;
                }
            }
        }

        return tal;
    }

    // Spørger om et telefonnummer indtil det er præcis 8 cifre
    static string InputPhone()
    {
        string telefon = "";
        bool gyldig = false;

        while (!gyldig)
        {
            Console.Write("Indtast telefonnummer (8 cifre): ");
            telefon = Console.ReadLine() ?? "";

            if (telefon.Length != 8 || !IsOnlyDigits(telefon))
            {
                ShowError("Telefonnummeret skal være præcis 8 cifre, f.eks. 12345678");
            }
            else
            {
                gyldig = true;
            }
        }

        return telefon;
    }

    // Spørger om en e-mail indtil den indeholder @ og punktum og ingen mellemrum
    static string InputEmail()
    {
        string email = "";
        bool gyldig = false;

        while (!gyldig)
        {
            Console.Write("E-mail: ");
            email = Console.ReadLine() ?? "";

            if (!email.Contains("@") || !email.Contains(".") || email.Contains(" "))
            {
                ShowError("Ugyldig e-mail - den skal f.eks. se sådan ud: navn@mail.dk");
            }
            else
            {
                gyldig = true;
            }
        }

        return email;
    }

    // Spørger hvor ofte brugeren vil have nyhedsbrevet (12, 4 eller 1 gang om året)
    static int InputFrequency()
    {
        int frekvens = 0;
        bool gyldig = false;

        Console.WriteLine("Hvor ofte vil du have nyhedsbrevet?");
        Console.WriteLine("  12 = Hver måned");
        Console.WriteLine("   4 = Hver 3. måned");
        Console.WriteLine("   1 = Én gang om året");

        while (!gyldig)
        {
            Console.Write("Vælg 12, 4 eller 1: ");
            string svar = Console.ReadLine() ?? "";

            if (svar == "12" || svar == "4" || svar == "1")
            {
                frekvens = int.Parse(svar);
                gyldig = true;
            }
            else
            {
                ShowError("Du skal vælge 12, 4 eller 1");
            }
        }

        return frekvens;
    }

    // ============================================================
    // ADMIN
    // ============================================================

    // Beder om password - brugeren har 3 forsøg
    static bool AdminLogin()
    {
        ShowHeader("Admin login");

        int forsøg = 0;
        bool godkendt = false;

        // Fortsætter indtil password er rigtigt eller der er brugt 3 forsøg
        while (!godkendt && forsøg < 3)
        {
            Console.Write("Indtast password: ");
            string password = Console.ReadLine() ?? "";
            forsøg++;

            if (password == adminPassword)
            {
                godkendt = true;
            }
            else
            {
                ShowError("Forkert password - du har " + (3 - forsøg) + " forsøg tilbage");
            }
        }

        // Hvis alle forsøg er brugt, sendes brugeren tilbage til hovedmenuen
        if (!godkendt)
        {
            ShowError("For mange forkerte forsøg - du sendes tilbage til hovedmenuen");
            WaitForEnter();
        }

        return godkendt;
    }

    // Menuen der kun kan ses når man er logget ind som admin
    static void AdminMenu()
    {
        bool iAdminMenu = true;

        while (iAdminMenu)
        {
            ShowHeader("Admin-menu");
            Console.WriteLine("1. Find bruger");
            Console.WriteLine("2. Vis alle brugere");
            Console.WriteLine("3. Vis gennemsnitsalder");
            Console.WriteLine("9. Tilbage til hovedmenu");
            Console.WriteLine(" ");
            Console.Write("Vælg 1, 2, 3 eller 9: ");

            string valg = Console.ReadLine() ?? "";

            switch (valg)
            {
                case "1":
                    FindUser();
                    break;

                case "2":
                    ShowAllUsers();
                    break;

                case "3":
                    ShowAverageAge();
                    break;

                case "9":
                    // Stopper løkken så man kommer tilbage til hovedmenuen
                    iAdminMenu = false;
                    break;

                default:
                    ShowError("Ugyldigt valg - tast 1, 2, 3 eller 9");
                    WaitForEnter();
                    break;
            }
        }
    }

    // Søger efter brugere på navn eller telefonnummer
    static void FindUser()
    {
        ShowHeader("Find bruger");

        // Søgeordet laves til små bogstaver, så store/små bogstaver er ligegyldige
        string søgeord = InputText("Indtast navn eller telefonnummer: ").ToLower();

        // Array til at gemme pladsen på de brugere der matcher søgningen
        int[] fundne = new int[maksBrugere];
        int antalFundne = 0;

        // Løber alle brugere igennem og tjekker om de matcher
        for (int i = 0; i < antalBrugere; i++)
        {
            string fuldtNavn = (fornavne[i] + " " + efternavne[i]).ToLower();

            if (fuldtNavn.Contains(søgeord) || telefoner[i].Contains(søgeord))
            {
                fundne[antalFundne] = i;
                antalFundne++;
            }
        }

        if (antalFundne == 0)
        {
            ShowError("Ingen brugere matcher: " + søgeord);
            WaitForEnter();
        }
        else
        {
            ShowUserList("Søgeresultat for: " + søgeord, fundne, antalFundne);
        }
    }

    // Viser alle oprettede brugere
    static void ShowAllUsers()
    {
        // Array med pladsen på alle brugere: 0, 1, 2, 3 ...
        int[] alle = new int[antalBrugere];

        for (int i = 0; i < antalBrugere; i++)
        {
            alle[i] = i;
        }

        ShowUserList("Alle brugere", alle, antalBrugere);
    }

    // Udskriver en liste af brugere - maks. 14 linjer ad gangen
    static void ShowUserList(string titel, int[] indekser, int antal)
    {
        // Udregner antal sider - hvis der er en rest, kommer der en side mere
        int antalSider = antal / linjerPrSide;
        if (antal % linjerPrSide != 0)
        {
            antalSider++;
        }

        int side = 1;
        ShowHeader($"{titel}   (side {side} af {antalSider})");
        ShowColumnHeader();

        for (int i = 0; i < antal; i++)
        {
            ShowUserLine(indekser[i]);

            // Når siden er fuld, og der er flere brugere, skiftes der til næste side
            if ((i + 1) % linjerPrSide == 0 && i + 1 < antal)
            {
                Console.WriteLine(" ");
                Console.Write("Tryk Enter for at se næste side...");
                Console.ReadLine();

                side++;
                ShowHeader($"{titel}   (side {side} af {antalSider})");
                ShowColumnHeader();
            }
        }

        Console.WriteLine(" ");
        Console.WriteLine($"Viser {antal} bruger(e) i alt");
        WaitForEnter();
    }

    // Udskriver overskriften over kolonnerne i brugerlisten
    static void ShowColumnHeader()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Telefon  | Navn | Alder | Adresse | E-mail | Nyhedsbrev");
        Console.ResetColor();
    }

    // Udskriver én bruger på én linje
    static void ShowUserLine(int i)
    {
        Console.WriteLine($"{telefoner[i]} | {fornavne[i]} {efternavne[i]} | {aldre[i]} år | {adresser[i]}, {postnumre[i]} {byer[i]} | {emails[i]} | {FrequencyText(frekvenser[i])}");
    }

    // ============================================================
    // MATEMATISK FUNKTION - GENNEMSNITSALDER
    // ============================================================

    // Beregner gennemsnitsalderen: summen af alle aldre divideret med antal brugere
    static double CalculateAverageAge()
    {
        int sumAfAldre = 0;

        for (int i = 0; i < antalBrugere; i++)
        {
            sumAfAldre += aldre[i];
        }

        // Undgår division med 0 hvis der ingen brugere er
        double gennemsnit = 0;
        if (antalBrugere > 0)
        {
            gennemsnit = (double)sumAfAldre / antalBrugere;
        }

        return gennemsnit;
    }

    // Viser resultatet af gennemsnitsberegningen
    static void ShowAverageAge()
    {
        ShowHeader("Gennemsnitsalder");

        double gennemsnit = CalculateAverageAge();

        Console.WriteLine($"Antal brugere: {antalBrugere}");
        Console.WriteLine($"Gennemsnitsalder: {gennemsnit:N1} år");

        WaitForEnter();
    }

    // ============================================================
    // SMÅ HJÆLPEMETODER
    // ============================================================

    // Oversætter frekvensen (12, 4, 1) til tekst
    static string FrequencyText(int frekvens)
    {
        string tekst = "";

        if (frekvens == 12)
        {
            tekst = "Hver måned";
        }
        else if (frekvens == 4)
        {
            tekst = "Hver 3. måned";
        }
        else if (frekvens == 1)
        {
            tekst = "Én gang om året";
        }

        return tekst;
    }

    // Rydder skærmen og viser en overskrift i farve
    static void ShowHeader(string titel)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine(" " + titel);
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.WriteLine(" ");
    }

    // Udskriver en fejlbesked med rød tekst
    static void ShowError(string besked)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(besked);
        Console.ResetColor();
    }

    // Venter på at brugeren trykker Enter
    static void WaitForEnter()
    {
        Console.WriteLine(" ");
        Console.Write("Tryk Enter for at fortsætte...");
        Console.ReadLine();
    }

    // ============================================================
    // STARTDATA - 20 brugere oprettet på forhånd
    // ============================================================

    static void LoadStartUsers()
    {
        AddUser("20113344", "Anne", "Jensen", 34, "Østergade 12", 4300, "Holbæk", "anne.jensen@mail.dk", 12);
        AddUser("22558899", "Peter", "Nielsen", 45, "Vestergade 5", 8000, "Aarhus C", "peter.n@mail.dk", 4);
        AddUser("30214567", "Mette", "Hansen", 28, "Nørregade 22", 1165, "København K", "mette.h@mail.dk", 1);
        AddUser("31459876", "Lars", "Pedersen", 52, "Søndergade 3", 5000, "Odense C", "lars.p@mail.dk", 12);
        AddUser("40123456", "Sofie", "Andersen", 23, "Algade 44", 4000, "Roskilde", "sofie.a@mail.dk", 4);
        AddUser("41987654", "Jonas", "Christensen", 31, "Kongensgade 8", 6700, "Esbjerg", "jonas.c@mail.dk", 1);
        AddUser("50246810", "Camilla", "Larsen", 39, "Parkvej 17", 9000, "Aalborg", "camilla.l@mail.dk", 12);
        AddUser("51357911", "Mads", "Sørensen", 60, "Skovvej 2", 7100, "Vejle", "mads.s@mail.dk", 4);
        AddUser("60112233", "Emma", "Rasmussen", 19, "Strandvejen 90", 2900, "Hellerup", "emma.r@mail.dk", 12);
        AddUser("61445566", "Frederik", "Jørgensen", 47, "Bygaden 1", 4200, "Slagelse", "frederik.j@mail.dk", 1);
        AddUser("70778899", "Ida", "Petersen", 26, "Havnegade 14", 8700, "Horsens", "ida.p@mail.dk", 4);
        AddUser("71223344", "Oliver", "Madsen", 33, "Bakkevej 9", 8600, "Silkeborg", "oliver.m@mail.dk", 12);
        AddUser("80556677", "Julie", "Kristensen", 41, "Møllevej 21", 7400, "Herning", "julie.k@mail.dk", 1);
        AddUser("81889900", "William", "Olsen", 55, "Kirkevej 6", 3000, "Helsingør", "william.o@mail.dk", 4);
        AddUser("90123789", "Laura", "Thomsen", 29, "Engvej 30", 4700, "Næstved", "laura.t@mail.dk", 12);
        AddUser("91456123", "Noah", "Poulsen", 37, "Torvet 4", 8900, "Randers", "noah.p@mail.dk", 1);
        AddUser("26789012", "Freja", "Johansen", 22, "Lindevej 11", 6000, "Kolding", "freja.j@mail.dk", 4);
        AddUser("27890123", "Emil", "Knudsen", 64, "Birkevej 13", 4100, "Ringsted", "emil.k@mail.dk", 12);
        AddUser("28901234", "Clara", "Mortensen", 44, "Rosenvej 7", 5700, "Svendborg", "clara.m@mail.dk", 1);
        AddUser("29012345", "Victor", "Møller", 50, "Egevej 19", 8800, "Viborg", "victor.m@mail.dk", 4);
    }
}
