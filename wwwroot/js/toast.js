let pooTimeout = undefined;

function ShowToast() {
  let x = document.getElementById('snackbar');
  x.classList.remove('show');
  setTimeout(() => { x.classList.add('show'); }, 100);

  if (pooTimeout !== undefined) {
    clearTimeout(pooTimeout);
  }
  pooTimeout = setTimeout(() => { x.classList.remove('show') }, 3000);
}

export default ShowToast;