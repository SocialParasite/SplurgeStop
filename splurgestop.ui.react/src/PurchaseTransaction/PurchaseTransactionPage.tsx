import React, { FC, useState, Fragment, useEffect } from 'react';
import { Page } from './../Page';
import { RouteComponentProps } from 'react-router-dom';
import {
  DetailedPurchaseTransactionData,
  getPurchaseTransaction,
} from './PurchaseTransactionData';
import { Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

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
                <p>{purchaseTransaction.purchaseDate.value}</p>
              </div>
              <div>{purchaseTransaction.totalPrice} </div>
              <div>
                <p>{purchaseTransaction.notes}</p>
              </div>
              <div>
                <Table bordered hover size="sm">
                  <thead>
                    <tr>
                      <th>Product</th>
                      <th>Price</th>
                      <th>Notes</th>
                    </tr>
                  </thead>
                  <tbody>
                    {purchaseTransaction.lineItems.map((item, i) => {
                      return (
                        <tr key={i}>
                          <td>
                            {console.log(
                              item.price.currency.positionRelativeToSource,
                            )}
                            Product name goes here...
                          </td>
                          <td>
                            {item.price.currency.positionRelativeToSource ===
                            'end'
                              ? String(
                                  item.price.amount +
                                    ' ' +
                                    item.price.currency.currencySymbol,
                                )
                              : String(
                                  item.price.currency.currencySymbol +
                                    ' ' +
                                    item.price.amount,
                                )}
                          </td>
                          <td>{item.price.currency.currencyCode}</td>
                        </tr>
                      );
                    })}
                  </tbody>
                </Table>
              </div>
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
};
