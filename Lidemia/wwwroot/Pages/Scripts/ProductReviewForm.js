var ProductReviewForm = ProductReviewForm || (function () {

    return {

		init: function (data) {
			ProductReviewForm.urls = data.urls;
			ProductReviewForm.values = data.values;

			this.initPlugins();
		},

		initPlugins: function () {
			ProductReviewForm.initRating();
		},

		initRating: function () {
			const rating = new FaceRating("#face-rating");
		},

		saveReview: async function () {
			const rate = document.querySelector("#face-rating").value;
			const reviewId = document.querySelector("#reviewForm").dataset.reviewId;

			const reviewText = document.querySelector("#reviewText").value;
			const positiveReviewText = document.querySelector("#positiveReviewText").value;
			const negativeReviewText = document.querySelector("#negativeReviewText").value;
			const isAnonymous = document.querySelector("#isAnonymousReview").checked;

			var payload = {
				review: reviewText,				
				positiveReview: positiveReviewText,
				negativeReview: negativeReviewText,
				rate: rate,
				isAnonymous: isAnonymous,
				productId: ProductReviewForm.values.productId
			};

			if (reviewId) {
				payload.id = reviewId;
			}

			const response = await window.appFetch(ProductReviewForm.urls.saveReviewUrl, {
				method: "POST",
				body: JSON.stringify(payload)
			});

			if (!response.ok) {

			}

		}

    }

})();

class FaceRating {
	constructor(qs) {
		this.input = document.querySelector(qs);
		this.input?.addEventListener("input", this.update.bind(this));
		this.face = this.input?.previousElementSibling;
		this.update();
	}
	update(e) {
		let value = this.input.defaultValue;

		// when manually set
		if (e) value = e.target?.value;
		// when initiated
		else this.input.value = value;

		const min = this.input.min || 0;
		const max = this.input.max || 100;
		const percentRaw = (value - min) / (max - min) * 100;
		const percent = +percentRaw.toFixed(2);

		this.input?.style.setProperty("--percent", `${percent}%`);

		// face and range fill colors
		const maxHue = 120;
		const hueExtend = 30;
		const hue = Math.round(maxHue * percent / 100);

		let hue2 = hue - hueExtend;
		if (hue2 < 0) hue2 += 360;

		const hues = [hue, hue2];
		hues.forEach((h, i) => {
			this.face?.style.setProperty(`--face-hue${i + 1}`, h);
		});

		this.input?.style.setProperty("--input-hue", hue);

		// emotions
		const duration = 1;
		const delay = -(duration * 0.99) * percent / 100;
		const parts = ["right", "left", "mouth-lower", "mouth-upper"];

		parts.forEach(p => {
			const el = this.face?.querySelector(`[data-${p}]`);
			el?.style.setProperty(`--delay-${p}`, `${delay}s`);
		});

		// aria label
		const faces = [
			"Sad face",
			"Slightly sad face",
			"Straight face",
			"Slightly happy face",
			"Happy face"
		];
		let faceIndex = Math.floor(faces.length * percent / 100);
		if (faceIndex === faces.length) --faceIndex;

		this.face?.setAttribute("aria-label", faces[faceIndex]);
	}
}