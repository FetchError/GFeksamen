//Eksamensprojekt GF2 - CMIS informationsstander til nyhedsbreve

namespace IntroProgram; // Navnerummet som programmets klasser hører til

class Program // Klassen der indeholder hele programmet
{
    

    // Data som gemmes i arrays som alle metoder kan bruge

    // Største antal brugere der kan gemmes (single source of truth)
    static int maksBrugere = 100; // Arrays får plads til 100 brugere

    // Antal brugere der er gemt lige nu
    static int antalBrugere = 0; // Starter på 0 - tælles op i AddUser

    // Password til admin-menuen
    static string adminPassword = "admin123"; // Det password admin skal skrive

    // Antal brugere der vises på én side
    static int linjerPrSide = 14; // Maks. 14 brugere pr. side i listen

    // Parallelle arrays - bruger nummer i ligger på plads i i alle arrays

    // Array med plads til 100 telefonnumre (maksBrugere) - én plads pr. bruger.
    // static betyder, at alle metoder i programmet kan bruge det samme array.
    static string[] telefoner = new string[maksBrugere]; // Telefonnumre (8 cifre som tekst)
    static string[] fornavne = new string[maksBrugere]; // Fornavne
    static string[] efternavne = new string[maksBrugere]; // Efternavne
    static int[] aldre = new int[maksBrugere]; // Alder i hele år
    static string[] adresser = new string[maksBrugere]; // Vejnavn og husnummer
    static int[] postnumre = new int[maksBrugere]; // Postnumre (4 cifre)
    static string[] byer = new string[maksBrugere]; // Bynavne
    static string[] emails = new string[maksBrugere]; // E-mailadresser
    static int[] frekvenser = new int[maksBrugere]; // Nyhedsbrev pr. år (12, 4 eller 1)


    // Hoved program

    static void Main(string[] args) // Programmet starter her
    {
        // Opretter de 20 brugere der skal findes på forhånd
        LoadStartUsers(); // Fylder arrays med startdata

        // Variabel der styrer om programmet skal køre videre
        bool kørProgram = true; // Sættes til false når brugeren vælger 9

        // Hovedmenuen vises igen og igen indtil brugeren vælger 9
        while (kørProgram) // Kører så længe kørProgram er true
        {
            ShowHeader("CMIS - Tilmelding til nyhedsbrev"); // Rydder skærmen og viser titel
            Console.WriteLine("1. Opret bruger"); // Menupunkt 1
            // Admin logger ind ved at skrive 2 (Ikke synlig i hovedemenuen)
            Console.WriteLine("9. Afslut"); // Menupunkt 9
            Console.WriteLine(" "); // Tom linje for luft
            Console.Write("Vælg 1 eller 9: "); // Beder om et valg (uden linjeskift)

            // Læser brugerens valg og sikrer mod null
            string valg = Console.ReadLine() ?? ""; // ?? "" giver tom tekst hvis ReadLine returnerer null

            // Tjekker hvilket menupunkt brugeren har valgt
            switch (valg) // Vælger case ud fra teksten i valg
            {
                case "1": // Brugeren vil oprette sig
                    CreateUser(); // Starter tilmeldingen
                    break; // Hopper ud af switch

                case "2": // Skjult valg til admin
                    // Admin-menuen vises kun hvis password er rigtigt
                    if (AdminLogin()) // AdminLogin returnerer true ved rigtigt password
                    {
                        AdminMenu(); // Viser admin-menuen
                    }
                    break; // Hopper ud af switch

                case "9": // Brugeren vil afslutte
                    // Stopper løkken så programmet afsluttes pænt
                    kørProgram = false; // while-løkken stopper ved næste tjek
                    break; // Hopper ud af switch

                default: // Alt andet end 1, 2 og 9
                    ShowError("Ugyldigt valg - tast 1, 2 eller 9"); // Rød fejlbesked
                    WaitForEnter(); // Venter så brugeren kan nå at læse fejlen
                    break; // Hopper ud af switch
            }
        }

        Console.WriteLine(" "); // Tom linje
        Console.WriteLine("Tak for besøget - programmet afsluttes."); // Afskedsbesked
        Console.Write("Tryk på en tast for at lukke..."); // Instruktion til brugeren
        Console.ReadKey(); // Venter på et tastetryk før vinduet lukker
    }


    // Opret bruger

