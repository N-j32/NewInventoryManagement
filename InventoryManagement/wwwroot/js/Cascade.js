$(document).ready(function () {
    GetCountry;
})
function GetCountry() {
    $.ajax({
        url: "/Cascade/Country",
        success: function (result) {
            $.each(result, function (i, data) {
                $("#Country").append('<Option value=' + data.id + '>' + data.name + '</Option>')
            });
        }
    });
}