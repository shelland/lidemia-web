var SupplierSignUp = SupplierSignUp || (function () {

    return {

        init: function (data) {
            SupplierSignUp.urls = data.urls;
            this.bindEvents();
        },

        bindEvents: function () {
            document.getElementById("saveBtn").addEventListener("click", SupplierSignUp.save);
        },

        save: async function () {

            var name = document.getElementById("name").value;
            var orgType = document.getElementById("orgType").value;
            var password = document.getElementById("password").value;
            var phone = document.getElementById("phone").value;
            var inn = document.getElementById("inn").value;
            var ogrn = document.getElementById("ogrn").value;
            var email = document.getElementById("email").value;

            var payload = {
                name: name,
                orgType: orgType,
                password: password,
                phone: phone,
                inn: inn,
                ogrn: ogrn,
                email: email
            };

            const response = await fetch(SupplierSignUp.urls.saveUrl, {
                method: "POST",
                body: JSON.stringify(payload),
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok) {

            }

            const json = await response.json();

            if (json.data.errors) {

                var textResponse = "";

                for (var err of json.data.errors) {
                    textResponse += "\n" + ClientResources[err];
                }

                alert(textResponse);

            }

        }

    }

})();