import React, { FC, useState, Fragment, useEffect } from 'react';
import { Page } from './../Page';
import { RouteComponentProps } from 'react-router-dom';
import {
  DetailedPurchaseTransactionData,
  getPurchaseTransaction,
  mapDetailedPurchaseTransactionFromServer,
  DetailedPurchaseTransactionDataFromServer,
} from './PurchaseTransactionData';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';

interface RouteParams {
  id: string;
}

export const PurchaseTransactionPage: FC<RouteComponentProps<RouteParams>> = ({
  match,
}) => {
  const [
    purchaseTransaction,
    setPurchaseTransaction,
  ] = useState<DetailedPurchaseTransactionData | null>(null);

  useEffect(() => {
    const doGetPurchaseTransaction = async (id: string) => {
      const foundPurchaseTransaction = await getPurchaseTransaction(id);
      setPurchaseTransaction(foundPurchaseTransaction);
    };

    if (match.params.id) {
      const purchaseTransactionId = match.params.id;
      doGetPurchaseTransaction(purchaseTransactionId);
    }
  }, [match.params.id]);
  return (
    <Page>
      <div>
        {purchaseTransaction !== null && (
          <Fragment>
            <div>
              <p>{purchaseTransaction.store.name}</p>
            </div>
            <div>
              <div>
                <p>
                  {purchaseTransaction.purchaseDate.value}
                  {/* {new Date(String(purchaseTransaction.purchaseDate.value))} */}
                </p>
              </div>
              <div>{purchaseTransaction.totalPrice} </div>
              <div>
                <p>{purchaseTransaction.notes}</p>
              </div>
              <div>{purchaseTransaction.id.value}</div>
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
};
