function PopoverAktivieren() {
    $(document).ready(function () {
        $("a").popover({
            trigger: "hover",
            delay: { show: 700 },
            placement: 'top'
        });
        $("button").popover({
            trigger: "hover",
            delay: { show: 700 },
            placement: 'top'
        });
        $("input").popover({
            trigger: "hover",
            delay: { show: 700 },
            placement: 'top'
        });
        $("select").popover({
            trigger: "hover",
            delay: { show: 700 },
            placement: 'right'
        });
    });
}

function PopoverSkala() {
    $("#Ska_Obj").popover({
        trigger: "hover",
        delay: { show: 700 },
        placement: 'left'
    });
}