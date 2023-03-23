import elements from "./elements.js";
import apiMethods from "./apiMethods.js";
import search from "./search.js";
import clearField from "./clearButton.js";

const allManagersUrl = '/api/Managers?active=false';
let managersHttpResponse;
let active, inactive, unassigned;
let editedNumId = undefined;

async function updateManagerList() {
  await fetch(allManagersUrl)
    .then(response => response.json())
    .then(r => {
      managersHttpResponse = r;
      console.log(r)
    });

  assignToGroups();
}

function displayManagerList() {
  const mainContent = document.getElementById('mainContent');

  active.forEach(manager => {
    mainContent.appendChild(elements.managerBlock(manager.id, manager.username, manager.num, true, true));
  });

  inactive.forEach(manager => {
    mainContent.appendChild(elements.managerBlock(manager.id, manager.username, manager.num, false, true));
  });
  console.log(inactive.length);

  unassigned.forEach(manager => {
    mainContent.appendChild(elements.managerBlock(manager.id, manager.username, manager.num, false, false))
  });

  document.querySelector('#searchBar').classList.remove('is-hidden');
}

function assignToGroups() {
  active = managersHttpResponse.active;
  inactive = managersHttpResponse.inactive;
  unassigned = managersHttpResponse.unassigned;
}

// DO NOT CHANGE CODE BELOW

function openModal($el) {
  $el.classList.add('is-active');
}

function closeModal($el) {
  $el.classList.remove('is-active');

  if (editedNumId !== undefined) {
    document.getElementById('swt-' + editedNumId).removeAttribute('checked');
    editedNumId = undefined;
  }
}

function closeAllModals() {
  (document.querySelectorAll('.modal') || []).forEach(($modal) => {
    closeModal($modal);
  });
}

document.addEventListener('DOMContentLoaded', async () => {
  // delete from final version
  document.querySelector('#testRest').onclick = async () => {
    // console.log((await apiMethods.testMethod()).result);
    
  };

  let searchBar = document.getElementById('searchInput');
  searchBar.oninput = () => {
    search(searchBar.value);
  };
  document.getElementById('clearSearch').onclick = clearField;

  // Add a click event on buttons to open a specific modal
  (document.querySelectorAll('.js-modal-trigger') || []).forEach(($trigger) => {
    const modal = $trigger.dataset.target;
    const $target = document.getElementById(modal);

    $trigger.addEventListener('click', () => {
      openModal($target);
    });
  });

  // Add a click event on various child elements to close the parent modal
  (document.querySelectorAll('.modal-background, .modal-close, .modal-card-head .delete, .modal-card-foot .button') || []).forEach(($close) => {
    const $target = $close.closest('.modal');

    $close.addEventListener('click', () => {
      closeModal($target);
    });
  });

  // Add a keyboard event to close all modals
  document.addEventListener('keydown', (event) => {
    const e = event || window.event;

    if (e.keyCode === 27) { // Escape key
      closeAllModals();
    }
  });

  document.getElementById('mainContent').appendChild(elements.loadingIcon());
  await updateManagerList();
  document.getElementById('mainContent').removeChild(document.getElementById('loadingIcon'));
  displayManagerList();
});