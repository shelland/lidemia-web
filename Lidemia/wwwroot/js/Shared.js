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
        }

    }

})();