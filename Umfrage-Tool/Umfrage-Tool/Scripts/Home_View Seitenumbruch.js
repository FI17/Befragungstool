function nächsten_Abschnitt_anzeigen(abschnitt) {
    document.getElementById("Abschnitt_" + (abschnitt + 1)).style.display = "inline";
    document.getElementById("Abschnitt_" + abschnitt).style.display = "none";
}
function vorherigen_Abschnitt_anzeigen(abschnitt) {
    document.getElementById("Abschnitt_" + (abschnitt - 1)).style.display = "inline";
    document.getElementById("Abschnitt_" + abschnitt).style.display = "none";
}