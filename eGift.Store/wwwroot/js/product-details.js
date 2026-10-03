$(document).ready(function () {

    var $thumbnails = $(".product-thumbnail");
    var $mainImage = $("#mainProductImage");
    var $dots = $(".pdp-dot");

    var currentIndex = 0;
    var totalImages = $thumbnails.length;


    // -----------------------------
    // IMAGE SLIDER
    // -----------------------------

    function goToImage(index) {

        if (totalImages === 0) {
            return;
        }

        if (index < 0) {
            index = totalImages - 1;
        }
        else if (index >= totalImages) {
            index = 0;
        }

        currentIndex = index;

        var $target = $thumbnails.eq(currentIndex);
        var imageUrl = $target.data("image");

        if (imageUrl) {
            $mainImage.attr("src", imageUrl);
        }

        $thumbnails.removeClass("active");
        $target.addClass("active");

        $dots.removeClass("active");
        $dots.eq(currentIndex).addClass("active");
    }


    // -----------------------------
    // AMAZON STYLE IMAGE ZOOM
    // -----------------------------

    var $zoomContainer = $("#mainImageZoom");
    var $zoomImage = $("#mainProductImage");

    $zoomContainer.on("mousemove", function (e) {

        var rect = this.getBoundingClientRect();

        var x = e.clientX - rect.left;
        var y = e.clientY - rect.top;

        var xPercent = (x / rect.width) * 100;
        var yPercent = (y / rect.height) * 100;

        $zoomImage.css({
            "transform": "scale(2)",
            "transform-origin": xPercent + "% " + yPercent + "%"
        });

    });

    $zoomContainer.on("mouseleave", function () {

        $zoomImage.css({
            "transform": "scale(1)",
            "transform-origin": "center center"
        });

    });


    // -----------------------------
    // THUMBNAIL CLICK
    // -----------------------------

    $thumbnails.on("click", function () {

        var index = $(this).data("index");

        goToImage(index);

    });


    // -----------------------------
    // DOT CLICK
    // -----------------------------

    $dots.on("click", function () {

        var index = $(this).data("index");

        goToImage(index);

    });


    // -----------------------------
    // PREVIOUS IMAGE
    // -----------------------------

    $("#prevImage").on("click", function () {

        goToImage(currentIndex - 1);

    });


    // -----------------------------
    // NEXT IMAGE
    // -----------------------------

    $("#nextImage").on("click", function () {

        goToImage(currentIndex + 1);

    });


    // -----------------------------
    // KEYBOARD LEFT / RIGHT
    // -----------------------------

    $(document).on("keydown", function (e) {

        if (totalImages <= 1) {
            return;
        }

        if (e.key === "ArrowLeft") {

            goToImage(currentIndex - 1);

        }
        else if (e.key === "ArrowRight") {

            goToImage(currentIndex + 1);

        }

    });


    // -----------------------------
    // WISHLIST TOGGLE
    // -----------------------------

    $("#wishlistBtn").on("click", function () {

        $(this).toggleClass("active");

    });


    // -----------------------------
    // QUANTITY STEPPER
    // -----------------------------

    var maxQty =
        parseInt($("#qtyValue").data("max-stock"), 10) || 1;


    function getQty() {

        return parseInt($("#qtyValue").val(), 10) || 1;

    }


    function setQty(value) {

        if (value < 1) {

            value = 1;

        }

        if (value > maxQty) {

            value = maxQty;

        }

        $("#qtyValue").val(value);

    }


    $("#qtyMinus").on("click", function () {

        setQty(getQty() - 1);

    });


    $("#qtyPlus").on("click", function () {

        setQty(getQty() + 1);

    });


    // -----------------------------
    // ADD TO CART
    // -----------------------------

    $(".add-to-cart").on("click", function () {

        var $button = $(this);

        var productId = $button.data("product-id");

        var qtySource = $button.data("qty-source");

        var quantity =
            parseInt($(qtySource).val(), 10) || 1;


        if (!productId) {

            toastr.error("Invalid product.");

            return;

        }


        if (quantity < 1) {

            toastr.error("Quantity must be at least 1.");

            return;

        }


        // Prevent multiple clicks

        $button.prop("disabled", true);


        $.ajax({

            type: "POST",

            url: "/MyCart/AddToCart",

            data: {

                productId: productId,

                quantity: quantity

            },


            success: function (response) {

                if (response.success) {

                    toastr.success(response.message);

                }
                else {

                    toastr.error(response.message);

                }

            },


            error: function (xhr) {

                console.log(xhr);

                toastr.error(
                    "An error occurred while adding the product to cart."
                );

            },


            complete: function () {

                $button.prop("disabled", false);

            }

        });

    });

});