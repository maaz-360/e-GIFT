$(document).ready(function () {

    // ADD TO CART
    $(".add-to-cart").on("click", function () {

        var button = $(this);

        var productId = button.data("product-id");

        if (!productId) {

            toastr.error("Invalid product.");

            return;
        }

        button.prop("disabled", true);

        $.ajax({
            type: "POST",
            url: "/MyCart/AddToCart",

            data: {
                productId: productId
            },

            success: function (response) {

                if (response.success) {

                    toastr.success(response.message);

                } else {

                    toastr.error(response.message);
                }
            },

            error: function () {

                toastr.error(
                    "An error occurred while adding the product to cart."
                );
            },

            complete: function () {

                button.prop("disabled", false);
            }

        });

    });


    // DECREASE QUANTITY
    $(".cart-quantity-minus").on("click", function () {

        var button = $(this);

        var cartId = button.data("cart-id");
        var productId = button.data("product-id");
        var currentQuantity = parseInt(
            button.data("quantity")
        );

        if (!cartId || !productId || isNaN(currentQuantity)) {

            toastr.error("Invalid cart item.");

            return;
        }

        var newQuantity = currentQuantity - 1;

        if (newQuantity < 1) {

            toastr.warning(
                "Quantity cannot be less than 1."
            );

            return;
        }

        updateCartQuantity(
            button,
            cartId,
            productId,
            newQuantity
        );

    });


    // INCREASE QUANTITY
    $(".cart-quantity-plus").on("click", function () {

        var button = $(this);

        var cartId = button.data("cart-id");
        var productId = button.data("product-id");
        var currentQuantity = parseInt(
            button.data("quantity")
        );

        if (!cartId || !productId || isNaN(currentQuantity)) {

            toastr.error("Invalid cart item.");

            return;
        }

        var newQuantity = currentQuantity + 1;

        updateCartQuantity(
            button,
            cartId,
            productId,
            newQuantity
        );

    });


    // UPDATE CART QUANTITY
    function updateCartQuantity(
        button,
        cartId,
        productId,
        newQuantity
    ) {

        button.prop("disabled", true);

        $.ajax({

            type: "POST",

            url: "/MyCart/UpdateQuantity",

            data: {
                id: cartId,
                productId: productId,
                quantity: newQuantity
            },

            success: function (response) {

                if (response.success) {

                    // Update quantity input
                    $('.cart-quantity[data-cart-id="' + cartId + '"]')
                        .val(newQuantity);


                    // Update minus button quantity
                    $('.cart-quantity-minus[data-cart-id="' + cartId + '"]')
                        .data("quantity", newQuantity);


                    // Update plus button quantity
                    $('.cart-quantity-plus[data-cart-id="' + cartId + '"]')
                        .data("quantity", newQuantity);


                    // Get cart item
                    var cartItem =
                        button.closest(".cart-item");


                    // Get product price
                    var priceText =
                        cartItem
                            .find(".col-md-4 .mt-1")
                            .text();


                    var price =
                        parseFloat(
                            priceText
                                .replace("₹", "")
                                .replace(/,/g, "")
                                .trim()
                        );


                    if (isNaN(price)) {

                        price = 0;
                    }


                    // Calculate subtotal
                    var subtotal =
                        price * newQuantity;


                    // Update hidden net amount
                    cartItem
                        .find(".product-net-amount")
                        .val(subtotal);


                    // Update subtotal
                    cartItem
                        .find(".cart-item-subtotal")
                        .text(
                            formatCurrency(subtotal)
                        );


                    // Update totals
                    updateCartTotals();


                    toastr.success(response.message);

                } else {

                    toastr.error(response.message);
                }

            },

            error: function (xhr) {

                console.log(xhr);

                toastr.error(
                    "An error occurred while updating the cart."
                );
            },

            complete: function () {

                button.prop("disabled", false);
            }

        });

    }


    // REMOVE CART ITEM
    $(".remove-cart-item").on("click", function () {

        var button = $(this);

        var cartId = button.data("cart-id");

        if (!cartId) {

            toastr.error("Invalid cart item.");

            return;
        }

        button.prop("disabled", true);

        $.ajax({

            type: "POST",

            url: "/MyCart/RemoveItem",

            data: {
                id: cartId,
                clearAll: false
            },

            success: function (response) {

                if (response.success) {

                    // Remove item from UI
                    button
                        .closest(".col-12")
                        .remove();


                    // Update totals
                    updateCartTotals();


                    toastr.success(response.message);


                    // If cart is now empty
                    if ($(".cart-item").length === 0) {

                        location.reload();
                    }

                } else {

                    toastr.error(response.message);
                }

            },

            error: function (xhr) {

                console.log(xhr);

                toastr.error(
                    "An error occurred while removing the product."
                );
            },

            complete: function () {

                button.prop("disabled", false);
            }

        });

    });


    // CLEAR CART
    $("#clear-cart").on("click", function () {

        var button = $(this);

        // Check if cart is empty
        if ($(".cart-item").length === 0) {

            toastr.info(
                "Your cart is already empty."
            );

            return;
        }

        // Confirmation
        Swal.fire({

            title: "Clear Cart?",

            text: "Are you sure you want to clear your entire cart?",

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: "Yes, clear it",

            cancelButtonText: "Cancel"

        }).then((result) => {

            if (result.isConfirmed) {

                button.prop("disabled", true);

                $.ajax({

                    type: "POST",

                    url: "/MyCart/RemoveItem",

                    data: {
                        id: 0,
                        clearAll: true
                    },

                    success: function (response) {

                        if (response.success) {

                            toastr.success(
                                response.message
                            );

                            // Reload once
                            location.reload();

                        } else {

                            toastr.error(
                                response.message
                            );
                        }

                    },

                    error: function (xhr) {

                        console.log(xhr);

                        toastr.error(
                            "An error occurred while clearing the cart."
                        );
                    },

                    complete: function () {

                        button.prop("disabled", false);
                    }

                });
            }

        });

    });


    // RECALCULATE CART TOTALS
    function updateCartTotals() {

        var totalItems = 0;

        var totalAmount = 0;


        $(".cart-item").each(function () {

            var cartItem = $(this);


            // Quantity
            var quantity =
                parseInt(
                    cartItem
                        .find(".cart-quantity")
                        .val()
                );


            if (isNaN(quantity)) {

                quantity = 0;
            }


            // Product price
            var priceText =
                cartItem
                    .find(".col-md-4 .mt-1")
                    .text();


            var price =
                parseFloat(
                    priceText
                        .replace("₹", "")
                        .replace(/,/g, "")
                        .trim()
                );


            if (isNaN(price)) {

                price = 0;
            }


            // Calculate
            totalItems += quantity;

            totalAmount += price * quantity;

        });


        // Format total
        var formattedTotal =
            formatCurrency(totalAmount);


        // Cart total
        $("#cart-total-items")
            .text(totalItems);

        $("#cart-total-amount")
            .text(formattedTotal);


        // Order summary
        $("#summary-total-items")
            .text(totalItems);

        $("#summary-subtotal")
            .text(formattedTotal);

        $("#summary-total")
            .text(formattedTotal);

    }


    // FORMAT CURRENCY
    function formatCurrency(amount) {

        return "₹" +
            amount.toLocaleString(
                "en-IN",
                {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                }
            );
    }

});