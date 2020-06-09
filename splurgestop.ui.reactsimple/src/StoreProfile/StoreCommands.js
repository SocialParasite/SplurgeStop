export const updateStore = async (store) => {
  try {
    const url = 'https://localhost:44304/api/Store/StoreInfo/';

    let response = await fetch(url, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(store),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const addStore = async (store) => {
  try {
    const url = 'https://localhost:44304/api/Store/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(store),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const deleteStore = async (store) => {
  try {
    const url = 'https://localhost:44304/api/Store/Delete/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(store),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log("Couldn't delete store!");
    console.log(ex);
    return undefined;
  }
};
