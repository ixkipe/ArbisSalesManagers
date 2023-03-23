import * as bulmaToast from '../css/bulma-toast-2.4.2/src/index.js';

/**
 * 
 * @param {string} managerName 
 */
function numberSetToast(managerName) {
  let text = document.createElement('span');
  let nameInBold = document.createElement('strong');

  nameInBold.innerText = managerName;

  text.appendChild(nameInBold);
  text.innerHTML += ': номер успешно изменён!';

  bulmaToast.toast({
    message: text,
    type: 'is-success',
    dismissible: true,
    position: 'bottom-left',
    animate: { in: 'bounceInLeft', out: 'bounceOutLeft' }
  });
}

/**
 * 
 * @param {string} name 
 * @param {boolean} switchOn 
 */
function toggledSwitchToast(name, switchOn) {
  let text = document.createElement('span');
  let nameInBold = document.createElement('strong');

  nameInBold.innerText = name;

  text.appendChild(nameInBold);
  text.innerHTML += (switchOn ? ' включен(а) в систему логирования.' : ' отключен(а) от системы логирования.');

  bulmaToast.toast({
    message: text,
    type: switchOn ? 'is-success' : 'is-danger',
    dismissible: true,
    position: 'bottom-left',
    animate: { in: 'bounceInLeft', out: 'bounceOutLeft' }
  });
}

export default { numberSetToast, toggledSwitchToast }