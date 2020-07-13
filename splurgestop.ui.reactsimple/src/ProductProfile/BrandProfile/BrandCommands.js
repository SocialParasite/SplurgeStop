export const updateBrand = async (brand) => {
  try {
    const url = 'https://localhost:44304/api/Brand/BrandInfo/';

    let response = await fetch(url, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(brand),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const addBrand = async (brand) => {
  try {
    const url = 'https://localhost:44304/api/Brand/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(brand),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const deleteBrand = async (brand) => {
  try {
    const url = 'https://localhost:44304/api/Brand/Delete/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(brand),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log("Couldn't delete the brand!");
    console.log(ex);
    return undefined;
  }
};
