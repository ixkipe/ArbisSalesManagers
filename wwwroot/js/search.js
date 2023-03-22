/**
 * 
 * @param {string} name 
 */
function search(name) {
  const managerBlocks = document.querySelectorAll('.manager-block');
  
  managerBlocks.forEach(block => {
    console.log(block.getElementsByClassName('is-size-4').item(0).innerText);
  });
}

export default search