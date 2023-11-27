﻿$(document).ready(function () {
    function getAllReviews() {
        $.ajax({
            url: '@Url.Action("GetReviews", "Home")',
            type: 'GET',
            contentType: false,
            processData: false,
            success: function (response) {
                let Reviews = JSON.parse(response);
                let rows = `<table class="widthfull">`;
                $.each(Reviews, function (index, review) {
                    let dat = new Date(review.ReviewDate);
                    let day = dat.getDay();
                    let month = dat.getMonth();
                    let year = dat.getFullYear();
                    let hour = dat.getHours();
                    let mins = dat.getMinutes();
                    rows += `<tr>
                                        <th>
                                            <span class="LoginLabel">${review.UserLogin}</span>
                                            <span class="DatetimeLabel">${day}-${month}-${year} ${hour}:${mins} </span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            <div class="TextReviewLabel">${review.ReviewText}</div>
                                        </th>
                                    </tr>`;
                })

                rows += `</table>`;

                $("#block-display-reviews").html(rows);
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }
    function tryToLogin() {
        let formData = new FormData();
        formData.append("Login", $("#loginform-login").val());
        formData.append("Password", $("#loginform-password").val());
        $.ajax({
            url: '@Url.Action("Login", "Home")',
            type: 'POST',
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                $("#ref-login").css("display", "none");
                $("#label-fullname").val(response);
                $("#label-fullname").css("display", "inline");
                $("#ref-logout").css("display", "inline");



            }, error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }
    function prepareIndexPage() {
        let rows = "";
        let login = sessionStorage.getItem('login');
        if (login) {
            rows += `<div id="block-for-add-review">
                <h1 style="text-align:center">Залиште свій відгук.</h1>
                <form asp-action="Index" asp-controller="Home" method="post">
                    <div style="margin: 5px 10px 5px 10px">
                        <input type="text" name="Login" value="${login}" readonly>
                        <br>
                        <textarea style="width:100%;resize:vertical;" name="ReviewText" placeholder="Опишіть свої враження тут..." required></textarea>
                        <input type="submit" value="Опублікувати">
                    </div>
                </form></div>`;
        }

        rows += `
                    <h3 style="text-align:center">Відгуки користувачів</h3>
                    <div id="block-display-reviews"></div>`;
        $("#pseudobody").html(rows);
    }

    function prepareLoginPage() {
        let rows = `<h1>Увійти</h1>
                        <hr />
                        <div class="row">
                            <div class="col-md-4">
                               <form asp-action="Login">
                                    <div asp-validation-summary="ModelOnly" class="text-danger" id="loginform-login-error-span"></div>
                                    <div class="form-group">
                                        <label class="control-label">Логін</label>
                                        <input class="form-control" id="loginform-login" name="loginform-login" required/>
                                    </div>
                                    <br />
                                    <div class="form-group">
                                        <label class="control-label">Пароль</label>
                                        <input class="form-control" id="loginform-password" name="loginform-password" required/>
                                    </div>
                                    <br />
                                    <div class="form-group">
                                        <input type="submit" value="Увійти" class="btn btn-primary" id="loginform-submit"/>
                                    </div>
                                </form>
                            </div>
                            <div>
                                <br />
                                <form asp-action="Register" method="get">
                                    <input type="submit" value="Реєстрація" id="loginform-register"/>
                                </form>
                                <br />
                                <form asp-action="EnterAsGuest" method="get">
                                    <input type="submit" value="Увійти як гість" id="loginform-guest"/>
                                </form>
                            </div>
                        </div>`;
        $("#pseudobody").html(rows);
        $("#loginform-submit").on("click", tryToLogin);
    }

    
    prepareIndexPage();
    getAllReviews();

    $("#ref-login").on("click", function () {
        prepareLoginPage();
    });
    $("#ref-index").on("click", function () {
        prepareIndexPage();
        getAllReviews();
    });
});