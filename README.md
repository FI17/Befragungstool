# Befragungstool - WIP
Projekt der FI17 zur Erstellung eines Befragungstool mit ASP.NET

## Installationsanweisungen
### Verwendung auf dem eigenen PC (localhost)
1. Projekt von [Github](https://github.com/FI17/Befragungstool/archive/master.zip) herunterladen
2. ZIP-Archiv mit Entpackungsprogramm entpacken
3. Im Projekt die [web.config Datei](Umfrage-Tool/Umfrage-Tool/Web.config) öffnen und in Zeile 15 Änderungen am Dateipfad vornehmen
  - Den Dateipfad auf einen Ordner mit Vollzugriff verweisen lassen
  - [Datenbankdateien](https://github.com/FI17/Befragungstool-Dokumente/tree/master/Datenbank/Datenbankdateien) in diesen Ordner einfügen
4. Dann kann das Projekt über Visual Studio ausgeführt werden

### Installation auf einem Windows Server (Webserver)
0. Windows Server installieren und Windows-Feature aktivieren (``Install-WindowsFeature -Name Web-Server -IncludeManagementTools``)
1. Projekt von [Github](https://github.com/FI17/Befragungstool/archive/master.zip) herunterladen
2. ZIP-Archiv mit Entpackungsprogramm entpacken
  - Dateien auf den Server kopieren
3. Im Projekt die [web.config Datei](Umfrage-Tool/Umfrage-Tool/Web.config) öffnen und in Zeile 15 Änderungen am Dateipfad vornehmen
  - Den Dateipfad auf einen Ordner mit Vollzugriff verweisen lassen
  - [Datenbankdateien](https://github.com/FI17/Befragungstool-Dokumente/tree/master/Datenbank/Datenbankdateien) in diesen Ordner einfügen
4. Dateipfad von "localhost:60480" in die IP des Servers ändern (Beispiel: ``10.4.55.10``) in den folgenden Dateien:
  - [Home/Index.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Home/Index.cshtml)
  - [Umfrage_Erstellung/FrageErstellung.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Umfrage_Erstellung/FrageErstellung.cshtml)

5. Bei URL-bezogenen Links, die auf eine andere Seite verweisen, den Dateipfad in den folgenden Dateien am Anfang um ``/UT/`` ergänzen:
- [Umfrage_Ergebnisse/Antworten.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Umfrage_Ergebnisse/Antworten.cshtml)
- [Umfrage_Ergebnisse/Ergebnisse.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Umfrage_Ergebnisse/Ergebnisse.cshtml)
- [Umfrage_Ergebnisse/Index.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Umfrage_Ergebnisse/Index.cshtml)
- [Umfrage_Erstellung/FrageErstellung.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Umfrage_Erstellung/FrageErstellung.cshtml)
- [Home/Index.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Home/Index.cshtml)

6. Im IIS-Manager eine Anwendung hinzufügen
  - den Alias (Unterseite) ``UT`` festlegen
  - den physischen Pfad zu zum ersten 2. Ordner ``Umfrage-tool`` des Projektes legen. Beispiel ``D:\Umfrage-Tool\Umfrage-Tool``, wenn das Projekt einfach auf den Datenträger ``D`` entpackt wurde.
  - mit ``Ok`` bestätigen.
  
  Nun kann das Projekt mit der IP des Servers und `/UT` aufgerufen werden. Beispiel: ``10.4.55.10/UT``
  
  Bei Fragen oder Problemen: ein [Issue](https://github.com/FI17/Befragungstool/issues) erstellen
