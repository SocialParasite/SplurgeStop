import React, { Fragment, useState, useEffect } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';

export function PurchaseTransactionHome() {
  const [transactions, setTransactions] = useState(null);
  const [transactionsLoading, setTransactionsLoading] = useState(true);

  useEffect(() => {
    const loadTransactions = async () => {
      const url = 'https://localhost:44304/api/PurchaseTransaction';
      const response = await fetch(url);
      const data = await response.json();
      setTransactions(data);
      setTransactionsLoading(false);
      console.log('load called');
    };
    loadTransactions();
  }, []);

  function formatDate(date) {
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear();

    return [day, month, year].join('.');
  }

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
        <title>Purchase transactions</title>
      </div>
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
        <Table bordered hover size="sm">
          <thead>
            <tr
              css={css`
                background: burlywood;
                text-align: left;
              `}
            >
              <th>Purchase date</th>
              <th>Store</th>
              <th>Total spent</th>
              <th>Item count</th>
            </tr>
          </thead>
          {transactions.map((transaction) => (
            <tbody key={transaction.id}>
              <tr>
                <Fragment key={transaction.id}>
                  <td>
                    <Link
                      css={css`
                        text-decoration: none;
                      `}
                      to={`PurchaseTransaction/${transaction.id}`}
                    >
                      {formatDate(transaction.purchaseDate)}
                    </Link>
                  </td>
                  <td> {transaction.storeName} </td>
                  <td> {transaction.totalPrice} </td>
                  <td> {transaction.itemCount} </td>
                </Fragment>
              </tr>
            </tbody>
          ))}
        </Table>
      )}
    </div>
  );
}
