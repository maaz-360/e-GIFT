document.getElementById("loginForm").addEventListener("submit", async function (event) {

    event.preventDefault();

    const form = this;
    const loginButton = document.getElementById("loginButton");
    const errorBox = document.getElementById("loginError");

    // Clear previous error
    errorBox.style.display = "none";
    errorBox.textContent = "";

    // Disable button while logging in
    loginButton.disabled = true;
    loginButton.textContent = "Logging in...";

    try {
        const formData = new FormData(form);

        const response = await fetch(form.action, {method: "POST",body: formData});
        const result = await response.json();

        if (result.success) {
            // Successful login
            window.location.href = "/Home/Index";
        }
        else {
            // Show error inside popup
            errorBox.textContent = result.message;
            errorBox.style.display = "block";
        }

    }
    catch (error) {
        errorBox.textContent =
            "Unable to login. Please try again.";
        errorBox.style.display = "block";

    }
    finally {
        loginButton.disabled = false;
        loginButton.textContent = "Login";
    }

});