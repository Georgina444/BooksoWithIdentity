var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else{
        loadDataTable("all");
        }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
            //"type":"GET"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "name", "width":"10%"},
            { "data": "phoneNumber", "width": "10%"},
           // { "data": "ApplicationUser.email", "width": "10%"},
            { "data": "orderStatus", "width": "10%"},
            { "data": "orderTotal", "width": "10%"},
            {
                "data": "id",
                "render": function (data) {     // data is the id of the product that the user selects
                    return `
                        <div class="w-35 btn-btn-group text-center" role = "group" >
                            <a href="/Admin/Order/Details?orderId=${data}"
                                class="btn btn-secondary mx-2"> <i class="bi bi-pen"></i> Details </a>
                    </div >
                    `
                },
                "width": "16%"
            },
        ]
        });   
}  

