import React, { FC, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { PurchaseTransactionList } from './PurchaseTransaction/PurchaseTransactionList';
import {
  getPurchaseTransactions,
  PurchaseTransactionData,
} from './PurchaseTransaction/PurchaseTransactionData';
import { RouteComponentProps } from 'react-router-dom';
import { Page } from './Page';
import { PageTitle } from './PageTitle';
import { connect } from 'react-redux';
import { ThunkDispatch } from 'redux-thunk';
import { AnyAction } from 'redux';
import { getPurchaseTransactionsActionCreator, AppState } from './Store';

export const HomePage: FC<RouteComponentProps> = () => {
  const [transactions, setTransactions] = useState<
    PurchaseTransactionData[] | null
  >(null);
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
          <PageTitle>Purchase transactions</PageTitle>
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
    </Page>
  );
};

const mapStateToProps = (store: AppState) => {
  return {
    transactions: store.transactions.transactions,
    transactionsLoading: store.transactions.loading,
  };
};

const mapDispatchToProps = (dispatch: ThunkDispatch<any, any, AnyAction>) => {
  return {
    getPurchaseTransactions: () =>
      dispatch(getPurchaseTransactionsActionCreator()),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(HomePage);
