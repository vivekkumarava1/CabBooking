document.addEventListener('DOMContentLoaded', function () {
    const sendOTPButton = document.getElementById('send-otp');
    const verifyOTPButton = document.getElementById('verify-otp');
    const resetPasswordButton = document.getElementById('reset-password');
    const otpContainer = document.getElementById('otp-container');
    const passwordContainer = document.getElementById('password-container');
    const messageDiv = document.getElementById('message');

    sendOTPButton.addEventListener('click', function () {
        
                    
                  
        // Make the API call to send OTP to the email address entered.
        // After a successful response from the API, show the OTP input field.
       
    });

    verifyOTPButton.addEventListener('click', function () {
        // Make the API call to verify the entered OTP.
        // After a successful response from the API, show the password input fields.
        passwordContainer.style.display = 'block';
    });

    resetPasswordButton.addEventListener('click', function () {
        // Make the API call to reset the password.
        // After a successful response from the API, display a success message.
        messageDiv.textContent = 'Password has been reset successfully!';
    });
});


$(document).ready(function () {
    $('#send-otp').click(function () {
        var email = $('#email').val();
        var data = { Email: email };

        // Send an AJAX request to the ForgetPasswordPage1 action
        $.ajax({
            type: 'POST',
            url: '/YourController/ForgetPasswordPage1', // Replace with your controller name
            data: data,
            success: function (response) {
                if (response.success) {
                    document.getElementById("otp-container").style.display = "block";
                    alert('OTP Sent Successfully');
                    // You can show the OTP input field or perform other actions here
                } else {
                    alert('Failed to send OTP. Please try again.');
                }
            },
            error: function () {
                alert('An error occurred while sending the OTP.');
            }
        });
    });
});
