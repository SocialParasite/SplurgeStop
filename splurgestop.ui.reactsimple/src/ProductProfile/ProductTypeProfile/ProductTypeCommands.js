export const updateProductType = async (productType) => {
  try {
    const url = 'https://localhost:44304/api/ProductType/ProductTypeInfo/';

    let response = await fetch(url, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(productType),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const addProductType = async (productType) => {
  try {
    const url = 'https://localhost:44304/api/ProductType/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(productType),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const deleteProductType = async (productType) => {
  try {
    const url = 'https://localhost:44304/api/ProductType/Delete/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(productType),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log("Couldn't delete the product type!");
    console.log(ex);
    return undefined;
  }
};
