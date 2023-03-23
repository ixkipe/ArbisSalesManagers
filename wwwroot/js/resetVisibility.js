function resetVisibility() {
  document.querySelectorAll('.manager-block').forEach(block => block.classList.remove('is-hidden'));
}

export default resetVisibility;