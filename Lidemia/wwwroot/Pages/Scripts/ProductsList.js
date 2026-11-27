var ProductsList = ProductsList || (function () {

    return {

        init: function (data) {
            ProductsList.urls = data.urls;
            this.bindEvents();
        },

        bindEvents: function () {
            //document.querySelector("#searchBtn").addEventListener("click", ProductsList.search);

            const searchControls = document.querySelectorAll('.search-filter');

            searchControls.forEach((elem) => {
                elem.addEventListener("change", ProductsList.handleFilterControls);
            })
            
        },

        addToCart: function (id) {

        },

        showPreview: function (id) {

        },

        search: function () {

            var uri = URI(ProductsList.urls.baseUrl);

            var priceFrom = document.querySelector("#priceFrom").value;
            var priceTo = document.querySelector("#priceTo").value;

            if (priceFrom) {
                uri.addSearch({ priceFrom: priceFrom });
            }

            if (priceTo) {
                uri.addSearch({ priceTo: priceTo });
            }

            window.location = uri.toString();
        },

        handleFilterControls: function (sender) {
            if (ProductsList.filterTimeout) {
                clearTimeout(ProductsList.filterTimeout);
            }

            ProductsList.filterTimeout = setTimeout(ProductsList.search, 600);
        }

    }

})();