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
    if (confirm("Wollen Sie diese Umfrage wirklich veröffentlichen?")) {
        return true;
    }
    else
    {
        return false;
    }
}
beenden_bestätigen = function (umfrage) {
    if (confirm("Wollen Sie diese Umfrage wirklich beenden?")) {
        return true;
    }
    else {
        return false;
    }
}


jaNein = function () {
    if (confirm("Wollen Sie diese Frage wirklich löschen?")) {
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

function Ersteller_der_Umfrage_ändern(wert, umfrageID) {
    if (confirm("Wollen Sie den Ersteller dieser Umfrage ändern?") == true) {
        $.ajax({
            url: 'Home/Ändere_Ersteller_in_Datenbank',
            data: { Umfrage: umfrageID, Ersteller: wert }
        }).done(function () {

        });
    }
}