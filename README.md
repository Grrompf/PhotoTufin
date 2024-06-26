Photo Tufin Finder
==================

Mit einen Scan wird ein Verzeichnis nach Fotos bzw. Bildern durchsucht. 
Die Bilder werden anhand ihrer Dateiendung (Filter) identifiziert. Dieser Filter ist einstellbar.
Für jedes Bild wird der md5 Hash vom Inhalt der Datei ermittelt. Bilder mit identischen Hash gelten als Duplikate.
Die Daten aller Bilder werden in der Datenbank abgelegt. Ebenso die wmi Informationen zu dem Speichermedium.
Angezeigt werden nur Duplikate bzw. Tuplets. Die Duplikate werden über die gesamte Datenbank ermittelt.
Durch einen Klick auf ein jeweiliges Duplikat wird ein neues Fenster (Detailansicht) geöffnet mit allen Duplikaten
zu diesem Bild und auf welchem Speichermedium sie zu finden sind.
Wird ein Speichermedium geändert, z.B. wenn man Duplikate entfernt oder neue Bilder hinzufügt, sollte die jeweilige
Datenbank zu dem Speichermedium gelöscht werden (Button!).
Danach kann man das Medium neu scannen, um den Datenstand zu aktualisieren. Anderenfalls erhält man falsch-positive
Ergebnisse bei der Duplikatsanzeige. Man kann auch den gesamten Datenstand löschen, muss danach aber alle Speichermedien
neu einlesen.   
Das Einlesen ist je nach Speichermedium und Prozessor mehr oder weniger schnell. Jedoch ist das Speichern neuer Bilder
in die Datenbank zeitaufwendig (Geduld ist angebracht). Erst wenn der Prozess beendet wurde, werden die Duplikate
angezeigt. 
Über die ComboBox kann man die Duplikate der jeweiligen Speichermedien anzeigen lassen (das ist recht flott). 
Die Speichermedien haben eine eindeutige Seriennummer. Um gleiche Modelle besser unterscheiden zu können, bekommen diese
eine Laufnummer.

Die Software ist Open Source und darf nicht-kommerziell weitergeben werden.  

DocHoliday 2022
