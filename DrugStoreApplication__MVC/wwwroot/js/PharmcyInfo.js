var editIcon = document.getElementById("edit");
var Box = document.getElementById("BoxContaier");

//create Update Button , Hide Edit Icon
editIcon.addEventListener("click", function () {
    editIcon.style.display = "none";
    document.getElementById('PharmcyName').removeAttribute('readonly');

    var x = document.createElement("button");
    x.classList.add("buttonEditInfo");
    x.setAttribute('type', 'submit');
    x.setAttribute('id', 'UpdateButton');
    var t = document.createTextNode("Update");
    x.appendChild(t);
    Box.appendChild(x);
    //When click on update Button Go to Controller to Update Value
    $('#UpdateButton').on('click', function () {
        editIcon.style.display = 'block';
        editIcon.style.marginLeft = '175px';
        editIcon.style.marginTop = '-40px';
        var PharmcyName = $('#PharmcyName').val();

        $.ajax({
            data: {
                pharmcyName: PharmcyName
            },
            url: '/Home/ChangePharmcyName',
            success: function (jsonString) {
                var data = JSON.parse(jsonString)
                $("nav h1").text(data.pharmcyName);
                x.style.display = "none";
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Pharmcy Name Changed Sucessfully :)',
                    showConfirmButton: false,
                    timer: 1500
                });

            },
            error: function () {
                alert("Wrong");
            }
        });
 
    });
});


//Edit password

var EditIcon2 = document.getElementById("editButon2");
var Container = document.getElementById("ContainerPassword");
EditIcon2.addEventListener('click', function () {
    Container.style.display = 'block';
    var x = document.createElement("button");
    x.classList.add("buttonEditInfo");
    x.setAttribute('type', 'submit');
    x.setAttribute('id', 'UpdateButton2');
    var t = document.createTextNode("Update");
    x.appendChild(t);
    Container.appendChild(x);

    //when click on update password  button
    $('#UpdateButton2').on('click', function () {
 
        var CurrentPassword = $('#CurrentPassword').val();
        var NewPassword = $('#NewPassword').val();
        $.ajax({
            data: {
                currentPassword: CurrentPassword,
                newPassword: NewPassword
            },
            url: '/Home/ChangePassword',
            success: function (result) {
                var data = JSON.parse(result)
                if (data) {
                    console.log(result.success);
                    var data = JSON.parse(result)
                   Swal.fire({
                   position: 'center',
                    icon: 'success',
                    title: 'Password Changed Sucessfully :)',
                    showConfirmButton: false,
                     timer: 1500
                    });
                    }
                  else {
                   Swal.fire({
                 icon: 'error',
                 title: 'Oops...',
                  text: 'Enter the correct password!',

           });
         }
            }
        });
    });
});



