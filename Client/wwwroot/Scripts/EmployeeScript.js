﻿var table = null;
var dateNow = new Date();
var Departments = []; //
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
            { "data": "departmentName" },
            { "data": "email" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            { "data": "phoneNumber" },
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
                    return " <td><button type='button' class='btn btn-warning' Id='Update' onclick=GetById('" + row.email + "');>Edit</button> <button type='button' class='btn btn-danger' Id='Delete' onclick=Delete('" + row.email + "');>Delete</button ></td >";
                }
            },
        ]
    });

    // tooltip
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })

    // hide button update modal
    //$('#btnUpdate').hide();

    LoadDepartment($('#DepartmentOption'));

    // show donut
    showDonut();
    // show bar
    showBar();
});



function Save() {
    var Employee = new Object(); // new object employee
    Employee.FirstName = $('#FirstName').val();
    Employee.LastName = $('#LastName').val();
    Employee.Email = $('#Email').val();
    Employee.Password = $('#Password').val(); //
    Employee.BirthDate = $('#BirthDate').val();
    Employee.PhoneNumber = $('#PhoneNumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();

        $.ajax({
            type: 'POST',
            url: '/Employee/Insert/',
            data: Employee
        }).then((result) => {
            if (result.statusCode == 200) {
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

function GetById(Email) {
    debugger;
    $.ajax({
        url: "/Employee/GetById/" + Email,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: { Email: Email },
        async: false,
        success: function (result) {
            debugger;
            //const obj = JSON.parse(result);
            // # atribut id pada html
            $('#Id').val(result[0].id);
            $('#FirstName').val(result[0].firstName);
            $('#LastName').val(result[0].lastName);
            $('#Email').val(result[0].email);
            $('#Password').val(result[0].password);
            $('#BirthDate').val(moment(result[0].birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(result[0].phoneNumber);
            $('#Address').val(result[0].address);
            $('#DepartmentOption').val(result[0].department_Id);
            $("#exampleModal").modal('show');
            $('#btnUpdate').show();
            $('#btnSave').hide();
            $('#inputPassword').hide();

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
    Employee.Password = $('#Password').val();
    Employee.BirthDate = $('#BirthDate').val();
    Employee.PhoneNumber = $('#PhoneNumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: '/Employee/Update',
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

function Delete(Email) {
    debugger;
    Swal.fire({
        title: "Are you sure ?",
        text: "You won't be able to Revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!"
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "/Employee/Delete/",
                data: { Email: Email }
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
    $('#FirstName').val('');
    $('#LastName').val('');
    $('#Email').val('');
    $('#Password').val('');
    $('#BirthDate').val('');
    $('#DepartmentOption').val('');
    $('#PhoneNumber').val('');
    $('#Address').val('');
    //$('#Save').show();
    //$('#Update').hide();
    //$('#Delete').hide();
    $('#exampleModal').modal('hide');
}

// handle error after action edit
document.getElementById("btncreatedept").addEventListener("click", function () {
    $('#btnSave').show();
    $('#btnUpdate').hide();
    ClearScreen();
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



// show donut
function showDonut() {
    debugger;
    $.ajax({
        type: 'GET',
        url: '/Employee/GetChart/',
        success: function (data) {
            debugger;
            Morris.Donut({
                element: 'EmployeeChart',
                data: $.each(JSON.parse(data), function (index, val) {
                    debugger;
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                resize: true,
                colors: ['#009efb', '#55ce63', '#2f3d4a']
            });
        }
    })
};

function showBar() {
    $.ajax({
        type: 'GET',
        url: '/Employee/GetChart/',
        success: function (data) {
            Morris.Bar({
                element: 'EmployeeBar',
                data: $.each(JSON.parse(data), function (index, val) {
                    [{
                        label: val.label,
                        value: val.value
                    }]
                }),
                xkey: 'label',
                ykeys: ['value'],
                labels: ['label'],
                barColors: ['#009efb', '#55ce63', '#2f3d4a'],
                hideHover: 'auto',
                gridLineColor: '#eef0f2',
                resize: true
            });
        }
    })
};

