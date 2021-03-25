# Mediathekar
Ein ASP.NET Core Client-Server-Projekt. Mediathekar durchsucht öffentliche Mediatheken und lädt Beiträge und Filme nach Wunsch runter.
Dieses Projekt stellt eine komfortable Lösung dar, um öffentliche Medien als Privatkopie herunterzuladen. Datenquellen sind MediathekViewWeb und PokemonTV (möglicherweise weitere Mediatheken in Zukunft). Es soll sich einfach in eine bestehende Medienserver-Infrastruktur integrieren lassen, beispielsweise containerisiert neben einem Plex Media Server. Über die Übersicht lassen sich Medien auswählen und sie serverseitig herunterladen. Mittels ffmpeg werden Videos und Streams in mp4-Dateien konvertiert.

An ASP.NET Core Client Server Project. Mediathekar lets you browse public media libraries and download shows and movies.
This project aims to be a comfortable solution for downloading public media as private copy. Data sources are MediathekViewWeb and PokemonTV (in future maybe more). It should integrate into a media server ecosystem (e.g. as Docker container next to a Plex Media Server). Select and download shows or movies via the user interface in browser. Ffmpeg is used to convert videos and streams to mp4.

![](Screenshot-Home.png?raw=true "Screenshot Home")