/**
 * 
 * @param {string} option 
 */
function sortBlocks(option) {
  if (option == 'active') {
    sortBlocks('has-number');
  }
  const arr = [];
  document.querySelectorAll('.manager-block').forEach(block => {
    arr.push(block);
    block.remove();
  });
  
  arr.sort((a, b) => {
    switch (option) {
      case 'name':
        let aName = a.querySelector('.name-paragraph').innerText;
        let bName = b.querySelector('.name-paragraph').innerText;
        return aName < bName ? -1 : aName > bName ? 1 : 0;

      case 'num':
        let aNum = a.querySelector('.num-input-field') || { value: -9999 };
        let bNum = b.querySelector('.num-input-field') || { value: -9999 };
        return parseInt(aNum.value) < parseInt(bNum.value) ? 1 : parseInt(aNum.value) > parseInt(bNum.value) ? -1 : 0;

      case 'active':
        let aActive = a.querySelector('.switch.is-rounded').hasAttribute('checked');
        let bActive = b.querySelector('.switch.is-rounded').hasAttribute('checked');
        return (aActive && bActive) || (!aActive && !bActive) ? 0 : aActive ? -1 : 1;

      case 'has-number':
        let aField = a.querySelector('.num-input-field');
        let bField = b.querySelector('.num-input-field');
        return (aField === null && bField === null) || (aField !== null && bField !== null) ? 0 : bField === null ? -1 : 1;
    }
  });

  const mainContent = document.querySelector('#mainContent');
  arr.forEach(el => mainContent.appendChild(el));
}

document.addEventListener('DOMContentLoaded', () => {
  document.getElementById('sortingDropdownContainer').onclick = () => {
    document.getElementById('sortingDropdownContainer').classList.toggle('is-active');
  };
  document.getElementById('sortingOptionName').onclick = () => sortBlocks('name');
  document.getElementById('sortingOptionNum').onclick = () => sortBlocks('num');
  document.getElementById('sortingOptionActive').onclick = () => sortBlocks('active');

});

document.addEventListener('click', (e) => {
  if (!e.target.classList.contains('dropdown-keep-open')) {
    document.querySelector('#sortingDropdownContainer').classList.remove('is-active');
  }
});