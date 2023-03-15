// remake everything using Fetch API

/**
 * 
 * @param {{id: string, username: string, num: string}} manager 
 * @param {boolean} alreadyExists 
 */
function setManagerNum(manager, alreadyExists) {
  let xhr = new XMLHttpRequest();
  if (alreadyExists) {
    xhr.open('PUT', '/api/Managers', false);
  }
  else {
    xhr.open('POST', '/api/Managers/true', false);
  }

  try {
    xhr.send(manager);

    if (xhr.status != 200) {
      return { status: xhr.status, message: "Что-то пошло не так. Не удалось изменить номер.", details: xhr.statusText }
    }

    return { status: xhr.status, message: "Номер успешно изменён.", details: xhr.statusText }
  }
  catch (e) {
    console.error(e);
    return { message: "Ошибка." };
  }
}

/**
 * 
 * @param {string} id 
 */
function setActive(id) {
  let xhr = new XMLHttpRequest();
  xhr.open('GET', '/api/Managers/enable/' + id, false);

  try {
    xhr.send();
    return { status: xhr.status, message: xhr.statusText };
  }
  catch (e) {
    console.log(e);
    return { message: "Ошибка." };
  }
}

/**
 * 
 * @param {string} id 
 */
function setInactive(id) {
  let xhr = new XMLHttpRequest();
  xhr.open('GET', '/api/Managers/disable/' + id, false);

  try {
    xhr.send();
    return { status: xhr.status, message: xhr.statusText };
  }
  catch (e) {
    console.log(e);
    return { message: "Ошибка." };
  }
}

export default { setActive, setInactive, setManagerNum, testRestApi }