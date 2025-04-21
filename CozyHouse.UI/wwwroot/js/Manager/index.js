$(document).ready(function () {
    $('#userTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        lengthChange: false
    });
});

$(document).ready(function () {
    $('#managerTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        lengthChange: false
    });
});

function showDetails(row) {
    const user = JSON.parse(row.getAttribute("data-user"));
    let userDetailsHtml =
    `<div class="user-details">
        <h3>User ID: ${user.id}</h3>
        <h3>Person Name: ${user.personName}</h3>
        <h3>Phone Number: ${user.phoneNumber}</h3>
        <h3>Phone Number Confirmed: ${user.phoneNumberConfirmed}</h3>
    </div>`;
    Swal.fire({
        title: 'User Details',
        html: userDetailsHtml,
        icon: 'info',
        confirmButtonText: 'Got it!',
        showCloseButton: true,
        width: '40rem',
        backdrop: `rgba(0, 0, 123, 0.4) url("/img/nyan-cat.gif") left top no-repeat`
    });
}