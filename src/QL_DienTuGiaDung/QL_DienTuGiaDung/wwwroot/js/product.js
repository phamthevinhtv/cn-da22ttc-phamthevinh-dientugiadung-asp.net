document.addEventListener("DOMContentLoaded", () => {
    const slider = document.querySelector(".product-image-slider");
    if (!slider) return;

    const images = JSON.parse(slider.dataset.images || "[]");
    if (images.length === 0) return;

    const img = slider.querySelector("#sliderImage");
    const btnLeft = slider.querySelector(".product-image-slider-nav.left");
    const btnRight = slider.querySelector(".product-image-slider-nav.right");

    let currentIndex = 0;

    function updateSlider() {
        img.src = images[currentIndex];

        btnLeft.style.display =
            currentIndex === 0 ? "none" : "block";

        btnRight.style.display =
            currentIndex === images.length - 1 ? "none" : "block";
    }

    btnLeft.addEventListener("click", () => {
        if (currentIndex > 0) {
            currentIndex--;
            updateSlider();
        }
    });

    btnRight.addEventListener("click", () => {
        if (currentIndex < images.length - 1) {
            currentIndex++;
            updateSlider();
        }
    });

    updateSlider();

    document.querySelector('.rating-form').addEventListener('submit', function (e) {
        const checkedStar = document.querySelector('input[name="rating"]:checked');

        if (!checkedStar) {
            e.preventDefault();
            alert('Vui lòng chọn số sao đánh giá');
        }
    });
});
