$(document).ready(function () {

    var num = 1;

    $("#Another").click(function () {

        num++;

        var input = $("<input>");
        input.addClass("form-control text-box single-line");
        input.attr("type", "text").attr("id", "Section" + num);
        input.attr("name", "Section[]").attr("placeholder", "Put your Section Name Here");

        var div = $("<div>");
        div.addClass("col-md-6 col-md-offset-2");
        div.append(input);

        var div2 = $("<div>")
        div2.addClass("form-group");
        div2.append(div);

        $("#button").before(div2);

    });

    var previous;

    $("[name='Order_value']").on('click', function () {
        // Store the current value on focus and on change
        previous = this.value;
    }).change(function () {
        
        var id = $(this).data('section');
        var mod = $(this).data('module');
        var prev = previous;
        var next = this.value;
        console.log(id + "-" + mod + "-" + prev + "-" + next);
        ChangeOrder(id,prev,next,mod);
    });

    function ChangeOrder(id, Old, New, mod) {

        $.ajax({
            method: "POST",
            url: "/Section/Change",
            data: {
                id: id,
                old_Order: Old,
                new_Order: New,
                Module_id: mod
            },
            dataType: 'json'
        })
        .done(function (data) {
            if (data.Section_id != null) {
                $("[name='Order_value']").filter("[data-section='" + data.Section_id + "']").filter("[data-module='" + mod + "']").val(Old);
            }
        })
        .fail(function (data) {
            alert("Internal Server Error 500");
        });

    }

});