# Projektrapport
> Av Merkur Hoxha (mh222vn) WP12 | 1DV409 | 1DV449

## Så här fungerar appen

Idén till appen föddes efter att jag tröttnat på imdb.com:s skeva betygsättning för filmer. Oftast ser jag filmer utan att utgå ifrån deras betyg för att sedan stämma av med deras sajt. Min åsikt om att de överskattar en del filmer vad gäller betyg, bekräftas gång på gång.

Vi har ju lärt oss i Webteknik II om data mining och hur kraftfullt data kan vara när det blir länkat. Därefter kändes konceptet för appen väldigt naturligt - samla in betyg och data för en viss film från så många datakällor (API:er) som möjligt och förhoppningsvis kunna ge användaren en mer rättvis betygssättning än den som kan fås utav en enda källa.

Ett bra exempel för "rättvis betygssättning" är en av datakällorna som appen använder sig av, den svenska filmsajten moviezine.se; dess användare tycks ha en väldigt kräsen smak - vilket medför att även om en film är överskattad hos, exempelvis, Imdb.com så sänks filmes genomsnittliga betyg påtagligt utav moviezine.

Nedan kan du se ett övergripande flödesschema för hur applikationen går tillväga, från giltig sökterm till presentation av sökresultat.

![](https://raw.github.com/metsson/ht13-2-1-moviescore/master/docs/schema.png)

**Serversidan**

Applikationens arkitektur består av ASP.net MVC 4 och MSSQL Server. Den består av flera lager däribland vyer, controllers, och models är de huvudsakliga. Men även webs ervices är ett viktigt lager just i detta sammanhanget, eftersom det är där all hantering av API:erna sker. 

Den giltiga söktermen, skickad ifrån användaren, tas om hand på serversidan utav en modelklass som i sin tur kollar filmtiteln mot databasen. Om filmdata finns cache:at i databasen och har en tidsstämpel som inte passerat giltighetstiden, så returnas filmdatat till klienten med hjälp utav en vyklass. 

Skulle inte filmen vara cache:at eller tidsstämpeln vara ogiltig, så gör modelklassen anrop till huvudklassen för web services. Om huvudklassen (just nu satt till omdbapi.com) svarar med ett JSON result vars "Response" är satt till true så betyder det att det är dags att samla in data för filmen - alltså finns det en filmträff. Varje API-anrop har dock sin egen felhanteringen, det temporära objektet som skapats för att samla allt API-data på sig har en egenskap för hämtningsstatus. Skulle något av API:erna sätta dess status till false så returneras ett felmeddelande till klienten att ingen film kunde hittas. I övrigt så returnerar samtliga exceptions ett "allmänt" felmeddelande till klienten.

**Klientsidan**

Huvudkomponenterna för klientsidan är *sammy.js*, *routing.js*, *jQuery/jQueryUI*, och *SignalR* (som egentligen använder sig av web sockets, ajax long polling som fallback, i bakgrunden).

Det är igenom sammy.js som applikationen beter sig som en SPA. Routing.js lägger ett # i slutet av roten för URL:en som sammy.js fångar upp och lägger till alla nya requests efter # för att sedan ladda in response-innehåll asynkront med hjälp utav AJAX.

Till exempel

    moviecheck.metsson.se/Home/Top100
    ... blir
    moviecheck.metsson.se/#/Home/Top100

jQueryUI används med modulen Autocomplete för att ge slutanvändern sökförslag när den använder appens huvudfunktion - filmsökning. Tillbaka skickas en array med filmträffar som direkt är hämtade ifrån databasen där filmerna är cache:ade. Notera att autocomplete här ska kopplas till ett API där det går att söka på miljontals titlar (feature request).

I övrigt så håller gränssnittet en väldigt ren stil och all fokus har lagts på just sökfunktion och att visa bra resultat. Det finns en hjälp sida /Home/About som förklarar hur data hanteras och en Topp 100 sida /Home/Top100 som listar de 100 filmerna som enligt appens scoringssytem har högsta betyg. En sökning i appen triggar också igång ett realflöde av sökningar som visas på huvudsidan; en bra funktion för de som bara kan komma på en filmtitel att söka och sedan tappar inspirationen!

## Reflektioner

Sammanfattningsvis har projektet gått bra. Det största problemet jag upplevde i början av projektet var att omvandla en vanlig ASP.net MVC applikation till en Single Page Application. Jag använde sammy.js för att åstadkomma detta och ASP ramverket tillåter inte att man skapar en rutt som börjar på #/controller/action/id vilket gjorde att # fick hårdkodas i samtliga url:er. Ett annat problem, som har varit ganska konstant, är att appen skulle publiceras på skolans nätverk. Det medförde att tester mot nätverket sköts upp i sista stunden, något som jag aldrig skulle göra om alternativt publicera appen på eget webbhotell och komma runt en VPN-anslutning. Ett av favorit API:erna, themoviedb.org, som jag valde ut för appen ville inte fungera genom API och verkar vilja ha en egen URL (ett snabbt test med att publicera appen på mitt webbhotell och göra anrop därifrån till API:et visar att det fungerar), så det fick bli att leta efter substitut. omdbapi.com visade sig vara kanon, där går det att söka med både filmtitel eller imdbID och API:et gör i sin tur även anrop till ett annat API som jag tänkte använda - rottentomatoes.com - så jag lyckades på så sätt göra ett anrop mindre genom att istället använda omdapi.com. 

Ytterligare ett problem med att jobba lokalt och gentemot VPN har varit delningsknapparna för en filmtitel. De tillåter inte att url:en skall vara utformad hur som helst och jag beslutade att kommentera bort delningsknapparna under tiden appen är i beta-version.

Jag har i skrivande stund inte planer på att implementera fler funktioner för appen. Att göra ett enkelt anrop till Netflix för att presentera för användaren om filmen fanns tillgänglig där uteblev eftersom Netflix inte accepterar nya utvecklare in på sin API-plattform. 

Skall dock jobba vidare med sökfunktionen för appen och vill att sökförslag istället hämtas ifrån themoviedb.org, eftersom det kommer ta längre tid att samla på sig så mycket data i cachedatabasen så att miljontals filmtitlar kan slåss upp i den. 

**Risker** finns det inga med applikationen. Worst case scenario som jag kan komma på är att data för en sökt filmtitel returnrar JSON/XML innehåll vars struktur är förändrad och gör att det inte går att läsa av. För användarens skull så medför detta bara ett generiskt felmeddelande. Kanske ska planera en fallback för detta fall, eller kanske en notifieringsfunktion som skickar ut ett mail till appägaren utifall att detta fel inträffar ofta. 

Vad jag anser vara betygshöjande funktioner för appen är i stort sett hela klientsidan, där jag använt mig av jQueryUI, AJAX och SignalR. SignalR är ett fantastiskt paket för ASP.net eftersom det hanterar både web sockets där det finns och long polling, alltså visas realtidsdata oavsett tillgänglig teknik. Annat än det, så använder sig appen av bootstrap och är mobilanpassad. Och jag tycker personligen att appen når fram med sitt budskap gränssnittsmässigt och gör det den är skapad för - söka efter film och visa mer rättvist betyg för given filmtitel. Jag har i optimeringsväg cache:at både filmdata för att minska antal API-anrop och bilderna som skickas med som URL:er i resultatet. Har även kollat med hjälp av Visual Studio på klockcykel och resultatet blev, som anat, att js filer upptar cirka 28% av dessa. Men detta är inte helt oväntat eftersom applikationen dels är byggd som SPA, dels använder sig av både jqueryUI och Web sockets. 