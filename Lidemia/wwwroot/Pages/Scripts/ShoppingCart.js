var ShoppingCart = ShoppingCart || (function () {

    return {

        init: function (data) {
            ShoppingCart.values = data.values;
            ShoppingCart.urls = data.urls;

            this.bindEvents();
        },

        bindEvents: function () {
            document.querySelectorAll("input.item-quantity").forEach(el => {
                el.addEventListener("change", function (evt) {
                    const quantity = this.value;
                    const productId = this.dataset.product;

                    ShoppingCart.updateQuantity(productId, quantity);
                })
            })
        },

        addToCart: function () {

        },

        updateQuantity: function (productId, quantity) {
            console.log(`${productId}; ${quantity}`);
        }

    }

})();