export const updateLocation = async (location) => {
  try {
    const url = 'https://localhost:44304/api/Location/LocationInfo/';

    let response = await fetch(url, {
      method: 'PUT',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(location),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const addLocation = async (location) => {
  try {
    const url = 'https://localhost:44304/api/Location/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(location),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log('Update failed.');
    console.log(ex);
    return undefined;
  }
};

export const deleteLocation = async (location) => {
  try {
    const url = 'https://localhost:44304/api/Location/Delete/';

    let response = await fetch(url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(location),
    });
    let responseJson = await response.json();
    return responseJson.result;
  } catch (ex) {
    console.log("Couldn't delete location!");
    console.log(ex);
    return undefined;
  }
};
