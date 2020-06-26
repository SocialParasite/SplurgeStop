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

  useEffect(() => {
    const loadStore = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/Store/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setStore(data);
      setStoresLoading(false);
    };

    if (match.params.id) {
      const storeId = match.params.id;
      loadStore(storeId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const handleSubmit = async () => {
    await updateStore({
      id: store.id.value,
      name: store.name,
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
