# Befragungstool

![Version](https://img.shields.io/badge/Version-0.7-yellowgreen.svg)
![Status](https://img.shields.io/badge/Status-WIP-important.svg)
![Framework](https://img.shields.io/badge/Framework-ASP.NET%20MVC-informational.svg)
![License](https://img.shields.io/github/license/FI17/Befragungstool.svg)

Befragungstool in ASP.NET
## Das Befragungstool
Das Befragungstool ist eine vollwertige ASP.NET MVC Website, welche die Erstellung, Beantwortung und Auswertung von Fragebögen ermöglicht.</br>
Das Tool ist nur auf Windows installierbar
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
4. Im Projekt die [web.config](Umfrage-Tool/Umfrage-Tool/Web.config)-Datei öffnen und in den Zeilen **16** (Login-Datenbank) und **20** (Umfragen-Datenbank) Änderungen am Dateipfad vornehmen
  - Login-Datenbank: Pfad auf ``|DataDirectory|\aspnet-Test_Login-20190130100435.mdf`` setzen
  - Umfragen-Datenbank: Pfad auf ``|DataDirectory|\Umfrage-tool.mdf``oder einen beliebigen Pfad mit Vollzugriff setzen
5. database.cs und Startup.cs anpassen
  - in der Datei [database.cs](Umfrage-Tool/Umfrage-Tool/database.cs) die Zeilen **16** und **17** einkommentieren (am Anfang der Zeile ``//`` entfernen)
  - in der Datei [Startup.cs](Umfrage-Tool/Umfrage-Tool/Startup.cs) die Zeile **14** einkommentieren (am Anfang der Zeile ``//`` entfernen)
6. Nun muss das Projekt für die Datenbankerstellung gestartet werden und nach ein paar Sekunden, wenn die Datenbank erstellt wurde, mit ``Shift + F5`` in Visual Studio beendet werden (Um Fehler zu vermeiden wechseln Sie zum Starten in die Datei [Home/Index.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Home/Index.cshtml))
7. Änderungen von 5. rückgängig machen
  - in der Datei [database.cs](Umfrage-Tool/Umfrage-Tool/database.cs) die Zeilen **16** und **17** auskommentieren (am Anfang der Zeile ``//`` hinzufügen)
  - in der Datei [Startup.cs](Umfrage-Tool/Umfrage-Tool/Startup.cs) die Zeile **14** auskommentieren (am Anfang der Zeile ``//`` hinzufügen)
8. Nun kann das Projekt erneut gestartet werden (Um Fehler zu vermeiden wechseln Sie zum Starten in die Datei [Home/Index.cshtml](Umfrage-Tool/Umfrage-Tool/Views/Home/Index.cshtml))

### Installation auf einem Windows Server (Web-Server)
0. Windows Server (Windows Server 2012/2016) installieren und Windows-Feature aktivieren (``Install-WindowsFeature -Name Web-Server -IncludeManagementTools``)
1. Projekt von [Github](https://github.com/FI17/Befragungstool/archive/master.zip) (direkter Download) herunterladen
2. ZIP-Archiv mit Entpackungsprogramm entpacken (Zum Beispie [7zip](http://www.7-zip.de/download.html))
  - Die entpackten Dateien auf den Windows-Server kopieren
3. Im Projekt die [web.config](Umfrage-Tool/Umfrage-Tool/Web.config)-Datei öffnen und in den Zeilen **16** (Login-Datenbank) und **20** (Umfragen-Datenbank) Änderungen am Dateipfad vornehmen
  - Login-Datenbank: Pfad auf ``|DataDirectory|\aspnet-Test_Login-20190130100435.mdf`` setzen
  - Umfragen-Datenbank: Pfad auf ``|DataDirectory|\Umfrage-tool.mdf``oder einen beliebigen Pfad mit Vollzugriff setzen
5. database.cs und Startup.cs anpassen
4. Im gesamten Projekt über die Funktion *Suchen und Ersetzen* ``localhost:60480`` durch die IP des Servers ((Beispiel: ``10.5.55.55``) ersetzen
6. Im IIS-Manager die ``Default Web Site`` anpassen
  - den physischen Pfad zu zum ersten 2. Unterordner ``Umfrage-tool`` des Projektes verweisen lassen (Beispiel ``D:\Umfrage-Tool\Umfrage-Tool``, wenn das Projekt einfach auf den Datenträger ``D`` entpackt wurde).
  - mit ``Ok`` bestätigen.
7. Im ISS-Manager die "Default Web Site" über einen Rechtsklick und "Website verwalten" starten

  Nun kann das Projekt mit der IP des Servers im bevorzugten Browser aufgerufen werden. Beispiel: ``10.5.55.55``
  
  Bei Fragen oder Problemen: ein [Issue](https://github.com/FI17/Befragungstool/issues) erstellen
