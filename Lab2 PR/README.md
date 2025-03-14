# README - Chat UDP cu Mesaje Publice și Private

## Descriere
Această lucrare de laborator implementează un **chat UDP** care permite trimiterea de **mesaje publice prin multicast** și **mesaje private prin unicast**. Utilizatorii care se conectează la chat pot comunica între ei, iar IP-urile acestora sunt afișate pentru identificare.

## Caracteristici
- **Multicast UDP** pentru mesaje publice.
- **Unicast UDP** pentru mesaje private.
- **Afișarea IP-urilor utilizatorilor conectați**.
- **Utilizarea a două socket-uri UDP**: unul pentru mesaje de grup și altul pentru mesaje directe.
- **Gestionarea utilizatorilor conectați** printr-un `HashSet<IPAddress>`.

## Cerințe
- **.NET 6.0+**
- **Rețea locală (LAN) pentru testare**

## Instalare și utilizare
1. **Compilarea codului:**
   ```sh
   dotnet build
   ```
2. **Executarea aplicației:**
   ```sh
   dotnet run
   ```
3. **Introducerea IP-ului local** la start.
4. **Utilizarea comenzilor disponibile:**
   - `/privat [IP]` – Trimite mesaje private către un anumit utilizator.
   - `/public` – Revine la chatul public (multicast).

## Funcționalitate
- La pornire, aplicația solicită utilizatorului să introducă **adresa IP locală**.
- Fiecare utilizator poate alege să trimită **mesaje publice** (vizibile tuturor) sau **mesaje private** (către un IP specific).
- Când un utilizator trimite un mesaj pentru prima dată, IP-ul său este **adăugat și afișat** în listă.
- **Mesajele sunt recepționate și afișate automat** în consolă.

## Structura codului
- **`Main()`** – Inițializează socket-urile și începe ascultarea mesajelor.
- **`TransmitMessage(string input)`** – Procesează mesajele și le trimite fie în grup, fie individual.
- **`ReceiveGroupMessages()`** – Ascultă și afișează mesajele publice.
- **`ReceiveDirectMessages()`** – Ascultă și afișează mesajele private.

## Exemple de utilizare
### Trimiterea unui mesaj public:
```sh
Salut, tuturor!
```
_(Acest mesaj va fi trimis tuturor utilizatorilor din grupul multicast)_

### Trimiterea unui mesaj privat:
```sh
/privat 192.168.1.50
Salut, doar pentru tine!
```
_(Acest mesaj va fi trimis doar la utilizatorul cu IP-ul `192.168.1.50`)_

### Revenirea la chatul public:
```sh
/public
```

## Observații
- **UDP nu garantează livrarea pachetelor**, deci unele mesaje pot fi pierdute în cazul unor probleme de rețea.
- **Checksum-ul UDP** este utilizat pentru verificarea integrității pachetelor.
- **Nu există retransmisie automată** a mesajelor (spre deosebire de TCP).

## Autori
- *Nume:* [Cotelea Marius]
- *Universitate:* [Universitatea Tehnică a Moldovei]
- *Data:* [14 martie 2025]
