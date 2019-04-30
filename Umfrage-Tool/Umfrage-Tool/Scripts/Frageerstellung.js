function Zeige_Text(Zahl, Last) {
    $('#Antwort_' + Zahl).prop('disabled', false);
    $('#Antwort_' + Zahl).attr('name', "model.questionViewModels[" + Last + "].choices[" + Zahl + "].text");
    document.getElementById("Minus_" + Zahl).style.display = "inline";
    document.getElementById("Plus_" + Zahl).style.display = "none";
    if (Zahl != 14) {
        document.getElementById("Zeile_" + (Zahl + 1)).style.display = "inline";
    }
    $('#Minus_' + (Zahl - 1)).prop('disabled', true);
}

function Verstecke_Text(Zahl) {
    $('#Antwort_' + Zahl).prop('disabled', true);
    $('#Antwort_' + Zahl).attr('name', "");
    $('#Antwort_' + Zahl).prop('value', "");
    document.getElementById("Minus_" + Zahl).style.display = "none";
    document.getElementById("Plus_" + Zahl).style.display = "inline";
    if (Zahl != 14) {
        document.getElementById("Zeile_" + (Zahl + 1)).style.display = "none";
    }
    $('#Minus_' + (Zahl - 1)).prop('disabled', false);
}

function Typ_aendern(Mehr, Sonst) {
    if (Mehr.checked == true) {
        document.getElementById("Fragentyp").value = "3";
    }
    if (Sonst.checked == true) {
        document.getElementById("Fragentyp").value = "4";
    }
    if (Sonst.checked == true && Mehr.checked == true) {
        document.getElementById("Fragentyp").value = "5";
    }
    if (Sonst.checked == false && Mehr.checked == false) {
        document.getElementById("Fragentyp").value = "1";
    }
}

function Passe_Antworten_an(Zahl, Last) {
    var skala = document.getElementById("Skala_fuer_Erstellung");
    var mittelfeld = document.getElementById("MittelFeld_Text");
    var maxifeld = document.getElementById("MaxiFeld_Text");
    if (Zahl % 2 == 0) {
        $('#MittelFeld_Text').prop('disabled', true);
        $('#MittelFeld_Text').attr('name', "");
        $('#MaxiFeld_Text').attr('name', "model.questionViewModels[" + Last + "].choices[1].text");
    }
    else {
        $('#MittelFeld_Text').prop('disabled', false);
        $('#MittelFeld_Text').attr('name', "model.questionViewModels[" + Last + "].choices[1].text");
        $('#MaxiFeld_Text').attr('name', "model.questionViewModels[" + Last + "].choices[2].text");
    }
    skala.max = Zahl;
}