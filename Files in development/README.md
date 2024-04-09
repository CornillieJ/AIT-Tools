# PE1 2324 2 IpCalculator  

## Opdracht  

Haal de repo binnen en bekijk aandachtig het WPF project dat je gekregen hebt.    
Alle controls zijn aangemaakt, alsook alle event-handlers.  
In code behind is ook een methode aanwezig die op een IMAGE-control een bepaalde afbeelding tekent (white.png).  In het programma zal je deze afbeelding moeten wijzigen (rood = niet OK, groen = wel OK) (zie verder)  
  
Via de comboboxen bovenaan dien je 2 (Host1 en Host2) private Ipv4 adressen te selecteren, alsook het CIDR nummer.  
Voor elk van de hosts dien je het volgende te berekenen : 
  * Het Ip adres (dotted decimale en binaire voorstelling)  
  * Het subnetmasker  (dotted decimale en binaire voorstelling)  
  * Het netwerknummer waartoe dit IP nummer behoort  (dotted decimale en binaire voorstelling)  
  * Het kleinste hostnummer binnen dit netwerk  (dotted decimale en binaire voorstelling)  
  * Het grootste hostnummer binnen dit netwerk  (dotted decimale en binaire voorstelling)  
  * Het broadcast adres van dit netwerk  (dotted decimale en binaire voorstelling)  

Is er voor beide hosts een IP-nummer en CIDR-nummer geselecteerd, dan dien je na te gaan of beide IP-nummers zich binnen hetzelfde netwerk bevinden.  
  * Is er een van beide hosts niet ingevuld dan toon je de witte bol (/images/white.png)  
  * Bevinden beide hosts zich binnen hetzelfde logische netwerk dan toon je de groene bol (/images/green.png)  
  * Bevinden beide hosts zich NIET binnen hetzelfde logische netwerk dan toon je de rode bol (/images/red.png)  
  
Met de comboboxen mag je enkel private IP-nummers kunnen aanmaken.  
Dat betekent concreet dat in de eerste combobox (cmbHost1B1 en cmbHost2B1) enkel de waarden 10, 172 en 192 mogen voorkomen.  
Afhankelijk van de keuze die in de eerste combobox gemaakt wordt dient de tweede combobox verschillend opgevuld te worden : 
  * 1° combobox (cmbHost1B1 of cmbHost2B1) = 10 => 2° comboboxen (cmbHost1B2 en cmbHost2B2) waarden van 0 t/m 255  
  * 1° combobox (cmbHost1B1 of cmbHost2B1) = 172 => 2° comboboxen (cmbHost1B2 en cmbHost2B2)  waarden van 16 t/m 31
  * 1° combobox (cmbHost1B1 of cmbHost2B1) = 192 => 2° comboboxen (cmbHost1B2 en cmbHost2B2)  enkel waarde 168
De derde comboboxen (cmbHost1B3 en cmbHost2B3) worden telkens gevuld met de waarden 0 t/m 255.
De vierde comboboxen (cmbHost1B4 en cmbHost2B4 )worden telkens gevuld met de waarden 1 t/m 244.    

De comboboxen met de CIDR nummers laat je vullen met waarden die terug afhankelijk zijn van de gekozen waarde in de eerste combobox :   
  * 1° combobox (cmbHost1B1 of cmbHost2B1) = 10 => CIDR waarden 8 t/m 30  
  * 1° combobox (cmbHost1B1 of cmbHost2B1) = 172 => CIDR waarden 12 t/m 30
  * 1° combobox (cmbHost1B1 of cmbHost2B1) = 192 => CIDR waarden 16 t/m 30

## Belangrijk  

  * Werk met een class-library  
  * Plaats zo veel als mogelijk de logica in klassen, NIET in je code behind  
  * Plaats in je code behind enkel code die betrekking heeft op je WPF venster  

## Demo
  
<img src="/assets/demo.gif" />





