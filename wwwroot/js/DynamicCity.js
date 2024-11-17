
const locationData = {
    "India": {
        "TamilNadu": ["Chennai","Madurai","Tirichy","Thirunelveli","Kanyakumari"],
        "Kerala": ["Thiruvananthapuram","Kochi","Palakad","Ernakulam"]
    },
    "USA": {
        "Alabama": ["Birmingham", "Montgomery", "Huntsville"],
        "Alaska":["Anchorage","Fairbanks","Juneau","Sitka"]
    }
}

function updateState()
{
    const countrySelect = document.getElementById("country");
    const stateSelect = document.getElementById("state");
    const citySelect = document.getElementById("city");
    const selectedCountry = countrySelect.value;
    //clear existing options
    stateSelect.innerHTML = '<option value="value" disabled selected>Select State</option>';
    citySelect.innerHTML = '<option value="value" disabled selected>Select City</option>';

    if (selectedCountry in locationData)
    {
        for (const states in locationData[selectedCountry]) {
            const option = document.createElement("option");
            option.value = states;
            option.text = states;
            stateSelect.add(option);
        }
    }

}

function updateCities()
{
    const countrySelect = document.getElementById("country");
    const stateSelect = document.getElementById("state");
    const citySelect = document.getElementById("city");
    const selectedCountry = countrySelect.value;
    const selectedState = stateSelect.value;
    //clear existing options
    citySelect.innerHTML = '<option value="value" disabled selected>Select City</option>';
    if (selectedCountry in locationData && selectedState in locationData[selectedCountry]) {
        for (const cities of locationData[selectedCountry][selectedState]) {

            const option = document.createElement("option");
            option.value = cities;
            option.text = cities;
            citySelect.add(option);

        }
    }

}