 document
    .getElementById("profileImage")
    .addEventListener("change", function (event) {

                const file = event.target.files[0];

    if (!file) {
                    return;
                }

    const reader = new FileReader();

    reader.onload = function (e) {

                    const preview =
    document.getElementById("profilePreview");

    const placeholder =
    document.getElementById("profilePlaceholder");

    preview.src = e.target.result;
    preview.style.display = "block";

    if (placeholder) {
        placeholder.style.display = "none";
                    }
                };

    reader.readAsDataURL(file);
            });

    // Country → State → City

    document
    .getElementById("CountryId")
    .addEventListener("change", function () {

                const countryId = this.value;

    const stateDropdown =
    document.getElementById("StateId");

    const cityDropdown =
    document.getElementById("CityId");


    stateDropdown.innerHTML =
    '<option value="">Select State</option>';

    cityDropdown.innerHTML =
    '<option value="">Select City</option>';


    if (!countryId) {
                    return;
                }


    fetch(`/Customer/GetStates?countryId=${ countryId }`)
    .then(function (res) {
                        return res.json();
                    })
    .then(function (states) {

        states.forEach(function (state) {

            const opt =
                document.createElement("option");

            opt.value = state.id;
            opt.textContent = state.stateName;

            stateDropdown.appendChild(opt);
        });
                    })
    .catch(function () {
        alert("Unable to load states.");
                    });
            });


    document
    .getElementById("StateId")
    .addEventListener("change", function () {

                const stateId = this.value;

    const cityDropdown =
    document.getElementById("CityId");


    cityDropdown.innerHTML =
    '<option value="">Select City</option>';


    if (!stateId) {
                    return;
                }


    fetch(`/Customer/GetCities?stateId=${ stateId }`)
    .then(function (res) {
                        return res.json();
                    })
    .then(function (cities) {

        cities.forEach(function (city) {

            const opt =
                document.createElement("option");

            opt.value = city.id;
            opt.textContent = city.cityName;

            cityDropdown.appendChild(opt);
        });
                    })
    .catch(function () {
        alert("Unable to load cities.");
                    });
            });
