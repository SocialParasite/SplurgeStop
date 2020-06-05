import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';

export function StoreHome() {
  const [stores, setStores] = useState(null);
  const [storesLoading, setStoresLoading] = useState(true);

  useEffect(() => {
    const loadStores = async () => {
      const url = 'https://localhost:44304/api/Store';
      const response = await fetch(url);
      const data = await response.json();
      setStores(data);
      setStoresLoading(false);
      return null;
    };

    loadStores();
  }, []);

  return (
    <div
      css={css`
        margin: 50px auto 20px auto;
        padding: 30px 12px;
        max-width: 1600px;
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
                text-align: left;
              `}
            >
              <th>Store name</th>
            </tr>
          </thead>
          {stores.map((store) => (
            <tbody>
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
                </Fragment>
              </tr>
            </tbody>
          ))}
        </Table>
      )}
    </div>
  );
}
