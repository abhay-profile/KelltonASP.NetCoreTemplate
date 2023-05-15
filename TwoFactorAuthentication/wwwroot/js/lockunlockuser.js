var dataTable;

$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $('#tableData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAllUsers"
        },
        "columns": [
            { "data": "email", "width": "70%" },
            {
                "data": { id:"id", lockoutEnd:"lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-unlock-fill"></i> UnLock
                                </a>
                            </div>
                        `
                    } else {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-lock-fill"></i> Lock
                                </a>
                            </div>
                        `
                    }
                },
                "width": "15%",
            },
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "PATCH",
        url: '/Admin/User/LockUnlock?userId=' + id,
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                alert(data.message);
                dataTable.ajax.reload();
            } else {
                alert(data.message);
            }
        }
    });
}