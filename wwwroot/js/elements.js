import api from "./apiMethods.js";

function managerBlock(id, name, num, isActive, hasNumber) {
  let columnsContainer = document.createElement('div');
  columnsContainer.className = 'box block manager-block';

  let columns = document.createElement('div');
  columns.className = 'columns';
  columnsContainer.appendChild(columns);

  columns.appendChild(nameColumn(name, id));
  columns.appendChild(numColumn(num, id));
  columns.appendChild(switchComponent(isActive, id, hasNumber));

  return columnsContainer;
}

function labelElement(text) {
  let label = document.createElement('label');
  label.className = 'label';
  label.innerText = text;

  return label;
}

function nameColumn(name, id) {
  let nameColumn = document.createElement('div');
  nameColumn.className = 'column is-half';

  nameColumn.appendChild(labelElement('Имя'));

  let nameParagraph = document.createElement('h4');
  nameParagraph.className = 'is-size-4';
  nameParagraph.id = "name-" + id;
  nameParagraph.innerText = name;
  nameColumn.appendChild(nameParagraph);

  return nameColumn;
}

function numColumn(num, id) {
  let numColumn = document.createElement('div');
  numColumn.className = 'column is-4';

  numColumn.appendChild(labelElement('Номер'));

  if (num === null) {
    // button with "Задать номер" goes here
    // let numComponent = document.createElement('p');
    // numComponent.className = 'is-size-5';
    // numComponent.innerText = 'Не указан';
    // numColumn.appendChild(numComponent);

    let setNumberButton = document.createElement('button');
    setNumberButton.className = 'button is-small is-primary';
    setNumberButton.innerText = 'Задать номер';
    setNumberButton.onclick = () => {
      numColumn.appendChild(numInputAddons('', id, true));
      setNumberButton.remove();
    };
    numColumn.appendChild(setNumberButton);

    return numColumn;
  }
  
  numColumn.appendChild(numInputAddons(num, id, false));
  return numColumn;
}

/**
 * 
 * @param {string} num 
 * @param {string} id 
 * @param {boolean} isStatic 
 * @returns 
 */
function numInputAddons(num, id, isStatic) {
  let result = document.createElement('div');
  result.className = 'field has-addons';

  let inputControl = document.createElement('div');
  inputControl.className = 'control';
  result.appendChild(inputControl);

  let numField = document.createElement('input');
  numField.className = 'input is-small';
  if (!isStatic) {
    numField.disabled = true;
  }
  numField.id = 'inp-' + id;
  numField.type = 'text';
  numField.value = isStatic ? '' : num;
  numField.oninput = () => {
    if (!validString(numField.value)) {
      document.getElementById(numField.id).classList.add('is-danger');
      document.getElementById('btn-' + id).classList.add('is-static');
    }
    else {
      document.getElementById(numField.id).classList.remove('is-danger');
      document.getElementById('btn-' + id).classList.remove('is-static');
    }
  };

  inputControl.appendChild(numField);

  let btnControl = document.createElement('div');
  btnControl.className = 'control';
  result.appendChild(btnControl);

  btnControl.appendChild(isStatic ? numSubmitButton(id, true) : numChangeButton(id));

  return result;
}

function switchComponent(isActive, id, hasNumber) {
  let switchColumn = document.createElement('div');
  switchColumn.className = 'column is-2';

  let bulmaSwitchContainer = document.createElement('div');
  bulmaSwitchContainer.className = 'field mt-4';
  
  let bulmaSwitch = document.createElement('input');
  bulmaSwitch.type = 'checkbox';
  bulmaSwitch.onclick = () => {
    bulmaSwitch.setAttribute('disabled', true);
    
    if (bulmaSwitch.hasAttribute('checked')) {
      api.setInactive(id).then(response => {
        console.log(response.status);
        bulmaSwitch.removeAttribute('checked');
      });
    }
    else {
      api.setActive(id).then(response => {
        console.log(response.status);
        bulmaSwitch.setAttribute('checked', 'checked');
      });
    }

    setTimeout(() => {
      // active to inactive and vice versa

      bulmaSwitch.removeAttribute('disabled');
    }, 1000);
  }
  bulmaSwitch.className = 'switch is-rounded';
  bulmaSwitch.id = 'swt-' + id;
  bulmaSwitch.name = bulmaSwitch.id;
  if (isActive) {
    bulmaSwitch.setAttribute('checked', 'checked');
  }
  if (!hasNumber) {
    bulmaSwitch.setAttribute('disabled', true);
  }

  let bulmaSwitchLabel = labelElement('Активен?');
  bulmaSwitchLabel.setAttribute('for', bulmaSwitch.name);

  bulmaSwitchContainer.appendChild(bulmaSwitch);
  bulmaSwitchContainer.appendChild(bulmaSwitchLabel);
  switchColumn.appendChild(bulmaSwitchContainer);
  return switchColumn;
}

function loadingIcon() {
  let icon = document.createElement('i');
  icon.className = 'fas fa-spinner fa-pulse is-size-1';
  
  let iconColumn = document.createElement('div');
  iconColumn.className = 'column is-6';
  iconColumn.appendChild(icon);
  
  let result = document.createElement('div');
  result.className = 'columns is-centered has-text-centered';
  result.id = 'loadingIcon'
  result.appendChild(iconColumn);
  return result;
}

/**
 * 
 * @param {string} id 
 */
function numChangeButton(id) {
  let button = document.createElement('button');
  button.className = 'button is-small is-light is-warning';
  button.innerText = 'Изменить'; 
  button.id = 'btn-' + id;
  button.onclick = () => {
    document.getElementById('inp-' + id).removeAttribute('disabled');
    document.getElementById(button.id).replaceWith(numSubmitButton(id, false));
  }
  return button;
}

/**
 * 
 * @param {string} id 
 * @param {boolean} createNew 
 * @returns 
 */
function numSubmitButton(id, createNew) {
  let button = document.createElement('button');
  button.className = createNew ? 'button is-small is-static is-primary' : 'button is-small is-primary';
  button.innerText = 'Отправить';
  button.id = 'btn-' + id;
  button.onclick = () => {
    if (createNew) {}
    document.getElementById('inp-' + id).setAttribute('disabled', 'true');
    button.classList.add('is-loading');

    // if (createNew) - insert new manager in inactive table; otherwise just modify the existing one
    // setTimeout(() => {
    //   document.getElementById(button.id).replaceWith(numChangeButton(id));
    // }, 1000);
    console.log(document.getElementById('name-' + id).innerText);

    if (createNew) {
      api.setManagerNum({
        id: id,
        username: document.getElementById('name-' + id).innerText,
        num: document.getElementById('inp-' + id).value
      }, false).then(response => {
        console.log(response.status);
        document.getElementById('swt-' + id).removeAttribute('disabled');
        document.getElementById(button.id).replaceWith(numChangeButton(id));
        // spawn toast that says "Number set successfully!"
      })
      .catch(e => console.log(e));
    }
    else {
      api.setManagerNum({
        id: id,
        username: document.getElementById('name-' + id).innerText,
        num: document.getElementById('inp-' + id).value
      }, true).then(response => {
        console.log(response.status);
        document.getElementById(button.id).replaceWith(numChangeButton(id));
        // spawn toast that says "Number changed successfully!"
      })
      .catch(e => console.log(e));
    }
  }
  return button;
}

/**
 * 
 * @param {string} str 
 */
function validString(str) {
  return (str.trim() !== '') && (!isNaN(str));
}

/**
 * 
 * @param {string} id 
 */
function retrieveId(id) {
  return id.substring(4);
}

export default { loadingIcon, managerBlock }