var CustomerSignUp = CustomerSignUp || (function () {

    return {

        init: function (data) {
            CustomerSignUp.urls = data.urls;
            this.bindEvents();
        },

        bindEvents: function () {
            document.querySelector("#saveBtn").addEventListener("click", CustomerSignUp.signUp);
        },

        signUp: async function () {

            var errors = CustomerSignUp.validate();

            if (errors.length > 0) {
                CustomerSignUp.showErrors(errors);
                return;
            }

            var firstName = document.querySelector("#firstName").value;
            var lastName = document.querySelector("#lastName").value;
            var email = document.querySelector("#email").value;
            var password = document.querySelector("#password").value;

            var payload = {
                firstName: firstName,
                lastName: lastName,
                email: email,
                password: password
            };

            const response = await fetch(CustomerSignUp.urls.signUpUrl, {
                method: "POST",
                body: JSON.stringify(payload),
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok) {
                const json = await response.json();

                if (json.data.errors) {
                    CustomerSignUp.showErrors(json.data.errors);
                }

                return;
            }

            window.location = CustomerSignUp.urls.welcomeUrl;
        },

        validate: function () {
            return true;
        },

        showErrors: function (errors) {
            var messages = [];

            for (var errorCode of errors) {
                messages.push(CustomerSignUp.getErrorMessage(errorCode));
            }

            alert(messages.join("\n"));
        },

        getErrorMessage: function (errorCode) {
            if (typeof ClientResources !== "undefined" && ClientResources[errorCode]) {
                return ClientResources[errorCode];
            }

            return errorCode;
        },

    }

})();