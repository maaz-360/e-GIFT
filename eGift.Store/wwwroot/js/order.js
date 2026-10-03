$(document).ready(function () {

    // PLACE ORDER
    $(".place-order").on("click", function () {

        var button = $(this);

        var cartItems = [];

        $(".cart-items .cart-item").each(function () {

            var cartItem = $(this);

            var productId = parseInt(
                cartItem.find(".product-id").val()
            );

            var quantity = parseInt(
                cartItem.find(".product-quantity").val()
            );

            var productPrice = parseFloat(
                cartItem.find(".product-price").val()
            );

            var netAmount = parseFloat(
                cartItem.find(".product-net-amount").val()
            );

        

            cartItems.push({
                ProductId: productId,
                UnitPrice: productPrice,
                Quantity: quantity,
                NetAmount: netAmount
            });
        });

        console.log("Cart Items:", cartItems);

        if (cartItems.length === 0) {

            toastr.warning("Your cart is empty.");

            return;
        }

        button.prop("disabled", true);

        $.ajax({
            type: "POST",
            url: "/Order/PlaceOrder",

            data: {
                Items: cartItems
            },

            success: function (response) {

                if (response.success) {

                    toastr.success(response.message);

                    // Go to My Orders after successful order
                    setTimeout(function () {
                        window.location.href = "/Order/MyOrder";
                    }, 1000);

                } else {

                    toastr.error(response.message);
                }
            },

            error: function (xhr) {

                console.log(xhr);

                toastr.error(
                    "An error occurred while placing the order."
                );
            },

            complete: function () {

                button.prop("disabled", false);
            }
        });

    });


    // CANCEL ORDER
    $(".cancel-order").on("click", function () {

        var button = $(this);

        var orderId = button.data("order-id");
        var orderNumber = button.data("order-number");

        if (!orderId) {

            toastr.error("Invalid order.");

            return;
        }

        // Confirmation
        Swal.fire({
            title: "Cancel Order?",
            text: "Are you sure you want to cancel order " + orderNumber + "?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, cancel it",
            cancelButtonText: "No"
        }).then((result) => {

            if (result.isConfirmed) {

                button.prop("disabled", true);

                $.ajax({

                    type: "POST",

                    url: "/Order/CancelOrder",

                    data: {
                        id: orderId
                    },

                    success: function (response) {

                        if (response.success) {

                            toastr.success(response.message);

                            // Reload MyOrder page
                            location.reload();

                        } else {

                            toastr.error(response.message);
                        }
                    },

                    error: function (xhr) {

                        console.log(xhr);

                        toastr.error(
                            "An error occurred while cancelling the order."
                        );
                    },

                    complete: function () {

                        button.prop("disabled", false);
                    }
                });
            }
        });

    });

});