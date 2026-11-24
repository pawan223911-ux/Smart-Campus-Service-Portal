document.addEventListener("DOMContentLoaded", function () {

    console.log("Smart Campus Service Portal Loaded");

    // Automatically close success alerts
    const alerts = document.querySelectorAll(".alert-success");

    alerts.forEach(function (alert) {

        setTimeout(function () {

            const closeButton = alert.querySelector(".btn-close");

            if (closeButton) {
                closeButton.click();
            }

        }, 4000);

    });

});
