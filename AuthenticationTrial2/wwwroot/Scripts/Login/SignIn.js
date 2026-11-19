$(document).ready((event) => {
    $("form").on("submit", (event) => {
        event.preventDefault();
    })
    $("#submitBtn").on("click", (event) => {
        let email = $("#emailtxt").val();
        let password = $("#passwordtxt").val();
        let UserObject = {
            UserName: email,
            Password: password,
            ConfirmPassword: password
        };
        $.ajax({
            url: window.location.origin + "/Login/AccessAccount",
            type: "Post",
            contentType: "application/json",
            dataType: "JSON",
            data: JSON.stringify(UserObject),
            success: (data) => {
                if (data == "Login Success") {
                    $("#successDisplayElement").text(data);
                    $("#errorDisplayElement").empty();
                    setTimeout(() => {
                        window.location.href = window.location.origin + "/Home/Index"
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