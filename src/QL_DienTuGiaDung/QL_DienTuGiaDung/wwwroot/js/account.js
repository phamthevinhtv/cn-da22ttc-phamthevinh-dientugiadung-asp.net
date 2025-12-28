document.addEventListener("DOMContentLoaded", () => {
    const btnAddAdress = document.getElementById('button-add-address');
    const btnCancelAddAdress = document.getElementById('button-cancel-add-address');
    const frmAddAdress = document.getElementById('form-add-address');

    if (btnAddAdress) {
        btnAddAdress.addEventListener('click', function() {
            frmAddAdress.style.display = 'block';
            btnCancelAddAdress.style.display = 'flex';
            btnAddAdress.style.display = 'none';
        })
    }

    if (btnCancelAddAdress) {
        btnCancelAddAdress.addEventListener('click', function() {
            frmAddAdress.style.display = 'none';
            btnCancelAddAdress.style.display = 'none';
            btnAddAdress.style.display = 'flex';
        })
    }

    //

    const btnSaveProfile = document.getElementById('button-save-profile');
    const btnEditProfile = document.getElementById('button-edit-profile');
    const btnCancelEditProfile = document.getElementById('button-cancel-edit-profile');
    const inpsDisabled = document.querySelectorAll('.input-disabled');

    if (btnEditProfile) {
        btnEditProfile.addEventListener('click', function() {
            btnSaveProfile.style.display = 'block';
            btnCancelEditProfile.style.display = 'flex';
            btnEditProfile.style.display = 'none';
            inpsDisabled.forEach(input => {
                input.disabled = false;
            });
        })
    }

    if (btnCancelEditProfile) {
        btnCancelEditProfile.addEventListener('click', function() {
            btnSaveProfile.style.display = 'none';
            btnCancelEditProfile.style.display = 'none';
            btnEditProfile.style.display = 'block';
            inpsDisabled.forEach(input => {
                input.disabled = true;
            });
        })
    }

    //
    
    const apiBase = '/api';

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

    //

    document.querySelectorAll('.btn-edit-address').forEach(btn => {
        btn.addEventListener('click', async () => {

            frmAddAdress.style.display = 'block';
            btnAddAdress.style.display = 'none';
            btnCancelAddAdress.style.display = 'flex';

            document.getElementById('MaDCCT').value = btn.dataset.madcct;

            document.querySelector('[name="DiaChiCuThe.TenDCCT"]').value = btn.dataset.tendcct;

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

    const form = document.querySelector('#form-change-password');
    const passwordInput = document.getElementById('input-password');
    const confirmInput = document.getElementById('confirm-password');

    form.addEventListener('submit', function (e) {
        if (passwordInput.value !== confirmInput.value) {
            e.preventDefault();
            alert('Mật khẩu xác nhận không khớp');
            confirmInput.focus();
        }
    });
});
