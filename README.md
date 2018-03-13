# Lofova chýše

ASP.NET - MVC / Razor / NHIbernate / Bootstrap / JQuery / SignalR / AnimateCSS / SWAL / Prism / UnicornButtons / Modernizr / TippyJS
<hr></hr>
Open source board bulletin project. Descendant of svobodneForum.eu

ETA1: Q1 of 2018

Postup verze 1.0-PDEV v obrázcích [zde](https://github.com/lofcz/LofovaChyse/wiki/1.0-P-DEV)  
Design dokument: https://docs.google.com/document/d/18cH6fJm2VhxDgQn9-nKaSf13YwZSgx2XhHrFYrW4Y_o/edit#

## Hotové funkce:
- Příspěvky
  - Render
  - Prémium
  - Kategorie a neomezené subkategorie
  - Ocenění pro příspěvky (cena moderátora / komunitní volba..) - stuhy
  - Přidat
  - Upravit
  - Smazat
  - Verzování
  - Náhled
  - Detail
    - AJAX
    - Synchronní
  - Komentáře
    - Reakce na komentáře (Hezky napsáno / Dobrá pointa / Souhlasím) 
    - Náhled uživatele při najetí myší na jméno
    - Odpovědi na komentáře
    - Upravit
    - Smazat
- Uživatelé
  - Přihlášení + registrace
  - Odhlášení
  - Role
  - Profil
    - Uvítací text
    - Profilový obrázek
    - Ocenění
    - Nálada / nabídky práce
  - Zeď
- Úrovně přístupu, autentifikace
- AJAX validace + WYSIWYG editor (sbohem bb)
- Linkování k databázi
- DaoModel
- MasterPage + partialní layouty
- Lazy loading
- Notifikace
  - Možnost odměny při potvrzení notifikace
  - AJAX
- ACP
- Ukládání obrázků na server, automatický resize 
- Paginace
  - AJAX
  - Synchronní
- Vyhledávání
  - Fulltext
  - Kategorie
  - Autocomplete
- Hodnocení
  - "To se mi líbí"
    - AJAX
- Menu autorouting
- Hlavní zeď aktivity, automaticky generované (novinky)

## Lokální setup: 
Prerekvizity:
  - Visual Studio 2017
  - Sql Server 17.4+
  
Naklonovat lokálně projekt  
Připojení k databázi
  - Otevřít Sql server  
  - Připojení:  
    - Server type: Database engine  
    - Server name: [jméno-tvého-desktopu]  
    - Auth: Win auth  
  - Connect  
  - V horní liště: File -> Open -> File -> dbdump.sql  
  
Připojení k db - VS  
  - Projekt LofovaChýše -> hibernate.cfg.xml (třetí soubor odzdola) -> nahradit hodnotu [Data Source] na řádku 6 názvem serveru (desktop name)

Projekt by měl jít zkompilovat s tím, že se připojí k lokální db.
