function updateQuatityProductCart() {
    document.querySelectorAll('.cart_item_form_quantity').forEach(form => {
        const input = form.querySelector('.cart_item_input_quantity');
        const btnAdd = form.querySelector('.button_add_quantity');
        const btnSub = form.querySelector('.button_subtract_quantity');

        let oldValue = parseInt(input.value) || 1;

        function submitIfChanged(newValue) {
            if (newValue !== oldValue) {
                input.value = newValue;
                form.submit();
            }
        }

        input.addEventListener('input', () => {
            if (!/^\d*$/.test(input.value)) {
                input.value = "";
            }
        });

        btnAdd.addEventListener('click', () => {
            let value = parseInt(input.value) || 1;
            value++;
            submitIfChanged(value);
        });

        btnSub.addEventListener('click', () => {
            let value = parseInt(input.value) || 1;
            if (value > 1) value--;
            submitIfChanged(value);
        });

        input.addEventListener('blur', () => {
            let value = parseInt(input.value) || 1;
            if (value < 1) value = 1;
            submitIfChanged(value);
        });

        input.addEventListener('keydown', (e) => {
            if (e.key === 'Enter') {
                e.preventDefault();
                let value = parseInt(input.value) || 1;
                if (value < 1) value = 1;
                submitIfChanged(value);
            }
        });

        input.addEventListener('focus', () => {
            oldValue = parseInt(input.value) || 1;
        });
    });
}

function setDataForOrder() {
    const radios = document.querySelectorAll('input[type="radio"][name="MaDCCT"]');
    const hiddenInput = document.getElementById('maDCCTHidden');

    radios.forEach(radio => {
        radio.addEventListener('change', function () {
            if (this.checked) {
                hiddenInput.value = this.value;
            }
        });
    });

    const checkedRadio = document.querySelector('input[name="MaDCCT"]:checked');
    if (checkedRadio) {
        hiddenInput.value = checkedRadio.value;
    }
}

document.addEventListener('DOMContentLoaded', function() {
    updateQuatityProductCart();
    setDataForOrder();
});
