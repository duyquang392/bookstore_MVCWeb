var oder = {
    init: function () {
        oder.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Order/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Hoàn thành');
                    }
                    else {
                        btn.text('Chưa hoàn thành');
                    }
                }
            });
        });
    }
}
oder.init();