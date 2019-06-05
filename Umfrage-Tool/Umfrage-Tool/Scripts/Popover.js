function PopoverAktivieren() {
    $(document).ready(function () {
        $('a').popover({
            delay: { show: 700 },
            trigger: 'hover focus',
            placement: 'top'
        });
        $('button').popover({
            delay: { show: 700 },
            trigger: 'hover focus',
            placement: 'top'
        });
        $('input').not(".input-range").popover({
            delay: { show: 700 },
            trigger: 'hover focus',
            placement: 'top'
        });
        $('select').popover({
            delay: { show: 700 },
            trigger: 'hover focus',
            placement: 'top'
        });
        $('.input-range').popover({
            delay: { show: 700 },
            trigger: 'hover focus',
            placement: 'top'
        });
    });
}