    // Guider brugeren igennem tilmeldingen til nyhedsbrevet
    static void CreateUser() // Metode uden returværdi (void)
    {
        ShowHeader("Opret bruger"); // Rydder skærmen og viser titel

        // Tjekker om der er plads til flere brugere i arrays
        if (antalBrugere >= maksBrugere) // Arrays er fyldt op
        {
            ShowError("Der er desværre ikke plads til flere brugere"); // Fejlbesked
        }
        else // Der er plads
        {
            string telefon = InputPhone(); // Beder om et gyldigt telefonnummer

            // Tjekker om telefonnummeret findes i forvejen
            if (FindPhoneIndex(telefon) != -1) // -1 betyder "ikke fundet"
            {
                ShowError("Telefonnummeret " + telefon + " er allerede oprettet"); // Nummeret er optaget
            }
            else // Nummeret er ledigt
            {
                Console.WriteLine("Nummeret er ledigt - udfyld resten af oplysningerne:"); // Besked til brugeren
                Console.WriteLine(" "); // Tom linje

                // Spørger om alle oplysninger - hver metode tjekker selv inputtet
                string fornavn = InputText("Fornavn: "); // Må ikke være tomt
                string efternavn = InputText("Efternavn: "); // Må ikke være tomt
                int alder = InputNumber("Alder: ", 1, 120); // Tal mellem 1 og 120
                string adresse = InputText("Adresse: "); // Må ikke være tomt
                int postnummer = InputNumber("Postnummer (4 cifre): ", 1000, 9999); // Tal mellem 1000 og 9999
                string by = InputText("By: "); // Må ikke være tomt
                string email = InputEmail(); // Skal indeholde @ og punktum
                int frekvens = InputFrequency(); // 12, 4 eller 1

                // Gemmer brugeren i arrays
                AddUser(telefon, fornavn, efternavn, alder, adresse, postnummer, by, email, frekvens); // Sender alle oplysninger videre

                // Kvittering til brugeren i grøn
                Console.WriteLine(" "); // Tom linje
                Console.ForegroundColor = ConsoleColor.Green; // Skifter tekstfarve til grøn
                Console.WriteLine($"Tak {fornavn}! Du er nu tilmeldt nyhedsbrevet: {FrequencyText(frekvens)}"); // Kvittering med navn og frekvens
                Console.ResetColor(); // Sætter farven tilbage til normal
            }
        }

        WaitForEnter(); // Venter før der vendes tilbage til menuen
    }

    // Gemmer én bruger på den første ledige plads i alle arrays
    static void AddUser(string telefon, string fornavn, string efternavn, int alder, string adresse, int postnummer, string by, string email, int frekvens) // Modtager alle oplysninger som parametre
    {
        telefoner[antalBrugere] = telefon; // Gemmer telefonnummer på første ledige plads
        fornavne[antalBrugere] = fornavn; // Gemmer fornavn på samme plads
        efternavne[antalBrugere] = efternavn; // Gemmer efternavn
        aldre[antalBrugere] = alder; // Gemmer alder
        adresser[antalBrugere] = adresse; // Gemmer adresse
        postnumre[antalBrugere] = postnummer; // Gemmer postnummer
        byer[antalBrugere] = by; // Gemmer by
        emails[antalBrugere] = email; // Gemmer e-mail
        frekvenser[antalBrugere] = frekvens; // Gemmer frekvens

        // Tæller antal brugere 1 op, så næste bruger kommer på næste plads
        antalBrugere++; // Samme som antalBrugere = antalBrugere + 1
    }

    // Finder pladsen i arrayet for et telefonnummer - giver -1 hvis det ikke findes
    static int FindPhoneIndex(string telefon) // Returnerer et heltal (pladsen)
    {
        int indeks = -1; // Starter med "ikke fundet"

        // Løber alle gemte brugere igennem
        for (int i = 0; i < antalBrugere; i++) // i går fra 0 til sidste bruger
        {
            if (telefoner[i] == telefon) // Er det det samme nummer?
            {
                indeks = i; // Husker pladsen
            }
        }

        return indeks; // Sender pladsen (eller -1) tilbage
    }


    // Bruger input

