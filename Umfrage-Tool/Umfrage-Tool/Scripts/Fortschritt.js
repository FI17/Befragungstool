var Aktuelle_Frage = 0;

function Naechste_Frage(Laenge) {
    document.getElementById('Frage ' + Aktuelle_Frage).style.display = "none";
    document.getElementById('Frage ' + Aktuelle_Frage).lastChild.tagName = "none";
    Aktuelle_Frage++;
    document.getElementById('Fortschrittszahl').innerHTML = "Frage " + (Aktuelle_Frage + 1) + " von " + Laenge + "";
    document.getElementById('Fortschrittsanzeige').value = Aktuelle_Frage + 1;

    if (Aktuelle_Frage === Laenge - 1) {
        document.getElementById('Fertigknopf').style.display = "inline";
        document.getElementById('Weiterknopf').style.display = "none";
    }

    try {
        document.getElementById('Frage ' + Aktuelle_Frage).style.display = "inline";
        document.getElementById('Frage ' + Aktuelle_Frage).lastChild.tagName = "Wichtig";
    }
    catch (Exception) {
        document.getElementById('Fertigknopf').style.display = "inline";
        document.getElementById('Weiterknopf').style.display = "none";
    }
}

function Alles_Zeigen(Laenge) {
    for (var i = 0; i < Laenge; i++) {
        document.getElementById('Frage ' + i).style.display = "inline";
    }
}