var table = null;
var dateNow = new Date();
var Departments = [];
$(document).ready(function () {
    table = $('#et').DataTable({ //Nama table pada index
        "ajax": {
            url: "/Employee/LoadEmployee", //Nama controller/fungsi pada index controller
            type: "GET",
            dataType: "json",
            dataSrc: ""
        },
        "columnDefs": [
            { "orderable": false, "targets": 8 },
            { "searchable": false, "targets": 8 }
        ],
        "columns": [
           //{ "data": "firstName" },
            {
                "data": null, render: function (data, type, row) {
                    return data.firstName + ' ' + data.lastName;
                }
            },
            { "data": "departmentName"},
            { "data": "email", "name": "Email" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            { "data": "phoneNumber", "name": "Phone Number" },
            { "data": "address", "name": "Address" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data === nulldate) {
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

    LoadDepartment($('#DepartmentOption'));
});



function Save() {
    var Employee = new Object(); // new object employee
    Employee.FirstName = $('#FirstName').val();
    Employee.LastName = $('#LastName').val();
    Employee.Email = $('#Email').val();
    Employee.BirthDate = $('#BirthDate').val();
    Employee.PhoneNumber = $('#PhoneNumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();

        $.ajax({
            type: 'POST',
            url: '/Employee/InsertOrUpdate/',
            data: Employee
        }).then((result) => {
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
                    ClearScreen(); 
                });
            }
            else {
                Swal.fire('Error', 'Failed to Add Employee', 'error');
                ShowModal();
            }
        })
    

}

function GetById(Id) {
    $.ajax({
        url: "/Employee/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            //const obj = JSON.parse(result);
            // # atribut id pada html
            $('#Id').val(result[0].id);
            $('#FirstName').val(result[0].firstName);
            $('#LastName').val(result[0].lastName);
            $('#Email').val(result[0].email);
            $('#BirthDate').val(result[0].birthDate);
            $('#PhoneNumber').val(result[0].phoneNumber);
            $('#Address').val(result[0].address);
            $('#DepartmentOption').val(result[0].department_Id);
            $("#exampleModal").modal('show');
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
    var Employee = new Object();
    Employee.Id = $('#Id').val();
    Employee.FirstName = $('#FirstName').val();
    Employee.LastName = $('#LastName').val();
    Employee.Email = $('#Email').val();
    Employee.BirthDate = $('#BirthDate').val();
    Employee.PhoneNumber = $('#PhoneNumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: 'Employee/InsertOrUpdate',
        data: Employee
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
                ClearScreen(); 
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
                url: "Employee/Delete/",
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
                        ClearScreen(); 
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


// Selectlist
function LoadDepartment(element) {
    if (Departments.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Department/LoadDepartment",
            success: function (data) {
                Departments = data.data;
                renderDepartment(element);
            }
        });
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name));
    });
}
// end function seleclist


