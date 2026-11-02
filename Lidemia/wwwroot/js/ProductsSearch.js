var ProductsSearch = ProductsSearch || (function () {

    return {

        init: function (data) {
            ProductsSearch.urls = data.urls;
            this.initSearch();
        },

        initSearch: function () {

            var autocomplete = window['@algolia/autocomplete-js'].autocomplete;

            autocomplete({
                container: '#autocomplete',
                placeholder: 'Search products...',
                openOnFocus: true,

                // Fired when Enter is pressed while no item is highlighted
                // (the input is submitted), e.g. when the user types a query and
                // hits Enter without picking a suggestion
                onSubmit: function (params) {
                    var query = params.state && params.state.query ? params.state.query.trim() : '';

                    if (!query || !ProductsSearch.urls.productsListUrl) return;

                    window.location.assign(ProductsSearch.buildProductsListUrl(query));
                },

                getSources: function () {
                    // Shared across keystrokes so we can cancel the previous request
                    var inflight = null;

                    return [
                        {
                            sourceId: 'remote-products',

                            // params: { query, state, refresh, ... }
                            getItems: async function (params) {
                                var query = params.query;
                                var state = params.state;

                                // Cancel the previous fetch if it's still running
                                if (inflight) inflight.abort();
                                inflight = new AbortController();

                                // Minimal debounce: drop the call if the query changed
                                // while we were waiting
                                await new Promise(function (r) { setTimeout(r, 150); });
                                if (state.query !== query) return [];

                                try {
                                    var url = ProductsSearch.urls.autocompleteUrl + '?limit=8&query=' + encodeURIComponent(query);

                                    var response = await fetch(url, { signal: inflight.signal });
                                    if (!response.ok) throw new Error('HTTP ' + response.status);

                                    var json = await response.json();     // { data: { results: [...] }, isSuccess: true }
                                    return json && json.data ? json.data.results : [];
                                } catch (err) {
                                    if (err.name === 'AbortError') return [];   // expected
                                    console.error('Product search failed:', err);
                                    return [];
                                }
                            },

                            templates: {
                                // params: { item, components, html, state, ... }
                                item: function (params) {
                                    var item = params.item;
                                    var html = params.html;
                                    var Highlight = params.components.Highlight;

                                    return html`
                    <div class="aa-ItemContent">
                      <div class="aa-ItemContentBody">
                        <div class="aa-ItemContentTitle">
                          ${Highlight({ hit: item, attribute: 'title' })}
                        </div>
                        <div class="aa-ItemContentDescription">
                          ${item.price}
                        </div>
                      </div>
                    </div>
                  `;
                                },

                                noResults: function (params) {
                                    var html = params.html;
                                    return html`
                    <div class="aa-NoResults">
                      No products found for “${params.query}”.
                    </div>
                  `;
                                }
                            },

                            // Used by autocomplete for Enter and for the Ctrl/Cmd/Shift+Click shortcuts
                            getItemUrl: function (params) {
                                return ProductsSearch.urls.productUrl + '/' + encodeURIComponent(params.item.id);
                            },

                            // A plain mouse click only fires onSelect (autocomplete-js does not call
                            // the navigator for clicks), so the redirect has to happen here.
                            onSelect: function (params) {
                                var keyboardEvent = params.event;

                                // Ctrl/Cmd/Shift+Click is already handled by autocomplete itself
                                // (it opens a new tab/window), so don't navigate the current tab too
                                if (keyboardEvent && (keyboardEvent.ctrlKey || keyboardEvent.metaKey || keyboardEvent.shiftKey)) return;

                                if (params.itemUrl) {
                                    params.navigator.navigate({
                                        itemUrl: params.itemUrl,
                                        item: params.item,
                                        state: params.state
                                    });
                                }
                            }
                        }
                    ];
                }
            });
        },

        buildProductsListUrl: function (query) {
            var url = ProductsSearch.urls.productsListUrl;
            var separator = url.indexOf('?') === -1 ? '?' : '&';

            return url + separator + 'title=' + encodeURIComponent(query);
        }


    }
})();