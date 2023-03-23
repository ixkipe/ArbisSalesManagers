import * as bulmaToast from '../css/bulma-toast-2.4.2/src/index.js';

/**
 * 
 * @param {string} managerName 
 */
function numberSetToast(managerName) {
  bulmaToast.toast({
    message: `${managerName}: номер успешно изменён!`,
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
  bulmaToast.toast({
    message: `${name} ` + (switchOn ? 'включен(а) в систему логирования.' : 'отключен(а) от системы логирования.'),
    type: switchOn ? 'is-success' : 'is-danger',
    dismissible: true,
    position: 'bottom-left',
    animate: { in: 'bounceInLeft', out: 'bounceOutLeft' }
  });
}

export default { numberSetToast, toggledSwitchToast }