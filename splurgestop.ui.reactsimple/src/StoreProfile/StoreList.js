import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../Components/Page';
import { deleteStore } from './StoreCommands';

export function StoreList() {
  const [stores, setStores] = useState(null);
  const [storesLoading, setStoresLoading] = useState(true);

  useEffect(() => {
    const loadStores = async () => {
      const url = 'https://localhost:44304/api/Store';
      const response = await fetch(url);
      const data = await response.json();
      setStores(data);
      setStoresLoading(false);
    };

    loadStores();
  }, []);

  const removeItem = (index) => {
    let data = stores.filter((_, i) => i !== index);
    setStores(data);
  };

  const handleDelete = async (store) => {
    let index = stores.findIndex((s) => s.id === store.id);
    removeItem(index);

    await deleteStore({
      id: store.id,
    });
  };

  return (
    <Page title="Stores">
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`Store/Add`}
      >
        {' '}
        <Button className="float-right">Add Store</Button>
      </Link>
      <div
        css={css`
          margin: 50px auto 20px auto;
          padding: 30px 12px;
        `}
      >
        <div
          css={css`
            display: flex;
            align-items: center;
            justify-content: space-between;
          `}
        >
          <title>Stores</title>
        </div>
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
          <Table bordered hover size="sm">
            <thead>
              <tr
                css={css`
                  background: burlywood;
                  text-align: center;
                  text-transform: uppercase;
                `}
              >
                <th>Store name</th>
                <th>Details</th>
                <th>Remove</th>
              </tr>
            </thead>
            {stores.map((store) => (
              <tbody key={store.id}>
                <tr>
                  <Fragment key={store.id}>
                    <td>
                      <Link
                        css={css`
                          text-decoration: none;
                        `}
                        to={`StoreInfo/${store.id}`}
                      >
                        {store.name}
                      </Link>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button variant="info" href={`StoreInfo/${store.id}`}>
                        Show
                      </Button>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button
                        variant="danger"
                        onClick={() => {
                          handleDelete(store);
                        }}
                      >
                        Delete
                      </Button>
                    </td>
                  </Fragment>
                </tr>
              </tbody>
            ))}
          </Table>
        )}
      </div>
    </Page>
  );
}
