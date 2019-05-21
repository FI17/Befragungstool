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
    });
}