var CreateProduct = CreateProduct || (function () {

	return {

		init: function (data) {
			CreateProduct.urls = data.urls;
			CreateProduct.values = data.values;
			CreateProduct.values.photos = (data.values.photos || []).map(function (photo) {
				return { photo: photo, file: null, uploaded: true };
			});
			CreateProduct.values.removedPhotoIds = [];
			CreateProduct.values.primaryPhotoId = CreateProduct.normalizeId(data.values.primaryPhotoId);
			CreateProduct.values.previewUrls = [];
			CreateProduct.values.labels = data.labels || {};

			this.bindEvents();
            this.initPlugins();
            this.renderImagesPreview();

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

            document.getElementById("hasDiscount").addEventListener("change", function () {
                CreateProduct.toggleDiscountSection(this.checked);
            });
        },

        toggleDiscountSection: function (isVisible) {
            var section = document.getElementById("discountSection");

            if (!section) {
                return;
            }

            section.classList.toggle("d-none", !isVisible);
        },

        loadProduct: function () {
            var product = CreateProduct.values.product;

            CreateProduct.values.tagify.addTags(product.tags);

            this.loadDiscount(product);
        },

        loadDiscount: function (product) {
            var hasDiscount = !!product.hasCurrentDiscount;

            document.getElementById("hasDiscount").checked = hasDiscount;
            document.getElementById("discountPrice").value = product.discountPrice != null ? product.discountPrice : "";
            document.getElementById("discountStartDate").value = CreateProduct.toDateTimeLocalValue(product.discountStartDate);
            document.getElementById("discountEndDate").value = CreateProduct.toDateTimeLocalValue(product.discountEndDate);

            this.toggleDiscountSection(hasDiscount);
        },

        toDateTimeLocalValue: function (value) {
            if (!value) {
                return "";
            }

            var date = new Date(value);

            if (isNaN(date.getTime())) {
                return "";
            }

            var pad = function (number) {
                return String(number).padStart(2, "0");
            };

            return date.getFullYear() +
                "-" + pad(date.getMonth() + 1) +
                "-" + pad(date.getDate()) +
                "T" + pad(date.getHours()) +
                ":" + pad(date.getMinutes());
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

                CreateProduct.values.photos.push({ photo: null, file: file, uploaded: false });
            });

            this.renderImagesPreview();
        },

        removeImage: function (item) {
            var index = CreateProduct.values.photos.indexOf(item);

            if (index === -1) {
                return;
            }

            this.closeImagePreview();
            CreateProduct.values.photos.splice(index, 1);

            if (this.isSavedPhoto(item)) {
                CreateProduct.values.removedPhotoIds.push(item.photo.id);

                if (this.isPrimaryPhoto(item)) {
                    CreateProduct.values.primaryPhotoId = null;
                }
            }

            this.renderImagesPreview();
        },

        setPrimaryPhoto: function (item) {
            if (!CreateProduct.isSavedPhoto(item)) {
                return;
            }

            CreateProduct.values.primaryPhotoId = CreateProduct.normalizeId(item.photo.id);
            this.renderImagesPreview();
        },

        isSavedPhoto: function (item) {
            return !!item.photo && !item.file && item.photo.id != null;
        },

        isPrimaryPhoto: function (item) {
            var photoId = CreateProduct.isSavedPhoto(item) ? CreateProduct.normalizeId(item.photo.id) : null;

            return photoId !== null && photoId === CreateProduct.values.primaryPhotoId;
        },

        canRemoveImage: function (item) {
            return this.isSavedPhoto(item) || (!!item.file && !item.uploaded);
        },

        normalizeId: function (id) {
            if (id === null || id === undefined || id === "" || id === 0 || id === "0") {
                return null;
            }

            return String(id);
        },

        markImagesUploaded: function () {
            CreateProduct.values.photos.forEach(function (item) {
                if (item.file) {
                    item.uploaded = true;
                }
            });

            this.renderImagesPreview();
        },

        getPendingImages: function () {
            return CreateProduct.values.photos
                .filter(function (item) { return item.file && !item.uploaded; })
                .map(function (item) { return item.file; });
        },

        getPhotoUrl: function (photo) {
            if (!photo) {
                return "";
            }

            return photo.mediumThumbPath || photo.smallThumbPath || photo.largeThumbPath || photo.originalFilePath || "";
        },

        getPhotoName: function (photo) {
            var path = (photo && photo.originalFilePath) || "";

            return path.substring(path.lastIndexOf("/") + 1);
        },

        createImageColumn: function (options) {
            var labels = CreateProduct.values.labels;

            var column = document.createElement("div");
            column.className = "col-6 col-sm-4 col-md-3";

            var wrapper = document.createElement("div");
            wrapper.className = "position-relative border rounded overflow-hidden" +
                (options.isPrimary ? " border-success border-2" : "");

            var previewBtn = document.createElement("button");
            previewBtn.type = "button";
            previewBtn.className = "btn p-0 border-0 w-100 d-block";
            previewBtn.title = options.imageTitle || "";
            previewBtn.addEventListener("click", options.onImageClick);

            var image = document.createElement("img");
            image.src = options.url;
            image.alt = options.title || "";
            image.className = "w-100 d-block";
            image.style.height = "120px";
            image.style.objectFit = "cover";

            previewBtn.appendChild(image);
            wrapper.appendChild(previewBtn);

            if (options.isPrimary) {
                var primaryBadge = document.createElement("span");
                primaryBadge.className = "badge bg-success position-absolute bottom-0 start-0 m-1";
                primaryBadge.title = labels.primaryImage || "";
                primaryBadge.innerHTML = '<i class="uil uil-check"></i>';

                wrapper.appendChild(primaryBadge);
            }
            else if (options.canSetPrimary) {
                var setPrimaryBtn = document.createElement("button");
                setPrimaryBtn.type = "button";
                setPrimaryBtn.className = "btn btn-sm btn-light border position-absolute bottom-0 start-0 m-1";
                setPrimaryBtn.title = labels.setPrimaryImage || "";
                setPrimaryBtn.innerHTML = '<i class="uil uil-check"></i>';
                setPrimaryBtn.addEventListener("click", options.onSetPrimary);

                wrapper.appendChild(setPrimaryBtn);
            }

            if (options.canPreview) {
                var viewBtn = document.createElement("button");
                viewBtn.type = "button";
                viewBtn.className = "btn btn-sm btn-light border position-absolute top-0 start-0 m-1";
                viewBtn.title = labels.viewImage || "";
                viewBtn.innerHTML = '<i class="uil uil-eye"></i>';
                viewBtn.addEventListener("click", options.onPreview);

                wrapper.appendChild(viewBtn);
            }

            if (options.onRemove) {
                var removeBtn = document.createElement("button");
                removeBtn.type = "button";
                removeBtn.className = "btn btn-sm btn-danger position-absolute top-0 end-0 m-1";
                removeBtn.title = labels.removeImage || "×";
                removeBtn.innerHTML = "&times;";
                removeBtn.addEventListener("click", options.onRemove);

                wrapper.appendChild(removeBtn);
            }

            column.appendChild(wrapper);

            if (options.caption) {
                var caption = document.createElement("div");
                caption.className = "small text-truncate mt-1";
                caption.title = options.caption;
                caption.textContent = options.caption;

                column.appendChild(caption);
            }

            return column;
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

            if (!container) {
                return;
            }

            CreateProduct.values.previewUrls.forEach(function (url) {
                URL.revokeObjectURL(url);
            });

            CreateProduct.values.previewUrls = [];
            container.innerHTML = "";

            CreateProduct.values.photos.forEach(function (item) {
                var isLocal = !!item.file;
                var isSaved = CreateProduct.isSavedPhoto(item);
                var url = isLocal ? CreateProduct.createPreviewUrl(item.file) : CreateProduct.getPhotoUrl(item.photo);
                var title = isLocal ? item.file.name : CreateProduct.getPhotoName(item.photo);
                var isPrimary = CreateProduct.isPrimaryPhoto(item);

                if (!url) {
                    return;
                }

                container.appendChild(CreateProduct.createImageColumn({
                    url: url,
                    title: title,
                    caption: isLocal ? item.file.name : null,
                    isPrimary: isPrimary,
                    canSetPrimary: isSaved && !isPrimary,
                    canPreview: isSaved,
                    imageTitle: isSaved
                        ? (isPrimary
                            ? CreateProduct.values.labels.primaryImage
                            : CreateProduct.values.labels.setPrimaryImage)
                        : CreateProduct.values.labels.viewImage,
                    onImageClick: isSaved
                        ? function () { CreateProduct.setPrimaryPhoto(item); }
                        : function () { CreateProduct.showImagePreview(url, title); },
                    onSetPrimary: function () { CreateProduct.setPrimaryPhoto(item); },
                    onPreview: function () { CreateProduct.showImagePreview(url, title); },
                    onRemove: CreateProduct.canRemoveImage(item)
                        ? function () { CreateProduct.removeImage(item); }
                        : null
                }));
            });
        },

        createPreviewUrl: function (file) {
            var url = URL.createObjectURL(file);
            CreateProduct.values.previewUrls.push(url);

            return url;
        },

        appendValue: function (formData, elementId, fieldName) {
            var element = document.getElementById(elementId);

            if (!element) {
                return;
            }

            formData.append(fieldName, element.value);
        },

        appendProductDetails: function (formData) {
            CreateProduct.appendValue(formData, "shortDescription", "shortDescription");
            CreateProduct.appendValue(formData, "price", "price");
            CreateProduct.appendValue(formData, "priceUnit", "priceUnit");
            CreateProduct.appendValue(formData, "availabilityType", "availabilityType");
            CreateProduct.appendValue(formData, "minQuantity", "minQuantity");
            CreateProduct.appendValue(formData, "maxQuantity", "maxQuantity");
            CreateProduct.appendValue(formData, "sku", "sku");
            CreateProduct.appendValue(formData, "parentId", "parentId");
        },

        appendDiscount: function (formData) {
            var hasDiscount = document.getElementById("hasDiscount").checked;

            formData.append("hasCurrentDiscount", hasDiscount ? "true" : "false");

            if (!hasDiscount) {
                return;
            }

            var discountPrice = document.getElementById("discountPrice").value;
            var discountStartDate = document.getElementById("discountStartDate").value;
            var discountEndDate = document.getElementById("discountEndDate").value;

            if (discountPrice) {
                formData.append("discountPrice", discountPrice);
            }

            if (discountStartDate) {
                formData.append("discountStartDate", discountStartDate);
            }

            if (discountEndDate) {
                formData.append("discountEndDate", discountEndDate);
            }
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

            CreateProduct.appendProductDetails(formData);
            CreateProduct.appendDiscount(formData);

            if (CreateProduct.values.product && CreateProduct.values.product.id) {
                formData.append("id", CreateProduct.values.product.id);
            }

            if (CreateProduct.values.primaryPhotoId) {
                formData.append("primaryPhotoId", CreateProduct.values.primaryPhotoId);
            }

            CreateProduct.getPendingImages().forEach(function (file) {
                formData.append("images", file);
            });

            CreateProduct.values.removedPhotoIds.forEach(function (photoId) {
                formData.append("removedPhotoIds", photoId);
            });

            const response = await window.appFetch(CreateProduct.urls.saveProductUrl, {
                method: "POST",
                body: formData
            });

            if (!response.ok) {

            }

            var result = await response.json();

            if (result.isSuccess) {
                CreateProduct.values.removedPhotoIds = [];
                CreateProduct.markImagesUploaded();
            }

            Shared.setLoading(false);

        }

	}

})();