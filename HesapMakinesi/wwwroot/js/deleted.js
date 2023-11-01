$(".deleteAction").click(function () {
    const id = $(this).attr("data-id");
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu işlem geri alınamaz!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'Vazgeç'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                url: '/Admin?handler=Deleted',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(id),
                success: function (result) {
                    if (result.isSuccess) {
                        Swal.fire(
                            'Silindi!',
                            result.message,
                            'success'
                        ).then(() => {
                            setTimeout(function () {
                                location.reload();
                            }, 1);
                        });

                    } else {

                        Swal.fire(
                            'Silinemedi!',
                            result.message,
                            'error'
                        ).then(() => {
                            setTimeout(function () {
                                location.reload();
                            }, 1);
                        });
                    }
                },
                error: function () {
                    Swal.fire(
                        'Hata!',
                        'İşlem sırasında bir hata oluştu.',
                        'error'
                    );
                }
            });
        }
    });
});