import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { addPurchaseTransaction } from './PurchaseTransactionCommands';

export function NewPurchaseTransaction() {
  const [transaction, setTransaction] = useState({
    transactionDate: '',
    storeId: '',
    lineItems: [
      {
        amount: 0.0,
        currencyCode: 'EUR',
        currencySymbol: '€',
        booking: 'Credit',
        positionRelativeToPrice: 'end',
        notes: '',
      },
    ],
  });

  // const [transaction, setTransaction] = useState({
  //   transactionDate: '2020-06-09',
  //   storeId: '6c05f0ce-2e1d-4ba9-a0fa-18a465b194e0',
  //   lineItems: [
  //     {
  //       amount: 1.23,
  //       currencyCode: 'EUR',
  //       currencySymbol: '€',
  //       booking: 'Credit',
  //       positionRelativeToPrice: 'end',
  //       notes: 'Line item 1',
  //     },
  //     {
  //       amount: 11.23,
  //       currencyCode: 'EUR',
  //       currencySymbol: '€',
  //       booking: 'Credit',
  //       positionRelativeToPrice: 'end',
  //       notes: 'Line item 2',
  //     },
  //   ],
  // });

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

  const handleChange = (idx) => (e) => {
    const { name, value } = e.target;
    const lineItems = [...transaction.lineItems];
    lineItems[idx] = {
      [name]: value,
    };
    setTransaction({
      transactionDate: transaction.transactionDate,
      storeId: transaction.storeId,
      lineItems,
    });
  };

  const handleAddRow = () => {
    const item = {
      product: 'new prod',
      amount: 0.0,
      notes: '',
    };

    setTransaction({
      transactionDate: transaction.transactionDate,
      storeId: transaction.storeId,
      lineItems: [...transaction.lineItems, item],
    });
  };

  const handleRemoveSpecificRow = (idx) => () => {
    const lineItems = [...transaction.lineItems];
    lineItems.splice(idx, 1);
    setTransaction({ lineItems });
  };

  const handleSubmit = async () => {
    await addPurchaseTransaction({
      id: null,
      transactionDate: transaction.transactionDate,
      store: transaction.storeId,
      lineItems: transaction.lineItems,
    });
  };

  return (
    <Page title="Add new purchase transaction">
      <div>
        <Fragment>
          <div>
            <form>
              <div
                css={css`
                  margin: 1em;
                `}
              >
                <input
                  type="date"
                  id="purchaseDate"
                  name="purchaseDate"
                ></input>
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
                <div
                  css={css`
                    margin: 1em;
                  `}
                >
                  <label for="stores">Select a store:</label>
                  <select name="stores" id="stores">
                    {stores.map((store) => (
                      <option>{store.name}</option>
                    ))}
                  </select>
                </div>
              )}
              <div
                css={css`
                  margin: 1em;
                `}
              >
                Total: {totalPrice().toFixed(2)}{' '}
                {transaction.lineItems[0].currencySymbol}{' '}
              </div>
              <div
                css={css`
                  margin: 1em;
                  width: 50em;
                `}
              >
                <lable for="notes">Notes:</lable>
                <textarea
                  id="notes"
                  name="notes"
                  css={css`
                    width: 50em;
                  `}
                />
              </div>
              <div>
                <Table bordered hover size="sm">
                  <thead>
                    <tr>
                      <th>Product</th>
                      <th>Price</th>
                      <th>Notes</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {transaction.lineItems == null ? (
                      <tr>
                        <td contentEditable="true">
                          Product name goes here...
                        </td>
                        <td contentEditable="true">price here</td>
                        <td contentEditable="true">notes</td>
                      </tr>
                    ) : (
                      transaction.lineItems.map((lineItem, idx) => (
                        <tr>
                          <td
                            css={css`
                              height: 30px;
                              width: 33%;
                            `}
                          >
                            <input
                              type="text"
                              name="booking"
                              value={transaction.lineItems[idx].booking}
                              onChange={handleChange(idx)}
                              css={css`
                                border: none;
                                height: 100%;
                                width: 100%;
                              `}
                            />
                          </td>
                          <td
                            css={css`
                              height: 30px;
                              width: 33%;
                            `}
                          >
                            <input
                              type="text"
                              name="amount"
                              value={transaction.lineItems[idx].amount}
                              onChange={handleChange(idx)}
                              css={css`
                                border: none;
                                height: 100%;
                                width: 100%;
                              `}
                            />
                          </td>
                          <td
                            css={css`
                              height: 30px;
                              width: 33%;
                            `}
                          >
                            <input
                              type="text"
                              name="notes"
                              value={transaction.lineItems[idx].notes}
                              onChange={handleChange(idx)}
                              css={css`
                                border: none;
                                height: 100%;
                                width: 100%;
                              `}
                            />
                          </td>
                          <td>
                            <Button
                              variant="danger"
                              onClick={handleRemoveSpecificRow(idx)}
                            >
                              -
                            </Button>
                          </td>
                        </tr>
                      ))
                    )}
                  </tbody>
                </Table>
                <Button onClick={handleAddRow} variant="success">
                  Add Row
                </Button>
              </div>
              <Button
                type="submit"
                className="btn btn-primary"
                onClick={handleSubmit}
                css={css`
                  margin: 1em 0 0 0;
                  width: 5.5em;
                `}
              >
                Save
              </Button>
            </form>
          </div>
        </Fragment>
      </div>
    </Page>
  );
}
