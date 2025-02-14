# Aplicație de Mesagerie Client-Server în C#

## Prezentare Generală
Acest proiect este o aplicație simplă de mesagerie client-server implementată în C#. Permite conectarea mai multor clienți la un server și transmiterea mesajelor între aceștia. Serverul acționează ca un releu, retransmițând mesajele de la un client către toți ceilalți clienți conectați.

## Funcționalități
- Suportă mai mulți clienți simultan.
- Clienții pot trimite și primi mesaje.
- Serverul retransmite mesajele către toți clienții conectați (excluzând expeditorul).
- Implementat folosind socket-uri TCP.

## Cerințe
- .NET 6.0 sau o versiune mai recentă.
- Visual Studio, RIDER (sau un alt IDE compatibil cu C#).

## Început Rapid
### 1. Clonează Repozitoriul
```sh
 git clone <repository-url>
```

### 2. Deschide Proiectul
- Deschide proiectul în Visual Studio sau în IDE-ul preferat.

### 3. Rulează Serverul
- Deschide proiectul `Server`.
- Rulează aplicația `Server` din IDE.
- Serverul va începe să asculte pe `127.0.0.1:9000`.

### 4. Rulează Clienții
- Deschide mai multe instanțe ale proiectului `Client`.
- Rulează fiecare instanță a clientului separat.
- Introdu un nume de utilizator când ți se solicită.
- Începe să trimiți mesaje, iar toți clienții conectați le vor primi.

## Cum Funcționează
### Server
- Ascultă conexiunile de la clienți pe `127.0.0.1:9000`.
- Acceptă clienți noi și îi adaugă într-o listă.
- Ascultă mesajele primite și le retransmite către toți ceilalți clienți conectați.
- Elimină clienții deconectați din listă.

### Client
- Se conectează la server la `127.0.0.1:9000`.
- Trimite mesaje către server, care sunt apoi retransmise către toți ceilalți clienți.
- Ascultă continuu mesajele primite de la server.

## Exemplu de Utilizare
1. Pornește serverul.
2. Rulează două sau mai multe instanțe de client.
3. Fiecare client introduce un nume de utilizator și începe să discute.
4. Mesajele trimise de un client apar în consola tuturor celorlalți clienți.

## Autor
- **Cotelea Marius**

## Link către repozitor
https://github.com/CoteleaMarius/PR-Practice.git