    // Tjekker om en tekst kun består af cifre (0-9)
    static bool IsOnlyDigits(string tekst) // Returnerer true eller false
    {
        string cifre = "0123456789"; // De tegn der er tilladt

        // En tom tekst er ikke et tal
        bool kunTal = tekst.Length > 0; // false hvis teksten er tom

        // Tjekker tegn for tegn om det er et ciffer
        for (int i = 0; i < tekst.Length; i++) // Løber hvert tegn igennem
        {
            if (!cifre.Contains(tekst.Substring(i, 1))) // Tegnet findes ikke blandt cifrene
            {
                kunTal = false; // Så er teksten ikke kun tal
            }
        }

        return kunTal; // Sender resultatet tilbage
    }

    // Spørger om en tekst indtil brugeren har skrevet noget
    static string InputText(string spørgsmål) // spørgsmål er teksten der vises
    {
        string svar = ""; // Her gemmes brugerens svar
        bool gyldig = false; // Bliver true når svaret er godkendt

        while (!gyldig) // Spørger igen indtil svaret er gyldigt
        {
            Console.Write(spørgsmål); // Viser spørgsmålet
            svar = Console.ReadLine() ?? ""; // Læser svaret (tom tekst hvis null)

            if (svar == "") // Brugeren skrev ingenting
            {
                ShowError("Feltet må ikke være tomt - prøv igen"); // Fejlbesked
            }
            else // Der er skrevet noget
            {
                gyldig = true; // Stopper løkken
            }
        }

        return svar; // Sender svaret tilbage
    }

    // Spørger om et heltal indtil det er et tal mellem min og max
    static int InputNumber(string spørgsmål, int min, int max) // min og max er grænserne
    {
        int tal = 0; // Her gemmes tallet
        bool gyldig = false; // Bliver true når tallet er godkendt

        while (!gyldig) // Spørger igen indtil tallet er gyldigt
        {
            Console.Write(spørgsmål); // Viser spørgsmålet
            string svar = Console.ReadLine() ?? ""; // Læser svaret som tekst

            // Tjekker først at der kun er cifre, så int.Parse ikke crasher
            if (!IsOnlyDigits(svar)) // Indeholder andet end cifre
            {
                ShowError("Du skal skrive et tal (kun cifre) - prøv igen"); // Fejlbesked
            }
            // Et tal med mere end 9 cifre er for stort til en int
            else if (svar.Length > 9) // For mange cifre
            {
                ShowError("Tallet er for stort - prøv igen"); // Fejlbesked
            }
            else // Teksten er et gyldigt tal
            {
                tal = int.Parse(svar); // Laver teksten om til et heltal

                // Tjekker at tallet er inden for grænserne
                if (tal < min || tal > max) // For lille eller for stort
                {
                    ShowError($"Tallet skal være mellem {min} og {max} - prøv igen"); // Fejlbesked med grænserne
                }
                else // Tallet er inden for grænserne
                {
                    gyldig = true; // Stopper løkken
                }
            }
        }

        return tal; // Sender tallet tilbage
    }

    // Spørger om et telefonnummer indtil det er præcis 8 cifre
    static string InputPhone() // Returnerer nummeret som tekst
    {
        string telefon = ""; // Her gemmes nummeret
        bool gyldig = false; // Bliver true når nummeret er godkendt

        while (!gyldig) // Spørger igen indtil nummeret er gyldigt
        {
            Console.Write("Indtast telefonnummer (8 cifre): "); // Viser spørgsmålet
            telefon = Console.ReadLine() ?? ""; // Læser nummeret

            if (telefon.Length != 8 || !IsOnlyDigits(telefon)) // Ikke 8 tegn, eller ikke kun cifre
            {
                ShowError("Telefonnummeret skal være præcis 8 cifre, f.eks. 12345678"); // Fejlbesked
            }
            else // Nummeret er gyldigt
            {
                gyldig = true; // Stopper løkken
            }
        }

        return telefon; // Sender nummeret tilbage
    }

    // Spørger om en e-mail indtil den indeholder @ og punktum og ingen mellemrum
    static string InputEmail() // Returnerer e-mailen som tekst
    {
        string email = ""; // Her gemmes e-mailen
        bool gyldig = false; // Bliver true når e-mailen er godkendt

        while (!gyldig) // Spørger igen indtil e-mailen er gyldig
        {
            Console.Write("E-mail: "); // Viser spørgsmålet
            email = Console.ReadLine() ?? ""; // Læser e-mailen

            if (!email.Contains("@") || !email.Contains(".") || email.Contains(" ")) // Mangler @ eller punktum, eller har mellemrum
            {
                ShowError("Ugyldig e-mail - den skal f.eks. se sådan ud: navn@mail.dk"); // Fejlbesked med eksempel
            }
            else // E-mailen er gyldig
            {
                gyldig = true; // Stopper løkken
            }
        }

        return email; // Sender e-mailen tilbage
    }

