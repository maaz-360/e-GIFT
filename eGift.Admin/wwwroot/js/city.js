$(document).ready(function () {

    function loadStates(countryId, selectedStateId = null) {

        var stateDropdown = $('#StateId');

        stateDropdown.empty();

        stateDropdown.append(
            '<option value="">-- Select State --</option>'
        );

        if (!countryId) {
            stateDropdown.prop('disabled', true);
            return;
        }

        $.ajax({
            url: '/City/GetStates',
            type: 'GET',
            data: {
                countryId: countryId
            },

            success: function (states) {

                $.each(states, function (index, state) {

                    var option = $('<option></option>')
                        .val(state.id)
                        .text(state.stateName);

                    if (selectedStateId &&
                        state.id == selectedStateId) {

                        option.prop('selected', true);
                    }

                    stateDropdown.append(option);
                });

                stateDropdown.prop('disabled', false);
            },

            error: function () {

                toastr.error('Unable to load states.');

                stateDropdown.prop('disabled', true);
            }
        });
    }


    // Country changed manually
    $('#CountryId').change(function () {

        var countryId = $(this).val();

        // When user changes country, don't keep old state
        loadStates(countryId);
    });


    // Page loaded
    var countryId = $('#CountryId').val();

    var selectedStateId = $('#StateId').val();

    if (countryId) {

        loadStates(
            countryId,
            selectedStateId
        );

    }
    else {

        $('#StateId').prop('disabled', true);

    }

});