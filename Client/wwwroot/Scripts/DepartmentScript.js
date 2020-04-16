﻿//var table = null;
$(document).ready(function () {
    //debugger;
    table = $('#dt').DataTable({
        "ajax": {
            url: "/Department/LoadDepartment",
            type: "GET",
            dataType: "json"
        },
        "columnDefs": [
            { "orderable": false, "targets": 3 },
            { "searchable": false, "targets": 3 }
        ],
        "columns": [
            { "data": "name", "name": "name" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data == nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).format('DD/MM/YYYY');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="BtnEdit" data-toggle="tooltip" data-placement="top" title="Edit" onclick="return GetById(' + row.id + ')"><i class="mdi mdi-pencil"></i></button> &nbsp; <button type="button" class="btn btn-danger" id="BtnDelete" data-toggle="tooltip" data-placement="top" title="Hapus" onclick="return Delete(' + row.id + ')"><i class="mdi mdi-delete"></i></button>';
                }
            },
        ]
    });

    // tooltip
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })

    // hide button update modal
    $('#Update').hide();

});



function Save() {
    var Department = new Object();
    Department.Name = $('#Name').val();

    if ($('#Name').val() == "") {
        Swal.fire({
            icon: 'info',
            title: 'Require',
            text: 'Name Cannot be Empty',
        })
        return false;
    } else {
        $.ajax({
            type: 'POST',
            url: '/Department/InsertOrUpdate/',
            data: Department
        }).then((result) => {
            //debugger;
            if (result.statusCode == 201) {
                Swal.fire({
                    icon: 'success',
                    position: 'center',
                    type: 'success',
                    showConfirmButton: false,
                    timer: 1500,
                    title: 'Added Succesfully'
                }).then(function () {
                    table.ajax.reload();
                    ClearScreen(); // delete value name department
                });
            }
            else {
                Swal.fire('Error', 'Failed to Add Department', 'error');
                ShowModal();
            }
        })
    }

}

function GetById(Id) {
    $.ajax({
        url: "/Department/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            //const obj = JSON.parse(result);
            // # atribut id pada html
            $('#Id').val(result.id);
            $('#Name').val(result.name);
            $('#exampleModal').modal('show');
            $('#Update').show();
            $('#Save').hide();

        },

        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}

function Edit() {
    //debugger;
    var Department = new Object();
    Department.Id = $('#Id').val();
    Department.Name = $('#Name').val();
    $.ajax({
        type: 'POST',
        url: 'Department/InsertOrUpdate',
        data: Department
    }).then((result) => {
        if (result.statusCode == 200) {
            Swal.fire({
                icon: 'success',
                position: 'center',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 1500
            }).then(function () {
                table.ajax.reload();
                ClearScreen(); // delete value name department
            });
        } else {
            Swal.fire('Error', 'Failed to input', 'error');
            ClearScreen();
        }
    })
}

function Delete(Id) {
    Swal.fire({
        title: "Are you sure ?",
        text: "You won't be able to Revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!"
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "Department/Delete/",
                data: { Id: Id }
            }).then((result) => {
                debugger;
                if (result.statusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Delete Successfully',
                        timer: 5000
                    }).then(function () {
                        table.ajax.reload();
                        ClearScreen(); // delete value name department
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to Delete',
                    })
                }
            })
        }
    });
}


function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    //$('#Save').show();
    //$('#Update').hide();
    //$('#Delete').hide();
    $('#exampleModal').modal('hide');
}

// handle error after action edit
document.getElementById("btncreatedept").addEventListener("click", function () {
    $('#Id').val('');
    $('#Name').val('');
    $('#Save').show();
    $('#Update').hide();
});

