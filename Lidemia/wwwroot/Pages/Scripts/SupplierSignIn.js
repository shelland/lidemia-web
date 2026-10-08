var SupplierSignIn = SupplierSignIn || (function () {

    return {

        init: function (data) {
            this.bindEvents();
            SupplierSignIn.urls = data.urls;
        },

        bindEvents: function () {
            document.getElementById("signInBtn").addEventListener("click", function () {
                SupplierSignIn.trySignIn();
            });
        },

        trySignIn: async function () {
            var email = document.getElementById("email").value;
            var password = document.getElementById("password").value;

            var payload = {
                email: email,
                password: password
            };

            /* const response = await fetch(SupplierSignUp.urls.saveUrl, {
                method: "POST",
                body: JSON.stringify(payload),
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok) {
                const json = await response.json();

                if (json.data.errors) {
                    SupplierSignUp.showErrors(json.data.errors);
                }

                return;
            }*/

            const response = await window.appFetch(SupplierSignIn.urls.signInUrl, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {

            }

            var result = await response.json();

            if (result.isSuccess) {
                window.location = SupplierSignIn.urls.returnUrl;
            }
            else {
                SupplierSignIn.showErrors(result.data.errors);
            }

            // debugger;

            /* const response = await fetch(SupplierSignIn.urls.signInUrl, {
                method: "POST",
                body: JSON.stringify(payload),
                headers: {
                    "Content-Type": "application/json"
                }
            }) */


        },

        showErrors: function (errors) {

        }

    }

})();