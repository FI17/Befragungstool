# Befragungstool

![Version](https://img.shields.io/badge/Version-0.7-yellowgreen.svg)
![Status](https://img.shields.io/badge/Status-WIP-important.svg)
![Framework](https://img.shields.io/badge/Framework-ASP.NET%20MVC-informational.svg)
![License](https://img.shields.io/github/license/FI17/Befragungstool.svg)

Befragungstool in ASP.NET
## Das Befragungstool
Das Befragungstool ist eine vollwertige ASP.NET MVC Website, welche die Erstellung, Beantwortung und Auswertung von Fragebögen ermöglicht.</br>
Das Tool ist <b>kein</b> ASP.NET Core-Projekt
## Features
- Admin-Oberfläche für die Administration des Tools
- verschiedene Fragetypen (zum Beispiel Freitextfragen, Multiple-Choice-Fragen, Skalenfragen)
- anonyme Beantwortung der Fragebögen
- kumulierte Auswertung erstellter Fragebögen
- Einzelauswertung erstellter Fragebögen
- verschiedene Status der Fragebögen möglich ("noch nicht veröffentlicht", "veröffentlicht", beendet"), die verschiedene Optionen für Fragebögen sperren oder freigeben (Bearbeitung, Auswertung, ...)
## Installationsanweisungen
### Verwendung auf dem eigenen PC (localhost)
1. Projekt von [Github](https://github.com/FI17/Befragungstool/archive/master.zip) (direkter Download) herunterladen
2. ZIP-Archiv mit Entpackungsprogramm entpacken (Zum Beispiel [7zip](http://www.7-zip.de/download.html))
3. Das Projekt in Visual Studio (Visual Studio 2015 oder neuer) öffnen
4. Im Projekt die [web.config](Umfrage-Tool/Umfrage-Tool/Web.config)-Datei öffnen und in Zeile **15** Änderungen am Dateipfad vornehmen
  - Den Dateipfad auf einen Ordner mit Vollzugriff verweisen lassen (dort wird die Datenbank abgespeichert)
5. database.cs und Startup.cs anpassen
  - in der Datei [database.cs](Umfrage-Tool/Umfrage-Tool/database.cs) die Zeilen **16** und **17** einkommentieren (am Anfang der Zeile ``//`` entfernen)
  - in der Datei [Startup.cs](Umfrage-Tool/Umfrage-Tool/Startup.cs) die Zeile **16** einkommentieren (am Anfang der Zeile ``//`` entfernen)
6. Nun muss das Projekt für die Datenbankerstellung gestartet werden und nach ein paar Sekunden, wenn die Datenbank erstellt wurde, mit ``Shift + F5`` beendet werden (Um Fehler zu vermeiden wechseln Sie in die Datei [Home/Index.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Home/Index.cshtml))
7. Änderungen von 5. rückgängig machen
  - in der Datei [database.cs](Umfrage-Tool/Umfrage-Tool/database.cs) die Zeilen **16** und **17** auskommentieren (am Anfang der Zeile ``//`` hinzufügen)
  - in der Datei [Startup.cs](Umfrage-Tool/Umfrage-Tool/Startup.cs) die Zeile **16** auskommentieren (am Anfang der Zeile ``//`` hinzufügen)
8. Nun kann das Projekt erneut gestartet werden (Um Fehler zu vermeiden wechseln Sie in die Datei [Home/Index.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Home/Index.cshtml))

### Installation auf einem Windows Server (Web-Server)
0. Windows Server (Windows Server 2012/2016) installieren und Windows-Feature aktivieren (``Install-WindowsFeature -Name Web-Server -IncludeManagementTools``)
1. Projekt von [Github](https://github.com/FI17/Befragungstool/archive/master.zip) (direkter Download) herunterladen
2. ZIP-Archiv mit Entpackungsprogramm entpacken (Zum Beispie [7zip](http://www.7-zip.de/download.html))
  - Die entpackten Dateien auf den Windows-Server kopieren
3. Im Projekt die [web.config](Umfrage-Tool/Umfrage-Tool/Web.config)-Datei öffnen und in Zeile **15** Änderungen am Dateipfad vornehmen
  - Den Dateipfad auf einen Ordner mit Vollzugriff verweisen lassen (dort wird die Datenbank abgespeichert)
4. Die Dateien  [Home/Index.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Home/Index.cshtml) und [Umfrage_Erstellung/FrageErstellung.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Umfrage_Erstellung/FrageErstellung.cshtml) anpassen
  - Pfad  von "localhost:60480" in die IP des Servers ändern (Beispiel: ``10.5.55.55``) in den folgenden Dateien:
    - in Home in Zeile **23**
    - in Umfrage-Erstellung in Zeile **79**
6. Im IIS-Manager die ``Default Web Site`` anpassen
  - den physischen Pfad zu zum ersten 2. Unterordner ``Umfrage-tool`` des Projektes legen. Beispiel ``D:\Umfrage-Tool\Umfrage-Tool``, wenn das Projekt einfach auf den Datenträger ``D`` entpackt wurde.
  - mit ``Ok`` bestätigen.
  
  Nun kann das Projekt mit der IP des Servers im bevorzugten Browser aufgerufen werden. Beispiel: ``10.5.55.55``
  
  Bei Fragen oder Problemen: ein [Issue](https://github.com/FI17/Befragungstool/issues) erstellen
