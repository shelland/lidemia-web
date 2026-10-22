(function () {
    const nativeFetch = window.fetch;

    window.appFetch = async function (resource, init = {}) {
        init.headers = new Headers(init.headers || {});
        var mu = init.body instanceof FormData;

        if (!init.headers.has('Content-Type') && !(init.body instanceof FormData)) {
            init.headers.set('Content-Type', 'application/json');
        }

        const token = localStorage.getItem('authToken');

        if (token && !init.headers.has('Authorization')) {
            init.headers.set('Authorization', `Bearer ${token}`);
        }

        return nativeFetch(resource, init);
    };
})();