import resetVisibility from "./resetVisibility.js";

function clearField() {
  resetVisibility();
  document.getElementById('searchInput').value = '';
  document.getElementById('searchDeleteButton').classList.add('is-hidden');
}

export default clearField;