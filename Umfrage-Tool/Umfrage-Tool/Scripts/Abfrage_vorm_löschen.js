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

function myFunction(text) {
    var copyText = document.getElementById(text);
    copyText.select();
    document.execCommand("copy");
}
