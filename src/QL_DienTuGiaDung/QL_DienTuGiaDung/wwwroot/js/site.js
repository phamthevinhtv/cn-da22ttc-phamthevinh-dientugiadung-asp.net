function updateLayout() {
    const headerHeight = document.querySelector(".header").getBoundingClientRect().height;
    const sidebarWidth = document.querySelector(".main_content_sidebar").getBoundingClientRect().width;
    const mainContentLeft= document.querySelector(".main_content").getBoundingClientRect().left;
    const footerTop = document.querySelector(".footer").getBoundingClientRect().top;

    $(".main").css("margin-top", headerHeight + "px");
    $(".main_content_container").css("margin-left", sidebarWidth + "px");
    $(".main_content_sidebar").css("top", headerHeight + "px");
    $(".main_content_sidebar").css("left", mainContentLeft + "px");
    
    if (window.innerHeight > footerTop) {
        $(".main_content_sidebar").css("height", (footerTop - headerHeight) + "px");
    } else {
        $(".main_content_sidebar").css("height", (window.innerHeight - headerHeight) + "px");
    }
}

function toggleSidebar() {
    $(".header_content_bottom_icon_hamburger").click(function (e) {
        e.stopPropagation(); 
        $(".main_content_sidebar").toggleClass("active");
    });

    $(".main_content_sidebar").click(function (e) {
        e.stopPropagation();
    });

    $(document).click(function () {
        $(".main_content_sidebar").removeClass("active");
    });
}

function toggleHamburgerBySidebar() {
    const hasSidebar = document.querySelector(".main_content_sidebar_nav");
    const hamburger = document.querySelector(".header_content_bottom_icon_hamburger");

    if (!hasSidebar && hamburger) {
        hamburger.style.setProperty("display", "none", "important");
    }
}

function loadProvinces(selectElement, selectedValue = null) {
    fetch('/api/provinces')
        .then(response => response.json())
        .then(data => {
            const select = document.querySelector(selectElement);
            select.innerHTML = '<option value="">-- Chọn Tỉnh/Thành phố --</option>';
            
            if (data.provinces) {
                data.provinces.forEach(province => {
                    const option = document.createElement('option');
                    option.value = `${province.code}|${province.name}`;
                    option.textContent = province.name;
                    select.appendChild(option);
                });
            }

            if (selectedValue) {
                select.value = selectedValue;
                select.dispatchEvent(new Event('change'));
            }
        })
        .catch(error => {
            console.error('Lỗi khi load tỉnh thành:', error);
        });
}

function loadCommunes(provinceCode, selectElement, selectedValue = null) {
    if (!provinceCode) {
        const select = document.querySelector(selectElement);
        select.innerHTML = '<option value="">-- Chọn Xã/Phường --</option>';
        return;
    }

    fetch(`/api/provinces/${provinceCode}/communes`)
        .then(response => response.json())
        .then(data => {
            const select = document.querySelector(selectElement);
            select.innerHTML = '<option value="">-- Chọn Xã/Phường --</option>';
            
            if (data.communes) {
                data.communes.forEach(commune => {
                    const option = document.createElement('option');
                    option.value = `${commune.code}|${commune.name}`;
                    option.textContent = commune.name;
                    select.appendChild(option);
                });
            }

            if (selectedValue) {
                select.value = selectedValue;
            }
        })
        .catch(error => {
            console.error('Lỗi khi load xã phường:', error);
        });
}

function initAddressForm(provinceSelectId, communeSelectId) {
    const provinceSelect = document.querySelector(`#${provinceSelectId}`);
    const communeSelect = document.querySelector(`#${communeSelectId}`);

    const oldTinh = document.getElementById('oldTinhTP')?.value;
    const oldXa = document.getElementById('oldXaPhuong')?.value;

    if (!provinceSelect || !communeSelect) {
        console.error('Không tìm thấy select element');
        return;
    }

    loadProvinces(`#${provinceSelectId}`, oldTinh);

    provinceSelect.addEventListener('change', function () {
        const selectedValue = this.value;
        if (selectedValue) {
            const provinceCode = selectedValue.split('|')[0];
            loadCommunes(provinceCode, `#${communeSelectId}`, oldXa);
        } else {
            communeSelect.innerHTML = '<option value="">-- Chọn Xã/Phường --</option>';
        }
    });
}

document.addEventListener('DOMContentLoaded', function() {
    updateLayout();
    setTimeout(function() {
        updateLayout();
    }, 500);

    toggleSidebar();
    toggleHamburgerBySidebar();
});

window.addEventListener('resize', function() {
    updateLayout();
    setTimeout(function() {
        updateLayout();
    }, 500);
});

window.addEventListener('scroll', function() {
    updateLayout();
    setTimeout(function() {
        updateLayout();
    }, 500);
});