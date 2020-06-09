import React, { FC, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { RouteComponentProps } from 'react-router-dom';
import { PurchaseTransactionList } from './PurchaseTransaction/PurchaseTransactionList';
import { getPurchaseTransactions } from './PurchaseTransaction/PurchaseTransactionData';

const getTransactions = async () => {
  const transactions = await getPurchaseTransactions();
  return transactions;
};

export function HomePage() {
  const [transactions, setTransactions] = useState(null);
  const [transactionsLoading, setTransactionsLoading] = useState(true);

  useEffect(() => {
    let cancelled = false;
    const doGetPurchaseTransactions = async () => {
      const purchaseTransactions = await getPurchaseTransactions();
      if (!cancelled) {
        setTransactions(purchaseTransactions);
        setTransactionsLoading(false);
      }
    };
    doGetPurchaseTransactions();
    return () => {
      cancelled = true;
    };
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
        <PurchaseTransactionList data={transactions || []} />
      )}
    </div>
  );
}
