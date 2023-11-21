// Wait for the DOM content to be fully loaded before executing JavaScript
document.addEventListener("DOMContentLoaded", function () {
    // Select an element by its ID and change its text content
    const dynamicText = document.getElementById("dynamic-text");
    dynamicText.textContent = "Welcome to Our Dynamic Landing Page";

    // Handle a button click event
    const clickButton = document.getElementById("click-me-button");
    clickButton.addEventListener("click", function () {
        alert("Button Clicked!");
    });

    // Create an animated effect for a specific element
    const animatedElement = document.getElementById("animated-element");
    animatedElement.addEventListener("mouseenter", function () {
        animatedElement.style.transform = "scale(1.1)";
    });
    animatedElement.addEventListener("mouseleave", function () {
        animatedElement.style.transform = "scale(1)";
    });

    // Handle a form submission
    const submitForm = document.getElementById("example-form");
    submitForm.addEventListener("submit", function (event) {
        event.preventDefault(); // Prevent the form from submitting
        const inputText = document.getElementById("input-text").value;
        alert("You entered: " + inputText);
    });
});

// Other custom JavaScript functions can be added here
