$("#logoutButton").click(function () {
    $.ajax({
        type: 'POST',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        url: '/Auth/Logout',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            window.location.href = '/Auth/Login';
        },
        error: function () {
            window.location.reload();
        }
    });
});