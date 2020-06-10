import React, { Fragment, useState, useEffect } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../Components/Page';
import { formatDate } from './../Common/DateTimeHelpers';
import { deletePurchaseTransaction } from './PurchaseTransactionCommands';

export function PurchaseTransactionList() {
  const [transactions, setTransactions] = useState(null);
  const [transactionsLoading, setTransactionsLoading] = useState(true);

  useEffect(() => {
    const loadTransactions = async () => {
      const url = 'https://localhost:44304/api/PurchaseTransaction';
      const response = await fetch(url);
      const data = await response.json();
      setTransactions(data);
      setTransactionsLoading(false);
    };
    loadTransactions();
  }, []);

  const removeItem = (index) => {
    let data = transactions.filter((_, i) => i !== index);
    setTransactions(data);
  };

  const handleDelete = async (transaction) => {
    let index = transactions.findIndex((t) => t.id === transaction.id);
    removeItem(index);

    await deletePurchaseTransaction({
      id: transaction.id,
    });
  };

  return (
    <Page title="Purchase transactions">
      <Button className="float-right" href={`PurchaseTransaction/Add`}>
        Add Purchase
      </Button>
      <div
        css={css`
          margin: 50px auto 20px auto;
          padding: 30px 12px;
          max-width: 1000px;
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
            <thead className="text-uppercase text-center background-burlywood">
              <tr
                css={css`
                  background: burlywood;
                `}
              >
                <th>Purchase date</th>
                <th>Store</th>
                <th>Total spent</th>
                <th>Item count</th>
                <th>Details</th>
                <th>Remove</th>
              </tr>
            </thead>
            {transactions.map((transaction) => (
              <tbody key={transaction.id}>
                <tr className="text-right">
                  <Fragment key={transaction.id}>
                    <td>{formatDate(transaction.purchaseDate)}</td>
                    <td> {transaction.storeName} </td>
                    <td> {transaction.totalPrice} </td>
                    <td> {transaction.itemCount} </td>
                    <td
                      css={css`
                        width: 50px;
                        text-align: center;
                      `}
                    >
                      <Button
                        variant="info"
                        href={`PurchaseTransaction/${transaction.id}`}
                      >
                        Show
                      </Button>
                    </td>
                    <td
                      css={css`
                        width: 50px;
                        text-align: center;
                      `}
                    >
                      <Button
                        variant="danger"
                        onClick={() => {
                          handleDelete(transaction);
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
