var SupplierProfileLogo = SupplierProfileLogo || (function () {

    var allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"];

    return {

        init: function (data) {
            SupplierProfileLogo.urls = data.urls;
            SupplierProfileLogo.labels = data.labels || {};
            SupplierProfileLogo.file = null;
            SupplierProfileLogo.previewUrl = null;

            this.bindEvents();
        },

        bindEvents: function () {
            document.getElementById("uploadLogoBtn").addEventListener("click", function () {
                SupplierProfileLogo.reset();
                SupplierProfileLogo.showModal();
            });

            document.getElementById("logoFile").addEventListener("change", function () {
                SupplierProfileLogo.selectFile(this.files && this.files.length ? this.files[0] : null);
            });

            document.getElementById("logoUploadBtn").addEventListener("click", function () {
                SupplierProfileLogo.upload();
            });
        },

        showModal: function () {
            var modalElement = document.getElementById("logoUploadModal");

            if (!modalElement || typeof bootstrap === "undefined") {
                return;
            }

            bootstrap.Modal.getOrCreateInstance(modalElement).show();
        },

        hideModal: function () {
            var modalElement = document.getElementById("logoUploadModal");

            if (!modalElement || typeof bootstrap === "undefined") {
                return;
            }

            bootstrap.Modal.getOrCreateInstance(modalElement).hide();
        },

        reset: function () {
            SupplierProfileLogo.file = null;

            if (SupplierProfileLogo.previewUrl) {
                URL.revokeObjectURL(SupplierProfileLogo.previewUrl);
                SupplierProfileLogo.previewUrl = null;
            }

            document.getElementById("logoFile").value = "";
            document.getElementById("logoFile").classList.remove("is-invalid");

            var preview = document.getElementById("logoPreview");

            if (preview) {
                preview.removeAttribute("src");
                preview.classList.add("d-none");
            }

            var placeholder = document.getElementById("logoPreviewPlaceholder");

            if (placeholder) {
                placeholder.classList.remove("d-none");
            }

            document.getElementById("logoUploadBtn").disabled = true;
        },

        selectFile: function (file) {
            var fileInput = document.getElementById("logoFile");

            if (!file) {
                SupplierProfileLogo.reset();
                return;
            }

            var extension = SupplierProfileLogo.getExtension(file.name);

            if (allowedExtensions.indexOf(extension) === -1) {
                SupplierProfileLogo.reset();
                fileInput.classList.add("is-invalid");
                SupplierProfileLogo.showErrors(["errors.supplier.logo.unsupportedFormat"]);
                return;
            }

            SupplierProfileLogo.file = file;

            if (SupplierProfileLogo.previewUrl) {
                URL.revokeObjectURL(SupplierProfileLogo.previewUrl);
            }

            SupplierProfileLogo.previewUrl = URL.createObjectURL(file);

            var preview = document.getElementById("logoPreview");

            if (preview) {
                preview.src = SupplierProfileLogo.previewUrl;
                preview.classList.remove("d-none");
            }

            var placeholder = document.getElementById("logoPreviewPlaceholder");

            if (placeholder) {
                placeholder.classList.add("d-none");
            }

            document.getElementById("logoUploadBtn").disabled = false;
        },

        getExtension: function (name) {
            var index = (name || "").lastIndexOf(".");

            return index === -1 ? "" : name.substring(index).toLowerCase();
        },

        upload: async function () {

            if (!SupplierProfileLogo.file) {
                SupplierProfileLogo.showErrors(["errors.supplier.logo.fileEmpty"]);
                return;
            }

            var formData = new FormData();
            formData.append("file", SupplierProfileLogo.file);

            var result = null;

            Shared.setLoading(true);

            try {
                const response = await window.appFetch(SupplierProfileLogo.urls.uploadLogoUrl, {
                    method: "POST",
                    body: formData
                });

                if (response.ok) {
                    result = await response.json();
                }
            }
            finally {
                Shared.setLoading(false);
            }

            if (!result) {
                return;
            }

            if (!result.isSuccess) {
                SupplierProfileLogo.showErrors(result.data && result.data.errors ? result.data.errors : []);
                return;
            }

            SupplierProfileLogo.reset();
            SupplierProfileLogo.hideModal();
            SupplierProfileLogo.setProcessingState();
        },

        setProcessingState: function () {
            var container = document.getElementById("currentLogo");

            if (container) {
                container.classList.add("d-none");
            }

            var status = document.getElementById("logoProcessingAlert");
            var label = SupplierProfileLogo.labels.processing;

            if (!status || !label) {
                return;
            }

            status.textContent = label;
            status.classList.remove("d-none");
        },

        getErrorMessage: function (errorCode) {
            if (typeof ClientResources !== "undefined" && ClientResources[errorCode]) {
                return ClientResources[errorCode];
            }

            return errorCode;
        },

        showErrors: function (errors) {
            var messages = [];

            for (var errorCode of errors || []) {
                messages.push(SupplierProfileLogo.getErrorMessage(errorCode));
            }

            if (messages.length) {
                alert(messages.join("\n"));
            }
        }

    }

})();
