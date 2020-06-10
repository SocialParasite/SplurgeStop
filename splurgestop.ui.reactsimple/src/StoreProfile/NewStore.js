import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { addStore } from './StoreCommands';

export function NewStore() {
  const [store, setStore] = useState(null);

  const handleInputChange = (event) => {
    setStore({
      id: null,
      name: event.target.value,
    });
  };

  const handleSubmit = async () => {
    await addStore({
      id: null,
      name: store.name,
    });
  };

  return (
    <Page title="Add new store">
      <Fragment>
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
            <input type="submit" value="Save" />
          </form>
        </div>
      </Fragment>
    </Page>
  );
}
