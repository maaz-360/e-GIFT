$(document).ready(function () {

    $('#CategoryId').change(function () {

        var categoryId = $(this).val();

        $('#SubCategoryId').empty();

        $('#SubCategoryId').append(
            $('<option>', {
                value: '',
                text: '-- Select Sub Category --'
            })
        );

        $('#SubCategoryId').prop('disabled', true);

        if (!categoryId) {
            return;
        }

        $.ajax({
            url: '/Product/GetSubCategories',
            type: 'GET',
            data: {
                categoryId: categoryId
            },

            success: function (subCategories) {

                $.each(subCategories, function (index, subCategory) {

                    $('#SubCategoryId').append(
                        $('<option>', {
                            value: subCategory.id,
                            text: subCategory.subCategoryName
                        })
                    );

                });

                $('#SubCategoryId').prop('disabled', false);
            },

            error: function () {

                alert('Unable to load sub categories.');

            }
        });

    });


    // Initial page load
    var categoryId = $('#CategoryId').val();

    if (categoryId) {
        $('#SubCategoryId').prop('disabled', false);
    }
    else {
        $('#SubCategoryId').prop('disabled', true);
    }

});