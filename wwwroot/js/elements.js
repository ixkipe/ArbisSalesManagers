function managerBlock(id, name, num, isActive) {
  let columnsContainer = document.createElement('div');
  columnsContainer.className = 'box block';

  let columns = document.createElement('div');
  columns.className = 'columns';
  columnsContainer.appendChild(columns);

  columns.appendChild(nameColumn(name));
  columns.appendChild(numColumn(num, id));
  columns.appendChild(switchComponent(isActive, id));

  return columnsContainer;
}

function labelElement(text) {
  let label = document.createElement('label');
  label.className = 'label';
  label.innerText = text;

  return label;
}

function nameColumn(name) {
  let nameColumn = document.createElement('div');
  nameColumn.className = 'column is-half';

  nameColumn.appendChild(labelElement('Имя'));

  let nameParagraph = document.createElement('h4');
  nameParagraph.className = 'is-size-4';
  nameParagraph.innerText = name;
  nameColumn.appendChild(nameParagraph);

  return nameColumn;
}

function numColumn(num, id) {
  let numColumn = document.createElement('div');
  numColumn.className = 'column is-4';

  numColumn.appendChild(labelElement('Номер'));

  if (num === null) {
    let numComponent = document.createElement('p');
    numComponent.className = 'is-size-5';
    numComponent.innerText = 'Не указан';
    numColumn.appendChild(numComponent);
    return numColumn;
  }
  
  numColumn.appendChild(numComponentIfNumberExists(num, id));

  return numColumn;
}

function numComponentIfNumberExists(num, id) {
  let numComponent = document.createElement('div');
  numComponent.className = 'columns';

  let numFieldContainer = halfColumn();
  let buttonContainer = halfColumn();

  let numFieldControl = document.createElement('div');
  numFieldControl.className = 'control';
  numFieldContainer.appendChild(numFieldControl);

  let numField = document.createElement('input');
  numField.className = 'input is-small';
  numField.id = 'inp-' + id;
  numField.disabled = true;
  numField.type = 'text';
  numField.value = num;
  numField.oninput = () => {
    if (!validString(numField.value)) {
      document.getElementById(numField.id).classList.add('is-danger');
      document.getElementById('btn-' + id).setAttribute('disabled', 'true');
    }
    else {
      document.getElementById(numField.id).classList.remove('is-danger');
      document.getElementById('btn-' + id).removeAttribute('disabled');
    }
  };
  numFieldControl.appendChild(numField);

  let button = numChangeButton(id);
  buttonContainer.appendChild(button);

  numComponent.appendChild(numFieldContainer);
  numComponent.appendChild(buttonContainer);
  return numComponent;
}

function halfColumn() {
  let column = document.createElement('div');
  column.className = 'column is-half';
  return column;
}

function switchComponent(isActive, id) {
  let switchColumn = document.createElement('div');
  switchColumn.className = 'column is-2';

  let bulmaSwitchContainer = document.createElement('div');
  bulmaSwitchContainer.className = 'field mt-4';
  
  let bulmaSwitch = document.createElement('input');
  bulmaSwitch.type = 'checkbox';
  bulmaSwitch.className = 'switch is-rounded';
  bulmaSwitch.id = 'swt-' + id;
  bulmaSwitch.name = bulmaSwitch.id;
  if (isActive) {
    bulmaSwitch.setAttribute('checked', 'checked');
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
 * @param {boolean} isSubmit 
 */
function numChangeButton(id) {
  let button = document.createElement('button');
  button.className = 'button is-small is-light is-warning';
  button.innerText = 'Изменить';
  button.id = 'btn-' + id;
  button.onclick = () => {
    document.getElementById('inp-' + id).removeAttribute('disabled');
    document.getElementById(button.id).replaceWith(numSubmitButton(id));
  }
  return button;
}

function numSubmitButton(id) {
  let button = document.createElement('button');
  button.className = 'button is-small is-primary';
  button.innerText = 'Отправить';
  button.id = 'btn-' + id;
  button.onclick = () => {
    document.getElementById('inp-' + id).setAttribute('disabled', 'true');
    button.classList.add('is-loading');
    setTimeout(() => {
      document.getElementById(button.id).replaceWith(numChangeButton(id));
    }, 1000);
  }
  return button;
}

/**
 * 
 * @param {string} str 
 */
function validString(str) {
  return str.trim() !== '';
}

/**
 * 
 * @param {string} id 
 */
function retrieveId(id) {
  return id.substring(4);
}

export default { loadingIcon, switchComponent, halfColumn, numComponentIfNumberExists, numColumn, nameColumn, labelElement, managerBlock }