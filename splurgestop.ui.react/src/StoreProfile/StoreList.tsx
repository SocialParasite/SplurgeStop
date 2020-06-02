import React, { FC, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { StoreData, getStores } from './StoreData';
import { Store } from './Store';
import { Page } from './../Page';
import { PageTitle } from './../PageTitle';

import { Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

interface Props {
  data: StoreData[];
  renderItem?: (item: StoreData) => JSX.Element;
}

export const StoreList: FC<Props> = () => {
  const [stores, setStores] = useState<StoreData[] | null>(null);
  const [storesLoading, setStoresLoading] = useState(true);

  useEffect(() => {
    let cancelled = false;
    const doGetStores = async () => {
      const stores = await getStores();
      if (!cancelled) {
        setStores(stores);
        setStoresLoading(false);
      }
    };
    doGetStores();
    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <Page>
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
          <PageTitle>Stores</PageTitle>
        </div>
        <div>
          {storesLoading ? (
            <div>Loading...</div>
          ) : (
            <Table bordered hover size="sm">
              <thead>
                <tr
                  css={css`
                    background: burlywood;
                    text-align: left;
                  `}
                >
                  <th>Store</th>
                </tr>
              </thead>
              {stores?.map((store) => (
                <tbody key={store.id}>
                  <tr
                    css={css`
                      text-align: right;
                    `}
                  >
                    <Store data={store} />
                  </tr>
                </tbody>
              ))}
            </Table>
          )}
        </div>
      </div>
    </Page>
  );
};
