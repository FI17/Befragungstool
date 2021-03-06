﻿var richtung="asc";

function sortiere_Tabelle(n, Spalte) {
    var table, rows, switching, i, x, y,dir, shouldSwitch, switchcount = 0;
    table = document.getElementById("Umfragen_Tabelle");
    switching = true;
    // Set the sorting direction to ascending:
    dir = "asc";
    /* Make a loop that will continue until
    no switching has been done: */
    while (switching) {
        // Start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /* Loop through all table rows (except the
        first, which contains table headers): */
        for (i = 1; i < (rows.length - 1) ; i++) {
            // Start by saying there should be no switching:
            shouldSwitch = false;
            /* Get the two elements you want to compare,
            one from current row and one from the next: */
            x = rows[i].getElementsByTagName("TD")[n];
            y = rows[i + 1].getElementsByTagName("TD")[n];
            /* Check if the two rows should switch place,
            based on the direction, asc or desc: */
            if (dir == "asc") {
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                    // If so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    // If so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            }
        }
        
        if (shouldSwitch) {
            /* If a switch has been marked, make the switch
            and mark that a switch has been done: */
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            // Each time a switch is done, increase this count by 1:
            switchcount++;
        } else {
            /* If no switching has been done AND the direction is "asc",
            set the direction to "desc" and run the while loop again. */
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }

    }
    if (switchcount == 0) { //Wenn nichts sortiert wurde...
        var arr = $.makeArray($("tr", "tbody").detach());
        arr.reverse();
        $("tbody").append(arr);//...drehe die Tabelle um...
        var richtungÄndern = false;
        if (richtung === "asc") {
            richtung = "desc";
            richtungÄndern = true;
        }
        if (richtung === "desc" && richtungÄndern === false) {
            richtung = "asc";
            richtungÄndern = true;//...und sorge dafür das die Pfeile geändert werden
        }
    } else { //Wenn sortiert wurde...
        richtung = dir; //...übergib die Sortier-Richtung
    }
    

    var zu_zeigender_Abschnitt = "0";
    var abschnittswechsel = 1;
    var abschnittsmarkierung = 0;
    var liste_der_zeilen = document.getElementsByTagName("tr");
    while (abschnittswechsel < liste_der_zeilen.length) {
        for (var j = 0; j < 15; j++) {
            
            if (liste_der_zeilen[j + abschnittswechsel] != undefined) {
                if (liste_der_zeilen[j + abschnittswechsel].style.display != "none") {
                    zu_zeigender_Abschnitt = liste_der_zeilen[j + abschnittswechsel].classList[0].substr(10);
                }
                liste_der_zeilen[j + abschnittswechsel].classList.remove(liste_der_zeilen[j + abschnittswechsel].classList[0]);
                liste_der_zeilen[j + abschnittswechsel].classList.add("Abschnitt_" + abschnittsmarkierung);
                liste_der_zeilen[j + abschnittswechsel].style.display = "none";
            }
        }
        abschnittswechsel += 15;
        abschnittsmarkierung++;
    }

    var liste_zeigen = document.getElementsByClassName("Abschnitt_" + zu_zeigender_Abschnitt);
    for (var i = 0; i < liste_zeigen.length; i++) {
        liste_zeigen[i].style.display = "table-row";
    }
    document.getElementById("Abschnitt_Boden_" + zu_zeigender_Abschnitt).style.display = "block";
    wechsel_Pfeil(Spalte, richtung);
}

function sortiere_nach_Ersteller() {
    sortiere_Tabelle(13, "SpalteErsteller");
}

function wechsel_Pfeil(spalte,dir) {
    var liste = document.getElementsByClassName("Anzeigpfeil");
    var Spalte_sortiert = document.getElementById(spalte);
    var pfeil = "";
    if (dir === "asc") {
        pfeil = "glyphicon-chevron-down";
    }
    if (dir === "desc") {
        pfeil = "glyphicon-chevron-up";
    }
    

    for (var i = 0; i < liste.length; i++) {
        liste[i].classList.remove("glyphicon-chevron-down");
        liste[i].classList.remove("glyphicon-chevron-up");
    }

    Spalte_sortiert.classList.add(pfeil);
}


function nächsten_Abschnitt_anzeigen(abschnitt) {
    var liste_zeigen = document.getElementsByClassName("Abschnitt_" + (abschnitt + 1));
    for (var i = 0; i < liste_zeigen.length; i++) {
        liste_zeigen[i].style.display = "table-row";
    }
    document.getElementById("Abschnitt_Boden_" + (abschnitt + 1)).style.display = "block";
    var liste_verstecken = document.getElementsByClassName("Abschnitt_" + abschnitt);
    for (var i = 0; i < liste_verstecken.length; i++) {
        liste_verstecken[i].style.display = "none";
    }
    document.getElementById("Abschnitt_Boden_" + abschnitt).style.display = "none";
}
function vorherigen_Abschnitt_anzeigen(abschnitt) {
    var liste_zeigen = document.getElementsByClassName("Abschnitt_" + (abschnitt - 1));
    for (var i = 0; i < liste_zeigen.length; i++) {
        liste_zeigen[i].style.display = "table-row";
    }
    document.getElementById("Abschnitt_Boden_" + (abschnitt - 1)).style.display = "block";

    var liste_verstecken = document.getElementsByClassName("Abschnitt_" + abschnitt);
    for (var i = 0; i < liste_verstecken.length; i++) {
        liste_verstecken[i].style.display = "none";
    }
    document.getElementById("Abschnitt_Boden_" + abschnitt).style.display = "none";
}

