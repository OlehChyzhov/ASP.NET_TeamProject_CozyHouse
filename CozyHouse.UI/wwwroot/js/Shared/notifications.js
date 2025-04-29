$(document).ready(function () {
    let result = document.getElementById("Value").value;
    let message = document.getElementById("Message").value;
    if (result == false || message == false) return;

    const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true
    });
    Toast.fire({
        icon: result,
        title: message
    });
});