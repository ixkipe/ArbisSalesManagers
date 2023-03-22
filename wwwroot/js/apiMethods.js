/**
 * 
 * @param {{id: string, username: string, num: string}} manager 
 * @param {boolean} alreadyExists 
 */
async function setManagerNum(manager, alreadyExists) {
  let xhr;
  if (alreadyExists) {
    xhr = await fetch(
      '/api/Managers/',
      {
        method: 'PUT',
        headers: {
          "Content-Type": 'application/json'
        },
        body: JSON.stringify(manager)
      }
    );
  }
  else {
    xhr = await fetch(
      '/api/Managers/true/',
      {
        method: 'POST',
        headers: {
          "Content-Type": 'application/json'
        },
        body: JSON.stringify(manager)
      }
    );
  }
  
  return xhr;
}

/**
 * 
 * @param {string} id 
 */
async function setActive(id) {
  const xhr = await fetch(
    '/api/Managers/enable/' + id, { method: 'GET' }
  );
  return xhr;
}

/**
 * 
 * @param {string} id 
 */
async function setInactive(id) {
  const xhr = await fetch(
    '/api/Managers/disable/' + id, { method: 'GET' }
  );
  return xhr;
}

// delete from final version
async function testMethod() {
  const xhr = await fetch(
    '/api/Managers/test', { method: 'GET' }
  );
  return xhr.json();
}

export default { setActive, setInactive, setManagerNum, testMethod }