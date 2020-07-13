export const updateCountry = async (country) => {
  try {
    const url = 'https://localhost:44304/api/Country/CountryInfo/';

    let response = await fetch(url, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(country),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const addCountry = async (country) => {
  try {
    const url = 'https://localhost:44304/api/Country/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(country),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const deleteCountry = async (country) => {
  try {
    const url = 'https://localhost:44304/api/Country/Delete/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(country),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log("Couldn't delete the country!");
    console.log(ex);
    return undefined;
  }
};
