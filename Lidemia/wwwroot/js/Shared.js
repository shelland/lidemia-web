var Shared = Shared || (function () {

    return {

        getHashTagRegExp: function () {
            return /^#[\p{Script=Cyrl}\p{Script=Latn}\d_-]+$/u;
        }

    }

})();