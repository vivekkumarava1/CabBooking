/*const emailInput = document.getElementById('Email');
const emailValidationMessage = document.getElementById('emailValidationMessage');
*/
const phoneInput = document.getElementById('PhoneNumber');
const phoneValidationMessage = document.getElementById('phoneValidationMessage');

const passwordInput = document.getElementById('Password');
const passwordValidationMessage = document.getElementById('passwordValidationMessage');

/*emailInput.addEventListener('input', function () {
    validateEmail(this.value);
});*/
phoneInput.addEventListener('input', function () {
    validatePhoneNumber(this.value);
});

passwordInput.addEventListener('input', function () {
    validatePassword(this.value);
});

/*function validateEmail(email) {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (email === '') {
        emailValidationMessage.innerText = '';
    } 
     
    else if (!emailRegex.test(email)) {
        emailValidationMessage.innerText = 'Invalid email address.';
    }
    else {
        emailValidationMessage.innerText = '';
    }
}*/


function validatePhoneNumber(phoneNumber) {
    const phoneRegex = /^\d{10}$/;  // Matches a 10-digit phone number

    if (phoneNumber === '') {
        phoneValidationMessage.innerText = '';  // Clear validation message if phone number is empty
    } else if (!phoneRegex.test(phoneNumber)) {
        phoneValidationMessage.innerText = 'Phone should be of 10 digits';
    } else {
        phoneValidationMessage.innerText = '';
    }
}

function validatePassword(password) {
    // Password rules: At least 8 characters, one uppercase, one lowercase, one digit
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$/;

    if (password === '') {
        passwordValidationMessage.innerText = '';  // Clear validation message if password is empty
    } else if (!passwordRegex.test(password)) {
        passwordValidationMessage.innerText = 'Password must be at least 8 characters, 1 uppercase , 1 lowercase, digit,1 character';
    } else {
        passwordValidationMessage.innerText = '';
    }
}




const loginEmailInput = document.getElementById('Email'); // Login form
const loginEmailValidationMessage = document.getElementById('emailValidationMessage'); // Login form

const signupEmailInput = document.getElementById('signupEmail'); // Signup form
const signupEmailValidationMessage = document.getElementById('signupEmailValidationMessage'); // Signup form

loginEmailInput.addEventListener('input', function () {
    validateEmail(this.value, loginEmailValidationMessage);
});

signupEmailInput.addEventListener('input', function () {
    validateEmail(this.value, signupEmailValidationMessage);
});

function validateEmail(email, validationMessage) {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (email === '') {
        validationMessage.innerText = ''; // Clear validation message if the email is empty
    } else if (!emailRegex.test(email)) {
        validationMessage.innerText = 'Invalid email address.';
    } else {
        validationMessage.innerText = ''; // Clear the error message
    }
}
