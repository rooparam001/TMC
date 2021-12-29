$(document).ready(function () {
    $("#contactForm").on("submit", function (event) {
        event.preventDefault();

        var formValues = $(this).serialize();

        $.post("process_form.php", formValues, function (data) {
            // Display the returned data in browser
            $("#result").html(data);
        });
    });
});