import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { addStore } from './StoreCommands';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.min.css';

export function NewStore() {
  const [store, setStore] = useState(null);
  const [locations, setLocations] = useState(null);
  const [locationsLoading, setLocationsLoading] = useState(true);

  useEffect(() => {
    const loadLocations = async () => {
      const url = 'https://localhost:44304/api/Location';
      const response = await fetch(url);
      const data = await response.json();
      setLocations(data);
      setLocationsLoading(false);
    };

    loadLocations();
  }, []);

  const handleInputChange = (event) => {
    setStore({
      id: null,
      name: event.target.value,
      locationId: store?.locationId,
    });
  };

  const handleLocationChange = (event) => {
    setStore({
      id: null,
      name: store?.name,
      locationId: event.target.value,
    });
  };

  const notify = (info) => {
    toast.info(info);
  };

  const handleSubmit = async () => {
    console.log(store);
    let error = await addStore({
      id: null,
      name: store?.name,
      locationId: store?.locationId,
    }).then(
      () => null,
      (store) => store,
    );

    if (error === null) {
      notify('Store added');
    } else {
      toast.error(
        <div>
          Store not added!
          <br />
          {error.message}
        </div>,
      );
    }
  };

  return (
    <Page title="Add new store">
      <Fragment>
        <ToastContainer />
        <div>
          <form onSubmit={handleSubmit}>
            <label>Store name:</label>
            <input
              type="text"
              id="storeName"
              name="name"
              title="Store name"
              onChange={handleInputChange}
              placeholder="store name"
            />
            {locationsLoading ? (
              <div
                css={css`
                  font-size: 16px;
                  font-style: italic;
                `}
              >
                Loading...
              </div>
            ) : (
              <div
                css={css`
                  margin: 1em;
                `}
              >
                <label for="locations">Select a store location:</label>
                <select
                  name="locationId"
                  id="locationId"
                  className="input"
                  type="text"
                  onChange={handleLocationChange}
                >
                  <option>Select store location</option>
                  {locations.map((location) => (
                    <option value={location.id} key={location.id}>
                      {location.cityName}, {location.countryName}
                    </option>
                  ))}
                </select>
              </div>
            )}
            <input type="submit" value="Save" />
          </form>
        </div>
      </Fragment>
    </Page>
  );
}
