

<h1 style="font-size:xx-large;color: yellow">
3 ASP.NET Core Web-API - Restful Beispiele mit Testing (ohne Autorize)
</h1>

Das Projekt zeigt drei ASP.NET Core Web-API-Beispiele mit schrittweiser Verbesserung: von synchronen CRUD-Operationen bis hin zu asynchroner Verarbeitung und detaillierten HTTP-Statuscodes. Begleitende Unit-Tests gewährleisten die Validierung, Datenintegrität und Skalierbarkeit der APIs.

----

# Bsp01 - Web API - PersonController (sync)

Das Projekt ist eine einfache ASP.NET Core Web API, die einen `PersonController` bereitstellt, 
um CRUD-Operationen (Erstellen, Lesen, Aktualisieren, Löschen) für Personen zu ermöglichen. 
Es verwendet Dependency Injection mit einem `PersonService`, der eine interne Liste von Personen verwaltet. 
OpenAPI/Swagger wird integriert, um die API zu dokumentieren und eine interaktive Oberfläche bereitzustellen. 
Das Modell `Person` enthält Felder wie `PersonId`, `Vorname`, und `Nachname`. 
Die API-Endpunkte validieren Eingabedaten (z. B. Namen) und bieten grundlegende Funktionen 
wie das Abrufen aller Personen, die Suche nach einer bestimmten Person, das Hinzufügen neuer Personen, 
das Aktualisieren bestehender Datensätze und das Löschen von Einträgen. Das Projekt dient als Grundlage 
für die Verwaltung und Erkundung von Personendaten über eine Webschnittstelle.

[Details zu Bsp01](Bsp01/Readme.md)

----

# Bsp02 - Web API - PersonController (sync) + ActionResult

Das Projekt `Bsp02` erweitert die Funktionalität der Web-API `Bsp01`, indem es `ActionResult` 
für die Rückgabe von HTTP-Antworten in den Controllermethoden einführt. 
Dadurch können detailliertere Statuscodes wie `400 Bad Request`, `404 Not Found` und `200 OK` zurückgegeben werden, 
um spezifischere Rückmeldungen bei Erfolgen oder Fehlern zu bieten. Die API bietet weiterhin CRUD-Funktionen 
für das `Person`-Modell, nutzt jedoch die verbesserte Struktur von `ActionResult`, um klarere und 
benutzerfreundlichere Antworten bereitzustellen. Der Hauptunterschied zu `Bsp01` liegt 
in der verbesserten Fehlerbehandlung und präziseren Kommunikation zwischen Client und Server 
durch die Rückgabe von HTTP-Statuscodes.

[Details zu Bsp02](Bsp02/Readme.md)

----

# Bsp03 - Web API - PersonController (async Task) + ActionResult 

Das Projekt `Bsp03` erweitert die Funktionalität der Web-API `Bsp02`, indem alle Operationen 
asynchron (`async/await`) implementiert werden, um die Skalierbarkeit und Effizienz zu erhöhen. 
Es verwendet weiterhin `ActionResult`, um detaillierte HTTP-Antworten zu liefern, ergänzt jedoch 
asynchrone Methoden im `PersonService` und `PersonController`, wodurch zukünftige datenbankbasierte Operationen 
nahtlos integriert werden können. Der Unterschied zu `Bsp02` liegt in der Nutzung von Asynchronität, 
wodurch nicht-blockierende Prozesse ermöglicht werden und die API auf anspruchsvollere Szenarien vorbereitet ist, 
insbesondere bei mehreren gleichzeitigen Anfragen.

[Details zu Bsp03](Bsp03/Readme.md)


----


# Bsp01.Tests

## PersonControllerTests

Das Projekt implementiert Unit-Tests für die Web-API `PersonController` aus `Bsp01`.
Es überprüft die Funktionalität der API-Endpunkte (`Get`, `Post`, `Put`, `Delete`) sowie
die Hilfsmethoden wie die Namensvalidierung. Die Tests simulieren realistische Szenarien,
wie das Abrufen, Hinzufügen, Aktualisieren und Löschen von Personen, und stellen sicher,
dass die Logik korrekt arbeitet, z. B. durch Prüfung von Rückgabewerten, Validierungen und Seiteneffekten.
Es wird der gleiche `PersonService` genutzt, um Konsistenz zwischen Tests zu gewährleisten.
Der Fokus liegt auf der Validierung von Eingaben, der korrekten Bearbeitung von Daten und
der Einhaltung der API-Spezifikationen.

## PersonServiceTests

Das Projekt `PersonServiceTests` testet die Kernfunktionalität des `PersonService` aus `Bsp01`. 
Es prüft alle CRUD-Operationen: das Abrufen aller Personen, das Suchen nach einer Person anhand ihrer ID, 
das Hinzufügen, Aktualisieren und Löschen von Personen. Die Tests validieren, ob der Service 
die erwarteten Ergebnisse liefert, z. B. korrekte Rückgaben bei gültigen IDs, Null-Werte bei ungültigen IDs und 
das Hinzufügen oder Entfernen von Personen aus der Liste. Besonderer Wert wird darauf gelegt, dass 
Änderungen am Datenbestand (z. B. durch `AddPerson`, `UpdatePerson` oder `DeletePerson`) korrekt reflektiert werden. 
Der Fokus liegt ausschließlich auf der internen Logik des Services, ohne Abhängigkeit von Controllern oder anderen Schichten.

