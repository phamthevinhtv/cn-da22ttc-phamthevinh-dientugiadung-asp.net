document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll('.form-quantity').forEach(form => {
        const input = form.querySelector('.input-quantity');
        const btnAdd = form.querySelector('.button-add-quantity');
        const btnSub = form.querySelector('.button-subtract-quantity');

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

    //

    const apiBase = '/api';

    const btnAddAdress = document.getElementById('button-add-address');
    const btnCancelAddAdress = document.getElementById('button-cancel-add-address');
    const frmAddAdress = document.getElementById('form-add-address');

    btnAddAdress.addEventListener('click', function() {
        frmAddAdress.style.display = 'block';
        btnCancelAddAdress.style.display = 'flex';
        btnAddAdress.style.display = 'none';
    })

    btnCancelAddAdress.addEventListener('click', function() {
        frmAddAdress.style.display = 'none';
        btnCancelAddAdress.style.display = 'none';
        btnAddAdress.style.display = 'flex';
    })

    async function loadCommunes(provinceCode) {
        try {
            const res = await fetch(
                `${apiBase}/provinces/${provinceCode}/communes`
            );
            if (!res.ok) throw new Error('Không thể tải danh sách xã/phường');

            const data = await res.json();

            const communes = data.communes;

            communeSelect.innerHTML = '<option value="">-- Chọn Xã/Phường --</option>';

            communes.forEach(c => {
                const option = document.createElement('option');
                option.value = `${c.code}|${c.name}`;
                option.textContent = c.name;
                communeSelect.appendChild(option);
            });
        } catch (err) {
            console.error(err);
        }
    }

    document.querySelectorAll('.btn-edit-address').forEach(btn => {
        btn.addEventListener('click', async () => {

            frmAddAdress.style.display = 'block';
            btnAddAdress.style.display = 'none';
            btnCancelAddAdress.style.display = 'flex';

            document.getElementById('MaDCCT').value = btn.dataset.madcct;

            document.querySelector('[name="TenDCCT"]').value = btn.dataset.tendcct;

            const provinceValue = `${btn.dataset.mattp}|${btn.dataset.tenttp}`;
            provinceSelect.value = provinceValue;

            await loadCommunes(btn.dataset.mattp);

            const communeValue = `${btn.dataset.maxp}|${btn.dataset.tenxp}`;
            communeSelect.value = communeValue;

            if(btn.dataset.macdinhdcct == 1) {
                document.querySelector('#check-macdinhdcct').style.display = 'none';
                document.querySelector('#address-default-in-form').style.display = 'block';
            } else {
                document.querySelector('#check-macdinhdcct').style.display = 'flex';
                document.querySelector('#address-default-in-form').style.display = 'none';
            }

            document.querySelector('#form-add-address h3').innerText = 'Chỉnh sửa địa chỉ';
        });
    });

    //

    const radios = document.querySelectorAll('#delivery-address-list input[type=radio]');
    const hiddenInput = document.getElementById('checkoutMaDCCT');

    const checkedRadio = document.querySelector('#delivery-address-list input[type=radio]:checked');
    if (checkedRadio) {
        hiddenInput.value = checkedRadio.value;
    }

    radios.forEach(radio => {
        radio.addEventListener('change', () => {
            hiddenInput.value = radio.value;
        });
    });
});