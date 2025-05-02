# Lucrare de Laborator: HTTP Requests în C#

## Descriere

Această lucrare de laborator demonstrează cum se pot efectua cereri HTTP (`GET`, `POST`, etc.) folosind limbajul **C#** și biblioteca `HttpClient`. Aplicația permite interacțiunea cu o API externă (ex: JSONPlaceholder, OpenWeatherMap, etc.) pentru a prelua sau trimite date.

## Obiective

- Înțelegerea protocolului HTTP și a diferenței dintre metodele `GET` și `POST`.
- Utilizarea clasei `HttpClient` din .NET pentru a trimite cereri către un server web.
- Prelucrarea răspunsului HTTP și afișarea rezultatului.
- Tratarea excepțiilor legate de rețea și requesturi.

## Tehnologii utilizate

- Limbaj: C#
- Framework: .NET 6.0 / .NET Core (sau .NET Framework)
- Biblioteci: `System.Net.Http`, `Newtonsoft.Json` (opțional pentru parsare JSON)
- IDE recomandat: Visual Studio / Visual Studio Code

## Funcționalități

- Trimiterea unei cereri `GET` către o API publică
- Trimiterea unei cereri `POST` cu body JSON
- Afișarea codului de stare HTTP și a conținutului răspunsului
- Parsarea răspunsului JSON (opțional)
- Tratarea erorilor de rețea și afișarea mesajelor corespunzătoare

## Cum se rulează proiectul

1. Clonează acest repository sau descarcă fișierele sursă.
2. Deschide soluția în Visual Studio sau terminal.
3. Rulează comanda:

```bash
dotnet run
