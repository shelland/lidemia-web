var ProductDetails = ProductDetails || (function () {

    return {

        init: function (data) {
            ProductDetails.urls = data.urls;
            ProductDetails.values = data.values;

            this.bindEvents();
            this.initPlugins();
        },

        bindEvents: function () {
            document.querySelector("#addReviewBtn").addEventListener("click", () => {
                ProductDetails.loadReviewForm();
            });
        },

        addToCart: async function () {

            var qty = document.querySelector("#qty").value;

            var payload = {
                productId: ProductDetails.values.id,
                qty: qty
            }

            const response = await window.appFetch(ProductDetails.urls.addToCartUrl, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {

            }

            var result = await response.json();

        },

        addToWishList: async function () {
            var payload = {
                productId: ProductDetails.values.id
            };

            const response = await window.appFetch(ProductDetails.urls.addToWishListUrl, {
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (!response.ok) {

            }
        },

        initPlugins: function () {
            this.initSwiperSlider();
            this.initProgressBars();
                       

            const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
            const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl));
        },

        initProgressBars: function () {
            const pline = document.querySelectorAll(".progressbar.line");
            const pcircle = document.querySelectorAll(".progressbar.semi-circle");
            pline.forEach(e => {
                var line = new ProgressBar.Line(e, {
                    strokeWidth: 6,
                    trailWidth: 6,
                    duration: 3000,
                    easing: 'easeInOut',
                    text: {
                        style: {
                            color: 'inherit',
                            position: 'absolute',
                            right: '0',
                            top: '-30px',
                            padding: 0,
                            margin: 0,
                            transform: null
                        },
                        autoStyleContainer: false
                    },
                    step: (state, line) => {
                        line.setText(Math.round(line.value() * 100) + ' %');
                    }
                });
                var value = e.getAttribute('data-value') / 100;
                new Waypoint({
                    element: e,
                    handler: function () {
                        line.animate(value);
                    },
                    offset: 'bottom-in-view',
                })
            });
            pcircle.forEach(e => {
                var circle = new ProgressBar.SemiCircle(e, {
                    strokeWidth: 6,
                    trailWidth: 6,
                    duration: 2000,
                    easing: 'easeInOut',
                    step: (state, circle) => {
                        circle.setText(Math.round(circle.value() * 100));
                    }
                });
                var value = e.getAttribute('data-value') / 100;
                new Waypoint({
                    element: e,
                    handler: function () {
                        circle.animate(value);
                    },
                    offset: 'bottom-in-view',
                })
            });
        },

        initSwiperSlider: function () {
            var carousel = document.querySelectorAll('.swiper-container');
            for (var i = 0; i < carousel.length; i++) {
                var slider1 = carousel[i];
                slider1.classList.add('swiper-container-' + i);
                var controls = document.createElement('div');
                controls.className = "swiper-controls";
                var pagi = document.createElement('div');
                pagi.className = "swiper-pagination";
                var navi = document.createElement('div');
                navi.className = "swiper-navigation";
                var prev = document.createElement('div');
                prev.className = "swiper-button swiper-button-prev";
                var next = document.createElement('div');
                next.className = "swiper-button swiper-button-next";
                slider1.appendChild(controls);
                controls.appendChild(navi);
                navi.appendChild(prev);
                navi.appendChild(next);
                controls.appendChild(pagi);
                var sliderEffect = slider1.getAttribute('data-effect') ? slider1.getAttribute('data-effect') : 'slide';
                if (slider1.getAttribute('data-items-auto') === 'true') {
                    var slidesPerViewInit = "auto";
                    var breakpointsInit = null;
                } else {
                    var sliderItems = slider1.getAttribute('data-items') ? slider1.getAttribute('data-items') : 3; // items in all devices
                    var sliderItemsXs = slider1.getAttribute('data-items-xs') ? slider1.getAttribute('data-items-xs') : 1; // start - 575
                    var sliderItemsSm = slider1.getAttribute('data-items-sm') ? slider1.getAttribute('data-items-sm') : Number(sliderItemsXs); // 576 - 767
                    var sliderItemsMd = slider1.getAttribute('data-items-md') ? slider1.getAttribute('data-items-md') : Number(sliderItemsSm); // 768 - 991
                    var sliderItemsLg = slider1.getAttribute('data-items-lg') ? slider1.getAttribute('data-items-lg') : Number(sliderItemsMd); // 992 - 1199
                    var sliderItemsXl = slider1.getAttribute('data-items-xl') ? slider1.getAttribute('data-items-xl') : Number(sliderItemsLg); // 1200 - end
                    var sliderItemsXxl = slider1.getAttribute('data-items-xxl') ? slider1.getAttribute('data-items-xxl') : Number(sliderItemsXl); // 1500 - end
                    var slidesPerViewInit = sliderItems;
                    var breakpointsInit = {
                        0: {
                            slidesPerView: Number(sliderItemsXs)
                        },
                        576: {
                            slidesPerView: Number(sliderItemsSm)
                        },
                        768: {
                            slidesPerView: Number(sliderItemsMd)
                        },
                        992: {
                            slidesPerView: Number(sliderItemsLg)
                        },
                        1200: {
                            slidesPerView: Number(sliderItemsXl)
                        },
                        1400: {
                            slidesPerView: Number(sliderItemsXxl)
                        }
                    }
                }
                var sliderSpeed = slider1.getAttribute('data-speed') ? slider1.getAttribute('data-speed') : 500;
                var sliderAutoPlay = slider1.getAttribute('data-autoplay') !== 'false';
                var sliderAutoPlayTime = slider1.getAttribute('data-autoplaytime') ? slider1.getAttribute('data-autoplaytime') : 5000;
                var sliderAutoHeight = slider1.getAttribute('data-autoheight') === 'true';
                var sliderResizeUpdate = slider1.getAttribute('data-resizeupdate') !== 'false';
                var sliderAllowTouchMove = slider1.getAttribute('data-drag') !== 'false';
                var sliderReverseDirection = slider1.getAttribute('data-reverse') === 'true';
                var sliderMargin = slider1.getAttribute('data-margin') ? slider1.getAttribute('data-margin') : 30;
                var sliderLoop = slider1.getAttribute('data-loop') === 'true';
                var sliderCentered = slider1.getAttribute('data-centered') === 'true';
                var sliderWatchOverflow = slider1.getAttribute('data-watchoverflow') !== 'false';
                var swiper = slider1.querySelector('.swiper:not(.swiper-thumbs)');
                var swiperTh = slider1.querySelector('.swiper-thumbs');
                var sliderTh = new Swiper(swiperTh, {
                    slidesPerView: 5,
                    spaceBetween: 10,
                    loop: false,
                    threshold: 2,
                    slideToClickedSlide: true
                });
                if (slider1.getAttribute('data-thumbs') === 'true') {
                    var thumbsInit = sliderTh;
                    var swiperMain = document.createElement('div');
                    swiperMain.className = "swiper-main";
                    swiper.parentNode.insertBefore(swiperMain, swiper);
                    swiperMain.appendChild(swiper);
                    slider1.removeChild(controls);
                    swiperMain.appendChild(controls);
                } else {
                    var thumbsInit = null;
                }
                var slider = new Swiper(swiper, {
                    on: {
                        beforeInit: function () {
                            if (slider1.getAttribute('data-nav') !== 'true' && slider1.getAttribute('data-dots') !== 'true') {
                                controls.remove();
                            }
                            if (slider1.getAttribute('data-dots') !== 'true') {
                                pagi.remove();
                            }
                            if (slider1.getAttribute('data-nav') !== 'true') {
                                navi.remove();
                            }
                        },
                        init: function () {
                            if (slider1.getAttribute('data-autoplay') !== 'true') {
                                this.autoplay.stop();
                            }
                            this.update();
                        }
                    },
                    autoplay: {
                        delay: sliderAutoPlayTime,
                        disableOnInteraction: false,
                        reverseDirection: sliderReverseDirection,
                        pauseOnMouseEnter: false
                    },
                    allowTouchMove: sliderAllowTouchMove,
                    speed: parseInt(sliderSpeed),
                    slidesPerView: slidesPerViewInit,
                    loop: sliderLoop,
                    centeredSlides: sliderCentered,
                    spaceBetween: Number(sliderMargin),
                    effect: sliderEffect,
                    autoHeight: sliderAutoHeight,
                    watchOverflow: sliderWatchOverflow,
                    grabCursor: true,
                    resizeObserver: false,
                    updateOnWindowResize: sliderResizeUpdate,
                    breakpoints: breakpointsInit,
                    pagination: {
                        el: carousel[i].querySelector('.swiper-pagination'),
                        clickable: true
                    },
                    navigation: {
                        prevEl: slider1.querySelector('.swiper-button-prev'),
                        nextEl: slider1.querySelector('.swiper-button-next'),
                    },
                    thumbs: {
                        swiper: thumbsInit,
                    },
                });
            }
        },

        loadReviewForm: async function () {

            const response = await window.appFetch(ProductDetails.urls.loadReviewFormUrl, {
                method: "GET"
            });

            if (!response.ok) {

            }

            const form = await response.text();


            document.querySelector("#reviewFormWrapper").innerHTML = form;


            const modalWindow = document.querySelector("#productRateFormModal");
            const modal = bootstrap.Modal.getOrCreateInstance(modalWindow);

            modalWindow.addEventListener("shown.bs.modal", () => {
                ProductReviewForm.init({
                    urls: {
                        saveReviewUrl: ProductDetails.urls.saveReviewUrl
                    },
                    values: {
                        productId: ProductDetails.values.id
                    }
                });
            });           

            modal.show();
        }

    }

})();