    // Spørger hvor ofte brugeren vil have nyhedsbrevet (12, 4 eller 1 gang om året)
    static int InputFrequency() // Returnerer 12, 4 eller 1
    {
        int frekvens = 0; // Her gemmes valget
        bool gyldig = false; // Bliver true når valget er godkendt

        Console.WriteLine("Hvor ofte vil du have nyhedsbrevet?"); // Spørgsmål
        Console.WriteLine("  12 = Hver måned"); // Mulighed 1
        Console.WriteLine("   4 = Hver 3. måned"); // Mulighed 2
        Console.WriteLine("   1 = Én gang om året"); // Mulighed 3

        while (!gyldig) // Spørger igen indtil valget er gyldigt
        {
            Console.Write("Vælg 12, 4 eller 1: "); // Beder om et valg
            string svar = Console.ReadLine() ?? ""; // Læser valget

            if (svar == "12" || svar == "4" || svar == "1") // Et af de tilladte valg
            {
                frekvens = int.Parse(svar); // Laver teksten om til et tal
                gyldig = true; // Stopper løkken
            }
            else // Ugyldigt valg
            {
                ShowError("Du skal vælge 12, 4 eller 1"); // Fejlbesked
            }
        }

        return frekvens; // Sender valget tilbage
    }


    // admin

    // Beder om password - brugeren har 3 forsøg
    static bool AdminLogin() // Returnerer true hvis login lykkes
    {
        ShowHeader("Admin login"); // Rydder skærmen og viser titel

        int forsøg = 0; // Tæller antal brugte forsøg
        bool godkendt = false; // Bliver true ved rigtigt password

        // Fortsætter indtil password er rigtigt eller der er brugt 3 forsøg
        while (!godkendt && forsøg < 3) // Begge betingelser skal være opfyldt
        {
            Console.Write("Indtast password: "); // Beder om password
            string password = Console.ReadLine() ?? ""; // Læser password
            forsøg++; // Tæller et forsøg op

            if (password == adminPassword) // Sammenligner med det rigtige password
            {
                godkendt = true; // Login lykkedes - stopper løkken
            }
            else // Forkert password
            {
                ShowError("Forkert password - du har " + (3 - forsøg) + " forsøg tilbage"); // Viser antal forsøg tilbage
            }
        }

        // Hvis alle forsøg er brugt, sendes brugeren tilbage til hovedmenuen
        if (!godkendt) // Login mislykkedes
        {
            ShowError("For mange forkerte forsøg - du sendes tilbage til hovedmenuen"); // Fejlbesked
            WaitForEnter(); // Venter så beskeden kan læses
        }

        return godkendt; // Sender resultatet tilbage til Main
    }

    // Menuen der kun kan ses når man er logget ind som admin
    static void AdminMenu() // Metode uden returværdi
    {
        bool iAdminMenu = true; // Styrer om admin-menuen skal vises igen

        while (iAdminMenu) // Kører indtil admin vælger 9
        {
            ShowHeader("Admin menu"); // Rydder skærmen og viser titel
            Console.WriteLine("1. Find bruger"); // Menupunkt 1
            Console.WriteLine("2. Vis alle brugere"); // Menupunkt 2
            Console.WriteLine("3. Vis gennemsnitsalder"); // Menupunkt 3
            Console.WriteLine("9. Tilbage til hovedmenu"); // Menupunkt 9
            Console.WriteLine(" "); // Tom linje
            Console.Write("Vælg 1, 2, 3 eller 9: "); // Beder om et valg

            string valg = Console.ReadLine() ?? ""; // Læser valget

            switch (valg) // Vælger case ud fra valget
            {
                case "1": // Søg efter bruger
                    FindUser(); // Starter søgningen
                    break; // Hopper ud af switch

                case "2": // Vis alle brugere
                    ShowAllUsers(); // Viser listen
                    break; // Hopper ud af switch

                case "3": // Vis gennemsnitsalder
                    ShowAverageAge(); // Beregner og viser gennemsnittet
                    break; // Hopper ud af switch

                case "9": // Tilbage til hovedmenu
                    // Stopper løkken så man kommer tilbage til hovedmenuen
                    iAdminMenu = false; // while-løkken stopper
                    break; // Hopper ud af switch

                default: // Alt andet end 1, 2, 3 og 9
                    ShowError("Ugyldigt valg - tast 1, 2, 3 eller 9"); // Fejlbesked
                    WaitForEnter(); // Venter så fejlen kan læses
                    break; // Hopper ud af switch
            }
        }
    }

