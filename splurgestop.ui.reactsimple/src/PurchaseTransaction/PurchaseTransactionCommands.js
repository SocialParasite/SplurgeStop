// export const updateStore = async (store) => {
//     try {
//       const url = 'https://localhost:44304/api/Store/StoreInfo/';

//       let response = await fetch(url, {
//         method: 'PUT',
//         headers: {
//           Accept: 'application/json',
//           'Content-Type': 'application/json',
//         },
//         body: JSON.stringify(store),
//       });
//       let responseJson = await response.json();
//       return responseJson.result;
//     } catch (ex) {
//       console.log('Update failed.');
//       console.log(ex);
//       return undefined;
//     }
//   };

export const addPurchaseTransaction = async (transaction) => {
  try {
    const url = 'https://localhost:44304/api/PurchaseTransaction/';
    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(transaction),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Purchase transaction not added.');
    throw ex;
  }
};

export const deletePurchaseTransaction = async (transaction) => {
  try {
    const url = 'https://localhost:44304/api/PurchaseTransaction/Delete/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(transaction),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log("Couldn't delete purchase transaction!");
    console.log(ex);
    return undefined;
  }
};
