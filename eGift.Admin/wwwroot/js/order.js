$(document).ready(function () {

    $("#btnUpdateStatus").click(function () {

        var orderId = $("#OrderId").val();
        var statusId = $("#StatusId").val();

        if (!statusId) {
            toastr.error("Please select a status.");
            return;
        }

        $.ajax({
            type: "POST",
            url: "/Order/UpdateStatus",
            data: {
                orderId: orderId,
                statusId: statusId
            },
            success: function (response) {

                if (response.success) {
                    toastr.success(response.message);
                }
                else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error("An error occurred while updating the order status.");
            }
        });

    });

});