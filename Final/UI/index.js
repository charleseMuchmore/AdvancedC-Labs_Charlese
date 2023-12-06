function handleLoginClick() {
    let loginDiv = document.getElementById("login");
    let loginFormHtml = `<form>
    <h3>Login</h3>
    <div class="form-group">
        <label for="username">username</label>
        <input type="text" id="username"/>
    </div><br/>
    <div class="form-group">
        <label for="password">password</label>
        <input type="text" id="password"/>
    </div>
    <div class="form-group">
        <input type="submit"/>
    </div>
</form>`;
    loginDiv.innerHTML = loginFormHtml;
}


const loginBtn = document.getElementById("loginBtn");
loginBtn.addEventListener("click", handleLoginClick);