document.querySelectorAll('#ttdh-form input[type="radio"]')
.forEach(radio => {
    radio.addEventListener('change', function () {
        const oldValue = this.dataset.current;

        if (confirm("Bạn có chắc muốn đổi trạng thái đơn hàng không?")) {
            this.form.submit();
        } else {
            document.querySelector(
                `input[name="${this.name}"][value="${oldValue}"]`
            ).checked = true;
        }
    });
});

document.querySelectorAll('#tttt-form input[type="radio"]')
.forEach(radio => {
    radio.addEventListener('change', function () {

        const oldValue = this.dataset.current;

        if (!confirm("Bạn có chắc muốn đổi trạng thái thanh toán không?")) {
            revertRadio(this, oldValue);
            return;
        }

        if (this.value == "2") {
            const magd = prompt("Nhập mã giao dịch (Nhấn OK để bỏ qua)");

            if (magd === null) {
                revertRadio(this, oldValue);
                return;
            }

            document.querySelector("#MaGiaoDichTT").value = magd ?? "";
        }

        this.form.submit();
    });
});

function revertRadio(radio, oldValue) {
    document.querySelector(
        `input[name="${radio.name}"][value="${oldValue}"]`
    ).checked = true;
}
