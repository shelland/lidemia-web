var Shared = Shared || (function () {

    return {

        getHashTagRegExp: function () {
            return /^#[\p{Script=Cyrl}\p{Script=Latn}\d_-]+$/u;
        },

        setLoading: function (state) {
            if (state) {
                document.querySelector(".page-loader").classList.remove("d-none");
            }
            else {
                document.querySelector(".page-loader").classList.add("d-none");
            }
        },

        setElementLoading: function (element, state) {

            if (state) {
                Shared.loadingOverlay = new LoadingOverlay(document.querySelector(element));
                Shared.loadingOverlay.show();
            }
            else {
                Shared.loadingOverlay.destroy();
            }

            
        }

    }

})();