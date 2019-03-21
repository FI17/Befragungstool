function CheckBox_aktivieren_Funktion(beantwortungstext) {    
    $("#Check_" + beantwortungstext).prop('checked', true);
}

function Sonstiges_eintragen(sonstigesfeld, text) {
    $("#Sonst_" + sonstigesfeld).val = text;
}