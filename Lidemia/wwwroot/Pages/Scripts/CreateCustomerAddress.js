var CreateCustomerAddress = CreateCustomerAddress || (function () {

    return {

        init: function (data) {
            CreateCustomerAddress.urls = data.urls;
            CreateCustomerAddress.values = data.values;

            this.bindEvents();
        },

        bindEvents: function () {
            document.getElementById("region").addEventListener("change", function () {
                CreateCustomerAddress.filterCities(this.value);
            });

            CreateCustomerAddress.filterCities(document.getElementById("region").value, CreateCustomerAddress.values.currentCity);

            document.getElementById("saveBtn").addEventListener("click", function () {
                CreateCustomerAddress.save();
            });

        },

        filterCities: function (regionId, selected) {

            var cities = _.filter(CreateCustomerAddress.values.cities, function (e) {
                return e.regionId == regionId;
            })

            document.getElementById("city").innerHTML = "";

            const selectBox = document.getElementById("city");
            selectBox.innerHTML = cities.map(item => `<option value="${item.id}">${item.name}</option>`).join("");

            if (selected) {
                selectBox.value = selected;
            }
        },

        save: async function () {

            var payload = {
                countryId: document.getElementById("country").value,
                regionId: document.getElementById("region").value,
                cityId: document.getElementById("city").value,
                streetName: document.getElementById("streetName").value,
                houseNumber: document.getElementById("houseNumber").value,
                flatNumber: document.getElementById("flatNumber").value,
                comment: document.getElementById("comment").value
            };

            if (CreateCustomerAddress.values.address) {
                payload.id = CreateCustomerAddress.values.address;
            }

            const response = await window.appFetch(CreateCustomerAddress.urls.saveUrl, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {

            }

            window.location = CreateCustomerAddress.urls.returnUrl;

        }

    }

})();