import clearField from './clearButton.js';

/**
 * 
 * @param {string} name 
 */
function search(name) {
  if (name.trim() == '') {
    clearField();
    return;
  }

  if (name !== '') {
    document.getElementById('searchDeleteButton').classList.remove('is-hidden');
  }

  name = name.toLowerCase();
  const managerBlocks = document.querySelectorAll('.manager-block');
  
  managerBlocks.forEach(block => {
    // console.log(block.getElementsByClassName('is-size-4').item(0).innerText);

    if (block.getElementsByClassName('is-size-4').item(0).innerText.toLowerCase().indexOf(name) < 0) {
      block.classList.add('is-hidden');
    }
  });
}

export default search