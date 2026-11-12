var CreateSupplierAddress = CreateSupplierAddress || (function () {

    return {

        init: function (data) {
            CreateSupplierAddress.urls = data.urls;
            CreateSupplierAddress.values = data.values;

            this.bindEvents();
        },

        bindEvents: function () {
            document.getElementById("region").addEventListener("change", function () {
                CreateSupplierAddress.filterCities(this.value);
            });

            CreateSupplierAddress.filterCities(document.getElementById("region").value, CreateSupplierAddress.values.currentCity);

            document.getElementById("saveBtn").addEventListener("click", function () {
                CreateSupplierAddress.save();
            });

        },

        filterCities: function (regionId, selected) {

            var cities = _.filter(CreateSupplierAddress.values.cities, function (e) {
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

            if (CreateSupplierAddress.values.address) {
                payload.id = CreateSupplierAddress.values.address;
            }

            const response = await window.appFetch(CreateSupplierAddress.urls.saveUrl, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {

            }

            window.location = CreateSupplierAddress.urls.returnUrl;
        }

    }

})();