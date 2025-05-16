🌐 NTP Time Zone Converter Console App
Acest program C# obține ora UTC de la un server NTP (pool.ntp.org) și permite utilizatorului să o convertească într-un fus orar specificat de tipul GMT+X sau GMT-X. Aplicația rulează în buclă până când utilizatorul introduce comanda exit.

🛠️ Funcționalități
Conectare la un server NTP folosind UDP.

Afișarea orei UTC actuale.

Conversia în fusuri orare pe baza unui offset introdus de utilizator (GMT+X / GMT-X).

Manipulare robustă a erorilor și validare a inputului.

Interfață de consolă interactivă.

🧾 Exemplu de rulare
bash
Copiază
Editează
UTC Time: 2025-05-16 11:00:00
Enter time zone (e.g., GMT+2 or GMT-5), or 'exit' to quit: GMT+3
Time in GMT+3: 2025-05-16 14:00:00

Enter time zone (e.g., GMT+2 or GMT-5), or 'exit' to quit: exit
Exiting application...
📄 Structura codului
Main()
Inițializează ora UTC folosind metoda GetNtpTime.

Calculează ora curentă pe baza unei valori inițiale fixe și a timpului scurs (elapsed).

Solicită de la utilizator un fus orar și afișează ora corespunzătoare.

GetNtpTime(string ntpServer)
Creează un pachet UDP pentru a solicita timpul de la serverul NTP specificat.

Parcurge răspunsul NTP și extrage timestamp-ul de transmisie.

Îl convertește într-un obiect DateTime în format UTC.

TryParseTimeZone(string input, out int offsetHours)
Verifică validitatea stringului introdus (GMT+X, GMT-X).

Extrage semnul și numărul și întoarce offsetul corespunzător în ore.

⚠️ Limitări
Acceptă doar offseturi întregi între -11 și +11.

Nu suportă minute (ex: GMT+5:30) sau timezone-uri cu denumiri (ex: CET, EST).

Nu are fallback în cazul în care serverul NTP nu răspunde.

💡 Îmbunătățiri posibile
Adăugarea unui mecanism de fallback pentru NTP (ex: listă cu mai multe servere).

Suport pentru fusuri orare cu minute sau cu nume.

Interfață grafică (GUI).

Suport pentru async/await pentru un cod mai modern și eficient.

▶️ Cerințe
.NET SDK 6.0+ (sau .NET Core 3.1+)

Conexiune la internet pentru accesarea serverului NTP
