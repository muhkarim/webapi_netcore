var Departments = [];
var element = $('#DepartmentSelect');
var option = 0;
var optionName = "";

$(document).ready(function () {

    // get department option
    $.ajax({
        url: "/Department/LoadDepartment",
        type: "GET",
        success: function (data) {
            //debugger;
            Departments = data.data;
            var $option = $(element);
            $option.empty();
            $option.append($('<option/>').val(option).text(optionName).hide());
            $.each(Departments, function (i, val) {
                //debugger;
                $option.append($('<option/>').val(val.id).text(val.name));
            });
        }
    });

    // call load department
    LoadEmployeeDetail();

    // hide navbar
    $(".hide-nav").hide();

});


function LoadEmployeeDetail() {
    //debugger;
    $.ajax({
        url: "/EmployeeDetail/LoadData/",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            debugger;
            $('#Email').val(result.email);
            $('#FirstName').val(result.firstName);
            $('#LastName').val(result.lastName);
            //$('#Password').val(result.password);
            $('#BirthDate').val(moment(result.birthDate).format('YYYY-MM-DD'));
            $('#PhoneNumber').val(result.phoneNumber);
            $('#Address').val(result.address);
            $('#DepartmentSelect').val(result.department_Id);
            option = result.department_Id;
            optionName = result.departmentName;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

// Selectlist department
//function LoadDepartment(element) {
//    if (Departments.length === 0) {
//        $.ajax({
//            type: "Get",
//            url: "/Department/LoadDepartment",
//            success: function (data) {
//                Departments = data.data;
//                renderDepartment(element);
//            }
//        });
//    }
//    else {
//        renderDepartment(element);
//    }
//}

//function renderDepartment(element) {
//    var $option = $(element);
//    $option.empty();
//    $option.append($('<option/>').val('0').text('Select Department').hide());
//    $.each(Departments, function (i, val) {
//        $option.append($('<option/>').val(val.id).text(val.name));
//    });
//}
// end function seleclist



function Edit() {
    debugger;
    var Employee = new Object();
    Employee.FirstName = $('#FirstName').val();
    Employee.LastName = $('#LastName').val();
    Employee.Email = $('#Email').val();
    //Employee.Password = $('#Password').val();
    Employee.BirthDate = $('#BirthDate').val();
    Employee.PhoneNumber = $('#PhoneNumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentSelect').val();
    debugger;
    $.ajax({
        type: 'POST',
        url: '/EmployeeDetail/Update',
        data: Employee
    }).then((result) => {
        if (result.statusCode === 200 || result.statusCode === 201 || result.statusCode === 204) {
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