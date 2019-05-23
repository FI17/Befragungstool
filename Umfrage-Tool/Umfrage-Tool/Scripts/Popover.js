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
        $("input").not(".input-range").popover({
            trigger: "hover",
            delay: { show: 700 },
            placement: 'top'
        });
        $("select").popover({
            trigger: "hover",
            delay: { show: 700 },
            placement: 'right'
        });
        $(".input-range").popover({
            trigger: "hover",
            delay: { show: 700 },
            placement: 'left'
        });
    });
}