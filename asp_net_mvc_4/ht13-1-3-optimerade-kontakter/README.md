# Optimerade kontakter
> Merkur Hoxha (mh222vn)

## Uppgift 3 - Tester
> Testerna nedan är utförda i den ordning som anges av uppgift 3 i dokumentationen.
> Svarstiderna är angivna i sekunder och avser första och andra exekevering av varje SQL-sats.

[Till testerna](https://github.com/metsson/ht13-1-3-optimerade-kontakter/blob/master/sql/tests.sql)

## Uppgift 4 - Databasdesign

**Konceptuell modell**

Normaliserad version av DemoMedlem.dbo.Medlem

- ADRESS.Gata är sammanslagning av kolumnerna Gata, Gatunr, Uppgång och conamn.
- DATUMTYP.Datumtyp innehåller värde för kolumnerna Anndatum, Betalttom, Enddatum, Inpabet, Huvkortdatum, Erskortdatum och Vervbetdatum
- INFORMATIONSTYP.Informationstyp innehåller värde för kolumnerna Tidning, Andrahand, Förhandl, Medlkort och Vervkod.

![](https://raw.github.com/metsson/ht13-1-3-optimerade-kontakter/master/konceptuell.png)


**Fysisk modell**

INDEX

- Index för PostnummerID i Medlem för att snabbare joina Medlem med Postnummer (t ex sök efter ort)
- Index för MEDLEM.Enamn och MEDLEM.Fnamn eftersom de kan vara vanligt förekommande söktermer i applikationen
- Index för MEDLEM.AdressID för att snabba på JOIN med ADRESS.
- Index för ADRESS.Postnummer för att snabba på JOIN med POSTNUMMER.
- Index för MEDLEMSDATUM.DatumtypID för att snabba på JOIN med DATUMTYP.

OPTIMERINGAR

- Flyttade annullerade medlemmar till ANNULLERAD.
Denna har en relation till ANNULLERADKONTAKT för att hålla koll på "gamla" medlemmars kontaktuppgifter.

DENORMALISERING

- INFORMATION och INFORMATIONSTYP togs bort och kolumnerna slogs ihop i MEDLEM. Dels för att de är av små mängd (bit), dels för att slippa JOIN när man vill söka efter information/inställningar för en specifik medlem.

UTEBLIVEN OPTIMERING

- Ville seperera FASTIGHET.Fastgl till en egen tabell, för att uppnå liknande resultat som i ANNULLERAD, men ansåg att det vore överflödigt för att endast minska FASTIGHET med en kolumn.

![](https://raw.github.com/metsson/ht13-1-3-optimerade-kontakter/master/fysisk.png)