# DNS Resolver Console App

Aceasta este o aplicație de consolă scrisă în C# care permite rezolvarea domeniilor și adreselor IP folosind DNS-ul sistemului sau un server DNS personalizat.

## 🧠 Scopul aplicației

Aplicația are ca scop:
- înțelegerea funcționării sistemului DNS (forward și reverse lookup);
- lucrul cu DNS-uri personalizate;
- exersarea programării asincrone (`async/await`);
- interacțiunea cu rețeaua folosind clasa `DnsClient`.

---

## ⚙️ Funcționalități

- 🔍 **Resolve forward** – obține adresele IP asociate unui domeniu (`resolve google.com`);
- 🔁 **Resolve reverse** – obține domeniul asociat unui IP (`resolve 8.8.8.8`);
- 🌐 **Setare DNS personalizat** – folosește un DNS de tipul `8.8.8.8`, `1.1.1.1`, etc.;
- ♻️ **Revenire la DNS-ul implicit al sistemului**;
- ⌛ Afișarea timpului de răspuns pentru fiecare interogare DNS.

---

## 🧪 Comenzi disponibile

| Comandă                  | Descriere                                         |
|--------------------------|--------------------------------------------------|
| `resolve <domeniu/IP>`   | Caută IP-urile unui domeniu sau domeniul unui IP |
| `use dns <IP>`           | Setează un DNS personalizat                      |
| `use default`            | Revine la DNS-ul implicit al sistemului          |
| `exit`                   | Închide aplicația                                |

---

## 🚀 Exemplu de utilizare

```bash
> resolve openai.com
Resolved IP addresses for openai.com (system DNS, 46 ms):
104.18.12.123
104.18.13.123

> use dns 1.1.1.1
Custom DNS set: 1.1.1.1
Custom DNS server responded successfully.

> resolve 8.8.8.8
Resolved domain(s) for 8.8.8.8 (custom DNS 1.1.1.1, 57 ms):
dns.google

> use default
Reverted to system DNS.

> exit
```

---

## 🧑‍💻 Autor

**Cotelea Marius**  
