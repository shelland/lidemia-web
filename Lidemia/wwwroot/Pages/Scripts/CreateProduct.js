var CreateProduct = CreateProduct || (function () {

	return {

		init: function (data) {
			CreateProduct.urls = data.urls;
			CreateProduct.values = data.values;
			CreateProduct.values.images = [];
			CreateProduct.values.previewUrls = [];
			CreateProduct.values.labels = data.labels || {};

			this.bindEvents();
            this.initPlugins();

            if (CreateProduct.values.product) {
                this.loadProduct();
            }
		},

        bindEvents: function () {
            document.getElementById("saveBtn").addEventListener("click", function () {
                CreateProduct.save();
            });

            document.getElementById("images").addEventListener("change", function () {
                CreateProduct.addImages(this.files);
                this.value = "";
            });
        },

        loadProduct: function () {
            var product = CreateProduct.values.product;

            CreateProduct.values.tagify.addTags(product.tags);
        },

		initPlugins: function () {
			var regExp = Shared.getHashTagRegExp();

			var tagify = new Tagify(document.getElementById("tags"), {
				pattern: regExp
            });

            CreateProduct.values.tagify = tagify;

            this.initEditor();
        },

        initEditor: function () {
            Wysi({
                el: '#descriptionEditor'
            });
        },

        addImages: function (files) {
            if (!files || !files.length) {
                return;
            }

            Array.prototype.forEach.call(files, function (file) {
                if (file.type.indexOf("image/") !== 0) {
                    return;
                }

                CreateProduct.values.images.push(file);
            });

            this.renderImagesPreview();
        },

        removeImage: function (index) {
            this.closeImagePreview();
            CreateProduct.values.images.splice(index, 1);
            this.renderImagesPreview();
        },

        clearImages: function () {
            this.closeImagePreview();
            CreateProduct.values.images = [];
            this.renderImagesPreview();
        },

        showImagePreview: function (url, title) {
            var modalElement = document.getElementById("imagePreviewModal");

            if (!modalElement || typeof bootstrap === "undefined") {
                window.open(url, "_blank");
                return;
            }

            document.getElementById("imagePreviewModalTitle").textContent = title;
            document.getElementById("imagePreviewModalImg").src = url;

            bootstrap.Modal.getOrCreateInstance(modalElement).show();
        },

        closeImagePreview: function () {
            var modalElement = document.getElementById("imagePreviewModal");

            if (!modalElement || typeof bootstrap === "undefined") {
                return;
            }

            var modal = bootstrap.Modal.getInstance(modalElement);

            if (modal) {
                modal.hide();
            }

            document.getElementById("imagePreviewModalImg").removeAttribute("src");
        },

        renderImagesPreview: function () {
            var container = document.getElementById("imagesPreview");

            CreateProduct.values.previewUrls.forEach(function (url) {
                URL.revokeObjectURL(url);
            });

            CreateProduct.values.previewUrls = [];
            container.innerHTML = "";

            CreateProduct.values.images.forEach(function (file, index) {

                var url = URL.createObjectURL(file);
                CreateProduct.values.previewUrls.push(url);

                var column = document.createElement("div");
                column.className = "col-6 col-sm-4 col-md-3";

                var wrapper = document.createElement("div");
                wrapper.className = "position-relative border rounded overflow-hidden";

                var previewBtn = document.createElement("button");
                previewBtn.type = "button";
                previewBtn.className = "btn p-0 border-0 w-100 d-block";
                previewBtn.title = CreateProduct.values.labels.viewImage || file.name;
                previewBtn.addEventListener("click", function () {
                    CreateProduct.showImagePreview(url, file.name);
                });

                var image = document.createElement("img");
                image.src = url;
                image.alt = file.name;
                image.className = "w-100 d-block";
                image.style.height = "120px";
                image.style.objectFit = "cover";

                previewBtn.appendChild(image);

                var removeBtn = document.createElement("button");
                removeBtn.type = "button";
                removeBtn.className = "btn btn-sm btn-danger position-absolute top-0 end-0 m-1";
                removeBtn.title = CreateProduct.values.labels.removeImage || "×";
                removeBtn.innerHTML = "&times;";
                removeBtn.addEventListener("click", function () {
                    CreateProduct.removeImage(index);
                });

                wrapper.appendChild(previewBtn);
                wrapper.appendChild(removeBtn);

                var caption = document.createElement("div");
                caption.className = "small text-truncate mt-1";
                caption.title = file.name;
                caption.textContent = file.name;

                column.appendChild(wrapper);
                column.appendChild(caption);

                container.appendChild(column);
            });
        },

        save: async function () {

            Shared.setLoading(true);

            var title = document.getElementById("title").value;
            var description = document.getElementById("descriptionEditor").value;

            var tags = CreateProduct.values.tagify.value.map(function (x) { return x.value })

            var formData = new FormData();

            formData.append("title", title);
            formData.append("description", description);

            tags.forEach(function (tag) {
                formData.append("tags", tag);
            });

            if (CreateProduct.values.product && CreateProduct.values.product.id) {
                formData.append("id", CreateProduct.values.product.id);
            }

            CreateProduct.values.images.forEach(function (file) {
                formData.append("images", file);
            });

            const response = await window.appFetch(CreateProduct.urls.saveProductUrl, {
                method: "POST",
                body: formData
            });

            if (!response.ok) {

            }

            var result = await response.json();

            if (result.isSuccess) {
                CreateProduct.clearImages();
            }

            Shared.setLoading(false);

        }

	}

})();