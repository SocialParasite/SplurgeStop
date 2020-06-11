import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { addPurchaseTransaction } from './PurchaseTransactionCommands';

export function NewPurchaseTransaction() {
  //https://stackoverflow.com/questions/54163796/react-usestate-hook-setter-has-no-result-with-array-variable

  //const [transaction, setTransaction] = useState(null);
  const [transaction, setTransaction] = useState({
    transactionDate: '2020-06-09',
    storeId: '6c05f0ce-2e1d-4ba9-a0fa-18a465b194e0',
    lineItems: [
      {
        amount: 1.23,
        currencyCode: 'EUR',
        currencySymbol: '€',
        booking: 'Credit',
        positionRelativeToPrice: 'end',
        notes: 'Line item 1',
      },
      {
        amount: 11.23,
        currencyCode: 'EUR',
        currencySymbol: '€',
        booking: 'Credit',
        positionRelativeToPrice: 'end',
        notes: 'Line item 2',
      },
    ],
  });

  const [stores, setStores] = useState(null);
  const [storesLoading, setStoresLoading] = useState(true);

  const totalPrice = () =>
    transaction.lineItems.reduce((sum, item) => sum + item.amount, 0);

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

  const handleInputChange = (event) => {
    setTransaction({
      id: null,
      purchaseDate: null,
      store: null,
      lineItems: null,
    });
  };

  const handleSubmit = async () => {
    await addPurchaseTransaction({
      id: null,
      transactionDate: null,
      store: null,
      lineItems: null,
    });
  };

  return (
    <Page title="Add new purchase transaction">
      <div>
        <Fragment>
          <div>
            <form>
              <p>
                <b>
                  <input
                    type="date"
                    id="purchaseDate"
                    name="purchaseDate"
                  ></input>
                </b>
                &nbsp;&nbsp;&nbsp;
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
                  <div>
                    <label for="stores">Select a store:</label>
                    <select name="stores" id="stores">
                      {stores.map((store) => (
                        <option>{store.name}</option>
                      ))}
                    </select>
                  </div>
                )}
              </p>
              <div>
                Total: {totalPrice().toFixed(2)}{' '}
                {transaction.lineItems[0].currencySymbol}{' '}
              </div>
              <div>
                <lable for="notes">Notes:</lable>
                <textarea id="notes" name="notes" />
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
                    {transaction?.lineItems == null ? (
                      <tr>
                        <td contentEditable="true">
                          Product name goes here...
                        </td>
                        <td contentEditable="true">price here</td>
                        <td contentEditable="true">notes</td>
                      </tr>
                    ) : (
                      transaction.lineItems.map((lineItem) => (
                        <tr>
                          <td contentEditable="true">{lineItem.booking}</td>
                          <td contentEditable="true">{lineItem.amount}</td>
                          <td contentEditable="true">{lineItem.notes}</td>
                        </tr>
                      ))
                    )}
                  </tbody>
                  <Button>+</Button>
                </Table>
              </div>
              <button type="submit">Save</button>
            </form>
          </div>
        </Fragment>
      </div>
    </Page>
  );
}