    // Søger efter brugere på navn eller telefonnummer
    static void FindUser() // Metode uden returværdi
    {
        ShowHeader("Find bruger"); // Rydder skærmen og viser titel

        // Søgeordet laves til små bogstaver, så store/små bogstaver er ligegyldige
        string søgeord = InputText("Indtast navn eller telefonnummer: ").ToLower(); // Læser og laver til små bogstaver

        // Array til at gemme pladsen på de brugere der matcher søgningen
        int[] fundne = new int[maksBrugere]; // Plads til alle brugere, hvis alle matcher
        int antalFundne = 0; // Tæller hvor mange der matcher

        // Løber alle brugere igennem og tjekker om de matcher
        for (int i = 0; i < antalBrugere; i++) // i går fra 0 til sidste bruger
        {
            string fuldtNavn = (fornavne[i] + " " + efternavne[i]).ToLower(); // Samler for- og efternavn med små bogstaver

            if (fuldtNavn.Contains(søgeord) || telefoner[i].Contains(søgeord)) // Matcher navn eller telefon
            {
                fundne[antalFundne] = i; // Gemmer brugerens plads
                antalFundne++; // Tæller et fund op
            }
        }

        if (antalFundne == 0) // Ingen brugere matchede
        {
            ShowError("Ingen brugere matcher: " + søgeord); // Fejlbesked
            WaitForEnter(); // Venter så beskeden kan læses
        }
        else // Der er mindst ét match
        {
            ShowUserList("Søgeresultat for: " + søgeord, fundne, antalFundne); // Viser de fundne brugere
        }
    }

    // Viser alle oprettede brugere
    static void ShowAllUsers() // Metode uden returværdi
    {
        // Array med pladsen på alle brugere: 0, 1, 2, 3 (Starter på 0)
        int[] alle = new int[antalBrugere]; // Én plads pr. oprettet bruger

        for (int i = 0; i < antalBrugere; i++) // Løber alle brugere igennem
        {
            alle[i] = i; // Plads i indeholder tallet i
        }

        ShowUserList("Alle brugere", alle, antalBrugere); // Viser hele listen
    }

    // Udskriver en liste af brugere - maks. 14 linjer ad gangen
    static void ShowUserList(string titel, int[] indekser, int antal) // indekser = hvilke brugere der skal vises
    {
        // Udregner antal sider - hvis der er en rest, kommer der en side mere
        int antalSider = antal / linjerPrSide; // Heltalsdivision - antal hele sider
        if (antal % linjerPrSide != 0) // % giver resten - er der brugere til overs?
        {
            antalSider++; // Så skal der bruges en side mere
        }

        int side = 1; // Starter på side 1
        ShowHeader($"{titel}   (side {side} af {antalSider})"); // Viser titel med sidetal
        ShowColumnHeader(); // Viser kolonneoverskrifterne

        for (int i = 0; i < antal; i++) // Løber alle brugere i listen igennem
        {
            ShowUserLine(indekser[i]); // Udskriver én bruger

            // Når siden er fuld, og der er flere brugere, skiftes der til næste side
            if ((i + 1) % linjerPrSide == 0 && i + 1 < antal) // Siden er fuld og der er flere tilbage
            {
                Console.WriteLine(" "); // Tom linje
                Console.Write("Tryk enter for at se næste side..."); // Instruktion
                Console.ReadLine(); // Venter på enter

                side++; // Går til næste side
                ShowHeader($"{titel}   (side {side} af {antalSider})"); // Ny titel med sidetal
                ShowColumnHeader(); // Kolonneoverskrifter igen
            }
        }

        Console.WriteLine(" "); // Tom linje
        Console.WriteLine($"Viser {antal} bruger(e) i alt"); // Viser antal brugere i listen
        WaitForEnter(); // Venter før der vendes tilbage til menuen
    }

