import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { updateStore } from './StoreCommands';

export function StorePage({ match }) {
  const [store, setStore] = useState(null);
  const [storesLoading, setStoresLoading] = useState(true);
  const [isEditing, setEditing] = useState(false);
  const [locations, setLocations] = useState(null);
  const [locationsLoading, setLocationsLoading] = useState(true);

  useEffect(() => {
    const loadStore = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/Store/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setStore(data);
      setStoresLoading(false);
    };

    const loadLocations = async () => {
      const url = 'https://localhost:44304/api/Location';
      const response = await fetch(url);
      const data = await response.json();
      setLocations(data);
      setLocationsLoading(false);
    };

    if (match.params.id) {
      const storeId = match.params.id;
      loadStore(storeId);
      loadLocations();
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleLocationChange = (event) => {
    setStore({
      id: store.id.value,
      name: store?.name,
      location: locations.find((x) => x.id === event.target.value),
    });
  };

  const handleSubmit = async () => {
    console.log(store.location);
    await updateStore({
      id: store.id,
      name: store.name,
      locationId: store.location.id,
    });
  };

  const changeHandler = (e) => {
    store.name = e.currentTarget.value;
    setStore(store);
  };

  return (
    <Page title={store?.name}>
      <Button onClick={editModeClick} className="float-right">
        Edit
      </Button>
      <div>
        {storesLoading ? (
          <div
            css={css`
              font-size: 16px;
              font-style: italic;
            `}
          >
            Loading...
          </div>
        ) : (
          <Fragment>
            <div
              css={css`
                margin-top: 5em;
              `}
            >
              {isEditing ? (
                <form onSubmit={handleSubmit}>
                  <input
                    type="text"
                    name="name"
                    label="Store name"
                    placeholder={store.name}
                    onChange={changeHandler}
                  />
                  <div
                    css={css`
                      margin: 1em;
                    `}
                  >
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
                      <div>
                        <label for="locations">Location:</label>
                        <br />
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
                  </div>
                  <input type="submit" value="Save" />
                </form>
              ) : (
                <div>
                  <h1>{store.name}</h1>
                  <h2>{store.location.city.name}</h2>
                  <h2>{store.location.country.name}</h2>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
}
