import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../Components/Page';
import { formatDate } from './../Common/DateTimeHelpers';

export function PurchaseTransactionPage({ match }) {
  const [transaction, setTransaction] = useState(null);
  const [transactionsLoading, setTransactionsLoading] = useState(true);
  const [isEditing, setEditing] = useState(false);

  useEffect(() => {
    const loadTransaction = async () => {
      const id = match.params.id;
      const url = 'https://localhost:44304/api/PurchaseTransaction/' + id;
      const response = await fetch(url);
      const data = await response.json();
      setTransaction(data);
      setTransactionsLoading(false);
    };

    loadTransaction();
  });

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const changeHandler = (e) => {
    console.log('change triggered!');
  };

  return (
    <Page>
      <div>
        <button onClick={editModeClick}>Edit</button>
        <div>
          {transactionsLoading ? (
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
                  <form>
                    <p>
                      <b>
                        <input
                          type="date"
                          id="purchaseDate"
                          value={transaction.purchaseDate.value}
                          name="purchaseDate"
                        ></input>
                      </b>
                      &nbsp;&nbsp;&nbsp;
                      <div>
                        <input
                          type="text"
                          name="store.name"
                          value={transaction.store.name}
                        ></input>
                      </div>
                    </p>
                    <div>Total: {transaction.totalPrice} </div>
                    <div>
                      <textarea name="notes" value={transaction.notes} />
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
                          {transaction.lineItems.map((item) => {
                            return (
                              <tr key={item.id.value}>
                                <td>Product name goes here...</td>
                                <td>
                                  {item.price.currency
                                    .positionRelativeToPrice === 'end'
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
                    <button type="submit">Save</button>
                  </form>
                ) : (
                  <div>
                    <p>
                      <i>{formatDate(transaction.purchaseDate.value)}</i>
                      <br />
                      <b>{transaction.store.name}</b>
                    </p>
                    <div>Total: {transaction.totalPrice} </div>
                    <div>
                      <p>{transaction.notes}</p>
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
                          {transaction.lineItems.map((item) => {
                            return (
                              <tr key={item.id}>
                                <td>Product name goes here...</td>
                                <td>
                                  {item.price.currency
                                    .positionRelativeToPrice === 'end'
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
                )}
              </div>
            </Fragment>
          )}
        </div>
      </div>
    </Page>
  );
}
