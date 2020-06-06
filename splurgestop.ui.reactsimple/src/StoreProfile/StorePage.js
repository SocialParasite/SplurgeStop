import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../Components/Page';

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
      return null;
    };

    loadStore();
  });

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const changeHandler = (e) => {
    console.log(e.currentTarget.name);
    console.log(e.currentTarget.value);
  };

  return (
    <Page title={store?.name}>
      <div>
        <button onClick={editModeClick}>Edit</button>
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
              <div>
                {isEditing ? (
                  <form submitCaption="Save" onChange={changeHandler}>
                    <input type="text" name="name" label="Store name" />
                  </form>
                ) : (
                  <div>
                    <h1>{store.name}</h1>
                  </div>
                )}
              </div>
            </Fragment>
          )}
        </div>
      </div>
    </Page>
  );
}
