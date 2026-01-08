document.addEventListener('DOMContentLoaded', function() {
    const editButton = document.getElementById('button-edit-profile');
    const cancelButton = document.getElementById('button-cancel-edit-profile');
    const saveButton = document.getElementById('button-save-profile');
    const usernameInput = document.querySelector('input[name="TenTK"]');
    const passwordInput = document.getElementById('input-password');
    const confirmPasswordInput = document.getElementById('confirm-password');
    const changePasswordForm = document.getElementById('form-change-password');

    // Initially hide cancel and save buttons
    cancelButton.style.display = 'none';
    saveButton.style.display = 'none';

    // Edit profile functionality
    editButton.addEventListener('click', function() {
        usernameInput.disabled = false;
        usernameInput.classList.remove('input-disabled');
        
        editButton.style.display = 'none';
        cancelButton.style.display = 'inline-block';
        saveButton.style.display = 'inline-block';
    });

    // Cancel edit functionality
    cancelButton.addEventListener('click', function() {
        usernameInput.disabled = true;
        usernameInput.classList.add('input-disabled');
        
        editButton.style.display = 'inline-block';
        cancelButton.style.display = 'none';
        saveButton.style.display = 'none';
        
        // Reset to original value if needed
        location.reload();
    });

    // Password confirmation validation
    changePasswordForm.addEventListener('submit', function(e) {
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;

        if (password !== confirmPassword) {
            e.preventDefault();
            alert('Mật khẩu xác nhận không khớp!');
            return false;
        }

        if (password.length < 6) {
            e.preventDefault();
            alert('Mật khẩu phải có ít nhất 6 ký tự!');
            return false;
        }
    });
});