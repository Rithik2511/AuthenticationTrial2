$(document).ready((event) => {
    $("form").on("submit", (event) => {
        event.preventDefault();
    })
    $("#submitBtn").on("click", (event) => {
        let email = $("#emailtxt").val();
        let password = $("#passwordtxt").val();
        let confirmPassword = $("#cnfPasswordtxt").val();
        let UserObject = {
            UserName: email,
            Password: password,
            ConfirmPassword: confirmPassword
        };
        $.ajax({
            url: "CreateAccount",
            type: "Post",
            contentType: "application/json",
            dataType: "JSON",
            data: JSON.stringify(UserObject),
            success: (data) => {
                if (data == "Account Created Successfully") {
                    $("#successDisplayElement").text(data);
                    $("#errorDisplayElement").empty();
                    setTimeout(() => {
                        window.location.href = window.location.origin + "/Login/SignIn"
                    }, 2000)
                }
                else {
                    $("#errorDisplayElement").html(data);
                    $("#successDisplayElement").empty();
                }
            },
            error: (error) => {
                console.log(error);
            }
        })
    })
})