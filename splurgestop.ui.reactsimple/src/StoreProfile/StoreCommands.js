export const updateStore = async (store) => {
  try {
    const id = store.id;
    const url = 'https://localhost:44304/api/Store/StoreInfo/';

    console.log(JSON.stringify(store));
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
    console.log("Couldn't update store!");
    console.log(ex);
    return undefined;
  }
};
