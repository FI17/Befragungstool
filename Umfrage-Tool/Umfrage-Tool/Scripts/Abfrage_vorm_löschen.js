confirm_delete = function (umfrage) {
    var umfrage_name_bool = false;

    var umfrage_name_text = prompt("Bitte geben Sie den Namen der zu löschenden Umfrage ein!");
    if (umfrage_name_text == umfrage) {
        umfrage_name_bool = true;
    }
    else {
        umfrage_name_bool = false;
    }
    return umfrage_name_bool;
}

veröffentlichen_bestätigen = function(umfrage) {
    //if (confirm("Wollen Sie diese Umfrage wirklich veröffentlichen?")) {
    //    return true;
    //}
    //else
    //{
    //    return false;
    //}
    return true;
}
beenden_bestätigen = function (umfrage) {
    if (confirm("Wollen Sie diese Umfrage wirklich beenden?")) {
        return true;
    }
    else {
        return false;
    }
}


function jaNein(input) {
    var inj = input;
    if (confirm("Wollen Sie " + inj + " wirklich löschen?")) {
        return true;
    }
    else {
        return false;
    }
}

jaNein_Antwort = function () {
    if (confirm("Wollen Sie diese Antwortmöglichkeit wirklich löschen?")) {
        return true;
    }
    else {
        return false;
    }
}

function copyLink(text) {
    var copyText = document.getElementById(text);
    copyText.select();
    document.execCommand("copy");
}

function kopiereLink(text)
{
    var feld = document.createElement('textarea'); // <-- Feld um Text einzuschreiben und auszulesen
    feld.value = text;
    feld.setAttribute('readonly', '');
    feld.style = { position: 'absolute', left: '-9999px' };
    document.body.appendChild(feld);
    feld.select();
    document.execCommand('copy');
    document.body.removeChild(feld);
}

function Ersteller_der_Umfrage_ändern(wert, umfrageID) {
    if (confirm("Wollen Sie den Ersteller dieser Umfrage ändern?") == false) {
        document.getElementById("Ersteller_Liste_" + umfrageID).value = document.getElementById("Alter_Ersteller_" + umfrageID).value;
    } else {
        $.ajax({
            url: 'Home/Ändere_Ersteller_in_Datenbank',
            data: { Umfrage: umfrageID, Ersteller: wert }
        }).done(function () {
            document.getElementById("Alter_Ersteller_" + umfrageID).value = wert;
        });
    }
}

WurdeEinDatumEingetragen = function () {
    if (document.getElementById("datum").value.toString().length === 0) {
        alert("Bitte geben Sie ein gültiges Datum ein!");
        return false;
    }
    return true;
}