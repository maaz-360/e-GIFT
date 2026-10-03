$(document).ready(function () {

    $('#CountryId').change(function () {

        var countryId = $(this).val();

        $('#StateId').empty();
        $('#CityId').empty();

        $('#StateId').append(
            $('<option>', {
                value: '',
                text: '-- Select State --'
            })
        );

        $('#CityId').append(
            $('<option>', {
                value: '',
                text: '-- Select City --'
            })
        );

        $('#StateId').prop('disabled', true);
        $('#CityId').prop('disabled', true);

        if (!countryId) {
            return;
        }

        $.ajax({
            url: '/Address/GetStates',
            type: 'GET',
            data: {
                countryId: countryId
            },

            success: function (states) {

                $.each(states, function (index, state) {

                    $('#StateId').append(
                        $('<option>', {
                            value: state.id,
                            text: state.stateName
                        })
                    );

                });

                $('#StateId').prop('disabled', false);
            },

            error: function () {

                alert('Unable to load states.');

            }
        });

    });


    $('#StateId').change(function () {

        var stateId = $(this).val();

        $('#CityId').empty();

        $('#CityId').append(
            $('<option>', {
                value: '',
                text: '-- Select City --'
            })
        );

        $('#CityId').prop('disabled', true);

        if (!stateId) {
            return;
        }

        $.ajax({
            url: '/Address/GetCities',
            type: 'GET',
            data: {
                stateId: stateId
            },

            success: function (cities) {

                $.each(cities, function (index, city) {

                    $('#CityId').append(
                        $('<option>', {
                            value: city.id,
                            text: city.cityName
                        })
                    );

                });

                $('#CityId').prop('disabled', false);
            },

            error: function () {

                alert('Unable to load cities.');

            }
        });

    });


    // Initial page load
    var countryId = $('#CountryId').val();
    var stateId = $('#StateId').val();

    if (countryId) {
        $('#StateId').prop('disabled', false);
    }
    else {
        $('#StateId').prop('disabled', true);
    }

    if (stateId) {
        $('#CityId').prop('disabled', false);
    }
    else {
        $('#CityId').prop('disabled', true);
    }

});