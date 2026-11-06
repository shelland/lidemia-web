var CustomerSignIn = CustomerSignIn || (function () {

    return {

        init: function (data) {
            CustomerSignIn.urls = data.urls;
            this.bindEvents();
        },

        bindEvents: function () {
            document.querySelector("#signInBtn").addEventListener("click", CustomerSignIn.signIn);
        },

        signIn: async function () {

            var email = document.querySelector("#email").value;
            var password = document.querySelector("#password").value;

            var payload = {
                email: email,
                password: password
            }

            const response = await window.appFetch(CustomerSignIn.urls.signInUrl, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {

            }

            var result = await response.json();

            if (result.isSuccess) {
                window.location = CustomerSignIn.urls.returnUrl;
            }
            else {
                CustomerSignIn.showErrors(result.data.errors);
            }

        }

    }

})();