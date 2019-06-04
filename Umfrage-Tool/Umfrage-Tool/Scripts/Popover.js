function PopoverAktivieren() {
    $(document).ready(function () {
        //    $("a").popover({
        //        trigger: "hover",
        //        delay: { show: 700 },
        //        placement: 'top'
        //    });
        //    $("button").popover({
        //        trigger: "hover",
        //        delay: { show: 700 },
        //        placement: 'top'
        //    });
        //    $("input").not(".input-range").popover({
        //        trigger: "hover",
        //        delay: { show: 700 },
        //        placement: 'top'
        //    });
        //    $("select").popover({
        //        trigger: "hover",
        //        delay: { show: 700 },
        //        placement: 'right'
        //    });
        //    $(".input-range").popover({
        //        trigger: "hover",
        //        delay: { show: 700 },
        //        placement: 'left'
        //});

        var showPopover = function () {
            $(this).popover("show");
        };
        var hidePopover = function () {
            $(this).popover('hide');
        };

        $('a').popover({
            delay: { show: 700 },
            placement: 'top'
        }).focus(showPopover).focusout(hidePopover).hover(showPopover, hidePopover);

        $('button').popover({
            trigger: 'manual',
            delay: { show: 700 },
            placement: 'top'
        }).focus(showPopover).focusout(hidePopover).hover(showPopover, hidePopover);

        $('input').not(".input-range").popover({
            trigger: 'manual',
            delay: { show: 700 },
            placement: 'top'
        }).focus(showPopover).focusout(hidePopover).hover(showPopover, hidePopover);

        $('select').popover({
            trigger: 'manual',
            delay: { show: 700 },
            placement: 'right'
        }).focus(showPopover).focusout(hidePopover).hover(showPopover, hidePopover);

        $('.input-range').popover({
            trigger: 'manual',
            delay: { show: 700 },
            placement: 'left'
        }).focus(showPopover).focusout(hidePopover).hover(showPopover, hidePopover);

    });
}