    // Udskriver overskriften over kolonnerne i brugerlisten
    static void ShowColumnHeader() // Metode uden returværdi
    {
        Console.ForegroundColor = ConsoleColor.Yellow; // Skifter tekstfarve til gul
        Console.WriteLine("Telefon  | Navn | Alder | Adresse | E-mail | Nyhedsbrev"); // Kolonnenavne
        Console.ResetColor(); // Sætter farven tilbage til normal
    }

    // Udskriver én bruger på én linje
    static void ShowUserLine(int i) // i = brugerens plads i arrays
    {
        Console.WriteLine($"{telefoner[i]} | {fornavne[i]} {efternavne[i]} | {aldre[i]} år | {adresser[i]}, {postnumre[i]} {byer[i]} | {emails[i]} | {FrequencyText(frekvenser[i])}"); // Henter data fra alle arrays på plads i
    }


    // Gennemsnitsalder (Matematik funktion)

    // Beregner gennemsnitsalderen: summen af alle aldre divideret med antal brugere
    static double CalculateAverageAge() // Returnerer et decimaltal
    {
        int sumAfAldre = 0; // Starter summen på 0

        for (int i = 0; i < antalBrugere; i++) // Løber alle brugere igennem
        {
            sumAfAldre += aldre[i]; // Lægger brugerens alder til summen
        }

        // Undgår division med 0 hvis der ingen brugere er
        double gennemsnit = 0; // Standardværdi hvis der ingen brugere er
        if (antalBrugere > 0) // Kun hvis der er brugere
        {
            gennemsnit = (double)sumAfAldre / antalBrugere; // (double) sikrer decimaler i resultatet
        }

        return gennemsnit; // Sender gennemsnittet tilbage
    }

    // Viser resultatet af gennemsnitsberegningen
    static void ShowAverageAge() // Metode uden returværdi
    {
        ShowHeader("Gennemsnitsalder"); // Rydder skærmen og viser titel

        double gennemsnit = CalculateAverageAge(); // Henter det beregnede gennemsnit

        Console.WriteLine($"Antal brugere: {antalBrugere}"); // Viser antal brugere
        Console.WriteLine($"Gennemsnitsalder: {gennemsnit:N1} år"); // :N1 viser én decimal

        WaitForEnter(); // Venter før der vendes tilbage til menuen
    }


    // Oversætter frekvensen (12, 4, 1) til tekst
    static string FrequencyText(int frekvens) // Returnerer en tekst
    {
        string tekst = ""; // Tom tekst hvis frekvensen er ukendt

        if (frekvens == 12) // 12 gange om året
        {
            tekst = "Hver måned"; // Tekst for 12
        }
        else if (frekvens == 4) // 4 gange om året
        {
            tekst = "Hver 3. måned"; // Tekst for 4
        }
        else if (frekvens == 1) // 1 gang om året
        {
            tekst = "Én gang om året"; // Tekst for 1
        }

        return tekst; // Sender teksten tilbage
    }

    // Rydder skærmen og viser en overskrift i farve
    // Genadvenlig til alle titler ved at bruge ShowHeader metoden
    static void ShowHeader(string titel) // titel = teksten i overskriften
    {
        Console.Clear(); // Rydder konsollen
        Console.ForegroundColor = ConsoleColor.Blue; // Skifter tekstfarve til blå
        Console.WriteLine("================================="); // Streg over titlen
        Console.WriteLine(" " + titel); // Selve titlen
        Console.WriteLine("================================="); // Streg under titlen
        Console.ResetColor(); // Sætter farven tilbage til normal
        Console.WriteLine(" "); // Tom linje
    }

    // Udskriver en fejlbesked med rød tekst
    static void ShowError(string besked) // besked = fejlteksten
    {
        Console.ForegroundColor = ConsoleColor.Red; // Skifter tekstfarve til rød
        Console.WriteLine(besked); // Udskriver fejlbeskeden
        Console.ResetColor(); // Sætter farven tilbage til normal
    }

    // Venter på at brugeren trykker enter
    static void WaitForEnter() // Metode uden returværdi
    {
        Console.WriteLine(" "); // Tom linje
        Console.Write("Tryk enter for at fortsætte..."); // Instruktion
        Console.ReadLine(); // Venter på enter
    }


    // 20 brugere oprettet på forhånd

    static void LoadStartUsers() // Hardkodet 20 testbrugere ved programstart
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
