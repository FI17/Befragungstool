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
            data: { umfrageIdString: umfrageID, ersteller: wert }
        }).done(function () {
            document.getElementById("Alter_Ersteller_" + umfrageID).value = wert;
            var select = document.getElementById('Ersteller_Liste_' + umfrageID);
            document.getElementById('ersteller_' + umfrageID).innerHTML =
                select.options[select.selectedIndex].text;
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

function Informationen_zeigen(text) {
    var splittung = text.split('|');
    for (var i = 0; i < splittung.length; i++) {
        splittung[i].replace(/Ü/g, "&#252;");
        splittung[i].replace(/<br>/g, "&lt;br&gt;");
        splittung[i].replace(/<ul>/g, "&lt;ul&gt;");
        splittung[i].replace(/<li>/g, "&lt;li&gt;");
        splittung[i].replace(/<\/li>/g, "&lt;/li&gt;");
        splittung[i].replace(/<\/ul>/g, "&lt;/ul&gt;");
        switch (splittung[i]) {
        case "&lt;br&gt;":
            document.getElementById('ZusatzInfo').innerHTML += "<br>";
            break;
        case "&lt;ul&gt;":
            document.getElementById('ZusatzInfo').innerHTML += "<ul>";
            break;
        case "&lt;li&gt;":
            document.getElementById('ZusatzInfo').innerHTML += "<li>" + splittung[i + 1] + "</li>";
            i += 2;
            document.getElementById("ZusatzInfo")
                .innerHTML.replace(/</g, "&lt;")
                .replace(/>/g, "&gt;");
            break;
        case "&lt;/ul&gt;":
            document.getElementById('ZusatzInfo').innerHTML += "</ul>";
            break;
        default:
            document.getElementById('ZusatzInfo').innerHTML += splittung[i];
            break;
        }
    }
}