----

# Bsp02.Tests

## PersonControllerTests

Das Projekt implementiert Unit-Tests für die Web-API `PersonController` aus `Bsp02`,
um deren Funktionalität umfassend zu prüfen. Es werden CRUD-Operationen (`Get`, `Post`, `Put`, `Delete`) und
die Namensvalidierung getestet, wobei `ActionResult`-basierte Rückgaben und HTTP-Statuscodes überprüft werden.
Die Tests validieren Eingaben, prüfen korrekte Rückgabewerte, und stellen sicher, dass
Fehlerfälle wie ungültige IDs oder null-Werte korrekt behandelt werden.
Im Vergleich zum Testcode aus `Bsp01` testet `Bsp02` zusätzlich die Einhaltung von HTTP-Standards
(`OkObjectResult`, `BadRequestObjectResult`, `NotFoundResult`), um eine robustere und
realistischere API-Interaktion sicherzustellen.
Der Fokus liegt stärker auf der Überprüfung von API-konformen Rückgaben und detaillierten Statuscodes.


## PersonServiceTests

Das Projekt `PersonServiceTests` testet die Kernlogik des `PersonService` aus `Bsp02`. Es umfasst Unit-Tests
für alle CRUD-Funktionen: Abrufen, Hinzufügen, Aktualisieren und Löschen von Personen, sowie die Behandlung
von fehlerhaften Eingaben, wie ungültigen IDs. Dabei wird sichergestellt, dass die Methoden erwartete Ergebnisse liefern
und Datenänderungen korrekt in der internen Liste reflektiert werden. Der Unterschied zum `Bsp02` Testcode liegt darin,
dass sich die Tests hier ausschließlich auf die Service-Ebene beschränken, ohne `ActionResult` oder
die HTTP-Kommunikation zu testen. Dadurch ist der Fokus stärker auf die Logik und Integrität der Daten
innerhalb des Services gerichtet.

----

# Bsp03.Tests

## PersonControllerTests

Das Projekt `Bsp03` testet die Web-API `PersonController`, wobei der Fokus auf asynchroner Verarbeitung liegt.
Es umfasst Unit-Tests für alle CRUD-Operationen (`Get`, `Post`, `Put`, `Delete`) und
prüft die Einhaltung der asynchronen API-Spezifikationen durch die Verwendung von `async` und `await`.
Zudem validiert es die Rückgabe von HTTP-Statuscodes
(`OkObjectResult`, `BadRequestObjectResult`, `CreatedAtActionResult`, `NotFoundObjectResult`) und
testet die Namensvalidierung detailliert. Im Unterschied zum Testcode von `Bsp02` liegt der Fokus
auf asynchronen Operationen, um realistische Szenarien für moderne, nicht-blockierende APIs zu simulieren
und zu gewährleisten, dass die API bei mehreren gleichzeitigen Anfragen stabil bleibt.

## PersonServiceTests

Das Projekt `PersonServiceTests` testet die Funktionalität des `PersonService` aus `Bsp03` 
mit einem Fokus auf asynchrone Methoden. Es prüft alle CRUD-Operationen (Abrufen, Hinzufügen, Aktualisieren 
und Löschen von Personen) und stellt sicher, dass die Ergebnisse konsistent sind, einschließlich 
der Handhabung von ungültigen IDs. Die Tests validieren, dass die Methoden erwartete Rückgabewerte liefern und 
Änderungen in der internen Liste korrekt reflektieren. Der Unterschied zum Testcode von `Bsp02` liegt 
in der asynchronen Implementierung: Alle Methoden verwenden `async` und `await`, was die Tests 
auf zukünftige Erweiterungen mit nicht-blockierenden Operationen (z. B. Datenbankabfragen) vorbereitet. 
Die Logik bleibt konsistent, jedoch wird die Skalierbarkeit und Effizienz durch Asynchronität verbessert.

----


## Git Konto wechseln und commiten

```` bash
git init

touch .gitignore

git add .
git commit -m "Initial commit"

git branch -M main

git remote add origin https://github.com/ugurat/3-ASP.NET-Core-Web-API-RESTful-Beispiele-mit-Testing.git

git push -u origin main
````

Falls Fehler:

```` bash
PASSWORT LÖSCHEN und ERNEUT ANMELDEN

Gehe zu "Windows-Anmeldeinformationen":
Unter Windows-Anmeldeinformationen "gespeicherte Zugangsdaten für Webseiten und Anwendungen" finden.

Suche nach gespeicherten GitHub-Einträgen:
git:https://github.com oder Ähnliches.

Eintrag löschen und erneut versuchen:

git push -u origin main
````

Aktualisiert

```` bash
git add .
git commit -m "aktualisiert"
git push -u origin main
````

Überschreiben

```` bash

git init

git add .
git commit -m "Initial commit"

git branch -M main

git remote add origin https://github.com/ugurat/3-ASP.NET-Core-Web-API-RESTful-Beispiele-mit-Testing.git


git push -u origin main --force

````

Mit dem Parameter `--force` wird Git-Repo überschrieben.

----


## Entwickler
- **Name**: Ugur CIGDEM
- **E-Mail**: [ugurat@gmail.com](mailto:ugurat@gmail.com)

---

## Markdown-Datei (.md)

---
