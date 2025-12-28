function updateLayout() {
    const header = document.querySelector('#header');
    const content = document.querySelector('#content');
    const main = document.querySelector('#main');
    const sidebar = document.querySelector('#sidebar');
    const footer = document.querySelector('#footer');

    let headerBottom = 0;

    if (header) {
        headerBottom = header.getBoundingClientRect().bottom;
        document.documentElement.style.setProperty('--sidebar-top', headerBottom + 'px');
    }

    if (content) {
        const contentLeft= content.getBoundingClientRect().left;
        document.documentElement.style.setProperty('--sidebar-left', contentLeft + 'px');
    }

    if (main && footer) {
        const footerHeight = footer.getBoundingClientRect().height;
        const mainMinHeight = window.innerHeight - footerHeight - headerBottom - 24;
        document.documentElement.style.setProperty('--main-min-height', mainMinHeight + 'px');
        
    }

    if (sidebar) {
        const sidebarWidth = sidebar.getBoundingClientRect().width;
        document.documentElement.style.setProperty('--main-margin-left', (sidebarWidth + 12) + 'px');
    }

    if (footer) {
        const footerTop = footer.getBoundingClientRect().top;
        const sidebarHeight = (footerTop) < window.innerHeight ? (footerTop - headerBottom) : (window.innerHeight - headerBottom);

        document.documentElement.style.setProperty('--sidebar-height', sidebarHeight + 'px');
    }
}

function toggleSidebar() {
    const sidebar = document.querySelector('#sidebar');
    const buttonToggle = document.querySelector('#button-toggle-sidebar');

    if(sidebar && buttonToggle) {
        buttonToggle.addEventListener('click', function(e) {
            e.stopPropagation();
            sidebar.classList.toggle('active');
        });

        sidebar.addEventListener('click', function (e) {
            e.stopPropagation();
        });

        document.addEventListener('click', function () {
            sidebar.classList.remove('active');
        });
    }
}

document.addEventListener('DOMContentLoaded', function() {
    updateLayout();

    setTimeout(() => {
        updateLayout();
    }, 500);
    
    window.addEventListener('resize', function() {
        updateLayout();

        setTimeout(() => {
            updateLayout();
        }, 500);
    });
    
    window.addEventListener('scroll', function() {
        updateLayout();

        setTimeout(() => {
            updateLayout();
        }, 500);
    });

    toggleSidebar();

    const provinceSelect = document.getElementById('provinceSelect');
    const communeSelect = document.getElementById('communeSelect');

    const apiBase = '/api';

    async function loadProvinces() {
        try {
            const res = await fetch(`${apiBase}/provinces`);
            if (!res.ok) throw new Error('Không thể tải danh sách tỉnh/thành phố');

            const data = await res.json();

            const provinces = data.provinces;

            provinceSelect.innerHTML = '<option value="">-- Chọn Tỉnh/Thành phố --</option>';

            provinces.forEach(p => {
                const option = document.createElement('option');
                option.value = `${p.code}|${p.name}`;   
                option.textContent = p.name;         
                provinceSelect.appendChild(option);
            });
        } catch (err) {
            console.error(err);
        }
    }

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

    provinceSelect.addEventListener('change', () => {
        const value = provinceSelect.value;
        communeSelect.innerHTML = '<option value="">-- Chọn Xã/Phường --</option>';

        if (!value) return;

        const [provinceCode] = value.split('|');
        loadCommunes(provinceCode);
    });

    loadProvinces();
});