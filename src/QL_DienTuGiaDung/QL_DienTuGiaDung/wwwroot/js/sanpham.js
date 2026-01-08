function sliderAnhSanPham() {
    const slider = document.querySelector(".product_image_slider");
    if (!slider) return;

    const images = JSON.parse(slider.dataset.images || "[]");
    if (images.length === 0) return;

    const img = slider.querySelector("#slider_image");
    const btnLeft = slider.querySelector(".product_image_slider_nav.left");
    const btnRight = slider.querySelector(".product_image_slider_nav.right");

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
}

function validFormRating() {
    const form = document.querySelector(".product_detail_rating_form");
    const radios = form.querySelectorAll("input[name='DiemDG']");
    const message = form.querySelector(".product_detail_rating_message");

    form.addEventListener("submit", function (e) {
        let isChecked = false;

        radios.forEach(radio => {
            if (radio.checked) isChecked = true;
        });

        if (!isChecked) {
            e.preventDefault();
            message.textContent = "Vui lòng chọn số sao";
            message.style.color = "red";
        }
    });

    radios.forEach(radio => {
        radio.addEventListener("change", function () {
            message.textContent = "";
        });
    });
}

document.addEventListener('DOMContentLoaded', function() {
    sliderAnhSanPham();
    validFormRating();
});
