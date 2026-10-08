var SupplierSignUp = SupplierSignUp || (function () {

    var requiredFields = [
        { id: "name", errorPrefix: "errors.supplier.signup.name" },
        { id: "email", errorPrefix: "errors.supplier.signup.email", isEmail: true },
        { id: "password", errorPrefix: "errors.supplier.signup.password" },
        { id: "phone", errorPrefix: "errors.supplier.signup.phone" },
        { id: "inn", errorPrefix: "errors.supplier.signup.inn" },
        { id: "ogrn", errorPrefix: "errors.supplier.signup.ogrn" },
        { id: "orgType", errorPrefix: "errors.supplier.signup.orgType" }
    ];

    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    return {

        init: function (data) {
            SupplierSignUp.urls = data.urls;
            this.bindEvents();
        },

        bindEvents: function () {
            document.getElementById("saveBtn").addEventListener("click", SupplierSignUp.save);

            for (var field of requiredFields) {
                var element = document.getElementById(field.id);

                if (element) {
                    var clearInvalidState = function () {
                        this.classList.remove("is-invalid");
                    };

                    element.addEventListener("input", clearInvalidState);
                    element.addEventListener("change", clearInvalidState);
                }
            }
        },

        validate: function () {
            var errors = [];

            for (var field of requiredFields) {
                var element = document.getElementById(field.id);

                if (!element) {
                    continue;
                }

                var value = (element.value || "").trim();
                var fieldErrors = [];

                if (!value) {
                    fieldErrors.push(field.errorPrefix + "Empty");
                } else {
                    var minLength = parseInt(element.getAttribute("minlength"), 10);

                    if (!isNaN(minLength) && value.length < minLength) {
                        fieldErrors.push(field.errorPrefix + "TooShort");
                    }

                    var maxLength = parseInt(element.getAttribute("maxlength"), 10);

                    if (!isNaN(maxLength) && value.length > maxLength) {
                        fieldErrors.push(field.errorPrefix + "TooLong");
                    }

                    if (field.isEmail && !emailPattern.test(value)) {
                        fieldErrors.push(field.errorPrefix + "Invalid");
                    }
                }

                SupplierSignUp.setFieldValidity(element, fieldErrors.length === 0);
                errors = errors.concat(fieldErrors);
            }

            return errors;
        },

        setFieldValidity: function (element, isValid) {
            element.classList.toggle("is-invalid", !isValid);
        },

        getErrorMessage: function (errorCode) {
            if (typeof ClientResources !== "undefined" && ClientResources[errorCode]) {
                return ClientResources[errorCode];
            }

            return errorCode;
        },

        showErrors: function (errors) {
            var messages = [];

            for (var errorCode of errors) {
                messages.push(SupplierSignUp.getErrorMessage(errorCode));
            }

            alert(messages.join("\n"));
        },

        save: async function () {

            var errors = SupplierSignUp.validate();

            if (errors.length > 0) {
                SupplierSignUp.showErrors(errors);
                return;
            }

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
                const json = await response.json();

                if (json.data.errors) {
                    SupplierSignUp.showErrors(json.data.errors);
                }

                return;
            }

            window.location = SupplierSignUp.urls.welcomeUrl;          

        }

    }

})();
