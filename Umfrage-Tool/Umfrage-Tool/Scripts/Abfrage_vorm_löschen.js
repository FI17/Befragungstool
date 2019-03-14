confirm_delete = function () {
    var umfrage_name_bool = false;
    var umfrage = document.getElementById("loeschen").getAttribute("name");

    var umfrage_name_text = prompt("Bitte geben Sie den Namen der zu löschenden Umfrage ein!");
    if (umfrage_name_text == umfrage) {
        umfrage_name_bool = true;
    }
    else {
        umfrage_name_bool = false;
    }
    return umfrage_name_bool;
}