var Checkout = Checkout || (function () {

    return {

        init: function (data) {
            Checkout.urls = data.urls;
            this.bindEvents();
        },

        bindEvents: function () {
            document.querySelector("#saveBtn").addEventListener("click", function () {
                Checkout.save();
            });
        },

        save: async function () {

            const selectItem = document.querySelector("#customerAddress");

            if (!selectItem) {
                alert('HRESULT_NO_CUSTOMER_ADDRESS');
                return;
            }

            var payload = {
                customerAddressId: selectItem.value
            };

            const items = [];

            for (const row of document.querySelectorAll("select.supplier-address")) {
                const supplierId = row.dataset.supplierId;
                const addressId = row.value;

                items.push({ supplierId: supplierId, supplierAddressId: addressId });
            }

            payload.items = items;

            const response = await window.appFetch(Checkout.urls.checkoutUrl, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {

            }

            const data = await response.json();
            const orderId = data.data;

            window.location = Checkout.urls.finishUrl + "/" + orderId;
        },

        verifyCart: async function () {

            var payload = {
                customerAddressId: -1
            };

            const response = await window.appFetch(Checkout.urls.verifyCartUrl, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {

            }

            const data = await response.json();

            if (data.data.missingProducts.length === 0) {

            }

        }

    }

})();