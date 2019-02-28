function Visuelles_Anzeigen(Skalenname) {
    var Skala = document.getElementById(Skalenname);
    var zahl = document.getElementById(Skala.value + "+Feld" + Skalenname);

    for (var i = 0; i <= Skala.max; i++) {
        document.getElementById(i + "+Feld" + Skalenname).style.fontWeight = "normal";
        document.getElementById(i + "+Feld" + Skalenname).style.fontSize = "15px";
    }
    zahl.style.fontWeight = "bold";
    zahl.style.fontSize = "18px";
}