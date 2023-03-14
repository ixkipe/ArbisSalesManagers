document.addEventListener('DOMContentLoaded', () => {
  let usernameField = document.getElementById('username-field');
  let passwordField = document.getElementById('password-field');
  let submitBtn = document.getElementById('submit-button');
  let modelValidation = document.getElementById('model-validation');
  let usernameValidation = document.getElementById('username-validation');
  let passwordValidation = document.getElementById('password-validation');
  const validInputs = [false, false];

  function toggleButton() {
    if (validInputs[0] === true && validInputs[1] === true) {
      submitBtn.removeAttribute('disabled');
      return;
    }

    submitBtn.setAttribute('disabled', 'true');
  }

  if (modelValidation !== undefined && modelValidation !== null) {
    if (modelValidation.innerHTML !== '') {
      usernameField.classList.add('is-danger');
      passwordField.classList.add('is-danger');
    }
  }

  submitBtn.onclick = () => {
    if (!submitBtn.hasAttribute('disabled')) {
      submitBtn.classList.add('is-loading');
    }
  };

  if (usernameValidation.innerHTML.length > 0) {
    usernameField.classList.add('is-danger');
  }

  if (passwordValidation.innerHTML.length > 0) {
    passwordField.classList.add('is-danger');
  }

  usernameField.oninput = () => {
    validInputs[0] = usernameField.value.length != 0 ? true : false;
    toggleButton();

    if (!validInputs[0]) {
      usernameField.classList.add('is-danger');
      usernameValidation.innerHTML = 'Пожалуйста, введите имя пользователя.'
      return;
    }

    usernameValidation.innerHTML = '';
    usernameField.classList.remove('is-danger');
  }

  passwordField.oninput = () => {
    validInputs[1] = passwordField.value.length != 0 ? true : false;
    toggleButton();

    if (!validInputs[1]) {
      passwordField.classList.add('is-danger');
      passwordValidation.innerHTML = 'Пожалуйста, введите пароль.'
      return;
    }
    
    passwordValidation.innerHTML = '';
    passwordField.classList.remove('is-danger');
  }
});