var ProductsFilters = ProductsFilters || (function () {

    return {

        init: function (data) {
            ProductsFilters.sidebar = document.querySelector(data.sidebarSelector);
            ProductsFilters.modal = document.querySelector(data.modalSelector);

            if (!ProductsFilters.sidebar || !ProductsFilters.modal) {
                return;
            }

            ProductsFilters.modalBody = ProductsFilters.modal.querySelector(data.modalBodySelector);

            if (!ProductsFilters.modalBody) {
                return;
            }

            ProductsFilters.sidebarParent = ProductsFilters.sidebar.parentElement;
            ProductsFilters.sidebarNextSibling = ProductsFilters.sidebar.nextElementSibling;
            ProductsFilters.desktopMedia = window.matchMedia("(min-width: " + (data.desktopMinWidth || 992) + "px)");

            this.bindEvents();
        },

        bindEvents: function () {
            ProductsFilters.modal.addEventListener("show.bs.modal", function () {
                ProductsFilters.moveToModal();
            });

            ProductsFilters.modal.addEventListener("hidden.bs.modal", function () {
                ProductsFilters.moveToSidebar();
            });

            var onDesktopChange = function () {
                ProductsFilters.hideOnDesktop();
            };

            if (ProductsFilters.desktopMedia.addEventListener) {
                ProductsFilters.desktopMedia.addEventListener("change", onDesktopChange);
            }
            else {
                ProductsFilters.desktopMedia.addListener(onDesktopChange);
            }
        },

        moveToModal: function () {
            if (ProductsFilters.sidebar.parentElement === ProductsFilters.modalBody) {
                return;
            }

            ProductsFilters.sidebar.classList.remove("d-none", "d-lg-block");
            ProductsFilters.modalBody.appendChild(ProductsFilters.sidebar);
        },

        moveToSidebar: function () {
            if (ProductsFilters.sidebar.parentElement === ProductsFilters.sidebarParent) {
                return;
            }

            ProductsFilters.sidebarParent.insertBefore(ProductsFilters.sidebar, ProductsFilters.sidebarNextSibling);
            ProductsFilters.sidebar.classList.add("d-none", "d-lg-block");
        },

        hideOnDesktop: function () {
            if (!ProductsFilters.desktopMedia.matches) {
                return;
            }

            if (ProductsFilters.modal.classList.contains("show") && typeof bootstrap !== "undefined") {
                bootstrap.Modal.getOrCreateInstance(ProductsFilters.modal).hide();
            }
        }

    }

})();